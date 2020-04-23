using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace ASCOM.SonyMirrorless
{
    public class SonyImage : SonyCommon
    {
        public enum ImageStatus
        {
            Created, Capturing, Reading, Ready, Failed, Cancelled
        }

        internal ImageInfo m_info;
        internal UInt32 m_cameraHandle;
        public DateTime StartTime = DateTime.Now;
        private int m_personality;
        private short m_readoutMode;

        internal ImageStatus m_status = ImageStatus.Created;

        public int Width = 0;
        public int Height = 0;
        public static int[,,] RGB;
        public static int[,] BAYER;

        public SonyImage(UInt32 handle, ImageInfo info, int personality, short readoutMode)
        {
            m_cameraHandle = handle;
            m_info = info;
            m_personality = personality;
            m_readoutMode = readoutMode;
            Status = SonyImage.ImageStatus.Capturing;

            if (m_info.Status == STATUS_COMPLETE)
            {
                ProcessImageData();
                Status = SonyImage.ImageStatus.Ready;
            }
        }

        public ImageStatus Status
        {
            get
            {
                switch (m_status)
                {
                    case ImageStatus.Created:
                    case ImageStatus.Ready:
                    case ImageStatus.Failed:
                    case ImageStatus.Cancelled:
                        break;

                    case ImageStatus.Capturing:
                        // Figure out if we've finished
                        GetCaptureStatus(m_cameraHandle, ref m_info);

                        switch (m_info.Status)
                        {
                            case STATUS_CANCELLED:
                                m_status = ImageStatus.Cancelled;
                                break;

                            case STATUS_FAILED:
                                m_status = ImageStatus.Failed;
                                break;

                            case STATUS_EXPOSING:
                                m_status = ImageStatus.Capturing;
                                break;

                            case STATUS_COMPLETE:
                                m_status = ImageStatus.Reading;
                                ProcessImageData();
                                m_status = ImageStatus.Ready;
                                break;
                        }
                        break;
                }

                return m_status;
            }

            set
            {
                m_status = value;
            }
        }

        public double Duration
        {
            get
            {
                return m_info.ExposureTime;
            }
        }

        public void ProcessImageData()
        {
            byte[] returndata = null;

            try
            {
                int numValues = (Int32)m_info.ImageSize / sizeof(ushort);

                returndata = new byte[m_info.ImageSize];

                Width = (int)m_info.Width;
                Height = (int)m_info.Height;

                Marshal.Copy(m_info.ImageData, returndata, 0, (Int32)m_info.ImageSize);

                switch (m_info.ImageMode)
                {
                    case IMAGEMODE_RAW:
                        // Reuse previous array
                        if (BAYER == null || BAYER.GetLength(0) != Width || BAYER.GetLength(1) != Height)
                        {
                            BAYER = new int[Width, Height];
                        }

                        for (int i = 0; i < m_info.ImageSize; i += 2)
                        {
                            int x = (i / 2) % Width;
                            int y = (i / 2) / Width;

                            BAYER[x, Height - y - 1] = returndata[i] + (returndata[i + 1] << 8);
                        }
                        break;

                    case IMAGEMODE_RGB:
                        // Reuse previous array
                        if (m_personality == PERSONALITY_NINA)
                        {
                            // NINA doesn't support multi-channel images, so convert the RGB To Mono
                            // Use BAYER obj since it is only x/y - avoids having another memory object
                            // Reuse previous array
                            if (BAYER == null || BAYER.GetLength(0) != Width || BAYER.GetLength(1) != Height)
                            {
                                BAYER = new int[Width, Height];
                            }

                            for (int i = 0; i < m_info.ImageSize; i += 6)
                            {
                                int x = (i / 6) % Width;
                                int y = Height - ((i / 6) / Width) - 1;
                                int r = returndata[i] + (returndata[i + 1] << 8);
                                int g = returndata[i + 2] + (returndata[i + 3] << 8);
                                int b = returndata[i + 4] + (returndata[i + 5] << 8);

                                BAYER[x, Height - y - 1] = (short)((0.2125 * r) + (0.7154 * g) + (0.0721 * b));
                            }
                        }
                        else
                        {
                            if (RGB == null || RGB.GetLength(0) != Width || RGB.GetLength(1) != Height)
                            {
                                RGB = new int[Width, Height, 3];
                            }

                            bool switchRB = m_readoutMode == OUTPUTFORMAT_BGR;
                            bool invert = m_personality == PERSONALITY_SHARPCAP;

                            for (int i = 0; i < m_info.ImageSize; i += 6)
                            {
                                int x = (i / 6) % Width;
                                int y = invert ? (i / 6) / Width : Height - ((i / 6) / Width) - 1;

                                RGB[x, y, switchRB ? 2 : 0] = returndata[i] + (returndata[i + 1] << 8);
                                RGB[x, y, 1] = returndata[i + 2] + (returndata[i + 3] << 8);
                                RGB[x, y, switchRB ? 0 : 2] = returndata[i + 4] + (returndata[i + 5] << 8);
                            }
                        }
                        break;
                }
            }
            finally
            {
                returndata = null;

                Cleanup();
            }
        }

        public void Cleanup()
        {
            if (m_info.ImageData != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_info.ImageData);
                m_info.ImageData = IntPtr.Zero;
            }
        }
    }
}
