//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/5 9:56:50.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider
{

    /// <summary>
    /// 
    /// </summary>
    public enum ChannelType
    {
        /// <summary>
        /// 透明传输
        /// </summary>
        Transparent,
        PortServer,
        PortClient,
        TcpServer,
        TcpClient,
        UdpServer,
        UdpClient,
        HttpServer,
        HttpClient,
        WebAPIServer,
        WebAPIClient,
        WebSocketServer,
        WebSocketClient,
        MQTTServer,
        MQTTClient,
        OPCUAServer,
        OPCUAClient
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class ChannelData
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
        public string Assembly { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 通道类型
        /// </summary>
        public ChannelType Type { get; set; }

        /// <summary>
        /// 通讯失败时，重试次数
        /// </summary>
        public int ReTryCount { get; set; }

        /// <summary>
        /// 通信失败时，重试间隔
        /// 单位:ms
        /// </summary>
        public int ReTryDuration { get; set; }

        /// <summary>
        /// 无数据通信超时时间，
        /// 单位:ms
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// 数据发送超时
        /// 单位:ms
        /// </summary>
        public int DataSendTimeout { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual XElement SaveToXML()
        {
            XElement xx = new XElement("Channel");
            xx.SetAttributeValue("Name", Name);
            xx.SetAttributeValue("Assembly", Assembly);
            xx.SetAttributeValue("Class", Class);
            xx.SetAttributeValue("Type", (int)Type);
            xx.SetAttributeValue("ReTryCount", ReTryCount);
            xx.SetAttributeValue("ReTryDuration", ReTryDuration);
            xx.SetAttributeValue("Timeout", Timeout);
            xx.SetAttributeValue("DataSendTimeout", DataSendTimeout);
            return xx;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public virtual void LoadFromXML(XElement xe)
        {
            if(xe.Attribute("Name") !=null)
            {
                this.Name = xe.Attribute("Name").Value;
            }

            this.Assembly = xe.Attribute("Assembly")?.Value;
            this.Class = xe.Attribute("Class")?.Value;

            if (xe.Attribute("Type") != null)
            {
                this.Type = (ChannelType)(int.Parse(xe.Attribute("Type").Value));
            }

            if (xe.Attribute("ReTryCount") != null)
            {
                this.ReTryCount = int.Parse(xe.Attribute("ReTryCount").Value);
            }

            if (xe.Attribute("ReTryDuration") != null)
            {
                this.ReTryDuration = int.Parse(xe.Attribute("ReTryDuration").Value);
            }

            if (xe.Attribute("Timeout") != null)
            {
                this.Timeout = int.Parse(xe.Attribute("Timeout").Value);
            }

            if (xe.Attribute("DataSendTimeout") != null)
            {
                this.DataSendTimeout = int.Parse(xe.Attribute("DataSendTimeout").Value);
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 账户安全信息通道
    /// </summary>
    public abstract class SecurityChannelData: ChannelData
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
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
            var re = base.SaveToXML();
            re.SetAttributeValue("UserName", UserName);
            re.SetAttributeValue("Password", Password);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);
            if (xe.Attribute("UserName") != null)
            {
                this.UserName = xe.Attribute("UserName").Value;
            }
            if (xe.Attribute("Password") != null)
            {
                this.Password = xe.Attribute("Password").Value;
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 
    /// </summary>
    public class NetworkServerChannelData : SecurityChannelData
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
        public int Port { get; set; }
        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
            var re = base.SaveToXML();
            re.SetAttributeValue("Port", Port);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);
            if (xe.Attribute("Port") != null)
            {
                this.Port = int.Parse(xe.Attribute("Port").Value);
            }
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }


    /// <summary>
    /// 
    /// </summary>
    public class NetworkClientChannelData : SecurityChannelData
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
        public string ServerIp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; }
        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
            var re = base.SaveToXML();
            re.SetAttributeValue("ServerIp", ServerIp);
            re.SetAttributeValue("Port", Port);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);
            if (xe.Attribute("ServerIp") != null)
            {
                this.ServerIp = xe.Attribute("ServerIp").Value;
            }
            if (xe.Attribute("Port") != null)
            {
                this.Port = int.Parse(xe.Attribute("Port").Value);
            }
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 
    /// </summary>
    public enum PortCheckType
    {
        None,
        Even,
        Odd,
        Mark,
        Space
    }

    /// <summary>
    /// 
    /// </summary>
    public enum PortStreamControlType
    {
        RTS,
        XON_XOFF,
        RTS_XON_XOFF
    }


    /// <summary>
    /// 
    /// </summary>
    public class PortChannelData : ChannelData
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 端口名称
        /// </summary>
        public string PortName { get; set; }
        /// <summary>
        /// 波特率
        /// </summary>
        public int BandRate { get; set; }

        /// <summary>
        /// 数据位
        /// </summary>
        public int DataSize { get; set; }

        /// <summary>
        /// 数据校验
        /// </summary>
        public PortCheckType Check { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Stop { get; set; }

        /// <summary>
        /// 使能流控制
        /// </summary>
        public bool EnableStreamControl { get; set; }

        /// <summary>
        /// 流控制类型
        /// </summary>
        public PortStreamControlType StreamControl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool EnableRTS { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool EnableDTR { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
            var re = base.SaveToXML();
            re.SetAttributeValue("PortName", PortName);
            re.SetAttributeValue("BandRate", BandRate);
            re.SetAttributeValue("DataSize", DataSize);
            re.SetAttributeValue("Stop", Stop);
            re.SetAttributeValue("Check", (int)Check);
            re.SetAttributeValue("EnableStreamControl", EnableStreamControl);
            re.SetAttributeValue("StreamControl", (int)StreamControl);
            re.SetAttributeValue("EnableRTS", EnableRTS);
            re.SetAttributeValue("EnableDTR", EnableDTR);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            if (xe.Attribute("PortName") != null)
            {
                this.PortName = xe.Attribute("PortName").Value;
            }
            if (xe.Attribute("BandRate") != null)
            {
                this.BandRate = int.Parse(xe.Attribute("BandRate").Value);
            }
            if (xe.Attribute("DataSize") != null)
            {
                this.DataSize = int.Parse(xe.Attribute("DataSize").Value);
            }
            if (xe.Attribute("Stop") != null)
            {
                this.Stop = int.Parse(xe.Attribute("Stop").Value);
            }
            if (xe.Attribute("Check") != null)
            {
                this.Check = (PortCheckType) int.Parse(xe.Attribute("Check").Value);
            }
            if (xe.Attribute("Stop") != null)
            {
                this.Stop = int.Parse(xe.Attribute("Stop").Value);
            }
            if (xe.Attribute("EnableStreamControl") != null)
            {
                this.EnableStreamControl = bool.Parse(xe.Attribute("EnableStreamControl").Value);
            }
            if (xe.Attribute("StreamControl") != null)
            {
                this.StreamControl = (PortStreamControlType) int.Parse(xe.Attribute("StreamControl").Value);
            }
            if (xe.Attribute("EnableRTS") != null)
            {
                this.EnableRTS = bool.Parse(xe.Attribute("EnableRTS").Value);
            }
            if (xe.Attribute("EnableDTR") != null)
            {
                this.EnableDTR = bool.Parse(xe.Attribute("EnableDTR").Value);
            }
            base.LoadFromXML(xe);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }


}
