//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/4 14:30:39.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace SpiderRuntime
{
    /// <summary>
    /// 
    /// </summary>
    public class DriverManager : IDriverRuntimeManager
    {

        #region ... Variables  ...
        
        /// <summary>
        /// 
        /// </summary>
        public static DriverManager Manager = new DriverManager();

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, IDriverRuntime> mDrivers = new Dictionary<string, IDriverRuntime>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        public string Name { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        /// <summary>
        /// 获取Driver
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IDriverRuntime GetDriver(string name)
        {
            if(mDrivers.ContainsKey(name))
            {
                return mDrivers[name];
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string GetAssemblyPath(string file)
        {
            return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), file);
        }


        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location),"Data",Name, "Driver.cfg");
            Load(sfile);
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="file"></param>
        public void Load(string file)
        {
            if(System.IO.File.Exists(file))
            {
                XElement xx = XElement.Load(file);
                foreach(var vv in xx.Elements())
                {
                    //string ass = vv.Attribute("Assembly").Value;
                    //string cls = vv.Attribute("Class").Value;
                    //var afile = GetAssemblyPath(ass);
                    string tname = vv.Attribute("TypeName").Value;
                    var asb = ServiceLocator.Locator.Resolve<IDriverFactory>().GetRuntimeInstance(tname);
                    if (asb == null)
                    {
                        LoggerService.Service.Warn("ChannelManager", "Load driver " + tname + " failed!");
                        continue;
                    }

                    asb.Load(vv);

                    asb.Name = this.Name +"."+ asb.Name;

                    if (mDrivers.ContainsKey(asb.Name))
                    {
                        mDrivers[asb.Name] = asb;
                    }
                    else
                    {
                        mDrivers.Add(asb.Name, asb);
                    }
                }
            }
        }



        #endregion ...Interfaces...

    }
}
