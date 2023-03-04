//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/17 9:24:49.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Xml.Linq;

namespace InSpiderDevelop
{
    public class DriverDocument: IDriverDevelopManager
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        public static DriverDocument Manager = new DriverDocument();

        private Dictionary<string, IDriverDevelop> mDrivers = new Dictionary<string, IDriverDevelop>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        
        public string Name { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseName"></param>
        /// <returns></returns>
        public string GetAvaiableName(string baseName)
        {
            return mDrivers.Keys.GetAvaiableName(baseName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDriverDevelop NewDriver(string sname)
        {
            var vv = ServiceLocator.Locator.Resolve<IDriverDevelopForFactory>().NewDriver();
            if (string.IsNullOrEmpty(sname))
            {
                vv.Name = GetAvaiableName("Driver");
            }
            else
            {
                vv.Name = sname;
            }
            if (AddDriver(vv))
            {
                return vv;
            }
            return vv;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IDriverDevelop> ListDrivers()
        {
            return mDrivers.Values.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IDriverDevelop GetDriver(string name)
        {
            return mDrivers.ContainsKey(name) ? mDrivers[name] : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Driver"></param>
        /// <param name="newName"></param>
        public bool ReName(IDriverDevelop Driver, string newName)
        {
            if (mDrivers.ContainsKey(Driver.Name))
            {
                mDrivers.Remove(Driver.Name);
                Driver.Name = newName;
                mDrivers.Add(Driver.Name, Driver);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Driver"></param>
        /// <returns></returns>
        public bool AddDriver(IDriverDevelop Driver)
        {
            if (!mDrivers.ContainsKey(Driver.Name))
            {
                mDrivers.Add(Driver.Name, Driver);
                return true;
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void RemoveDriver(string name)
        {
            if (mDrivers.ContainsKey(name))
            {
                mDrivers.Remove(name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load(Context context)
        {
            string sfile = string.IsNullOrEmpty(context.Solution)? System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", Name, "Driver.cfg") : System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data",context.Solution, Name, "Driver.cfg");
            Load(sfile,context);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reload(Context context)
        {
            this.mDrivers.Clear();
            Load(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sfile"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public DriverDocument LoadFromString(string sfile, Context context)
        {
            if (!string.IsNullOrEmpty(sfile))
            {
                string sname = System.IO.Path.GetTempFileName();
                System.IO.File.WriteAllText(sname, sfile);
                XElement xx = XElement.Load(sname);
                foreach (var vv in xx.Elements())
                {
                    string tname = vv.Attribute("TypeName").Value;
                    var asb = ServiceLocator.Locator.Resolve<IDriverFactory>().GetDevelopInstance(tname);
                    if (asb != null)
                    {
                        asb.Load(vv);
                        AddDriver(asb);
                    }
                }
                context.Add(typeof(IDriverDevelopManager), this);
                System.IO.File.Delete(sname);
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sfile"></param>
        public void Load(string sfile, Context context)
        {
            if (System.IO.File.Exists(sfile))
            {
                XElement xx = XElement.Load(sfile);

                foreach (var vv in xx.Elements())
                {
                    string tname = vv.Attribute("TypeName").Value;
                    var asb = ServiceLocator.Locator.Resolve<IDriverFactory>().GetDevelopInstance(tname);
                    if (asb != null)
                    {
                        asb.Load(vv);
                        AddDriver(asb);
                    }
                }
                context.Add(typeof(IDriverDevelopManager), this);

                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveToSolution(string solution)
        {
            string sfile = string.IsNullOrEmpty(solution)? System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", Name, "Driver.cfg") : System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data",solution, Name, "Driver.cfg");
            CheckDirExistOrCreat(sfile);
            Save(sfile);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveTo(string dir)
        {
            string sfile = System.IO.Path.Combine(dir, "Driver.cfg");
            CheckDirExistOrCreat(sfile);
            Save(sfile);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public void SaveWithString(string content, string solution)
        {
            string sfile = string.IsNullOrEmpty(solution) ? System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", Name, "Driver.cfg") : System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", solution, Name, "Driver.cfg");
            CheckDirExistOrCreat(sfile);
            System.IO.File.WriteAllText(sfile, content);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string SaveToString()
        {
            System.IO.MemoryStream sb = new System.IO.MemoryStream();
            XElement xx = new XElement("Drivers");
            foreach (var vv in mDrivers)
            {
                xx.Add(vv.Value.Save());
            }
            xx.Save(sb);
            return Encoding.UTF8.GetString(sb.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sfile"></param>
        private void CheckDirExistOrCreat(string sfile)
        {
            string sdir = System.IO.Path.GetDirectoryName(sfile);
            if(!System.IO.Directory.Exists(sdir))
            {
                System.IO.Directory.CreateDirectory(sdir);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sfile"></param>
        public void Save(string sfile)
        {
            sfile.BackFile();
            XElement xx = new XElement("Drivers");
            foreach (var vv in mDrivers)
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
