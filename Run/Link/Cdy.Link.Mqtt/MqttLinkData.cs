using System;
using System.Xml.Linq;

namespace Cdy.Link.Mqtt
{
    public class MqttLinkData:Cdy.Spider.LinkData
    {

        /// <summary>
        /// 
        /// </summary>
        public string ServerUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ServerPort { get; set; } = 1803;

        /// <summary>
        /// 
        /// </summary>
        public string ServerUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ServerPassword { get; set; }


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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            if (xe.Attribute("RemoteTopic") != null)
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

            if (xe.Attribute("ServerUrl") != null)
            {
                ServerUrl = xe.Attribute("ServerUrl").Value;
            }
            if (xe.Attribute("ServerUser") != null)
            {
                ServerUser = xe.Attribute("ServerUser").Value;
            }
            if (xe.Attribute("ServerPassword") != null)
            {
                ServerPassword = xe.Attribute("ServerPassword").Value;
            }
            if (xe.Attribute("ServerPort") != null)
            {
                ServerPort = int.Parse(xe.Attribute("ServerPort").Value);
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
            re.SetAttributeValue("RemoteTopic", RemoteTopic);
            re.SetAttributeValue("RemoteResponseTopic", RemoteResponseTopic);
            re.SetAttributeValue("LocalTopic", LocalTopic);
            re.SetAttributeValue("LocalResponseTopic", LocalResponseTopic);
            re.SetAttributeValue("ServerUrl", ServerUrl);
            re.SetAttributeValue("ServerPort", ServerPort);

            re.SetAttributeValue("ServerUser", ServerUser);
            re.SetAttributeValue("ServerPassword", ServerPassword);
            return re;
        }

    }
}
