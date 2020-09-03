//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/4 14:25:50.
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
    public interface IDeviceDevelop
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// 
        /// </summary>
        string Group { get; set; }

        /// <summary>
        /// 
        /// </summary>
        DeviceData Data { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...




        object Config();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        XElement Save();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        /// <param name="context"></param>
        void Load(XElement xe, Context context);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDeviceDevelop Clone();

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
