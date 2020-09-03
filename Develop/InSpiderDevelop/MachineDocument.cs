//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/28 15:54:06.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider;
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

        private bool mIsLoad = false;

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
        /// <param name="name"></param>
        public void ReName(string name)
        {
            this.Name = name;
            Api.Name = name;
            Channel.Name = name;
            Device.Name = name;
            Driver.Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        public void New()
        {
            Api = new APIDocument() { Name = Name };
            Api.New();
            Channel = new ChannelDocument() { Name = Name };
            Device = new DeviceDocument() { Name = Name };
            Driver = new DriverDocument() { Name = Name };
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            if (mIsLoad) return;
            mIsLoad = true;
            using (Context context = new Context())
            {
                Api = new APIDocument() { Name = Name };
                Channel = new ChannelDocument() { Name = Name };
                Device = new DeviceDocument() { Name = Name };
                Driver = new DriverDocument() { Name = Name };

                Api.Load();
                Channel.Load(context);
                Driver.Load(context);
                Device.Load(context);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reload()
        {
            using (Context context = new Context())
            {
                Api.Reload();
                Channel.Reload(context);
                Driver.Reload(context);
                Device.Reload(context);
            }
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
            Driver.Save();
            Device.Save();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
