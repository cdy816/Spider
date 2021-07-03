using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.WebApiClient
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiClientChannelData: ServerClientChannelData
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
        public WebApiMethod Method { get; set; }



        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {

            if (xe.Attribute("Method") != null)
            {
                Method = (WebApiMethod)(int.Parse(xe.Attribute("Method").Value));
            }
            base.LoadFromXML(xe);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
            var re = base.SaveToXML();
            re.SetAttributeValue("Method", (int)Method);
            return re;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 
    /// </summary>
    public enum WebApiMethod
    {
        /// <summary>
        /// 
        /// </summary>
        Get,
        /// <summary>
        /// 
        /// </summary>
        Post
    }

}
