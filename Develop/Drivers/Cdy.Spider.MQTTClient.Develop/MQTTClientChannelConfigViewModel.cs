//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/9/1 11:47:06.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider.MQTTClient.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class MQTTClientChannelConfigViewModel : ViewModelBase
    {

        #region ... Variables  ...

        /// <summary>
        /// 
        /// </summary>
        private MQTTChannelData mModel;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public MQTTChannelData Model
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

        #endregion ...Properties...

        #region ... Methods    ...

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
        public int ReTryCount
        {
            get
            {
                return mModel.ReTryCount;
            }
            set
            {
                if (mModel.ReTryCount != value)
                {
                    mModel.ReTryCount = value;
                    OnPropertyChanged("ReTryCount");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ReTryDuration
        {
            get
            {
                return mModel.ReTryDuration;
            }
            set
            {
                if (mModel.ReTryDuration != value)
                {
                    mModel.ReTryDuration = value;
                    OnPropertyChanged("ReTryDuration");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Timeout
        {
            get
            {
                return mModel.Timeout;
            }
            set
            {
                if (mModel.Timeout != value)
                {
                    mModel.Timeout = value;
                    OnPropertyChanged("Timeout");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int DataSendTimeout
        {
            get
            {
                return mModel.DataSendTimeout;
            }
            set
            {
                if (mModel.DataSendTimeout != value)
                {
                    mModel.DataSendTimeout = value;
                    OnPropertyChanged("DataSendTimeout");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ServerTopicAppendString
        {
            get
            {
                return mModel.ServerTopicAppendString;
            }
            set
            {
                if (mModel.ServerTopicAppendString != value)
                {
                    mModel.ServerTopicAppendString = value;
                    OnPropertyChanged("ServerTopicAppendString");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ClientTopicAppendString
        {
            get
            {
                return mModel.ClientTopicAppendString;
            }
            set
            {
                if (mModel.ClientTopicAppendString != value)
                {
                    mModel.ClientTopicAppendString = value;
                    OnPropertyChanged("ClientTopicAppendString");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ResponseTopicAppendString
        {
            get
            {
                return mModel.ResponseTopicAppendString;
            }
            set
            {
                if (mModel.ResponseTopicAppendString != value)
                {
                    mModel.ResponseTopicAppendString = value;
                    OnPropertyChanged("ResponseTopicAppendString");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public string TopicHeadString
        {
            get
            {
                return mModel.TopicHeadString;
            }
            set
            {
                if (mModel.TopicHeadString != value)
                {
                    mModel.TopicHeadString = value;
                    OnPropertyChanged("TopicHeadString");
                }
            }
        }



        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
