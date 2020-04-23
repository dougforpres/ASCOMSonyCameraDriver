using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.SonyMirrorless
{
    public class CameraProperty : SonyCommon
    {
        private PropertyDescriptor m_descriptor;
        private PropertyValueOption[] m_options;

        public CameraProperty(PropertyDescriptor descriptor, PropertyValueOption[] options)
        {
            m_descriptor = descriptor;
            m_options = options;
        }

        public PropertyValue CurrentValue(UInt32 hCamera)
        {
            PropertyValue v = new PropertyValue();

            GetSinglePropertyValue(hCamera, m_descriptor.Id, ref v);

            return v;
        }
    }
}
