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
    public class StringFormateConvert : IValueConvert
    {
        public string Name { get => "StringFormate";  }

        /// <summary>
        /// 
        /// </summary>
        public string Formate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object ConvertBackTo(object value)
        {
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object ConvertTo(object value)
        {
            return string.Format(Formate, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string SaveToString()
        {
            return Formate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IValueConvert LoadFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return new StringFormateConvert();
            return new StringFormateConvert() { Formate = value};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool SupportTag(Tagbase tag)
        {
            if (tag is StringTag) return true;
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
            return new StringFormateConvert() { Formate = this.Formate };
        }
    }
}
