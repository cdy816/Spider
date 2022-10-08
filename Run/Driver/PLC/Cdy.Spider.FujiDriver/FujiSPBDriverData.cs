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
    public class FujiSPBDriverData : DriverData
    {

        #region ... Variables  ...
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public int Station
        {
            get;
            set;
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
            re.SetAttributeValue("Station", (byte)this.Station);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);
            this.Station = xe.Attribute("Station") != null ? int.Parse(xe.Attribute("Station").Value) : 1;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
