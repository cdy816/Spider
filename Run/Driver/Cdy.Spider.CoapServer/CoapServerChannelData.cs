using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.CoapServer
{
    /// <summary>
    /// 
    /// </summary>
    public class CoapServerChannelData : NetworkServerChannelData
    {

        #region ... Variables  ...
        private string mServerName = "";
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public string ServerName
        {
            get
            {
                return mServerName;
            }
            set
            {
                if (mServerName != value)
                {
                    mServerName = value;
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
            if(xe.Attribute("ServerName") !=null)
            {
                this.ServerName = xe.Attribute("ServerName").Value;
            }
        }

        public override XElement SaveToXML()
        {
            var re = base.SaveToXML();
            re.SetAttributeValue("ServerName", ServerName);
            return re;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
