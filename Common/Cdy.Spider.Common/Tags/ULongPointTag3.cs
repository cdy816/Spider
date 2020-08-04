//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/4 15:13:31.
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
    public struct ULongPoint3
    {
        public ulong X { get; set; }

        public ulong Y { get; set; }
        public ulong Z { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ULongPoint3Tag:Tagbae
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private ULongPoint3 mValue;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        
        /// <summary>
        /// 
        /// </summary>
        public override TagType Type => TagType.ULongPoint3;

        /// <summary>
        /// 
        /// </summary>
        public override object Value { get => mValue; set => mValue = (ULongPoint3)(value); }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
