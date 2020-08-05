//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/5 13:42:22.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider
{
    public class DeviceData
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

        /// <summary>
        /// 变量的集合
        /// </summary>
        public TagCollection Tags { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DriverName { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public void LoadFromXML(XElement xe)
        {
            this.Name = xe.Attribute("Name")?.Value;
            this.DriverName = xe.Attribute("DriverName")?.Value;
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
            xe.SetAttributeValue("Name", Name);
            xe.SetAttributeValue("DriverName", DriverName);
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
