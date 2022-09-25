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

namespace Cdy.Spider.IEC60870Driver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class IEC60870_104DriverDevelop : DriverDevelop
    {

        #region ... Variables  ...
        
        private IEC60870_104_DriverData mData;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        
        /// <summary>
        /// 
        /// </summary>
        public override DriverData Data { get => mData; set => mData = value as IEC60870_104_DriverData; }

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "IEC60870_104";

        ///// <summary>
        ///// 
        ///// </summary>
        //public override ChannelType[] SupportChannelTypes => null;

        /// <summary>
        /// 
        /// </summary>
        public override string[] SupportRegistors => null;

        /// <summary>
        /// 
        /// </summary>
        public override string Desc => Res.Get("IEC60870_104_Desc");

        #endregion ...Properties...

        #region ... Methods    ...


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Config()
        {
            return new IEC60870_104DriverDevelopViewModel() { Model = mData };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IRegistorConfigModel RegistorConfig()
        {
            return new IEC60870_104DriverRegistorConfigModel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverDevelop NewDriver()
        {
            return new IEC60870_104DriverDevelop() { Data = new IEC60870_104_DriverData() };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override DriverData CreatNewData()
        {
            return new IEC60870_104_DriverData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public override void CheckTagDeviceInfo(Tagbase tag)
        {
            //tag.DeviceInfo = tag.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override List<string> ListSupportChannels()
        {
            return new List<string>() { "TcpClient" };
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
