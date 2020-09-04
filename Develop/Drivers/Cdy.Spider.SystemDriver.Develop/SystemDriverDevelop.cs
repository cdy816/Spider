//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/9/1 16:24:31.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider.SystemDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemDriverDevelop : DriverDevelop
    {

        #region ... Variables  ...
        
        private SystemDriverData mData;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        
        /// <summary>
        /// 
        /// </summary>
        public override DriverData Data { get => mData; set => mData = value as SystemDriverData; }

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "SystemDriver";

        /// <summary>
        /// 
        /// </summary>
        public override ChannelType[] SupportChannelTypes => null;

        /// <summary>
        /// 
        /// </summary>
        public override string[] SupportRegistors => null;

        #endregion ...Properties...

        #region ... Methods    ...


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Config()
        {
            return new SystemDriverDevelopViewModel() { Model = mData };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverDevelop NewDriver()
        {
            return new SystemDriverDevelop() { Data = new SystemDriverData() };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override DriverData CreatNewData()
        {
            return new SystemDriverData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public override void CheckTagDeviceInfo(Tagbae tag)
        {
            tag.DeviceInfo = tag.Name;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
