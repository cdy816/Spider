using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.OmronDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class OmronFinsDriverData : DriverData
    {

        #region ... Variables  ...
        private int mDA2;
        private int mSA2;
        private int mSID;
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

        /// <summary>
        /// PLC 单元号
        /// </summary>
        public int DA2
        {
            get
            {
                return mDA2;
            }
            set
            {
                if (mDA2 != value)
                {
                    mDA2 = value;
                }
            }
        }

        /// <summary>
        /// 上位机单元号
        /// </summary>
        public int SA2
        {
            get
            {
                return mSA2;
            }
            set
            {
                if (mSA2 != value)
                {
                    mSA2 = value;
                }
            }
        }

        /// <summary>
        /// 设备标识号
        /// </summary>
        public int SID
        {
            get
            {
                return mSID;
            }
            set
            {
                if (mSID != value)
                {
                    mSID = value;
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
            re.SetAttributeValue("DA2", this.DA2);
            re.SetAttributeValue("SA2", this.SA2);
            re.SetAttributeValue("SID", this.SID);
            re.SetAttributeValue("DataFormate", (byte)this.DataFormate);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);
            this.DA2 = xe.Attribute("DA2")!=null?int.Parse(xe.Attribute("DA2").Value):0;
            this.SA2 = xe.Attribute("SA2") != null ? int.Parse(xe.Attribute("SA2").Value):0;
            this.SID = xe.Attribute("SID") != null ? int.Parse(xe.Attribute("SID").Value):0;
            this.DataFormate = xe.Attribute("DataFormate") != null ? (DataFormat)int.Parse(xe.Attribute("DataFormate").Value) : DataFormat.CDAB;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
