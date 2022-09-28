using Cdy.Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Api.OpcUAServer
{
    /// <summary>
    /// 
    /// </summary>
    public class OPCUAApiData : ApiData
    {

        #region ... Variables  ...

        private bool mNoneSecurityMode = false;

        private int mServerPort = 24440;

        private string mUserName;

        private string mPassword;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public bool NoneSecurityMode
        {
            get
            {
                return mNoneSecurityMode;
            }
            set
            {
                if (mNoneSecurityMode != value)
                {
                    mNoneSecurityMode = value;
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
            re.SetAttributeValue("NoneSecurityMode", NoneSecurityMode);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);
            if(xe.Attribute("NoneSecurityMode") !=null)
            {
                NoneSecurityMode = bool.Parse(xe.Attribute("NoneSecurityMode").Value);
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
