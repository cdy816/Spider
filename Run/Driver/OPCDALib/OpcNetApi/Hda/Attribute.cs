using System;

namespace Opc.Hda
{
    [Serializable]
    public class Attribute : ICloneable
    {
        private int m_id;

        private string m_name;

        private string m_description;

        private System.Type m_datatype;

        public int ID
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id = value;
            }
        }

        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        public string Description
        {
            get
            {
                return m_description;
            }
            set
            {
                m_description = value;
            }
        }

        public System.Type DataType
        {
            get
            {
                return m_datatype;
            }
            set
            {
                m_datatype = value;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
}
