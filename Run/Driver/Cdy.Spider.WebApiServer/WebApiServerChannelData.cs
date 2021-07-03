using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.WebApiServer
{
    public class WebApiServerChannelData:NetworkServerChannelData
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 使用HTTPs
        /// </summary>
        public bool UseHttps { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
           var xe = base.SaveToXML();
            xe.SetAttributeValue("UseHttps", UseHttps);
            return xe;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);
            if(xe.Attribute("UseHttps")!=null)
            {
                UseHttps = bool.Parse(xe.Attribute("UseHttps").Value);
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
