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
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SpiderRuntime
{
    public class DeviceManager: IDeviceRuntimeManager, IRealDataService
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        public static DeviceManager Manager = new DeviceManager();

        /// <summary>
        /// 
        /// </summary>
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
        public string Name { get; set; }


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
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CheckExist(string name)
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(typeof(DeviceManager).Assembly.Location), "Data", name, "Device.cfg");
            return System.IO.File.Exists(sfile);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", Name, "Device.cfg");
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
                
                foreach (var vv in xx.Element("Devices").Elements())
                {
                    DeviceData data = new DeviceData();
                    data.LoadFromXML(vv);
                    data.Name =  string.IsNullOrEmpty(data.Group) ?  data.Name :  data.Group + "." + data.Name;
                    DeviceRunner runner = new DeviceRunner() { Device = data };
                    AddDevice(runner);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IDeviceRuntime> ListDevice()
        {
            return mDevices.Values.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public IEnumerable<string> ListTags(string device)
        {
            if(mDevices.ContainsKey(device))
            {
                mDevices[device].ListTags().Select(e => e.Name);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public Dictionary<string, Tuple<object, byte>> GetDeviceALLTagValues(string device)
        {
            Dictionary<string, Tuple<object, byte>> re = new Dictionary<string, Tuple<object, byte>>();
            if(mDevices.ContainsKey(device))
            {
                var vtags = mDevices[device].ListTags();
                if(vtags!=null)
                {
                    foreach(var vv in vtags)
                    {
                        re.Add(vv.Name, new Tuple<object, byte>(vv.Value, vv.Quality));
                    }
                }
            }
            return re;
        }

        public Dictionary<string, Tuple<object, byte>> GetTagValues(string device, IEnumerable<string> tags)
        {
            Dictionary<string, Tuple<object, byte>> re = new Dictionary<string, Tuple<object, byte>>();
            if (mDevices.ContainsKey(device))
            {
                var vtags = mDevices[device].ListTags();
                if (vtags != null)
                {
                    foreach (var vv in vtags)
                    {
                        re.Add(vv.Name, new Tuple<object, byte>(vv.Value, vv.Quality));
                    }
                }
            }
            return re;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
