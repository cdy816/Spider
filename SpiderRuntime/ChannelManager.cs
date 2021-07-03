//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/4 14:30:39.
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
    public class ChannelManager : ICommChannelRuntimeManager
    {

        #region ... Variables  ...
        
        /// <summary>
        /// 
        /// </summary>
        public static ChannelManager Manager = new ChannelManager();

        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<string, ICommChannel2> mChannels = new Dictionary<string, ICommChannel2>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
            /// 
            /// </summary>
        public string Name
        {
            get;
            set;
        }


        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        /// <summary>
        /// 获取Driver
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ICommChannel2 GetChannel(string name)
        {
            if(mChannels.ContainsKey(name))
            {
                return mChannels[name];
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
        public void Load()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Data", Name, "Channel.cfg");
            Load(sfile);
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="file"></param>
        public void Load(string file)
        {
            if(System.IO.File.Exists(file))
            {
                XElement xx = XElement.Load(file);
                foreach(var vv in xx.Elements())
                {
                    string tname = vv.Attribute("TypeName").Value;
                    var asb = ServiceLocator.Locator.Resolve<ICommChannelFactory2>().GetRuntimeIntance(tname);
                    if (asb == null)
                    {
                        LoggerService.Service.Warn("ChannelManager", "Load channel "+tname+" failed!");
                        continue;
                    }

                    asb.Load(vv);
                    if (mChannels.ContainsKey(asb.Name))
                    {
                        mChannels[asb.Name] = asb;
                    }
                    else
                    {
                        mChannels.Add(asb.Name, asb);
                    }
                }
            }
        }



        #endregion ...Interfaces...

    }
}
