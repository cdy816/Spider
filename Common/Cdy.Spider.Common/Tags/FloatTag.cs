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
    public class FloatTag:Tagbase, INumberTag
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private float mValue;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        
        /// <summary>
        /// 
        /// </summary>
        public override TagType Type => TagType.Float;

        /// <summary>
        /// 
        /// </summary>
        public override object Value { get => mValue; set { mValue = Convert.ToSingle(ConvertValue(value)); AppendHisValue(mValue); } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private void AppendHisValue(float value)
        {
            if (mIsBufferEnabled)
            {
                this.HisValueBuffer.AppendValue(DateTime.UtcNow, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<HisValue> ReadHisValues()
        {
            DateTime time;
            float value;
            while (this.HisValueBuffer.ReadValue(out time, out value))
            {
                yield return new HisValue() { Time = time, Value = value };
            }
        }

        protected override void AllocDataBuffer(int valueCount)
        {
            this.HisValueBuffer = new HisDataMemory(12, valueCount);
        }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
