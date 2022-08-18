using Cdy.Spider.DevelopCommon;
using Cdy.Spider.OpcDAClient;
using OpcCom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Cdy.Spider.OpcDA.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class OpcDAChannelConfigViewModel : ViewModelBase
    {

        #region ... Variables  ...

        /// <summary>
        /// 
        /// </summary>
        private OpcDAChannelData mModel;

        private List<string> mServerNameList;

        private ICommand mListServerCommand;

        private static ServerEnumerator m_discovery = new ServerEnumerator();
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public OpcDAChannelData Model
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
                    Task.Run(() => {
                        ListServers();
                    });
                  
                    OnPropertyChanged("Model");
                }
            }
        }

        public ICommand ListServerCommand
        {
            get
            {
                if(mListServerCommand==null)
                {
                    mListServerCommand = new RelayCommand(() => {
                        ListServers();
                    });
                }
                return mListServerCommand;
            }
        }

        #endregion ...Properties...

        #region ... Methods    ...

        private  void ListServers()
        {
            try
            {
                var slist = new List<string>();

                if (!string.IsNullOrEmpty(UserName) || (mModel.ServerIp != "localhost" && mModel.ServerIp != "127.0.0.1"))
                {
                    var res = m_discovery.GetAvailableServers(Opc.Specification.COM_DA_10, mModel.ServerIp, new Opc.ConnectData(new System.Net.NetworkCredential() { UserName = UserName, Password = Password }));
                    if (res.Any())
                    {
                        slist.AddRange(res.Select(e => e.Name));
                    }
                    res = m_discovery.GetAvailableServers(Opc.Specification.COM_DA_20, mModel.ServerIp, new Opc.ConnectData(new System.Net.NetworkCredential() { UserName = UserName, Password = Password }));
                    if (res.Any())
                    {
                        slist.AddRange(res.Select(e => e.Name));
                    }

                    res = m_discovery.GetAvailableServers(Opc.Specification.COM_DA_30, mModel.ServerIp, new Opc.ConnectData(new System.Net.NetworkCredential() { UserName = UserName, Password = Password }));
                    if (res.Any())
                    {
                        slist.AddRange(res.Select(e => e.Name));
                    }
                }
                else
                {
                    var res = m_discovery.GetAvailableServers(Opc.Specification.COM_DA_10, "localhost", null);
                    if (res.Any())
                    {
                        slist.AddRange(res.Where(e => e != null).Select(e => e.Name));
                    }
                    res = m_discovery.GetAvailableServers(Opc.Specification.COM_DA_20, "localhost", null);
                    if (res.Any())
                    {
                        slist.AddRange(res.Where(e => e != null).Select(e => e.Name));
                    }

                    res = m_discovery.GetAvailableServers(Opc.Specification.COM_DA_30, "localhost", null);
                    if (res.Any())
                    {
                        slist.AddRange(res.Where(e => e != null).Select(e => e.Name));
                    }
                }
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    ServerNameList = slist.Distinct().ToList();
                }));
            }
            catch
            {

            }
        }

        /// <summary>
            /// 
            /// </summary>
        public List<string> ServerNameList
        {
            get
            {
                return mServerNameList;
            }
            set
            {
                if (mServerNameList != value)
                {
                    mServerNameList = value;
                    OnPropertyChanged("ServerNameList");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string ServerName
        {
            get
            {
                return mModel.ServerName;
            }
            set
            {
                if (mModel.ServerName != value)
                {
                    mModel.ServerName = value;
                
                    OnPropertyChanged("ServerName");
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
                    ListServers();
                    OnPropertyChanged("ServerIp");
                }
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public int Port
        //{
        //    get
        //    {
        //        return mModel.Port;
        //    }
        //    set
        //    {
        //        if (mModel.Port != value)
        //        {
        //            mModel.Port = value;
        //            OnPropertyChanged("Port");
        //        }
        //    }
        //}


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
