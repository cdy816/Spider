//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/17 9:22:31.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Xml.Linq;

namespace InSpiderDevelop
{
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
        private Dictionary<string, IApiDevelop> mApis = new Dictionary<string, IApiDevelop>();

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
        /// <returns></returns>
        public List<IApiDevelop> ListApis()
        {
            return mApis.Values.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IApiDevelop FirstOrNull()
        {
            return mApis.Values.Count > 0 ? mApis.Values.First() : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IApiDevelop GetApi(string name)
        {
            return mApis.ContainsKey(name) ? mApis[name] : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="api"></param>
        /// <param name="newName"></param>
        public bool ReName(IApiDevelop api,string newName)
        {
            if(mApis.ContainsKey(api.Name))
            {
                mApis.Remove(api.Name);
                api.Name = newName;
                mApis.Add(api.Name, api);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="api"></param>
        /// <returns></returns>
        public bool AddApi(IApiDevelop api)
        {
            if(!mApis.ContainsKey(api.Name))
            {
                mApis.Add(api.Name, api);
                return true;
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void RemoveApi(string name)
        {
            if(mApis.ContainsKey(name))
            {
                mApis.Remove(name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reload()
        {
            this.mApis.Clear();
            Load();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", "Api.cfg");
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
                    var asb = ServiceLocator.Locator.Resolve<IApiFactory>().GetDevelopIntance(tname);
                    asb.Load(vv);
                    AddApi(asb);
                }
            }
            if(mApis.Count==0)
            {
                var vff = ServiceLocator.Locator.Resolve<IApiDevelopForFactory>();
                if(vff!=null)
                AddApi(vff.NewApi());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", "Api.cfg");
            Save(sfile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sfile"></param>
        public void Save(string sfile)
        {
            sfile.BackFile();
            XElement xx = new XElement("Apis");
            foreach(var vv in mApis)
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
