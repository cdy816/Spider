using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.IEC60870Driver
{
    /// <summary>
    /// 
    /// </summary>
    public class IEC60870_104_DriverData : DriverData
    {

        #region ... Variables  ...
        private int mStationId = 1;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public int StationId
        {
            get
            {
                return mStationId;
            }
            set
            {
                if (mStationId != value)
                {
                    mStationId = value;
                }
            }
        }

        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);
            if(xe.Attribute("StationId")!=null)
            {
                StationId = int.Parse(xe.Attribute("StationId").Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
            var re = base.SaveToXML();
            re.SetAttributeValue("StationId", StationId);
            return re;
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
