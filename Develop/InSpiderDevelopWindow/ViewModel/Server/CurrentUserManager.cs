//==============================================================
//  Copyright (C) 2020 Chongdaoyang Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/6/18 15:11:47 .
//  Version 1.0
//  CDYWORK
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace InSpiderDevelopWindow.ViewModel
{
    public class CurrentUserManager
    {

        #region ... Variables  ...
        public static CurrentUserManager Manager = new CurrentUserManager();

        public event EventHandler RefreshNameEvent;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        public void OnRefreshName()
        {
            RefreshNameEvent?.Invoke(this, null);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
