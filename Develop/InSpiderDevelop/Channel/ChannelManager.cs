//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/17 9:23:50.
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
    public class ChannelManager
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        public static ChannelManager Manager = new ChannelManager();

        private Dictionary<string, ICommChannelDevelop> mChannels = new Dictionary<string, ICommChannelDevelop>();

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
        /// <param name="baseName"></param>
        /// <returns></returns>
        public string GetAvaiableName(string baseName)
        {
            return mChannels.Keys.GetAvaiableName(baseName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICommChannelDevelop NewChannel(string sname)
        {
            var vv = ServiceLocator.Locator.Resolve<ICommChannelDevelopForFactory>().NewChannel();
            if (string.IsNullOrEmpty(sname))
            {
                vv.Name = GetAvaiableName("Channel");
            }
            else
            {
                vv.Name = sname;
            }
            if(AddChannel(vv))
            {
                return vv;
            }
            return vv;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ICommChannelDevelop> ListChannels()
        {
            return mChannels.Values.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ICommChannelDevelop GetChannel(string name)
        {
            return mChannels.ContainsKey(name) ? mChannels[name] : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Channel"></param>
        /// <param name="newName"></param>
        public bool ReName(ICommChannelDevelop Channel, string newName)
        {
            if (mChannels.ContainsKey(Channel.Name))
            {
                mChannels.Remove(Channel.Name);
                Channel.Name = newName;
                mChannels.Add(Channel.Name, Channel);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Channel"></param>
        /// <returns></returns>
        public bool AddChannel(ICommChannelDevelop Channel)
        {
            if (!mChannels.ContainsKey(Channel.Name))
            {
                mChannels.Add(Channel.Name, Channel);
                return true;
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void RemoveChannel(string name)
        {
            if (mChannels.ContainsKey(name))
            {
                mChannels.Remove(name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reload()
        {
            this.mChannels.Clear();
            Load();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", "Channel.cfg");
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
                    var asb = ServiceLocator.Locator.Resolve<ICommChannelFactory>().GetDevelopIntance(tname);
                    asb.Load(vv);
                    AddChannel(asb);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", "Channel.cfg");
            Save(sfile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sfile"></param>
        public void Save(string sfile)
        {
            sfile.BackFile();
            XElement xx = new XElement("Channels");
            foreach (var vv in mChannels)
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
