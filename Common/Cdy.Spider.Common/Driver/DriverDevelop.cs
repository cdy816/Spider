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

        /// <summary>
        /// 
        /// </summary>
        public virtual string Desc { get; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public abstract ChannelType[] SupportChannelTypes { get; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string[] SupportRegistors => null;

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
        public abstract IRegistorConfigModel RegistorConfig();

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
            var re = this.Data.SaveToXML();
            re.SetAttributeValue("TypeName", this.TypeName);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract IDriverDevelop NewDriver();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDriverDevelop Clone()
        {
            var dd = NewDriver();
            var xx = dd.Save();
            dd.Load(xx);
            return dd;
        }


        /// <summary>
        /// 校验变量的设备信息
        /// </summary>
        /// <param name="tag"></param>
        public virtual void CheckTagDeviceInfo(Tagbase tag)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract List<string> ListSupportChannels();
        

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
