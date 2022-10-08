using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.FujiDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class FujiSPHDriverData : DriverData
    {

        #region ... Variables  ...
        private DataFormat mDataFormate = DataFormat.CDAB;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public int ConnectionID
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFormat DataFormate
        {
            get
            {
                return mDataFormate;
            }
            set
            {
                if (mDataFormate != value)
                {
                    mDataFormate = value;
                }
            }
        }
        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
            var re = base.SaveToXML();
            re.SetAttributeValue("DataFormate", (byte)this.DataFormate);
            re.SetAttributeValue("ConnectionID", ConnectionID);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);
            this.DataFormate = xe.Attribute("DataFormate") != null ? (DataFormat)int.Parse(xe.Attribute("DataFormate").Value) : DataFormat.CDAB;
            this.ConnectionID = xe.Attribute("ConnectionID") != null ? int.Parse(xe.Attribute("ConnectionID").Value) : 254;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
