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

namespace Cdy.Link.Mqtt.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiConfigViewModel:Cdy.Spider.DevelopCommon.ViewModelBase
    {

        #region ... Variables  ...
        
        private MqttLinkData mModel;

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
        public MqttLinkData Model
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
                return mModel.ServerUrl;
            }
            set
            {
                if (mModel.ServerUrl != value)
                {
                    mModel.ServerUrl = value;
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
                return mModel.ServerPort;
            }
            set
            {
                if (mModel.ServerPort != value)
                {
                    mModel.ServerPort = value;
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
                return mModel.ServerUser;
            }
            set
            {
                if (mModel.ServerUser != value)
                {
                    mModel.ServerUser = value;
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
                return mModel.ServerPassword;
            }
            set
            {
                if (mModel.ServerPassword != value)
                {
                    mModel.ServerPassword = value;
                    OnPropertyChanged("Password");
                }
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
                    RemoteResponseTopic = value + "_response";
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
