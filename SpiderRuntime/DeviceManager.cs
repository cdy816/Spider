//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/10 8:54:19.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace SpiderRuntime
{
    public class DeviceManager
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        public static DeviceManager Manager = new DeviceManager();

        private Dictionary<string, IDeviceRuntime> mDevices = new Dictionary<string, IDeviceRuntime>();

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
        public IEnumerable<IDeviceRuntime> Devices
        {
            get
            {
                return mDevices.Values;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IDeviceRuntime GetDevice(string key)
        {
            if(mDevices.ContainsKey(key))
            {
                return mDevices[key];
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        private void AddDevice(IDeviceRuntime device)
        {
            if(!mDevices.ContainsKey(device.Name))
            {
                mDevices.Add(device.Name, device);
            }
            else
            {
                mDevices[device.Name] = device;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", "Device.cfg");
            Load(sfile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sfile"></param>
        public void Load(string sfile)
        {
            if(System.IO.File.Exists(sfile))
            {
                XElement xx = XElement.Load(sfile);
                foreach(var vv in xx.Elements())
                {
                    DeviceData data = new DeviceData();
                    data.LoadFromXML(vv);
                    DeviceRunner runner = new DeviceRunner() { Device = data };
                    AddDevice(runner);
                }
            }
        }



        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
