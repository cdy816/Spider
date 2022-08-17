using System;

namespace Opc.Da
{
    [Serializable]
    public class ItemProperty : ICloneable, IResult
    {
        private PropertyID m_id;

        private string m_description;

        private System.Type m_datatype;

        private object m_value;

        private string m_itemName;

        private string m_itemPath;

        private ResultID m_resultID = ResultID.S_OK;

        private string m_diagnosticInfo;

        public PropertyID ID
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

        public object Value
        {
            get
            {
                return m_value;
            }
            set
            {
                m_value = value;
            }
        }

        public string ItemName
        {
            get
            {
                return m_itemName;
            }
            set
            {
                m_itemName = value;
            }
        }

        public string ItemPath
        {
            get
            {
                return m_itemPath;
            }
            set
            {
                m_itemPath = value;
            }
        }

        public ResultID ResultID
        {
            get
            {
                return m_resultID;
            }
            set
            {
                m_resultID = value;
            }
        }

        public string DiagnosticInfo
        {
            get
            {
                return m_diagnosticInfo;
            }
            set
            {
                m_diagnosticInfo = value;
            }
        }

        public virtual object Clone()
        {
            ItemProperty itemProperty = (ItemProperty)MemberwiseClone();
            itemProperty.Value = Convert.Clone(Value);
            return itemProperty;
        }
    }

}
