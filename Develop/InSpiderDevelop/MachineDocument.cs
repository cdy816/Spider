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
using System.Diagnostics;
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

        /// <summary>
        /// 
        /// </summary>
        public LinkDocument Link { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public bool IsDirty { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void ReName(string name)
        {
            string sname = this.Name;
            this.Name = name;
            Api.Name = name;
            Channel.Name = name;
            Device.Name = name;
            Driver.Name = name;
            Link.Name = name;

            var sfile1 = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", sname);
            var sfile2 = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", name);
            if (sfile1 != sfile2 && System.IO.Directory.Exists(sfile1))
            {
                System.IO.Directory.Move(sfile1, sfile2);
            }

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
            Link = new LinkDocument();
            Link.New();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="api"></param>
        /// <param name="channel"></param>
        /// <param name="device"></param>
        /// <param name="driver"></param>
        /// <param name="link"></param>
        public bool UpdateWithString(string api,string channel,string device,string driver,string link)
        {
            try
            {
                Api.SaveWithString(api);
                Channel.SaveWithString(channel);
                Device.SaveWithString(device);
                Link.SaveWithString(link);
                Driver.SaveWithString(driver);
                Reload();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public MachineDocument Load()
        {
            if (mIsLoad) return this;
            mIsLoad = true;
            using (Context context = new Context())
            {
                Api = new APIDocument() { Name = Name };
                Channel = new ChannelDocument() { Name = Name };
                Device = new DeviceDocument() { Name = Name };
                Driver = new DriverDocument() { Name = Name };
                Link = new LinkDocument() { Name = Name };
                Api.Load();
                Channel.Load(context);
                Driver.Load(context);
                Device.Load(context);
                Link.Load();
            }
            return this;
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
                Link.Reload();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Remove()
        {
            var sfile1 = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", this.Name);
            if (System.IO.Directory.Exists(sfile1))
            {
                System.IO.Directory.Delete(sfile1,true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", Name);
            if (!System.IO.Directory.Exists(sfile))
            {
                System.IO.Directory.CreateDirectory(sfile);
            }
            if (Api != null)
                Api.Save();
            if (Channel != null)
                Channel.Save();
            if (Driver != null)
                Driver.Save();
            if (Device != null)
                Device.Save();
            if (Link != null)
                Link.Save();
            IsDirty = false;
        }

        public void Save(string targetDir)
        {
            string sfile = System.IO.Path.Combine(targetDir);
            if (!System.IO.Directory.Exists(sfile))
            {
                System.IO.Directory.CreateDirectory(sfile);
            }
            if (Api != null)
                Api.SaveTo(sfile);
            if (Channel != null)
                Channel.SaveTo(sfile);
            if (Driver != null)
                Driver.SaveTo(sfile);
            if (Device != null)
                Device.SaveTo(sfile);
            if (Link != null)
                Link.SaveTo(sfile);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
