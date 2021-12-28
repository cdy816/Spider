using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public class OpcDriverData:DriverData
    {
        /// <summary>
        /// 数据打包大小
        /// </summary>
        public int PackageCount { get; set; } = 100;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);
            if(xe.Attribute("PackageCount")!=null)
            {
                PackageCount = int.Parse(xe.Attribute("PackageCount").Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
            var re = base.SaveToXML();
            re.SetAttributeValue("PackageCount", PackageCount);
            return re;
        }
    }
}
