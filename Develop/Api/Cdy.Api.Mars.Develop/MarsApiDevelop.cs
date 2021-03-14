//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/17 10:20:56.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider;
using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Api.Mars
{
    /// <summary>
    /// 
    /// </summary>
    public class MarsApiDevelop : Cdy.Spider.ApiDevelopBase
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
        public override ApiData Data { get; set; } = new ApiData();

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "MarsApi";

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Config()
        {
            if (Data == null) Data = new ApiData();

            return new ApiConfigViewModel() { Model = Data };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ApiData CreatNewData()
        {
            return new ApiData();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IApiDevelop NewApi()
        {
            return new MarsApiDevelop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ConfigTags()
        {
            TagBrowserViewModel tmm = new TagBrowserViewModel();
            if(tmm.ShowDialog().Value)
            {
                return tmm.SelectTagName;
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> ConfigMutiTags()
        {
            TagBrowserViewModel tmm = new TagBrowserViewModel();
            if (tmm.ShowDialog().Value)
            {
                return tmm.GetSelectTags();
            }
            return base.ConfigMutiTags();
        }


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
