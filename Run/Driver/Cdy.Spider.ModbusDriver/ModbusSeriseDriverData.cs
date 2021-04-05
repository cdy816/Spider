using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public class ModbusSeriseDriverData: ModbusIpDriverData
    {

        #region ... Variables  ...
        
        private ModbusSeriseType mType;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public ModbusSeriseType Type
        {
            get
            {
                return mType;
            }
            set
            {
                if (mType != value)
                {
                    mType = value;
                }
            }
        }


        #endregion ...Properties...

        #region ... Methods    ...

        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);
            if (xe.Attribute("Type") != null)
            {
                Type = (ModbusSeriseType)int.Parse(xe.Attribute("Type").Value);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
            var re = base.SaveToXML();
            re.SetAttributeValue("Type", (int)Type);
            return re;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ModbusSeriseType
    {
        Ascii,
        Rtu
    }


}
