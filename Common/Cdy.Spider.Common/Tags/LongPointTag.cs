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
        public override object Value { get => mValue; set { mValue = (LongPoint)(value); AppendHisValue(mValue); } }

        public override IEnumerable<HisValue> ReadHisValues()
        {
            DateTime time;
            LongPoint value;
            while (this.HisValueBuffer.ReadValue(out time, out value))
            {
                yield return new HisValue() { Time = time, Value = value };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private void AppendHisValue(LongPoint value)
        {
            if (mIsBufferEnabled && IsBufferStarted)
            {
                this.HisValueBuffer.AppendValue(DateTime.UtcNow, value);
            }
            ValueChangedCallBack?.Invoke(this, value);
        }

        protected override void AllocDataBuffer(int valueCount)
        {
            this.HisValueBuffer = new HisDataMemory(24, valueCount);
        }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
