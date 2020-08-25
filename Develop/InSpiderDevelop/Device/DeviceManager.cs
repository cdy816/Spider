//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/17 9:23:22.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider;
using InSpiderDevelop.Device;
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

        private Dictionary<string, DeviceGroup> mDeviceGroups = new Dictionary<string, DeviceGroup>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public bool IsDirty { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseName"></param>
        /// <returns></returns>
        public string GetAvaiableName(string baseName,string group="")
        {
            return GetDeviceNames(group).GetAvaiableName(baseName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public string GetAvaiableGroupName(string baseName, string group = "")
        {
            return GetDeviceGroupNames(group).GetAvaiableName(baseName);
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
        public List<IDeviceDevelop> ListAllDevices()
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
        /// <param name="group"></param>
        /// <returns></returns>
        public List<IDeviceDevelop> ListDevice(string group)
        {
            return mDevices.Values.Where(e => e.Group == group).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<DeviceGroup> ListDeviceGroup(string group)
        {
            return mDeviceGroups.Values.Where(e => e.Parent!=null?e.Parent.FullName== group:group=="").ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<string> GetDeviceNames(string group)
        {
            return mDevices.Values.Where(e => e.Group == group).Select(e=>e.Name).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<string> GetDeviceGroupNames(string group)
        {
            return mDeviceGroups.Values.Where(e => e.Parent != null ? e.Parent.FullName == group : group == "").Select(e=>e.Name).ToList();
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
            CheckAndAddGroup(Device.Group)?.Devices.Add(Device);
            if (!mDevices.ContainsKey(Device.FullName))
            {
                mDevices.Add(Device.FullName, Device);
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



        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        public void RemoveGroup(string group)
        {
            if (this.mDeviceGroups.ContainsKey(group))
            {
                var vv = this.mDeviceGroups[group].Devices;
                foreach (var vvv in vv)
                {
                    mDevices.Remove(vvv.FullName);
                }

                //获取改组的所有子组
                var gg = GetAllChildGroups(this.mDeviceGroups[group]);
                var ggnames = gg.Select(e => e.FullName);
                foreach (var vvg in gg)
                {
                    foreach (var vvv in vvg.Devices)
                    {
                        this.mDevices.Remove(vvv.FullName);
                    }
                    vvg.Devices.Clear();
                }

                this.mDeviceGroups.Remove(group);
                foreach (var vvn in ggnames)
                    this.mDeviceGroups.Remove(vvn);

                vv.Clear();
                IsDirty = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public List<DeviceGroup> GetAllChildGroups(DeviceGroup parent)
        {
            List<DeviceGroup> re = new List<DeviceGroup>();
            var grps = GetGroups(parent);
            re.AddRange(grps);
            foreach (var vv in grps)
            {
                re.AddRange(GetAllChildGroups(vv));
            }
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public List<DeviceGroup> GetGroups(DeviceGroup parent)
        {
            return this.mDeviceGroups.Values.Where(e => e.Parent == parent).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childGroupName"></param>
        /// <returns></returns>
        public bool HasChildGroup(DeviceGroup parent, string childGroupName)
        {
            var vss = this.mDeviceGroups.Values.Where(e => e.Parent == parent).Select(e => e.Name);
            if (vss.Count() > 0 && vss.Contains(childGroupName))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 更改设备所在的组
        /// </summary>
        /// <param name="device"></param>
        /// <param name="group"></param>
        public void ChangedDeviceGroup(IDeviceDevelop device, string group)
        {
            if (string.IsNullOrEmpty(device.Group))
            {
                device.Group = group;
                CheckAndAddGroup(device.Group)?.Devices.Add(device);
            }
            else
            {
                if (mDeviceGroups.ContainsKey(device.Group))
                {
                    var vv = mDeviceGroups[device.Group].Devices;
                    if (vv.Contains(device))
                    {
                        vv.Remove(device);
                        device.Group = group;
                        CheckAndAddGroup(device.Group)?.Devices.Add(device);
                    }
                }
            }
            IsDirty = true;
        }

        /// <summary>
        /// 检查并添加组
        /// </summary>
        /// <param name="groupName">组名称,多级组之间通过"."分割</param>
        public DeviceGroup CheckAndAddGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName)) return null;
            if (!mDeviceGroups.ContainsKey(groupName))
            {
                DeviceGroup parent = null;
                if (groupName.LastIndexOf(".") > 0)
                {
                    string sparentName = groupName.Substring(0, groupName.LastIndexOf("."));
                    parent = CheckAndAddGroup(sparentName);
                }

                string sname = groupName;
                if (parent != null)
                {
                    sname = groupName.Substring(parent.FullName.Length + 1);
                }

                DeviceGroup tg = new DeviceGroup() { Parent = parent, Name = sname };
                mDeviceGroups.Add(tg.FullName, tg);
                IsDirty = true;
                return tg;
            }
            else
            {
                return mDeviceGroups[groupName];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool AddDeviceGroup(DeviceGroup parent,DeviceGroup group)
        {
            group.Parent = parent;
            if(mDeviceGroups.ContainsKey(group.FullName))
            {
                mDeviceGroups.Add(group.FullName, group);
                return true;
            }
            else
            {
                group.Parent = null;
                return false;
            }
        }

        /// <summary>
        /// 改变变量组的父类
        /// </summary>
        /// <param name="group"></param>
        /// <param name="oldParentName"></param>
        /// <param name="newParentName"></param>
        public void ChangeGroupParent(string group, string oldParentName, string newParentName)
        {
            string oldgroupFullName = oldParentName + "." + group;
            if (mDeviceGroups.ContainsKey(oldgroupFullName))
            {
                var grp = mDeviceGroups[oldgroupFullName];
                //获取改组的所有子组
                var gg = GetAllChildGroups(grp);

                //从mDeviceGroups删除目标组以及所有子组
                var vnames = gg.Select(e => e.FullName).ToList();

                mDeviceGroups.Remove(oldgroupFullName);

                foreach (var vv in vnames)
                {
                    if (mDeviceGroups.ContainsKey(vv))
                    {
                        mDeviceGroups.Remove(vv);
                    }
                }

                if (mDeviceGroups.ContainsKey(newParentName))
                {
                    grp.Parent = mDeviceGroups[newParentName];
                }

                //修改子组变量对变量组的引用
                string fullname = string.Empty;
                foreach (var vv in gg)
                {
                    fullname = vv.FullName;
                    foreach (var vvt in vv.Devices)
                    {
                        vvt.Group = fullname;
                    }
                }

                //修改本组变量对变量组的引用
                fullname = grp.FullName;
                if (grp.Devices != null)
                {
                    foreach (var vv in grp.Devices)
                    {
                        vv.Group = fullname;
                    }
                }
                //将目标组以及所有子组添加至mDeviceGroups中
                mDeviceGroups.Add(fullname, grp);
                foreach (var vv in gg)
                {
                    mDeviceGroups.Add(vv.FullName, vv);
                }
                IsDirty = true;
            }
        }

        /// <summary>
        /// 变量组改名
        /// </summary>
        /// <param name="group"></param>
        /// <param name="newName"></param>
        public bool ChangeGroupName(string oldgroupFullName, string newName)
        {
            if (mDeviceGroups.ContainsKey(oldgroupFullName))
            {
                var grp = mDeviceGroups[oldgroupFullName];

                var ss = grp.Parent != null ? grp.Parent.FullName + "." + newName : newName;

                if (mDeviceGroups.ContainsKey(ss))
                {
                    return false;
                }

                //获取改组的所有子组
                var gg = GetAllChildGroups(grp);

                //从mDeviceGroups删除目标组以及所有子组

                var vnames = gg.Select(e => e.FullName).ToList();

                foreach (var vv in vnames)
                {
                    if (mDeviceGroups.ContainsKey(vv))
                    {
                        mDeviceGroups.Remove(vv);
                    }
                }

                mDeviceGroups.Remove(oldgroupFullName);

                

                grp.Name = newName;

                //修改子组变量对变量组的引用
                string fullname = string.Empty;
                foreach (var vv in gg)
                {
                    fullname = vv.FullName;
                    foreach (var vvt in vv.Devices)
                    {
                        vvt.Group = fullname;
                    }
                }

                //修改本组变量对变量组的引用
                fullname = grp.FullName;
                if (grp.Devices != null)
                {
                    foreach (var vv in grp.Devices)
                    {
                        vv.Group = fullname;
                    }
                }
                //将目标组以及所有子组添加至mDeviceGroups中
                mDeviceGroups.Add(fullname, grp);
                foreach (var vv in gg)
                {
                    mDeviceGroups.Add(vv.FullName, vv);
                }

                IsDirty = true;
                return true;
            }
            return false;
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
