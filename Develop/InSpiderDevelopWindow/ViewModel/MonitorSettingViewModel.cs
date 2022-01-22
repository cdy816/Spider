using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InSpiderDevelopWindow.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class MonitorSettingViewModel:WindowViewModelBase
    {

        #region ... Variables  ...
        private string mServer;
        private string mUserName;
        private string mPassword;
        private int mScanCircle;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        public MonitorSettingViewModel()
        {
            DefaultWidth = 400;
            DefaultHeight = 200;
            Title = Res.Get("MonitorSetting");
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public string Server
        {
            get
            {
                return mServer;
            }
            set
            {
                if (mServer != value)
                {
                    mServer = value;
                    OnPropertyChanged("Server");
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
                return mUserName;
            }
            set
            {
                if (mUserName != value)
                {
                    mUserName = value;
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
                return mPassword;
            }
            set
            {
                if (mPassword != value)
                {
                    mPassword = value;
                    OnPropertyChanged("Password");
                }
            }
        }


        /// <summary>
        /// 扫描周期
        /// </summary>
        public int ScanCircle
        {
            get
            {
                return mScanCircle;
            }
            set
            {
                if (mScanCircle != value)
                {
                    mScanCircle = value;
                    OnPropertyChanged("ScanCircle");
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
