//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/3/17 17:13:55.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace Cdy.Spider.IEC60870Driver.Develop
{
    [MarkupExtensionReturnType(typeof(object))]
    [ContentProperty("Key")]
    public class ResMarkerExtension : MarkupExtension
    {
        #region ... Variables  ...

        private static List<ResMarkerExtension> mCachItems = new List<ResMarkerExtension>();

        private FrameworkElement mTarget;

        private string mAppendChar = "";

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        public ResMarkerExtension()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public ResMarkerExtension(string key) : this()
        {
            Key = key;
            if (mCachItems != null)
            {
                mCachItems.Add(this);
            }
        }

        public ResMarkerExtension(string key,string appendChar) : this(key)
        {
            mAppendChar = appendChar;
        }

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public FrameworkElement Target
        {
            get
            {
                return mTarget;
            }
            set
            {
                if (mTarget != value)
                {
                    mTarget = value;
                    if (value != null)
                        mTarget.Unloaded += (mTarget_Unloaded);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object Property { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var vv = (serviceProvider.GetService(typeof(System.Xaml.IRootObjectProvider)) as System.Xaml.IRootObjectProvider);
            if (vv != null)
            {
                var root = vv.RootObject.GetType().Assembly;
                var target = serviceProvider.GetService(typeof(System.Windows.Markup.IProvideValueTarget)) as System.Windows.Markup.IProvideValueTarget;
                if (target != null)
                {
                    Target = target.TargetObject as FrameworkElement;
                    Property = target.TargetProperty;
                }
            }
            return GetValue();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetValue()
        {
            if (string.IsNullOrEmpty(Key))
            {
                return " "+ mAppendChar;
            }
            else
            {
                System.Globalization.CultureInfo cinfo = Thread.CurrentThread.CurrentUICulture;
                var res = Develop.Properties.Resources.ResourceManager.GetString(Key, cinfo);
                if (string.IsNullOrEmpty(res))
                    res = Key;
                return res+ mAppendChar;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mTarget_Unloaded(object sender, RoutedEventArgs e)
        {
            if (mCachItems != null && mCachItems.Contains(this))
            {
                mCachItems.Remove(this);
            }
            mTarget.Unloaded -= (mTarget_Unloaded);
            mTarget = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            if (Target != null)
                Target.SetValue(Property as DependencyProperty, GetValue());
        }

        /// <summary>
        /// 更新资源
        /// </summary>
        public static void UpdateResource()
        {
            if (mCachItems != null)
            {
                foreach (var vv in mCachItems)
                {
                    vv.Update();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ClearCach()
        {
            mCachItems.Clear();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...


    }
}
