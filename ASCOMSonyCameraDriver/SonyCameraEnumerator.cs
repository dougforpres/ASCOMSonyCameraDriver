using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace ASCOM.SonyMirrorless
{
    public class SonyCameraEnumerator : SonyCommon
    {
        public ArrayList Cameras
        {
            get
            {
                ArrayList result = new ArrayList();
                UInt32 count = GetDeviceCount();
                UInt32 hr;
                PortableDeviceInfo portableDeviceInfo = new PortableDeviceInfo();

                for (UInt32 iter = 0; iter < count; iter++)
                {
                    hr = GetPortableDeviceInfo(iter, ref portableDeviceInfo);

                    if (hr == ERROR_SUCCESS)
                    {
                        // Try to open the device
                        UInt32 handle = OpenDevice(portableDeviceInfo.id);

                        if (handle != INVALID_HANDLE_VALUE)
                        {
                            DeviceInfo info = new DeviceInfo()
                            {
                                Version = 1
                            };

                            hr = GetDeviceInfo(handle, ref info);

                            if (hr == ERROR_SUCCESS)
                            {
                                result.Add(new SonyCamera(info));
                            }

                            CloseDevice(handle);
                        }
                    }
                }

                return result;
            }
        }
    }
}
