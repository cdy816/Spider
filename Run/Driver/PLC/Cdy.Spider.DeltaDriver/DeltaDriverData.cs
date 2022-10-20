using Cdy.Spider.Common;
using Cdy.Spider.DeltaDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.DeltaDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class DeltaDriverData : DriverData
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
        public bool IsStringReverse
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        public int Station
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        public bool AddressStartWithZero
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DeltaSeries Serise
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
            re.SetAttributeValue("Station", this.Station);
            re.SetAttributeValue("IsStringReverse", this.IsStringReverse);
            re.SetAttributeValue("AddressStartWithZero", this.AddressStartWithZero);
            re.SetAttributeValue("Serise", (int)this.Serise);
            re.SetAttributeValue("DataFormate", this.DataFormate);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);
            if(xe.Attribute("Station") !=null)
            {
                this.Station = byte.Parse(xe.Attribute("Station").Value);
            }
            if (xe.Attribute("IsStringReverse") != null)
            {
                this.IsStringReverse = bool.Parse(xe.Attribute("IsStringReverse").Value);
            }
            if (xe.Attribute("AddressStartWithZero") != null)
            {
                this.AddressStartWithZero = bool.Parse(xe.Attribute("AddressStartWithZero").Value);
            }
            if (xe.Attribute("Serise") != null)
            {
                this.Serise = (DeltaSeries) byte.Parse(xe.Attribute("Serise").Value);
            }
            if (xe.Attribute("DataFormate") != null)
            {
                this.DataFormate = (DataFormat)byte.Parse(xe.Attribute("DataFormate").Value);
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
