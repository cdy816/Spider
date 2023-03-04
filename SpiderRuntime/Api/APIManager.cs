//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/10 12:41:13.
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
    public class APIManager
    {

        #region ... Variables  ...

        /// <summary>
        /// 
        /// </summary>
        public static APIManager Manager = new APIManager();

        /// <summary>
        /// 
        /// </summary>
        private List<IApi> mApis = new List<IApi>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<IApi> Apis { get { return mApis; } }

        #endregion ...Properties...

        #region ... Methods    ...


        /// <summary>
        /// 
        /// </summary>
        public void LoadSolution(string solution)
        {
            string sfile = string.IsNullOrEmpty(solution) ? System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", Name, "Api.cfg") : System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", solution, Name, "Api.cfg");
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
                    string tname = vv.Attribute("TypeName").Value;
                    var asb = ServiceLocator.Locator.Resolve<IApiFactory>().GetRuntimeInstance(tname);
                    asb.Load(vv);
                    mApis.Add(asb);
                }
            }
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

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
