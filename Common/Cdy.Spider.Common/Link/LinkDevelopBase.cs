//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/17 9:45:00.
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
    public abstract class LinkDevelopBase : ILinkDevelop,ILinkDevelopForFactory
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
        public string Name { get { return Data.Name; } set { Data.Name = value; } }

        /// <summary>
        /// 
        /// </summary>
        public abstract LinkData Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public abstract string TypeName { get; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 通过界面配置
        /// </summary>
        public virtual object Config()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual LinkData CreatNewData()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public void Load(XElement xe)
        {
            Data = CreatNewData();
            Data.LoadFromXML(xe);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement Save()
        {
            var vv = Data.SaveToXML();
            vv.SetAttributeValue("TypeName", TypeName);
            return vv;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract ILinkDevelop NewApi();


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...




    }
}
