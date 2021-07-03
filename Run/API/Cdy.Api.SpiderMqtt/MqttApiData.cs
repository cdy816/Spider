using Cdy.Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Api.SpiderMqtt
{
    /// <summary>
    /// 
    /// </summary>
    public class MqttApiData:ApiData
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 远端主动上传数据主题
        /// </summary>
        public string RemoteTopic { get; set; }

        /// <summary>
        /// 远端回复主题
        /// </summary>
        public string RemoteResponseTopic { get; set; }


        /// <summary>
        /// 本地主题
        /// </summary>
        public string LocalTopic { get; set; }

        /// <summary>
        /// 本地回复主题
        /// </summary>
        public string LocalResponseTopic { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
            var re = base.SaveToXML();
            re.SetAttributeValue("RemoteTopic", RemoteTopic);
            re.SetAttributeValue("RemoteResponseTopic", RemoteResponseTopic);
            re.SetAttributeValue("LocalTopic", LocalTopic);
            re.SetAttributeValue("LocalResponseTopic", LocalResponseTopic);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            if(xe.Attribute("RemoteTopic") !=null)
            {
                RemoteTopic = xe.Attribute("RemoteTopic").Value;
            }
            if (xe.Attribute("RemoteResponseTopic") != null)
            {
                RemoteResponseTopic = xe.Attribute("RemoteResponseTopic").Value;
            }
            if (xe.Attribute("LocalTopic") != null)
            {
                LocalTopic = xe.Attribute("LocalTopic").Value;
            }
            if (xe.Attribute("LocalResponseTopic") != null)
            {
                LocalResponseTopic = xe.Attribute("LocalResponseTopic").Value;
            }
            base.LoadFromXML(xe);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
