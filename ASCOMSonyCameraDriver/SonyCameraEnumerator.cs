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

                for (UInt32 iter = 0; iter < count; iter++)
                {
                    DeviceInfo info = new DeviceInfo()
                    {
                        Version = 1
                    };

                    UInt32 hr = GetDeviceInfo(iter, ref info);

                    if (hr == ERROR_SUCCESS)
                    {
                        result.Add(new SonyCamera(info));
                    }
                }

                return result;
            }
        }
    }
}
