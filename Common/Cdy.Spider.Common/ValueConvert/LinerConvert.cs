//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/5/8 9:38:53.
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
    public class LinerConvert : IValueConvert
    {
        public string Name { get => "Linear";  }

        /// <summary>
        /// 
        /// </summary>
        public double K { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double T { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object ConvertBackTo(object value)
        {
            var val = Convert.ToDouble(value);
            return (val - T) / K;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object ConvertTo(object value)
        {
            var val = Convert.ToDouble(value);
            return val * K + T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string SaveToString()
        {
            return K + "," + T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IValueConvert LoadFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return new LinerConvert();

            string[] sval = value.Split(new char[] { ',' });
            return new LinerConvert() { K = double.Parse(sval[0]), T = double.Parse(sval[1]) };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool SupportTag(Tagbase tag)
        {
            
            if (tag is INumberTag) return true;
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IValueConvert Clone()
        {
            return new LinerConvert() { K = this.K, T = this.T };
        }
    }
}
