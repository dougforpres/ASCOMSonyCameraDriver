using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ASCOM.SonyMirrorless
{
    abstract public class SonyCommon
    {
        protected static readonly UInt32 ERROR_SUCCESS = 0;
        protected static readonly UInt32 INVALID_HANDLE_VALUE = 0xffffffff;
        protected const UInt32 STATUS_EXPOSING = 0x01;
        protected const UInt32 STATUS_FAILED = 0x02;
        protected const UInt32 STATUS_CANCELLED = 0x03;
        protected const UInt32 STATUS_COMPLETE = 0x04;
        protected const UInt32 STATUS_STARTING = 0x8001;
        protected const UInt32 STATUS_READING = 0x8002;
        protected const UInt32 FORMAT_ARW = 0xb101;
        protected const UInt32 FORMAT_JPEG = 0x3801;
        protected const UInt32 IMAGEMODE_RAW = 1;
        protected const UInt32 IMAGEMODE_RGB = 2;
        protected const UInt32 IMAGEMODE_JPEG = 3;
        protected const UInt32 INFOFLAG_ACTIVE = 1;

        public const int PERSONALITY_APT = 1;
        public const int PERSONALITY_NINA = 2;
        public const int PERSONALITY_SHARPCAP = 3;

        public const short OUTPUTFORMAT_RGB = (short)IMAGEMODE_RGB;
        public const short OUTPUTFORMAT_BGR = OUTPUTFORMAT_RGB | 0x1000;
        public const short OUTPUTFORMAT_RGGB = (short)IMAGEMODE_RAW;

        public const UInt16 PROPERTY_ISO = 0xd21e;
        public const UInt16 PROPERTY_ISO_OPTIONS = 0xfffe;

        protected const UInt32 CAMERA_SUPPORTS_LIVEVIEW = 0x00000001;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct PortableDeviceInfo
        {
            public string id;
            public string manufacturer;
            public string model;
            public string devicePath;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DeviceInfo
        {
            public UInt32 Version;
            public UInt32 ImageWidthPixels;
            public UInt32 ImageHeightPixels;
            public UInt32 ImageWidthCroppedPixels;
            public UInt32 ImageHeightCroppedPixels;
            public UInt32 BayerXOffset;
            public UInt32 BayerYOffset;
            public UInt32 CropMode;
            public Double ExposureTimeMin;
            public Double ExposureTimeMax;
            public Double ExposureTimeStep;
            public Double PixelWidth;
            public Double PixelHeight;

            public string Manufacturer;
            public string Model;
            public string SerialNumber;
            public string DeviceName;
            public string SensorName;
            public string DeviceVersion;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct ImageInfo
        {
            public UInt32 ImageSize;
            public IntPtr ImageData;
            public UInt32 Status;
            public UInt32 ImageMode;
            public UInt32 Width;
            public UInt32 Height;
            public UInt32 Flags;
            public Double ExposureTime;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct CameraInfo
        {
            public UInt32 CameraFlags;
            public UInt32 ImageWidthPixels;
            public UInt32 ImageHeightPixels;
            public UInt32 ImageWidthCroppedPixels;
            public UInt32 ImageHeightCroppedPixels;
            public UInt32 PreviewWidthPixels;
            public UInt32 PreviewHeightPixels;
            public UInt32 BayerXOffset;
            public UInt32 BayerYOffset;
            public Double PixelWidth;
            public Double PixelHeight;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct PropertyValueOption
        {
            public UInt32 Value;
            public string Name;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct PropertyValue
        {
            public UInt32 Id;
            public UInt32 Value;
            public string Text;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct PropertyDescriptor
        {
            public UInt32 Id;
            public UInt16 Type;
            public UInt16 Flags;
            public string Name;
            public UInt32 ValueCount;
        }

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        protected static extern UInt32 GetDeviceCount();

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        protected static extern UInt32 GetPortableDeviceCount();

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        protected static extern UInt32 GetPortableDeviceInfo(UInt32 id, ref PortableDeviceInfo portableDeviceInfo);

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern UInt32 GetDeviceInfo(uint hDevice, ref DeviceInfo info);

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        protected static extern UInt32 OpenDevice(string DeviceName);

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        protected static extern void CloseDevice(UInt32 hDevice);

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        protected static extern UInt32 GetPreviewImage(UInt32 hDevice, ref ImageInfo Data);

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        protected static extern UInt32 StartCapture(UInt32 hDevice, ref ImageInfo Data);

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        protected static extern UInt32 CancelCapture(UInt32 hDevice, ref ImageInfo Data);

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        protected static extern UInt32 GetCaptureStatus(UInt32 hDevice, ref ImageInfo Data);

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        protected static extern UInt32 GetCameraInfo(UInt32 hDevice, ref CameraInfo Data, UInt32 Flags);

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        protected static extern UInt32 RefreshPropertyList(UInt32 hCamers);

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        protected static extern UInt32 GetPropertyList(UInt32 hCamera, IntPtr list, ref UInt32 listSize);

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        protected static extern UInt32 GetPropertyDescriptor(UInt32 hCamera, UInt32 propertyId, ref PropertyDescriptor descriptor);

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        protected static extern UInt32 GetPropertyValueOption(UInt32 hCamera, UInt32 propertyId, ref PropertyValueOption option, UInt32 index);

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        protected static extern UInt32 GetSinglePropertyValue(UInt32 hCamera, UInt32 propertyId, ref PropertyValue value);

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        protected static extern UInt32 GetAllPropertyValues(UInt32 hCamera, ref PropertyValue[] values, ref UInt32 count);

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        protected static extern UInt32 SetExposureTime(UInt32 hCamera, float exposureTime, ref PropertyValue valueOut);

        [DllImport("SonyMTPCamera.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        protected static extern UInt32 SetPropertyValue(UInt32 hCamera, UInt32 propertyId, UInt32 value);
    }
}
