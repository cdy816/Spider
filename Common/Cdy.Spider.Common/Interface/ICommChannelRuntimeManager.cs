//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/4 14:46:21.
//  Version 1.0
//  种道洋
//==============================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommChannelRuntimeManager
    {

        #region ... Variables  ...

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
        void Load(string file);

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ICommChannel GetChannel(string name);

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
