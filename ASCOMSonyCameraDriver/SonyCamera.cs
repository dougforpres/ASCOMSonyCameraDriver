﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System.IO;

namespace ASCOM.SonyMirrorless
{
    public class SonyCamera : SonyCommon
    {
        private DeviceInfo m_info;
        private UInt32 m_handle = INVALID_HANDLE_VALUE;
        internal SonyImage m_lastImage = null;
        internal CaptureMode m_mode;
        internal ImageMode m_outputMode = ImageMode.RGB;
        internal CameraInfo m_resolutions;
        internal Dictionary<UInt32, CameraProperty> m_properties = new Dictionary<UInt32, CameraProperty>();
        internal Boolean m_bulbMode = false;
        internal short m_bulbModeTime = 1;
        internal short m_desiredGain = -1;
        private ArrayList m_gains = new ArrayList();

        public enum ImageMode
        {
            RGB = (int)IMAGEMODE_RGB,
            RGGB = (int)IMAGEMODE_RAW,
        }

        public class CaptureMode
        {
            public Boolean Preview = false;
            public UInt32 ImageWidthPixels = 1;
            public UInt32 ImageHeightPixels = 1;
        }

        public SonyCamera(DeviceInfo info)
        {
            m_info = info;
            m_mode = new CaptureMode();
            m_resolutions = new CameraInfo();

            m_mode.ImageWidthPixels = m_info.CropMode == 0 ? m_info.ImageWidthPixels : m_info.ImageWidthCroppedPixels;
            m_mode.ImageHeightPixels = m_info.CropMode == 0 ? m_info.ImageHeightPixels : m_info.ImageHeightCroppedPixels;
        }

        public bool Connected
        {
            get
            {
                return m_handle != INVALID_HANDLE_VALUE;
            }

            set
            {
                if (value)
                {
                    if (m_handle == INVALID_HANDLE_VALUE)
                    {
                        m_handle = OpenDevice(m_info.DeviceName);
                        GetCameraInfo(m_handle, ref m_resolutions, 0);
                        Log("Connected to Camera");
                    }
                }
                else
                {
                    if (m_handle != 1)
                    {
                        CloseDevice(m_handle);
                    }

                    m_handle = INVALID_HANDLE_VALUE;
                    Log("Unable to connect to camera");
                }
            }
        }

        public Boolean BulbMode
        {
            get
            {
                return m_bulbMode;
            }

            set
            {
                m_bulbMode = value;
            }
        }

        public short BulbModeTime
        {
            get
            {
                return m_bulbModeTime;
            }

            set
            {
                m_bulbModeTime = value;
            }
        }

        public DeviceInfo Info
        {
            get
            {
                return m_info;
            }
        }

        public CaptureMode Mode
        {
            get
            {
                return m_mode;
            }
        }

        public CameraStates State
        {
            get
            {
                if (m_lastImage != null)
                {
                    switch (m_lastImage.Status)
                    {
                        case SonyImage.ImageStatus.Capturing:
                            return CameraStates.cameraExposing;

                        case SonyImage.ImageStatus.Reading:
                            return CameraStates.cameraReading;

                        default:
                            return CameraStates.cameraIdle;
                    }
                }
                else
                {
                    return CameraStates.cameraIdle;
                }
            }
        }

        public void MoveFocus(int direction)
        {
            uint focusValue = 0;

            switch (direction)
            {
                case 1:
                    focusValue = 1;
                    break;

                case 2:
                    focusValue = 3;
                    break;

                case -1:
                    focusValue = 0xffff;
                    break;

                case -2:
                    focusValue = 0xfffd;
                    break;
            }

            SetPropertyValue(m_handle, SonyCommon.PROPERTY_FOCUS_CONTROL, focusValue);
        }

        public SonyImage StartCapture(double duration, int personality, short readoutMode)
        {
            ImageInfo info = new ImageInfo();

            if (m_lastImage != null)
            {
                m_lastImage.Cleanup();
                m_lastImage = null;
            }

            // 32-bit apps tend to run close to the line of memory
            // The auto garbage collection tends to fail when we're talking the large files
            // a 60MP camera can deliver, and an attempt to allocate more than is available doesn't
            // seem to trigger a collection - so we'll just do it here.
            // TODO: Make this conditional on 32-bit execution
            if (!Environment.Is64BitProcess)
            {
                GC.Collect();
            }

            info.Status = STATUS_EXPOSING;
            info.ImageMode = (uint)m_outputMode;
            info.ExposureTime = duration;

            if (Mode.Preview)
            {
                info.ImageMode = IMAGEMODE_RGB;
                GetPreviewImage(m_handle, ref info);
                m_lastImage = new SonyImage(m_handle, info, personality, readoutMode);
            }
            else
            {
                if (m_bulbMode)
                {
                    PropertyValue pv = new PropertyValue();
                    SetExposureTime(m_handle, (float)(duration > m_bulbModeTime ? 0 : duration), ref pv);
                }

                if (m_desiredGain != -1 && Gains.Count > 0)
                {
                    SetPropertyValue(m_handle, SonyCommon.PROPERTY_ISO, UInt32.Parse((string)Gains[m_desiredGain]));
                }

                StartCapture(m_handle, ref info);
                m_lastImage = new SonyImage(m_handle, info, personality, readoutMode);
            }

            return m_lastImage;
        }

        public void StopCapture()
        {
            if (m_lastImage != null && m_lastImage.Status == SonyImage.ImageStatus.Capturing)
            {
                CancelCapture(m_handle, ref m_lastImage.m_info);
            }
        }

        public CameraInfo Resolutions
        {
            get
            {
                GetCameraInfo(m_handle, ref m_resolutions, INFOFLAG_ACTIVE);

                return m_resolutions;
            }
        }

        public SonyImage LastImage
        {
            get
            {
                return m_lastImage;
            }
        }

        public Boolean ImageReady
        {
            get
            {
                return m_lastImage != null && m_lastImage.Status == SonyImage.ImageStatus.Ready;
            }
        }

        public String Model
        {
            get
            {
                return m_info.Model;
            }
        }

        public String SerialNumber
        {
            get
            {
                return m_info.SerialNumber.TrimStart(new char[] { '0' });
            }
        }

        public String DisplayName
        {
            get
            {
                return String.Format("{0} (s/n: {1})", Model, SerialNumber);
            }
        }

        public Boolean HasLiveView
        {
            get
            {
                return (m_resolutions.CameraFlags & CAMERA_SUPPORTS_LIVEVIEW) != 0;
            }
        }

        public Boolean PreviewMode
        {
            get
            {
                return Mode.Preview;
            }

            set
            {
                if (value != Mode.Preview)
                {
                    Mode.Preview = value && ((m_resolutions.CameraFlags & CAMERA_SUPPORTS_LIVEVIEW) != 0);

                    if (!Mode.Preview)
                    {
                        Mode.ImageWidthPixels = m_info.CropMode == 0 ? m_resolutions.ImageWidthPixels : m_resolutions.ImageWidthCroppedPixels;
                        Mode.ImageHeightPixels = m_info.CropMode == 0 ? m_resolutions.ImageHeightPixels : m_resolutions.ImageHeightCroppedPixels;
                    }
                    else
                    {
                        Mode.ImageWidthPixels = m_resolutions.PreviewWidthPixels;
                        Mode.ImageHeightPixels = m_resolutions.PreviewHeightPixels;
                    }

                    // This allows the driver to return assumed image size not the size of the last image
                    // which will probably now be different
                    if (m_lastImage != null)
                    {
                        m_lastImage.Cleanup();
                        m_lastImage = null;
                    }
                }
            }
        }

        public ImageMode OutputMode
        {
            get
            {
                return m_outputMode;
            }

            set
            {
                m_outputMode = value;
            }
        }

        public ArrayList Gains
        {
            get
            {
                if (m_gains.Count == 0)
                {
                    CameraProperty iso = GetProperty(SonyCommon.PROPERTY_ISO_OPTIONS);

                    SonyCommon.PropertyValueOption[] options = iso.Options;

                    foreach (SonyCommon.PropertyValueOption option in options)
                    {
                        // Unsure what the top-byte-set values are for, so filter them out
                        // 0x00ffffff = auto
                        if ((option.Value & 0xff000000) == 0)
                        {
                            m_gains.Add(option.Value.ToString());
                        }
                    }

                }

                return m_gains;
            }
        }

        public short GainIndex
        {
            get
            {
                CameraProperty iso = GetProperty(SonyCommon.PROPERTY_ISO);

                UInt32 value = iso.CurrentValue(m_handle).Value;
                short index = (short)Gains.IndexOf(value.ToString());

                Log(String.Format("get GainIndex: camera reports '{0}', which maps to index {1}", value, index));

                return index;
            }

            set
            {
                if (value >= 0 && value < Gains.Count)
                {
                    m_desiredGain = value;
                }
                else if (value >= Gains.Count)
                {
                    Log(String.Format("Attempting to find gain {0} match", value));
                    int desiredIndex = Gains.IndexOf(value.ToString());

                    if (desiredIndex >= 0)
                    {
                        Log(String.Format("Setting gain to index {0} which matches {1}", desiredIndex, value));
                        m_desiredGain = (short)desiredIndex;
                    }
                }
            }
        }

        private void PopulatePropertyInfo()
        {
            if (m_properties.Count == 0)
            {
                // Need to actually fetch properties
                // Start with getting a list of all the properties
                UInt32 count = 0;

                // Get # of properties
                UInt32 hr = GetPropertyList(m_handle, IntPtr.Zero, ref count);
                int[] ids = new int[count];
                IntPtr pIds = Marshal.AllocCoTaskMem((int)count * sizeof(UInt32));

                hr = GetPropertyList(m_handle, pIds, ref count);

                Marshal.Copy(pIds, ids, 0, (int)count);
                Marshal.FreeCoTaskMem(pIds);

                // Now I want the property descriptors
                for (int i = 0; i < count; i++)
                {
                    PropertyDescriptor descriptor = new PropertyDescriptor();

                    hr = GetPropertyDescriptor(m_handle, (uint)ids[i], ref descriptor);

                    PropertyValueOption[] options = new PropertyValueOption[descriptor.ValueCount];

                    for (uint index = 0; index < descriptor.ValueCount; index++)
                    {
                        GetPropertyValueOption(m_handle, descriptor.Id, ref options[index], index);
//                        UInt32 countReturned = descriptor.ValueCount;
//                        GetPropertyValueOptions(m_handle, descriptor.Id, ref options, ref countReturned);
                    }

                    m_properties[descriptor.Id] = new CameraProperty(descriptor, options);
                }
            }
        }

        public Dictionary<UInt32, CameraProperty> Properties
        {
            get
            {
                PopulatePropertyInfo();

                return m_properties;
            }
        }

        public CameraProperty GetProperty(UInt32 id)
        {
            PopulatePropertyInfo();
            return m_properties.ContainsKey(id) ? m_properties[id] : null;
        }

        public PropertyValue GetPropertyValue(UInt32 id)
        {
            PopulatePropertyInfo();
            return m_properties.ContainsKey(id) ? m_properties[id].CurrentValue(m_handle) : new PropertyValue();
        }

        private void Log(String message)
        {
            DriverCommon.LogCameraMessage("SonyCamera", message);
        }
    }
}
