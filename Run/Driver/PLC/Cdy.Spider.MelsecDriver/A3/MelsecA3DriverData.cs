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
    public class MelsecA3DriverData : DriverData
    {
        /// <summary>
        /// 
        /// </summary>
        public int Station { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte Formate { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
            var re = base.SaveToXML();
            re.SetAttributeValue("Station", Station);
            re.SetAttributeValue("Formate", Formate);
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
                this.Station = int.Parse(xe.Attribute("Station").Value);
            }
            if (xe.Attribute("Formate") != null)
            {
                this.Formate = byte.Parse(xe.Attribute("Formate").Value);
            }
        }
    }
}
