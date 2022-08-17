using System;

namespace Opc.Ae
{
    [Serializable]
    public class Attribute : ICloneable
    {
        private int m_id;

        private string m_name;

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
