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

namespace Cdy.Spider.IEC60870Driver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class IEC60870_104DriverDevelopViewModel : ViewModelBase
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
        public IEC60870_104_DriverData Model { get; set; }


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
        public int StationId
        {
            get
            {
                return Model.StationId;
            }
            set
            {
                if (Model.StationId != value)
                {
                    Model.StationId = value;
                    OnPropertyChanged("StationId");
                }
            }
        }





        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
