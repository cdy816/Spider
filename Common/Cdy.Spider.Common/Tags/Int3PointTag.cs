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
    public struct IntPoint3
    {
        /// <summary>
        /// 
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Z { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    public class IntPoint3Tag:Tagbase
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private IntPoint3 mValue;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        
        /// <summary>
        /// 
        /// </summary>
        public override TagType Type => TagType.IntPoint3;

        /// <summary>
        /// 
        /// </summary>
        public override object Value { get => mValue; set { mValue = (IntPoint3)(value); AppendHisValue(mValue); } }

        private void AppendHisValue(IntPoint3 value)
        {
            if (mIsBufferEnabled)
            {
                this.HisValueBuffer.AppendValue(DateTime.UtcNow, value);
            }
            ValueChangedCallBack?.Invoke(this, value);
        }


        public override IEnumerable<HisValue> ReadHisValues()
        {
            DateTime time;
            IntPoint3 value;
            while (this.HisValueBuffer.ReadValue(out time, out value))
            {
                yield return new HisValue() { Time = time, Value = value };
            }
        }

        protected override void AllocDataBuffer(int valueCount)
        {
            this.HisValueBuffer = new HisDataMemory(20, valueCount);
        }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
