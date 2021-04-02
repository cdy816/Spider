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

namespace Cdy.Spider.ModbusDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class ModbusDriverDevelop : DriverDevelop
    {

        #region ... Variables  ...
        
        private ModbusDriverData mData;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        
        /// <summary>
        /// 
        /// </summary>
        public override DriverData Data { get => mData; set => mData = value as ModbusDriverData; }

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "ModbusDriver";

        ///// <summary>
        ///// 
        ///// </summary>
        //public override ChannelType[] SupportChannelTypes => null;

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
             return new ModbusDriverDevelopViewModel() { Model = mData };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IRegistorConfigModel RegistorConfig()
        {
             return new ModbusRegistorConfigModel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverDevelop NewDriver()
        {
            return new ModbusDriverDevelop() { Data = new ModbusDriverData() };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override DriverData CreatNewData()
        {
            return new ModbusDriverData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public override void CheckTagDeviceInfo(Tagbase tag)
        {
            tag.DeviceInfo = tag.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override List<string> ListSupportChannels()
        {
            return new List<string>() { "TcpClient", "UdpClient", "SerisePort" };
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
