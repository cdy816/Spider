//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/17 11:52:50.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider
{
    public class DriverFactory : IDriverFactory
    {

        #region ... Variables  ...

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, IDriverForFactory> mRuntimeManagers = new Dictionary<string, IDriverForFactory>();

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, IDriverDevelopForFactory> mDevelopManagers = new Dictionary<string, IDriverDevelopForFactory>();

        /// <summary>
        /// 
        /// </summary>
        public static DriverFactory Factory = new DriverFactory();

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
        /// <param name="type"></param>
        /// <returns></returns>
        public IDriverDevelop GetDevelopInstance(string type)
        {
            if (!string.IsNullOrEmpty(type) && mDevelopManagers.ContainsKey(type))
            {
                return mDevelopManagers[type].NewDriver();
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IDriverRuntime GetRuntimeInstance(string type)
        {
            if (!string.IsNullOrEmpty(type) && mRuntimeManagers.ContainsKey(type))
            {
                return mRuntimeManagers[type].NewApi();
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IDriverDevelop> ListDevelopInstance()
        {
            return mDevelopManagers.Values.Select(e => e.NewDriver()).ToList();
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
        public void LoadForRun()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Config", "DriverRuntime.cfg");
            if (System.IO.File.Exists(sfile))
            {
                XElement xx = XElement.Load(sfile);
                foreach (var vv in xx.Elements())
                {
                    string ass = vv.Attribute("Assembly").Value;
                    string cls = vv.Attribute("Class").Value;
                    var afile = GetAssemblyPath(ass);
                    if (System.IO.File.Exists(afile) && !string.IsNullOrEmpty(cls))
                    {
                        var asb = Assembly.LoadFrom(afile).CreateInstance(cls) as IDriverForFactory;
                        if (!mRuntimeManagers.ContainsKey(asb.TypeName))
                        {
                            mRuntimeManagers.Add(asb.TypeName, asb);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadForDevelop()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Config", "DriverDevelop.cfg");
            if (System.IO.File.Exists(sfile))
            {
                XElement xx = XElement.Load(sfile);
                foreach (var vv in xx.Elements())
                {
                    string ass = vv.Attribute("Assembly").Value;
                    string cls = vv.Attribute("Class").Value;
                    var afile = GetAssemblyPath(ass);
                    if (System.IO.File.Exists(afile) && !string.IsNullOrEmpty(cls))
                    {
                        var asb = Assembly.LoadFrom(afile).CreateInstance(cls) as IDriverDevelopForFactory;
                        if (!mDevelopManagers.ContainsKey(asb.TypeName))
                        {
                            mDevelopManagers.Add(asb.TypeName, asb);
                        }
                    }
                }
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
