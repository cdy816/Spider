//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/18 10:33:14.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace InSpiderDevelop
{
    /// <summary>
    /// 
    /// </summary>
    public class DevelopManager
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        public static DevelopManager Manager = new DevelopManager();
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            APIManager.Manager.Load();
            ChannelManager.Manager.Load();
            DeviceManager.Manager.Load();
            DriverManager.Manager.Load();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reload()
        {
            APIManager.Manager.Reload();
            ChannelManager.Manager.Reload();
            DeviceManager.Manager.Reload();
            DriverManager.Manager.Reload();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            APIManager.Manager.Save();
            ChannelManager.Manager.Save();
            DeviceManager.Manager.Save();
            DriverManager.Manager.Save();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
