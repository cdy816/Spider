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
        private Dictionary<string, Dictionary<string, MachineDocument>> mMachines = new Dictionary<string, Dictionary<string, MachineDocument>>();

        private bool mIsLoaded = false;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        public Dictionary<string, Dictionary<string, MachineDocument>> Machines
        { get { return mMachines; } }


        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public MachineDocument NewMachine(string solution,string name)
        {
            if(!mMachines.ContainsKey(solution))
            {
                var re = new MachineDocument { Name = name, Solution = solution };
                re.New();
                var dd = new Dictionary<string, MachineDocument>();
                dd.Add(name, re);
                mMachines.Add(solution, dd);
                return re;
            }
            else
            {
                if (!mMachines[solution].ContainsKey(name))
                {
                    var re = new MachineDocument { Name = name,Solution=solution };
                    re.New();
                    mMachines[solution].Add(name, re);
                    return re;
                }
                else
                {
                    return NewMachine(solution, GetAvaiableName(solution, name));
                }
            }
        }

        public string GetAvaiableName(string solution,string name)
        {
            if(mMachines.ContainsKey(solution))
            {
                var vnames = mMachines[solution].Keys.ToArray();
                string sname = name;
                if(!vnames.Contains(sname))
                {
                    return sname;
                }
                for(int i=1;i<int.MaxValue;i++)
                {
                    sname = name + i;
                    if (!vnames.Contains(sname))
                    {
                        return sname;
                    }
                }
                return name;
            }
            else
            {
                return name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="machine"></param>
        /// <returns></returns>
        public bool Add(MachineDocument machine)
        {
            if(mMachines.ContainsKey(machine.Solution))
            {
                if (!mMachines[machine.Solution].ContainsKey(machine.Name))
                mMachines[machine.Solution].Add(machine.Name, machine);
                return true;
            }
            else
            {
                var dd = new Dictionary<string,MachineDocument>();
                dd.Add(machine.Name, machine);
                mMachines.Add(machine.Solution, dd);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public bool ReName(string solution,string oldName,string newName)
        {
            if (mMachines.ContainsKey(solution))
            {
                var mm = mMachines[solution];
                if (mm.ContainsKey(oldName) && !mm.ContainsKey(newName))
                {
                    var vitem = mm[oldName];
                    mm.Remove(oldName);
                    vitem.ReName(newName);
                    mm.Add(newName, vitem);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Remove(string solution,string name)
        {
            if (mMachines.ContainsKey(solution) && mMachines[solution].ContainsKey(name))
            {
                var vfile = mMachines[solution][name];
                mMachines[solution].Remove(name);
                vfile.Remove();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> ListMachinesNames(string solution)
        {
            if (mMachines.ContainsKey(solution))
                return mMachines[solution].Keys.ToList();
            else
                return new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<MachineDocument> ListMachines(string solution)
        {
            if (mMachines.ContainsKey(solution))
                return mMachines[solution].Values.ToList();
            else
                return new List<MachineDocument>();
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
                    string sfile = System.IO.Path.Combine(vv.FullName, "Device.cfg");
                    if(System.IO.File.Exists(sfile))
                    {
                        if(mMachines.ContainsKey(""))
                        {
                            mMachines[""].Add(vv.Name, new MachineDocument() { Name = vv.Name }.Load());
                        }
                        else
                        {
                            mMachines.Add("", new Dictionary<string, MachineDocument>());
                            mMachines[""].Add(vv.Name, new MachineDocument() { Name = vv.Name }.Load());
                        }
                        
                    }
                    else
                    {
                        LoadSolution(vv.FullName);
                    }
                }
            }
            else
            {
                var local = new MachineDocument() { Name = "local" };
                local.New();
                Dictionary<string, MachineDocument> dd = new Dictionary<string, MachineDocument>();
                dd.Add("local", local);
                mMachines.Add("", dd);
            }
        }

        private void LoadSolution(string path)
        {
            var data = new System.IO.DirectoryInfo(path);
            if (data.Exists)
            {
                foreach (var vv in data.EnumerateDirectories())
                {
                    string sfile = System.IO.Path.Combine(vv.FullName, "Device.cfg");
                    if (System.IO.File.Exists(sfile))
                    {
                        if (mMachines.ContainsKey(data.Name))
                        {
                            mMachines[data.Name].Add(vv.Name, new MachineDocument() { Name = vv.Name,Solution=data.Name }.Load());
                        }
                        else
                        {
                            mMachines.Add(data.Name, new Dictionary<string, MachineDocument>());
                            mMachines[data.Name].Add(vv.Name, new MachineDocument() { Name = vv.Name, Solution = data.Name }.Load());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="machine"></param>
        public void ReLoadMachine(string solution,string machine)
        {
            if (mMachines.ContainsKey(solution) && mMachines[solution].ContainsKey(machine))
            {
                mMachines[solution][machine].Reload();
            }
            else
            {
                var dd = new Dictionary<string, MachineDocument>();
                
                var md = new MachineDocument() { Name = machine,Solution=solution };
                md.Load();
                dd.Add(machine, md);
                mMachines.Add(solution, dd);
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
        public bool UpdateWithString(string solution,string machine,string api, string channel, string device, string driver, string link)
        {
            try
            {
                if (mMachines.ContainsKey(solution) && mMachines[solution].ContainsKey(machine))
                {
                    return mMachines[solution][machine].UpdateWithString(api, channel, device, driver, link);
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
        public bool Save(string solution, string machine)
        {
            try
            {
                if (mMachines.ContainsKey(solution) && mMachines[solution].ContainsKey(machine))
                {
                    mMachines[solution][machine].Save();
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
                    foreach(var vvv in vv.Value)
                    vvv.Value.Save();
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
