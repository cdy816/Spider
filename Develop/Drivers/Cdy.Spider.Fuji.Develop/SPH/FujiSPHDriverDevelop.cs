//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2022/9/4 16:24:31.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider.FujiDriver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider.Fuji.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class FujiSPHDriverDevelop : DriverDevelop
    {

        #region ... Variables  ...

        private FujiSPHDriverData mData;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override DriverData Data { get => mData; set => mData = value as FujiSPHDriverData; }

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "FujiSPH";

        /// <summary>
        /// 
        /// </summary>
        public override string[] SupportRegistors => null;

        private TagType mTagType;

        /// <summary>
        /// 
        /// </summary>
        public override string Desc => Res.Get("FujiSPHDesc");

        #endregion ...Properties...

        #region ... Methods    ...



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IRegistorConfigModel RegistorConfig()
        {
            return new FujiSPHRegistorConfigModel() { TagType = mTagType };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverDevelop NewDriver()
        {
            return new FujiSPHDriverDevelop() { Data = new FujiSPHDriverData() };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override DriverData CreatNewData()
        {
            return new FujiSPHDriverData();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override List<string> ListSupportChannels()
        {
            return new List<string>() { "TcpClient" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Config()
        {
            return new FujiSPHDriverDevelopViewModel() { Model = mData };
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
