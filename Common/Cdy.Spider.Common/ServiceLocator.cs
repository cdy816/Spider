//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/5 14:59:31.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider
{
    public class ServiceLocator
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        public static ServiceLocator Locator = new ServiceLocator();

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, object> mNamedService = new Dictionary<string, object>();

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<Type, object> mTypeRegistores = new Dictionary<Type, object>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="service"></param>
        public void Registor(string name, object service)
        {
            if (!mNamedService.ContainsKey(name))
            {
                mNamedService.Add(name, service);
            }
        }


        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            mTypeRegistores.Clear();
            mNamedService.Clear();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="value"></param>
        public void Registor(Type tp, object value)
        {
            if (mTypeRegistores.ContainsKey(tp))
            {
                mTypeRegistores[tp] = value;
            }
            else
            {
                mTypeRegistores.Add(tp, value);
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="value"></param>
        public void Registor<T>(object value)
        {
            Type tp = typeof(T);
            if (mTypeRegistores.ContainsKey(tp))
            {
                mTypeRegistores[tp] = value;
            }
            else
            {
                mTypeRegistores.Add(tp, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object Resolve(string name)
        {
            if (this.mNamedService.ContainsKey(name))
            {
                return mNamedService[name];
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public T Resolve<T>(string name)
        {
            if (mNamedService.ContainsKey(name))
            {
                return (T)mNamedService[name];
            }
            return default(T);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public object Resolve(Type tp)
        {
            if (mTypeRegistores.ContainsKey(tp))
            {
                return mTypeRegistores[tp];
            }
            return null;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tp"></param>
        /// <returns></returns>
        public T Resolve<T>(Type tp)
        {
            if (mTypeRegistores.ContainsKey(tp))
            {
                return (T)mTypeRegistores[tp];
            }
            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            var tp = typeof(T);
            if (mTypeRegistores.ContainsKey(tp))
            {
                return (T)mTypeRegistores[tp];
            }
            return default(T);
        }


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
