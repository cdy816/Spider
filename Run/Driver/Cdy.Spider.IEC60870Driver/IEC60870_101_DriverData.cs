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
    public class IEC60870_101_DriverData : DriverData
    {

        #region ... Variables  ...
        private int mStationId = 1;

        private int mOwnAddress = 1;

        private bool mBalanced = false;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public bool Balanced
        {
            get
            {
                return mBalanced;
            }
            set
            {
                if (mBalanced != value)
                {
                    mBalanced = value;
                }
            }
        }



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

        /// <summary>
            /// 
            /// </summary>
        public int OwnAddress
        {
            get
            {
                return mOwnAddress;
            }
            set
            {
                if (mOwnAddress != value)
                {
                    mOwnAddress = value;
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
            if (xe.Attribute("OwnAddress") != null)
            {
                OwnAddress = int.Parse(xe.Attribute("OwnAddress").Value);
            }
            if (xe.Attribute("StationId")!=null)
            {
                StationId = int.Parse(xe.Attribute("StationId").Value);
            }
            if (xe.Attribute("Balanced") != null)
            {
                Balanced = bool.Parse(xe.Attribute("Balanced").Value);
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
            re.SetAttributeValue("OwnAddress", OwnAddress);
            re.SetAttributeValue("Balanced", Balanced);
            return re;
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
