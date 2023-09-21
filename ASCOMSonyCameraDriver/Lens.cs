using System;
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
    public class Lens : SonyCommon
    {
        private string id;
        private string manufacturer;
        private string model;
        private string path;

        public Lens(LensInfo info)
        {
            id = info.Id;
            manufacturer = info.Manufacturer;
            model = info.Model;
            path = info.LensPath;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", manufacturer, model);
        }

        public string Id
        {
            get
            {
                return id;
            }
        }

        public string Manufacturer
        {
            get
            {
                return manufacturer;
            }
        }

        public string Model
        {
            get
            {
                return model;
            }
        }

        public string Path
        {
            get
            {
                return path;
            }
        }
    }
}
