﻿//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/9/1 16:29:31.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider.SystemDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemDriverDevelopViewModel: ViewModelBase
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
        public SystemDriverData Model { get; set; }


        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
            /// 
            /// </summary>
        public int WorkModel
        {
            get
            {
                return (int)Model.Model;
            }
            set
            {
                if ((int)Model.Model != value)
                {
                    Model.Model = (WorkMode) value;
                    OnPropertyChanged("WorkModel");
                    OnPropertyChanged("ScanCircleVisibility");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Visibility ScanCircleVisibility
        {
            get
            {
                return Model.Model == WorkMode.Active ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
        }


        /// <summary>
            /// 
            /// </summary>
        public int ScanCircle
        {
            get
            {
                return Model.ScanCircle;
            }
            set
            {
                if (Model.ScanCircle != value)
                {
                    Model.ScanCircle = value;
                    OnPropertyChanged("ScanCircle");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public string UserName
        {
            get
            {
                return Model.UserName;
            }
            set
            {
                if (Model.UserName != value)
                {
                    Model.UserName = value;
                    OnPropertyChanged("UserName");
                }
            }
        }


        /// <summary>
            /// 
            /// </summary>
        public string Password
        {
            get
            {
                return Model.Password;
            }
            set
            {
                if (Model.Password != value)
                {
                    Model.Password = value;
                    OnPropertyChanged("Password");
                }
            }
        }




        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
