//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/10 12:41:13.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Text;

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

        /// <summary>
        /// 
        /// </summary>
        public List<IApi> Apis { get { return mApis; } }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {

        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
