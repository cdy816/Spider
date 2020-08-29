//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/28 15:54:06.
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
    public class MachineDocument
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
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public APIDocument Api { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ChannelDocument Channel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DeviceDocument Device { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DriverDocument Driver { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            Api = new APIDocument() { Name = Name };
            Channel = new ChannelDocument() { Name = Name };
            Device = new DeviceDocument() { Name = Name };
            Driver = new DriverDocument() { Name = Name };
            Api.Load();
            Channel.Load();
            Device.Load();
            Driver.Load();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reload()
        {
            Api.Reload();
            Channel.Reload();
            Device.Reload();
            Driver.Reload();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", Name);
            if(!System.IO.Directory.Exists(sfile))
            {
                System.IO.Directory.CreateDirectory(sfile);
            }
            Api.Save();
            Channel.Save();
            Device.Save();
            Driver.Save();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
