//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/5/8 9:28:54.
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
    public interface IValueConvert
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        
        /// <summary>
        /// 
        /// </summary>
        public string Name { get;}

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object ConvertTo(object value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object ConvertBackTo(object value);

        /// <summary>
        /// 支持指定类型的变量
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool SupportTag(Tagbase tag);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string SaveToString();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IValueConvert LoadFromString(string value);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IValueConvert Clone();

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    public static class IValueConvertExtend
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="convert"></param>
        /// <returns></returns>
        public static string SeriseToString(this IValueConvert convert)
        {
            return convert.Name + ":" + convert.SaveToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="convert"></param>
        /// <returns></returns>
        public static IValueConvert DeSeriseToValueConvert(this string convert)
        {
            string[] sval = convert.Split(new char[] { ':' });
            var vtmp = ValueConvertManager.manager.GetConvert(sval[0]);
            if (vtmp != null)
            {
               return vtmp.LoadFromString(convert.Replace(sval[0] + ":", ""));
            }
            return null;
        }

    }

}
