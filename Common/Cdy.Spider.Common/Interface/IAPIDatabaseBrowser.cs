//==============================================================
//  Copyright (C) 2020 Chongdaoyang  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/9/6 10:17:53.
//  Version 1.0
//  CDYWORK
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAPIDatabaseBrowser
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
        /// <param name="fileter"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<string> ListTags(string fileter, int start, int count);


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
