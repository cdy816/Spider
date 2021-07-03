//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2021/6/12 0:23:02.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Cdy.Link.Tcp.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiConfigViewModel:Cdy.Spider.DevelopCommon.ViewModelBase
    {

        #region ... Variables  ...
        
        private TcpLinkData mModel;

        private List<string> mTransTypes;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        public ApiConfigViewModel()
        {
            mTransTypes = Enum.GetNames(typeof(ApiData.TransType)).ToList();
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public TcpLinkData Model
        {
            get
            {
                return mModel;
            }
            set
            {
                if (mModel != value)
                {
                    mModel = value;
                    OnPropertyChanged("Model");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int Port
        {
            get
            {
                return mModel.Port;
            }
            set
            {
                if (mModel.Port != value)
                {
                    mModel.Port = value;
                    OnPropertyChanged("Port");
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
                return mModel.UserName;
            }
            set
            {
                if (mModel.UserName != value)
                {
                    mModel.UserName = value;
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
                return mModel.Password;
            }
            set
            {
                if (mModel.Password != value)
                {
                    mModel.Password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
