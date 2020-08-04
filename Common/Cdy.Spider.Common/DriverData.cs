﻿//==============================================================
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
        /// 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 变量的集合
        /// </summary>
        public TagCollection Tags { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public void LoadFromXML(XElement xe)
        {
            this.Type = xe.Attribute("Type")?.Value;
            this.Name = xe.Attribute("Name")?.Value;
            this.ChannelName = xe.Attribute("ChannelName")?.Value;
            Tags = new TagCollection();
            foreach (var vv in xe.Elements())
            {
                Tags.AddTag(vv.CreatFromXML());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public virtual XElement SaveToXML()
        {
            XElement xe = new XElement("Device");
            xe.SetAttributeValue("Type", Type);
            xe.SetAttributeValue("Name", Name);
            xe.SetAttributeValue("ChannelName", ChannelName);
            XElement xx = new XElement("Tags");
            if (Tags != null)
            {
                foreach (var vv in Tags)
                {
                    xx.Add(vv.Value.SaveToXML());
                }
            }
            xe.Add(xx);
            return xe;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
