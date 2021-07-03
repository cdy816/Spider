using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.CoapClient.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class CoapClientChannelConfigViewModel : ViewModelBase
    {

        #region ... Variables  ...

        /// <summary>
        /// 
        /// </summary>
        private CoapClient.CoapClientChannelData mModel;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public CoapClient.CoapClientChannelData Model
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

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
