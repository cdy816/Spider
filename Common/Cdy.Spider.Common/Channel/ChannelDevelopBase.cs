//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/17 14:56:06.
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
    public abstract class ChannelDevelopBase : ICommChannelDevelop, ICommChannelDevelopForFactory
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
        public string Name 
        {
            get { return Data.Name; } 
            set { Data.Name = value; }
        }


        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

        /// <summary>
        /// 
        /// </summary>
        public abstract ChannelData Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public abstract string TypeName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract ChannelData CreatNewData();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract object Config();
       


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public void Load(XElement xe)
        {
            var data = CreatNewData();
            data.LoadFromXML(xe);
            Data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement Save()
        {
            return this.Data.SaveToXML();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract ICommChannelDevelop NewChannel();
    }
}
