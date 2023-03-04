//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/9/1 13:44:00.
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
    public class Context:Dictionary<Type,object>,IDisposable
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
        public string Solution { get; set; }
        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Add(object value)
        {
            var tp = value.GetType();
            if (!this.ContainsKey(tp))
            {
                this.Add(tp, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tp"></param>
        /// <returns></returns>
        public T Get<T>(Type tp)
        {
            if(this.ContainsKey(tp))
            {
                return (T)this[tp];
            }
            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>()
        {
            Type tp = typeof(T);
            if (this.ContainsKey(tp))
            {
                return (T)this[tp];
            }
            return default(T);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
