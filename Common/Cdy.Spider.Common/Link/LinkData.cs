using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider
{
    public class LinkData
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }


        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual XElement SaveToXML()
        {
            XElement xx = new XElement("LinkData");
            xx.SetAttributeValue("Name", Name);
           
            return xx;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public virtual void LoadFromXML(XElement xe)
        {
            if (xe.Attribute("Name") != null)
            {
                this.Name = xe.Attribute("Name").Value;
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
