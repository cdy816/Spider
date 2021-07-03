//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/6 16:52:33.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider.MQTTServer
{
    /// <summary>
    /// 
    /// </summary>
    public class MQTTChannelData: NetworkClientChannelData
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
        public override ChannelType Type => ChannelType.MQTTClient;

        /// <summary>
        /// 本地设备主题
        /// </summary>
        public string LocalTopic { get; set; }

        /// <summary>
        /// 本地回复主题
        /// </summary>
        public string LocalReponseTopic { get; set; }

        /// <summary>
        /// 远端设备主题
        /// </summary>
        public string RemoteTopic { get; set; }

        /// <summary>
        /// 远端回复主题
        /// </summary>
        public string RemoteResponseTopic { get; set; }

        ///// <summary>
        ///// 服务端主题附件字符串
        ///// </summary>
        //public string ServerTopicAppendString { get; set; } = "/s";

        ///// <summary>
        ///// 客户端主题附件字符串
        ///// </summary>
        //public string ClientTopicAppendString { get; set; } = "/c";

        ///// <summary>
        ///// 回复消息附加字符串
        ///// </summary>
        //public string ResponseTopicAppendString { get; set; } = "/r";


        ///// <summary>
        ///// 主题前缀字符串
        ///// </summary>
        //public string TopicHeadString { get; set; } = "";

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
            var xx = base.SaveToXML();
            xx.SetAttributeValue("LocalTopic", this.LocalTopic);
            xx.SetAttributeValue("LocalReponseTopic", this.LocalReponseTopic);

            xx.SetAttributeValue("RemoteTopic", this.RemoteTopic);

            xx.SetAttributeValue("RemoteResponseTopic", this.RemoteResponseTopic);
            

            //xx.SetAttributeValue("ServerTopicAppendString", ServerTopicAppendString);
            //xx.SetAttributeValue("ClientTopicAppendString", ClientTopicAppendString);
            //xx.SetAttributeValue("ResponseTopicAppendString", ResponseTopicAppendString);
            //xx.SetAttributeValue("TopicHeadString", TopicHeadString);
            return xx;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);

            if (xe.Attribute("LocalTopic") != null)
            {
                LocalTopic = xe.Attribute("LocalTopic").Value;
            }

            if (xe.Attribute("LocalReponseTopic") != null)
            {
                LocalReponseTopic = xe.Attribute("LocalReponseTopic").Value;
            }


            if (xe.Attribute("RemoteTopic") != null)
            {
                RemoteTopic = xe.Attribute("RemoteTopic").Value;
            }

            if (xe.Attribute("RemoteResponseTopic") != null)
            {
                RemoteResponseTopic = xe.Attribute("RemoteResponseTopic").Value;
            }

            //if(xe.Attribute("ServerTopicAppendString") !=null)
            //{
            //    ServerTopicAppendString = xe.Attribute("ServerTopicAppendString").Value;
            //}

            //if (xe.Attribute("ClientTopicAppendString") != null)
            //{
            //    ClientTopicAppendString = xe.Attribute("ClientTopicAppendString").Value;
            //}

            //if (xe.Attribute("ResponseTopicAppendString") != null)
            //{
            //    ResponseTopicAppendString = xe.Attribute("ResponseTopicAppendString").Value;
            //}

            //if (xe.Attribute("TopicHeadString") != null)
            //{
            //    TopicHeadString = xe.Attribute("TopicHeadString").Value;
            //}
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
