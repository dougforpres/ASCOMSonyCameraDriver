using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace ASCOM.SonyMirrorless
{
    public class LensEnumerator : SonyCommon
    {
        public ArrayList Lenses
        {
            get
            {
                ArrayList result = new ArrayList();
                UInt32 count = GetLensCount();
                LensInfo lens = new LensInfo();

                for (UInt32 iter = 0; iter < count; iter++)
                {
                    UInt32 hr = GetLensInfo(iter, ref lens);

                    if (hr == 0)
                    {
                        result.Add(new Lens(lens));
                    }
                }

                return result;
            }
        }
    }
}
