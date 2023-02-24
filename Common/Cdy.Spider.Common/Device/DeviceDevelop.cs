//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/17 15:12:03.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public class DeviceDevelop : IDeviceDevelop
    {

        #region ... Variables  ...
        private ICommChannelDevelop mCommChannel;
        private IDriverDevelopManager mDriverManager;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...


        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return Data.Name;
            }
            set
            {
                Data.Name = value;
                UpdateDriverName();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DeviceData Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FullName => string.IsNullOrEmpty(Group) ? Name : Group + "." + Name;

        /// <summary>
        /// 
        /// </summary>
        public string Group { get { return Data.Group; } set { Data.Group = value; UpdateDriverName(); } }

        /// <summary>
        /// 
        /// </summary>
        public ICommChannelDevelop Channel 
        {
            get 
            {
                return mCommChannel;
            }
            set
            {
                mCommChannel =value;
                if (value != null)
                {
                    Data.ChannelName = value.Name;
                }
                else
                {
                    Data.ChannelName = string.Empty;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDriverDevelop Driver 
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDirty { get; set; } = false;


        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void UpdateDriverName()
        {
            //if (this.Driver != null)
            //    this.Driver.Name = FullName;
            if (this.Driver != null)
            {
                mDriverManager?.ReName(this.Driver, FullName);
                IsDirty = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Config()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        /// <param name="context"></param>
        public void Load(XElement xe, Context context)
        {
            this.Data = new DeviceData();
            this.Data.LoadFromXML(xe);
            if (!string.IsNullOrEmpty(this.Data.ChannelName))
            {
                this.mCommChannel = context.Get<ICommChannelDevelopManager>().GetChannel(this.Data.ChannelName);
            }
            if (!string.IsNullOrEmpty(this.Name))
            {
                this.Driver = context.Get<IDriverDevelopManager>().GetDriver(this.Name);
            }
            mDriverManager = context.Get<IDriverDevelopManager>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement Save()
        {
            IsDirty = false;
            return this.Data.SaveToXML();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IDeviceDevelop Clone()
        {
            return new DeviceDevelop() { Data = this.Data.Clone(), Driver = Driver != null ? Driver.Clone() : null };
        }



        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...


    }
}
