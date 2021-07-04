//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/26 13:23:02.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Cdy.Api.SpiderMqtt.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiConfigViewModel:Cdy.Spider.DevelopCommon.ViewModelBase
    {

        #region ... Variables  ...
        
        private MqttApiData mModel;

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
        public MqttApiData Model
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
        public string ServerIp
        {
            get
            {
                return mModel.ServerIp;
            }
            set
            {
                if (mModel.ServerIp != value)
                {
                    mModel.ServerIp = value;
                    OnPropertyChanged("ServerIp");
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

        /// <summary>
            /// 
            /// </summary>
        public int Circle
        {
            get
            {
                return mModel.Circle;
            }
            set
            {
                if (mModel.Circle != value)
                {
                    mModel.Circle = value;
                    OnPropertyChanged("Circle");
                }
            }
        }


        /// <summary>
            /// 
            /// </summary>
        public int TransType
        {
            get
            {
                return (int)mModel.Type;
            }
            set
            {
                if ((int)Model.Type != value)
                {
                    Model.Type = (ApiData.TransType)value;
                    OnPropertyChanged("TransType");
                    OnPropertyChanged("CircleVisiable");
                }
            }
        }


        /// <summary>
            /// 
            /// </summary>
        public List<string> TransTypes
        {
            get
            {
                return mTransTypes;
            }
            set
            {
                if (mTransTypes != value)
                {
                    mTransTypes = value;
                    OnPropertyChanged("TransTypes");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Visibility CircleVisiable
        {
            get
            {
                return Model.Type == ApiData.TransType.Timer ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RemoteTopic
        {
            get
            {
                return mModel.RemoteTopic;
            }
            set
            {
                if (mModel.RemoteTopic != value)
                {
                    mModel.RemoteTopic = value;
                    RemoteResponseTopic = value+ "_response";
                    OnPropertyChanged("RemoteTopic");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RemoteResponseTopic
        {
            get
            {
                return mModel.RemoteResponseTopic;
            }
            set
            {
                if (mModel.RemoteResponseTopic != value)
                {
                    mModel.RemoteResponseTopic = value;
                    OnPropertyChanged("RemoteResponseTopic");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LocalTopic
        {
            get
            {
                return mModel.LocalTopic;
            }
            set
            {
                if (mModel.LocalTopic != value)
                {
                    mModel.LocalTopic = value;
                    LocalResponseTopic = value + "_response";
                    OnPropertyChanged("LocalTopic");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LocalResponseTopic
        {
            get
            {
                return mModel.LocalResponseTopic;
            }
            set
            {
                if (mModel.LocalResponseTopic != value)
                {
                    mModel.LocalResponseTopic = value;
                    OnPropertyChanged("LocalResponseTopic");
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
