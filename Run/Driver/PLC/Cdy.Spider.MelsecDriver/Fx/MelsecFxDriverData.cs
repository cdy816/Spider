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
    public class MelsecFxDriverData : DriverData
    {
        /// <summary>
        /// 站号
        /// </summary>
        public int Station { get; set; }
        /// <summary>
        /// 格式
        /// </summary>
        public byte Formate { get; set; }

        /// <summary>
        /// 校验和
        /// </summary>
        public bool IsCheckSum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
            var re = base.SaveToXML();
            re.SetAttributeValue("Station", Station);
            re.SetAttributeValue("Formate", Formate);
            re.SetAttributeValue("IsCheckSum", IsCheckSum);
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
            if (xe.Attribute("IsCheckSum") != null)
            {
                this.IsCheckSum = bool.Parse(xe.Attribute("IsCheckSum").Value);
            }
        }
    }
}
