//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/11 16:53:36.
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
    public abstract class ApiBase : IApi, IApiForFactory
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
        public abstract ApiData Data { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract string TypeName { get; }


        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        public virtual void Init()
        {
           
        }



        /// <summary>
        /// 
        /// </summary>
        public virtual void Start()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Stop()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public virtual void Load(XElement xe)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract IApi NewApi();
        


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
