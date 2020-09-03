//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/17 9:35:26.
//  Version 1.0
//  种道洋
//==============================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider
{
    public interface IDriverDevelop
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
        string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        DriverData Data { get; set; }

        /// <summary>
        /// Protocol 类型
        /// </summary>
        string TypeName { get; }

        /// <summary>
        /// 支持的通道类型
        /// </summary>
        ChannelType[] SupportChannelTypes { get; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 校验变量的设备信息
        /// </summary>
        /// <param name="tag"></param>
        void CheckTagDeviceInfo(Tagbae tag);

        object Config();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        XElement Save();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        void Load(XElement xe);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDriverDevelop Clone();

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    public interface IDriverDevelopForFactory
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
        string TypeName { get; }
        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDriverDevelop NewDriver();

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
