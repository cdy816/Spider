﻿using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cdy.Spider.OpcDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class OpcDriverConfigModel : ViewModelBase, IRegistorConfigModel
    {
        #region ... Variables  ...
        private string mRegistor = string.Empty;

        private ICommand mRegistorBrowsingCommand;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...


        /// <summary>
        /// 
        /// </summary>
        public ICommand RegistorBrowsingCommand
        {
            get
            {
                if(mRegistorBrowsingCommand == null)
                {
                    mRegistorBrowsingCommand = new RelayCommand(() => {

                        if (Service != null)
                        {
                            var vss = Service.GetChannelType();

                            if (vss == "OpcDA")
                            {
                                OpcDABrowserViewModel opv = new OpcDABrowserViewModel();
                                string ss = Service.GetConfigServerUrl();
                                if (!string.IsNullOrEmpty(ss))
                                {
                                    opv.ServerAddress = ss;
                                }
                                string user = Service.GetConfigUserName();
                                if (!string.IsNullOrEmpty(user))
                                {
                                    opv.UserName = user;
                                }

                                string pass = Service.GetConfigPassword();
                                if (!string.IsNullOrEmpty(pass))
                                {
                                    opv.Password = pass;
                                }

                                var pps = Service.GetChannelParameter();
                                if(pps!=null && pps.ContainsKey("ServerName"))
                                {
                                    opv.ServerName = pps["ServerName"];
                                }

                                if (opv.ShowDialog().Value)
                                {
                                    Registor = opv.SelectVariable.NodeId + "||" + opv.SelectVariable.DataType;
                                }
                            }
                            else
                            {
                                OpcBrowserViewModel opv = new OpcBrowserViewModel();

                                string ss = Service.GetConfigServerUrl();
                                if (!string.IsNullOrEmpty(ss))
                                {
                                    opv.ServerAddress = ss;
                                }
                                string user = Service.GetConfigUserName();
                                if (!string.IsNullOrEmpty(user))
                                {
                                    opv.UserName = user;
                                }

                                string pass = Service.GetConfigPassword();
                                if (!string.IsNullOrEmpty(pass))
                                {
                                    opv.Password = pass;
                                }
                                if (opv.ShowDialog().Value)
                                {
                                    Registor = opv.SelectVariable.NodeId + "||" + opv.SelectVariable.DataType;
                                }
                            }
                           
                        }
                    });
                }
                return mRegistorBrowsingCommand;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string Registor
        {
            get
            {
                return mRegistor;
            }
            set
            {
                if (mRegistor != value)
                {
                    mRegistor = value;
                    UpdateRegistorCallBack?.Invoke(value);
                    OnPropertyChanged("Registor");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Action<string> UpdateRegistorCallBack { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDeviceDevelopService Service { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Config()
        {
            OpcBrowserViewModel opv = new OpcBrowserViewModel();
            if (Service != null)
            {
                string ss = Service.GetConfigServerUrl();
                if (!string.IsNullOrEmpty(ss))
                {
                    opv.ServerAddress = ss;
                }
                string user = Service.GetConfigUserName();
                if (!string.IsNullOrEmpty(ss))
                {
                    opv.UserName = user;
                }

                string pass = Service.GetConfigPassword();
                if (!string.IsNullOrEmpty(pass))
                {
                    opv.Password = pass;
                }
            }
            if (opv.ShowDialog().Value)
            {
                return opv.GetSelectTags();
            }
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="registor"></param>
        public void FreshRegistor(string registor)
        {
            this.mRegistor = registor;
            OnPropertyChanged("Registor");
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            UpdateRegistorCallBack = null;
            base.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnDisActived()
        {
            
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...






    }
}
