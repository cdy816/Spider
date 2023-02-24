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
using System.Linq;
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

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, MachineDocument> mMachines = new Dictionary<string, MachineDocument>();

        private bool mIsLoaded = false;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        public Dictionary<string, MachineDocument> Machines
        { get { return mMachines; } }


        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public MachineDocument NewMachine(string name)
        {
            if(!mMachines.ContainsKey(name))
            {
                var re = new MachineDocument { Name = name };
                re.New();
                mMachines.Add(name, re);
                return re;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="machine"></param>
        /// <returns></returns>
        public bool Add(MachineDocument machine)
        {
            if(!mMachines.ContainsKey(machine.Name))
            {
                mMachines.Add(machine.Name, machine);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public bool ReName(string oldName,string newName)
        {
            if(mMachines.ContainsKey(oldName) && !mMachines.ContainsKey(newName))
            {
                var vitem = mMachines[oldName];
                mMachines.Remove(oldName);
                vitem.ReName(newName);
                mMachines.Add(newName, vitem);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Remove(string name)
        {
            if(mMachines.ContainsKey(name))
            {
                var vfile = mMachines[name];
                mMachines.Remove(name);
                vfile.Remove();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> ListMachinesNames()
        {
            return mMachines.Keys.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<MachineDocument> ListMachines()
        {
            return mMachines.Values.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            mMachines.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReLoad()
        {
            mIsLoaded = false;
            mMachines.Clear();
            Load();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load(string sdata="")
        {
            if (mIsLoaded) return;
            mIsLoaded = true;

            var data = new System.IO.DirectoryInfo(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data"));
            if(!string.IsNullOrEmpty(sdata))
            {
                data = new System.IO.DirectoryInfo(sdata);
            }
            if (data.Exists)
            {
                foreach (var vv in data.EnumerateDirectories())
                {
                    mMachines.Add(vv.Name, new MachineDocument() { Name = vv.Name }.Load());
                }
            }
            else
            {
                var local = new MachineDocument() { Name = "local" };
                local.New();
                mMachines.Add("local", local);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="machine"></param>
        public void ReLoadMachine(string machine)
        {
            if (mMachines.ContainsKey(machine))
            {
                mMachines[machine].Reload();
            }
            else
            {
                var md = new MachineDocument() { Name = machine };
                md.Load();
                mMachines.Add(machine, md);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="api"></param>
        /// <param name="channel"></param>
        /// <param name="device"></param>
        /// <param name="driver"></param>
        /// <param name="link"></param>
        public bool UpdateWithString(string machine,string api, string channel, string device, string driver, string link)
        {
            try
            {
                if (mMachines.ContainsKey(machine))
                {
                    return mMachines[machine].UpdateWithString(api, channel, device, driver, link);
                }
            }
            catch
            {

            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="machine"></param>
        public bool Save(string machine)
        {
            try
            {
                if (mMachines.ContainsKey(machine))
                {
                    mMachines[machine].Save();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Save()
        {
            try
            {
                foreach (var vv in mMachines)
                {
                    vv.Value.Save();
                }
                return true;
                    
            }
            catch
            {
                return false;
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
