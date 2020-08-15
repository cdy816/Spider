//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/4 14:50:35.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider
{
    public abstract class DriverData
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 类型
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 通道名称
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 扫描周期
        /// 单位毫秒
        /// </summary>
        public int ScanCircle { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public void LoadFromXML(XElement xe)
        {
            this.Assembly = xe.Attribute("Assembly")?.Value;
            this.Class = xe.Attribute("Class")?.Value;
            this.Name = xe.Attribute("Name")?.Value;
            this.ChannelName = xe.Attribute("ChannelName")?.Value;
            this.ScanCircle = int.Parse(xe.Attribute("ScanCircle")?.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public virtual XElement SaveToXML()
        {
            XElement xe = new XElement("Device");
            xe.SetAttributeValue("Assembly", Assembly);
            xe.SetAttributeValue("Class", Class);
            xe.SetAttributeValue("Name", Name);
            xe.SetAttributeValue("ChannelName", ChannelName);
            xe.SetAttributeValue("ScanCircle", ScanCircle);
            return xe;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
