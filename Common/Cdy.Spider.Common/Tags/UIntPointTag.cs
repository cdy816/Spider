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
    public struct UIntPoint
    {
        public uint X { get; set; }

        public uint Y { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UIntPointTag:Tagbae
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private UIntPoint mValue;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        
        /// <summary>
        /// 
        /// </summary>
        public override TagType Type => TagType.UIntPoint;

        /// <summary>
        /// 
        /// </summary>
        public override object Value { get => mValue; set => mValue = (UIntPoint)(value); }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
