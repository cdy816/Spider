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
using System.Text;
using System.Xml.Linq;

namespace InSpiderDevelop
{
    public class APIDocument
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        public static APIDocument Manager = new APIDocument();

        private IApiDevelop mApi;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public IApiDevelop Api { get { return mApi; } set { mApi = value; } }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }


        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void New()
        {
            Api = ServiceLocator.Locator.Resolve<IApiFactory>().GetDevelopInstance();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reload()
        {
            this.mApi = null;
            Load();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", Name, "Api.cfg");
            Load(sfile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sval"></param>
        /// <returns></returns>
        public APIDocument LoadFromString(string sval)
        {
            try
            {
                string sname = System.IO.Path.GetTempFileName();
                System.IO.File.WriteAllText(sname, sval);

                XElement xx = XElement.Load(sname);
                foreach (var vv in xx.Elements())
                {
                    string tname = vv.Attribute("TypeName").Value;
                    var asb = ServiceLocator.Locator.Resolve<IApiFactory>()?.GetDevelopInstance(tname);
                    asb.Load(vv);
                    mApi = asb;
                }

                if (mApi == null)
                {
                    mApi = ServiceLocator.Locator.Resolve<IApiFactory>()?.GetDevelopInstance();
                }

                System.IO.File.Delete(sname);
            }
            catch
            {

            }
            return this;
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
                    var asb = ServiceLocator.Locator.Resolve<IApiFactory>()?.GetDevelopInstance(tname);
                    asb.Load(vv);
                    mApi = asb;
                }
            }
            if(mApi==null)
            {
                mApi = ServiceLocator.Locator.Resolve<IApiFactory>()?.GetDevelopInstance();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", Name, "Api.cfg");
            CheckDirExistOrCreat(sfile);
            Save(sfile);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveTo(string dir)
        {
            string sfile = System.IO.Path.Combine(dir, "Api.cfg");
            CheckDirExistOrCreat(sfile);
            Save(sfile);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public void SaveWithString(string content)
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", Name, "Api.cfg");
            System.IO.File.WriteAllText(sfile, content);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string SaveToString()
        {
            System.IO.MemoryStream sb = new System.IO.MemoryStream();
            XElement xx = new XElement("Apis");
            xx.Add(mApi.Save());
            xx.Save(sb);
            return Encoding.UTF8.GetString(sb.ToArray());
        }

        private void CheckDirExistOrCreat(string sfile)
        {
            string sdir = System.IO.Path.GetDirectoryName(sfile);
            if (!System.IO.Directory.Exists(sdir))
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
            XElement xx = new XElement("Apis");
            xx.Add(mApi.Save());
            xx.Save(sfile);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
