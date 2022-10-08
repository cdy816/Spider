using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using System.Xml.Linq;

namespace Cdy.Spider.MelsecDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class MelsecA1DriverData : DriverData
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsAsciiModel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
            var re = base.SaveToXML();
            re.SetAttributeValue("IsAsciiModel", IsAsciiModel);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);
            if (xe.Attribute("IsAsciiModel") != null)
            {
                this.IsAsciiModel = bool.Parse(xe.Attribute("IsAsciiModel").Value);
            }
        }
    }
}
