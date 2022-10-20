using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.BeckhoffDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class AdsData : DriverData
    {

        #region ... Variables  ...
        
        private bool mUseAutoAmsNetID = true;
        private string mSenderAMSNetId = "";
        private string mTargetAmsNetID = "";
        private int mAmsPort = 851;


        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public bool UseAutoAmsNetID
        {
            get
            {
                return mUseAutoAmsNetID;
            }
            set
            {
                if (mUseAutoAmsNetID != value)
                {
                    mUseAutoAmsNetID = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AmsPort
        {
            get
            {
                return mAmsPort;
            }
            set
            {
                if (mAmsPort != value)
                {
                    mAmsPort = value;
                }
            }
        }


        /// <summary>
            /// 
            /// </summary>
        public string SenderAMSNetId
        {
            get
            {
                return mSenderAMSNetId;
            }
            set
            {
                if (mSenderAMSNetId != value)
                {
                    mSenderAMSNetId = value;
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public string TargetAmsNetID
        {
            get
            {
                return mTargetAmsNetID;
            }
            set
            {
                if (mTargetAmsNetID != value)
                {
                    mTargetAmsNetID = value;
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
            re.SetAttributeValue("AmsPort", this.AmsPort);
            re.SetAttributeValue("UseAutoAmsNetID", this.UseAutoAmsNetID);
            re.SetAttributeValue("TargetAmsNetID", this.TargetAmsNetID);
            re.SetAttributeValue("SenderAMSNetId", this.SenderAMSNetId);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);
            this.AmsPort = xe.Attribute("AmsPort") != null ? int.Parse(xe.Attribute("AmsPort").Value) : 0;
            this.UseAutoAmsNetID = xe.Attribute("UseAutoAmsNetID") != null ?bool.Parse(xe.Attribute("UseAutoAmsNetID").Value) : true;
            this.TargetAmsNetID = xe.Attribute("TargetAmsNetID") != null ? xe.Attribute("TargetAmsNetID").Value : "";
            this.SenderAMSNetId = xe.Attribute("SenderAMSNetId") != null ? xe.Attribute("SenderAMSNetId").Value : "";
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
