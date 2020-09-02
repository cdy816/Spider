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
    public class DeviceData:IDisposable
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
        /// 
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 变量的集合
        /// </summary>
        public TagCollection Tags { get; set; } = new TagCollection();

        /// <summary>
        /// 
        /// </summary>
        public string ChannelName { get; set; } = "";

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public bool AddTag(Tagbae tag)
        {
            return Tags.AddTag(tag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool UpdateOrAdd(Tagbae tag)
        {
            return Tags.UpdateOrAdd(tag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public bool AppendTag(Tagbae tag)
        {
            return Tags.AppendTag(tag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public bool RemoveTag(int id)
        {
            return Tags.RemoveTagById(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public bool RemoveTag(Tagbae tag)
        {
            return Tags.RemoveTag(tag);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public void LoadFromXML(XElement xe)
        {
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

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Tags.Clear();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
