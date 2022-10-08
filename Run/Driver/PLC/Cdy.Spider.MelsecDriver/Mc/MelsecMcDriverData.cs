using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.MelsecDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class MelsecMcDriverData : DriverData
    {
        /// <summary>
        /// R系列PLC
        /// </summary>
        public bool IsRType { get; set; }

        /// <summary>
        /// Ascii 编码通信
        /// </summary>
        public bool IsAscii { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
            var re = base.SaveToXML();
            re.SetAttributeValue("IsRType", IsRType);
            re.SetAttributeValue("IsAscii", IsAscii);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);
            if(xe.Attribute("IsRType") !=null)
            {
                this.IsRType = bool.Parse(xe.Attribute("IsRType").Value);
            }
            if (xe.Attribute("IsAscii") != null)
            {
                this.IsAscii = bool.Parse(xe.Attribute("IsAscii").Value);
            }
        }
    }
}
