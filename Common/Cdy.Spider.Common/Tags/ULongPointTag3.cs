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
    public class ULongPoint3Tag : Tagbase
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
        public override object Value { get => mValue; set { mValue = (ULongPoint3)(value); AppendHisValue(mValue); } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private void AppendHisValue(ULongPoint3 value)
        {
            if (mIsBufferEnabled && IsBufferStarted)
            {
                this.HisValueBuffer.AppendValue(DateTime.UtcNow, value);
            }
            ValueChangedCallBack?.Invoke(this, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<HisValue> ReadHisValues()
        {
            DateTime time;
            ULongPoint3 value;
            while (this.HisValueBuffer.ReadValue(out time, out value))
            {
                yield return new HisValue() { Time = time, Value = value };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueCount"></param>
        protected override void AllocDataBuffer(int valueCount)
        {
            this.HisValueBuffer = new HisDataMemory(32, valueCount);
        }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
