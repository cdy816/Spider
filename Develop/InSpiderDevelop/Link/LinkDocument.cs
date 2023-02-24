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
    public class LinkDocument
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        public static LinkDocument Manager = new LinkDocument();

        private ILinkDevelop mLink;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public ILinkDevelop Link { get { return mLink; } set { mLink = value; } }

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
            Link = ServiceLocator.Locator.Resolve<ILinkFactory>().GetDevelopInstance();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reload()
        {
            this.mLink = null;
            Load();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", Name, "Link.cfg");
            Load(sfile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sfile"></param>
        /// <returns></returns>
        public LinkDocument LoadFromString(string sfile)
        {
            if (!string.IsNullOrEmpty(sfile))
            {
                string sname = System.IO.Path.GetTempFileName();
                System.IO.File.WriteAllText(sname, sfile);
                XElement xx = XElement.Load(sname);
                foreach (var vv in xx.Elements())
                {
                    string tname = vv.Attribute("TypeName").Value;
                    var asb = ServiceLocator.Locator.Resolve<ILinkFactory>()?.GetDevelopInstance(tname);
                    asb.Load(vv);
                    mLink = asb;
                }
                System.IO.File.Delete(sname);
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
                    var asb = ServiceLocator.Locator.Resolve<ILinkFactory>()?.GetDevelopInstance(tname);
                    asb.Load(vv);
                    mLink = asb;
                }
            }
            //if(mLink==null)
            //{
            //    mLink = ServiceLocator.Locator.Resolve<ILinkFactory>()?.GetDevelopInstance();
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", Name, "Link.cfg");
            CheckDirExistOrCreat(sfile);
            Save(sfile);
        }

        public void SaveTo(string dir)
        {
            string sfile = System.IO.Path.Combine(dir, "Link.cfg");
            CheckDirExistOrCreat(sfile);
            Save(sfile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public void SaveWithString(string content)
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", Name, "Link.cfg");
            System.IO.File.WriteAllText(sfile, content);
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
        /// <returns></returns>
        public string SaveToString()
        {
            System.IO.MemoryStream sb = new System.IO.MemoryStream();
            XElement xx = new XElement("Links");
            if (mLink != null)
                xx.Add(mLink.Save());
            xx.Save(sb);
            return Encoding.UTF8.GetString(sb.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sfile"></param>
        public void Save(string sfile)
        {
            sfile.BackFile();
            XElement xx = new XElement("Links");
            if (mLink != null)
                xx.Add(mLink.Save());
            xx.Save(sfile);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
