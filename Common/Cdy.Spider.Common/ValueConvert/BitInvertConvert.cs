using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider
{
    public class BitInvertConvert : IValueConvert
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name => "BitInvert";

        /// <summary>
        /// 
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IValueConvert Clone()
        {
            return new BitInvertConvert() { Enable = this.Enable };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object ConvertBackTo(object value)
        {
            if (Enable)
                return !Convert.ToBoolean(value);
            else
                return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object ConvertTo(object value)
        {
            if (Enable)
                return !Convert.ToBoolean(value);
            else
                return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IValueConvert LoadFromString(string value)
        {
            return new BitInvertConvert() { Enable = bool.Parse(value) };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string SaveToString()
        {
            return Enable.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool SupportTag(Tagbase tag)
        {
            if (tag is BoolTag) return true;
            else
            {
                return false;
            }
        }
    }
}
