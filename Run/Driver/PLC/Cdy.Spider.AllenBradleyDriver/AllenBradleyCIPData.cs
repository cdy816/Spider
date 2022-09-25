using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.AllenBradleyDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class AllenBradleyCIPData : DriverData
    {

        #region ... Variables  ...
        
        private int mSlot = 0;
        
        private string mRouter = "";

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
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
            /// 
            /// </summary>
        public string Router
        {
            get
            {
                return mRouter;
            }
            set
            {
                if (mRouter != value)
                {
                    mRouter = value;
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
            re.SetAttributeValue("Slot", this.Slot);
            if(!string.IsNullOrEmpty(this.Router))
            re.SetAttributeValue("Router", this.Router);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);
            this.Slot = xe.Attribute("Slot") != null ? int.Parse(xe.Attribute("Slot").Value) : 0;
            this.Router = xe.Attribute("Router") != null ? xe.Attribute("Router").Value : "";
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
