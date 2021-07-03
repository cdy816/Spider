//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/17 11:37:01.
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
    /// <summary>
    /// 
    /// </summary>
    public class ChannelFactory2: ICommChannelFactory2
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, ICommChannel2ForFactory> mRuntimeManagers = new Dictionary<string, ICommChannel2ForFactory>();

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, ICommChannelDevelopForFactory> mDevelopManagers = new Dictionary<string, ICommChannelDevelopForFactory>();

        /// <summary>
        /// 
        /// </summary>
        public static ChannelFactory2 Factory = new ChannelFactory2();

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
        public ICommChannelDevelop GetDevelopIntance(string type)
        {
            if (!string.IsNullOrEmpty(type) && mDevelopManagers.ContainsKey(type))
            {
                return mDevelopManagers[type].NewChannel();
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ICommChannel2 GetRuntimeIntance(string type)
        {
            if (mRuntimeManagers.ContainsKey(type))
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
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Config", "ChannelRuntime.cfg");
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
                        var asb = Assembly.LoadFrom(afile).CreateInstance(cls) as ICommChannel2ForFactory;

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
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Config", "ChannelDevelop.cfg");
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
                        var asb = Assembly.LoadFrom(afile).CreateInstance(cls) as ICommChannelDevelopForFactory;

                        if (asb!=null && !mDevelopManagers.ContainsKey(asb.TypeName))
                        {
                            mDevelopManagers.Add(asb.TypeName, asb);
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ICommChannelDevelop> ListDevelopInstance()
        {
            return mDevelopManagers.Values.Select(e => e.NewChannel()).ToList();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
