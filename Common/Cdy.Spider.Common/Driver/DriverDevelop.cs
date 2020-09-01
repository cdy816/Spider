//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/17 15:21:59.
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
    public abstract class DriverDevelop : IDriverDevelop, IDriverDevelopForFactory
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
        public string Name { get => Data.Name; set => Data.Name = value; }

        /// <summary>
        /// 
        /// </summary>
        public abstract DriverData Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public abstract string TypeName { get; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract object Config();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract DriverData CreatNewData();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public void Load(XElement xe)
        {
            this.Data = CreatNewData();
            this.Data.LoadFromXML(xe);
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
        public abstract IDriverDevelop NewDriver();



        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...







    }
}
