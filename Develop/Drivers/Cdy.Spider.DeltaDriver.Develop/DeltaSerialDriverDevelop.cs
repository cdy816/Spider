//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2022/9/4 16:24:31.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider.DeltaDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class DeltaSerialDriverDevelop : DriverDevelop
    {

        #region ... Variables  ...

        private DeltaDriverData mData;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override DriverData Data { get => mData; set => mData = value as DeltaDriverData; }

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "DeltaSerial";

        /// <summary>
        /// 
        /// </summary>
        public override string[] SupportRegistors => null;

        private TagType mTagType;

        /// <summary>
        /// 
        /// </summary>
        public override string Desc => Res.Get("DeltaSerialDesc");

        #endregion ...Properties...

        #region ... Methods    ...



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IRegistorConfigModel RegistorConfig()
        {
            return new DeltaRegistorConfigModel() { TagType = mTagType };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverDevelop NewDriver()
        {
            return new DeltaSerialDriverDevelop() { Data = new DeltaDriverData() };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override DriverData CreatNewData()
        {
            return new DeltaDriverData();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override List<string> ListSupportChannels()
        {
            return new List<string>() { "TcpClient", "SerisePort" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Config()
        {
            return new DeltaSerialDriverDevelopViewModel() { Model = mData };
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
