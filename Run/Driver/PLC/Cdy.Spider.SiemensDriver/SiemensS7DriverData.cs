using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.SiemensDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class SiemensS7DriverData : DriverData
    {

        #region ... Variables  ...
        private int mRack;
        private int mSlot;
        private int mConnectionType=1;
        private int mLocalTSAP=258;
        private DataFormat mDataFormate = DataFormat.CDAB;
        private int mPlcType = 1;
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
        /// PLC的机架号
        /// </summary>
        public int Rack
        {
            get
            {
                return mRack;
            }
            set
            {
                if (mRack != value)
                {
                    mRack = value;
                }
            }
        }

        /// <summary>
        /// PLC的槽号
        /// </summary>
        public int Slot
        {
            get
            {
                return mSlot;
            }
            set
            {
                if (mSlot != value)
                {
                    mSlot = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置当前PLC的连接方式，PG: 0x01，OP: 0x02，S7Basic: 0x03...0x10
        /// </summary>
        public int ConnectionType
        {
            get
            {
                return mConnectionType;
            }
            set
            {
                if (mConnectionType != value)
                {
                    mConnectionType = value;
                }
            }
        }

        /// <summary>
        /// 西门子相关的本地TSAP参数信息
        /// </summary>
        public int LocalTSAP
        {
            get
            {
                return mLocalTSAP;
            }
            set
            {
                if (mLocalTSAP != value)
                {
                    mLocalTSAP = value;
                }
            }
        }

        /// <summary>
        /// Plc 类型，S1200 = 1,S300,S400,S1500,S200Smart,S200
        /// </summary>
        public int PlcType
        {
            get
            {
                return mPlcType;
            }
            set
            {
                if (mPlcType != value)
                {
                    mPlcType = value;
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
            re.SetAttributeValue("Rack", this.Rack);
            re.SetAttributeValue("Slot", this.Slot);
            re.SetAttributeValue("ConnectionType", this.ConnectionType);
            re.SetAttributeValue("LocalTSAP", this.LocalTSAP);
            re.SetAttributeValue("DataFormate", (byte)this.DataFormate);
            re.SetAttributeValue("PlcType", (byte)this.PlcType);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);
            this.Rack = xe.Attribute("Rack") !=null?int.Parse(xe.Attribute("Rack").Value):0;
            this.Slot = xe.Attribute("Slot") != null ? int.Parse(xe.Attribute("Slot").Value):0;
            this.ConnectionType = xe.Attribute("ConnectionType") != null ? int.Parse(xe.Attribute("ConnectionType").Value):0;
            this.DataFormate = xe.Attribute("DataFormate") != null ? (DataFormat)int.Parse(xe.Attribute("DataFormate").Value) : DataFormat.CDAB;
            this.PlcType = xe.Attribute("PlcType") != null ? int.Parse(xe.Attribute("PlcType").Value) : 1;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
