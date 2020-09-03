//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/24 13:05:26.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace InSpiderDevelop.Device
{
    /// <summary>
    /// 
    /// </summary>
    public class DeviceGroup
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DeviceGroup Parent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FullName { get { return Parent == null ? Name : Parent.FullName + "." + Name; } }

        /// <summary>
        /// 
        /// </summary>
        public List<IDeviceDevelop> Devices { get; set; } = new List<IDeviceDevelop>();

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DeviceGroup Clone()
        {
            DeviceGroup dg = new DeviceGroup() { Name = Name };
            if (Devices != null)
            {
                foreach (var vv in Devices)
                {
                    dg.Devices.Add(vv.Clone());
                }
            }
            return dg;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
