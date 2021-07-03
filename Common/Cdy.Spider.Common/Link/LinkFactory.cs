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
    public class LinkFactory : ILinkFactory
    {

        #region ... Variables  ...

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, ILinkForFactory> mRuntimeManagers = new Dictionary<string, ILinkForFactory>();

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, ILinkDevelopForFactory> mDevelopManagers = new Dictionary<string, ILinkDevelopForFactory>();

        /// <summary>
        /// 
        /// </summary>
        public static LinkFactory Factory = new LinkFactory();

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
        public ILinkDevelop GetDevelopInstance(string type)
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
        public ILinkDevelop GetDevelopInstance()
        {
            return GetDevelopInstance(mDevelopManagers.Keys.First());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ILink GetRuntimeInstance(string type)
        {
            if(mRuntimeManagers.ContainsKey(type))
            {
                return mRuntimeManagers[type].NewLink();
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
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Config", "LinkRuntime.cfg");
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
                        var asb = Assembly.LoadFrom(afile).CreateInstance(cls) as ILinkForFactory;
                        
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
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location),"Config", "LinkDevelop.cfg");
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
                            var css = asb.CreateInstance(cls) as ILinkDevelopForFactory;

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
        public List<string> ListDevelopLinks()
        {
            return mDevelopManagers.Keys.ToList();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
