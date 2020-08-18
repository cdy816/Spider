//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/17 9:23:22.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace InSpiderDevelop
{
    public class DeviceManager
    {

        #region ... Variables  ...

      
        /// <summary>
        /// 
        /// </summary>
        public static DeviceManager Manager = new DeviceManager();

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, IDeviceDevelop> mDevices = new Dictionary<string, IDeviceDevelop>();

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
        /// <param name="baseName"></param>
        /// <returns></returns>
        public string GetAvaiableName(string baseName)
        {
            return mDevices.Keys.GetAvaiableName(baseName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDeviceDevelop NewDevice(string sname)
        {
            var vv = new DeviceDevelop();
            if (string.IsNullOrEmpty(sname))
            {
                vv.Name = GetAvaiableName("Device");
            }
            else
            {
                vv.Name = sname;
            }
            if (AddDevice(vv))
            {
                return vv;
            }
            return vv;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IDeviceDevelop> ListDevices()
        {
            return mDevices.Values.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IDeviceDevelop GetDevice(string name)
        {
            return mDevices.ContainsKey(name) ? mDevices[name] : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Device"></param>
        /// <param name="newName"></param>
        public bool ReName(IDeviceDevelop Device, string newName)
        {
            if (mDevices.ContainsKey(Device.Name))
            {
                mDevices.Remove(Device.Name);
                Device.Name = newName;
                mDevices.Add(Device.Name, Device);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Device"></param>
        /// <returns></returns>
        public bool AddDevice(IDeviceDevelop Device)
        {
            if (!mDevices.ContainsKey(Device.Name))
            {
                mDevices.Add(Device.Name, Device);
                return true;
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void RemoveDevice(string name)
        {
            if (mDevices.ContainsKey(name))
            {
                mDevices.Remove(name);
            }
        }

        public void Reload()
        {
            this.mDevices.Clear();
            Load();
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
            if (System.IO.File.Exists(sfile))
            {
                XElement xx = XElement.Load(sfile);
                foreach (var vv in xx.Elements())
                {
                    DeviceDevelop asb = new DeviceDevelop();
                    asb.Load(vv);
                    AddDevice(asb);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", "Device.cfg");
            Save(sfile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sfile"></param>
        public void Save(string sfile)
        {
            sfile.BackFile();
            XElement xx = new XElement("Devices");
            foreach (var vv in mDevices)
            {
                xx.Add(vv.Value.Save());
            }
            xx.Save(sfile);
        }


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
