//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/4 15:36:55.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public class TagCollection:Dictionary<int,Tagbae>
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
        public int MaxId { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...



        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public void AddTag(Tagbae tag)
        {
            if(!this.ContainsKey(tag.Id))
            {
                Add(tag.Id, tag);
                MaxId = Math.Max(tag.Id, MaxId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public void RemoveTag(Tagbae tag)
        {
            if(this.ContainsKey(tag.Id))
            {
                this.Remove(tag.Id);
            }
        }



        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
