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
    public struct LongPoint
    {
        public long X { get; set; }

        public long Y { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LongPointTag:Tagbase
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private LongPoint mValue;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        
        /// <summary>
        /// 
        /// </summary>
        public override TagType Type => TagType.LongPoint;

        /// <summary>
        /// 
        /// </summary>
        public override object Value { get => mValue; set => mValue = (LongPoint)(value); }

        public override IEnumerable<HisValue> ReadHisValues()
        {
            throw new NotImplementedException();
        }

        protected override void AllocDataBuffer(int valueCount)
        {
            throw new NotImplementedException();
        }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
