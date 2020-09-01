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

    /// <summary>
    /// 工作方式
    /// </summary>
    public enum WorkMode
    {
        /// <summary>
        /// 主动
        /// </summary>
        Active,
        /// <summary>
        /// 被动
        /// </summary>
        Passivity
    }


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
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工作模式
        /// </summary>
        public WorkMode Model { get; set; }

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
            this.Name = xe.Attribute("Name")?.Value;
            this.ScanCircle = int.Parse(xe.Attribute("ScanCircle")?.Value);
            this.Model = (WorkMode)int.Parse(xe.Attribute("Model")?.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public virtual XElement SaveToXML()
        {
            XElement xe = new XElement("Device");
            xe.SetAttributeValue("Name", Name);
            xe.SetAttributeValue("Model", (int)Model);
            xe.SetAttributeValue("ScanCircle", ScanCircle);
            return xe;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
