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
