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
using System.Xml.Linq;

namespace InSpiderDevelop
{
    public class DriverManager
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        public static DriverManager Manager = new DriverManager();

        private Dictionary<string, IDriverDevelop> mDrivers = new Dictionary<string, IDriverDevelop>();

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
        public void Load()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", "Driver.cfg");
            Load(sfile);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reload()
        {
            this.mDrivers.Clear();
            Load();
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
                    string tname = vv.Attribute("TypeName").Value;
                    var asb = ServiceLocator.Locator.Resolve<IDriverFactory>().GetDevelopIntance(tname);
                    asb.Load(vv);
                    AddDriver(asb);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", "Driver.cfg");
            Save(sfile);
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
