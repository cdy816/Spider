//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/17 10:52:11.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiFactory : IApiFactory
    {

        #region ... Variables  ...

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, IApiForFactory> mRuntimeManagers = new Dictionary<string, IApiForFactory>();

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, IApiDevelopForFactory> mDevelopManagers = new Dictionary<string, IApiDevelopForFactory>();

        /// <summary>
        /// 
        /// </summary>
        public static ApiFactory Factory = new ApiFactory();

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
        public IApiDevelop GetDevelopInstance(string type)
        {
            if(mDevelopManagers.ContainsKey(type))
            {
                return mDevelopManagers[type].NewApi();
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IApiDevelop GetDevelopInstance()
        {
            return GetDevelopInstance(mDevelopManagers.Keys.First());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IApi GetRuntimeInstance(string type)
        {
            if(mRuntimeManagers.ContainsKey(type))
            {
                return mRuntimeManagers[type].NewApi();
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
        public void LoadForRun()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Config", "ApiRuntime.cfg");
            if(System.IO.File.Exists(sfile))
            {
                XElement xx = XElement.Load(sfile);
                foreach (var vv in xx.Elements())
                {
                    string ass = vv.Attribute("Assembly").Value;
                    string cls = vv.Attribute("Class").Value;
                    var afile = GetAssemblyPath(ass);
                    if (System.IO.File.Exists(afile) && !string.IsNullOrEmpty(cls))
                    {
                        var asb = Assembly.LoadFrom(afile).CreateInstance(cls) as IApiForFactory;
                        
                        if(!mRuntimeManagers.ContainsKey(asb.TypeName))
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
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location),"Config", "ApiDevelop.cfg");
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
                        try
                        {
                            var asb = Assembly.LoadFrom(afile);
                            var css = asb.CreateInstance(cls) as IApiDevelopForFactory;

                            if (!mDevelopManagers.ContainsKey(css.TypeName))
                            {
                                mDevelopManagers.Add(css.TypeName, css);
                            }
                        }
                        catch(Exception ex)
                        {
                            Debug.Print(ex.Message);
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> ListDevelopApis()
        {
            return mDevelopManagers.Keys.ToList();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
