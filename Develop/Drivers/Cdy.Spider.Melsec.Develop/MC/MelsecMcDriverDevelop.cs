//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2022/9/4 16:24:31.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider.MelsecDriver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider.Melsec.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class MelsecMcDriverDevelop : DriverDevelop
    {

        #region ... Variables  ...

        private MelsecMcDriverData mData;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override DriverData Data { get => mData; set => mData = value as MelsecMcDriverData; }

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "MelsecMc";

        /// <summary>
        /// 
        /// </summary>
        public override string[] SupportRegistors => null;

        private TagType mTagType;

        /// <summary>
        /// 
        /// </summary>
        public override string Desc => Res.Get("MelsecMcDesc");

        #endregion ...Properties...

        #region ... Methods    ...



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IRegistorConfigModel RegistorConfig()
        {
            return new MelsecMcRegistorConfigModel() { TagType = mTagType };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverDevelop NewDriver()
        {
            return new MelsecMcDriverDevelop() { Data = new MelsecMcDriverData() };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override DriverData CreatNewData()
        {
            return new MelsecMcDriverData();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override List<string> ListSupportChannels()
        {
            return new List<string>() { "TcpClient", "UdpClient", "SerisePort" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Config()
        {
            return new MelsecMcDriverDevelopViewModel() { Model = mData };
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
