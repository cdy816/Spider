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

namespace Cdy.Spider.MQTTClient
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
        /// 服务端主题附件字符串
        /// </summary>
        public string ServerTopicAppendString { get; set; } = "/s";

        /// <summary>
        /// 客户端主题附件字符串
        /// </summary>
        public string ClientTopicAppendString { get; set; } = "/c";

        /// <summary>
        /// 回复消息附加字符串
        /// </summary>
        public string ResponseTopicAppendString { get; set; } = "/r";

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
            var xx = base.SaveToXML();
            xx.SetAttributeValue("ServerTopicAppendString", ServerTopicAppendString);
            xx.SetAttributeValue("ClientTopicAppendString", ClientTopicAppendString);
            xx.SetAttributeValue("ResponseTopicAppendString", ResponseTopicAppendString);
            return xx;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);

            if(xe.Attribute("ServerTopicAppendString") !=null)
            {
                ServerTopicAppendString = xe.Attribute("ServerTopicAppendString").Value;
            }

            if (xe.Attribute("ClientTopicAppendString") != null)
            {
                ServerTopicAppendString = xe.Attribute("ClientTopicAppendString").Value;
            }

            if (xe.Attribute("ResponseTopicAppendString") != null)
            {
                ServerTopicAppendString = xe.Attribute("ResponseTopicAppendString").Value;
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
