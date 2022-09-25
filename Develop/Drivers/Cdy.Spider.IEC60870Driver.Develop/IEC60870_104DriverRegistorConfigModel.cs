//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/9/17 16:40:44.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Text;
using System.Windows.Input;

namespace Cdy.Spider.IEC60870Driver.Develop
{
    public class IEC60870_104DriverRegistorConfigModel : ViewModelBase, IRegistorConfigModel
    {

        #region ... Variables  ...
        private string mRegistor = string.Empty;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
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
        private ICommand mHelpCommand;
        public ICommand HelpCommand
        {

            get
            {
                if(mHelpCommand==null)
                {
                    mHelpCommand = new RelayCommand(() => { 
                    
                    });
                }
                return mHelpCommand;
            }
        }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

        /// <summary>
        /// 
        /// </summary>
        public Action<string> UpdateRegistorCallBack { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDeviceDevelopService Service { get; set; }

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
        /// <returns></returns>
        public IEnumerable<string> Config()
        {
            return null;
        }

        public void OnDisActived()
        {
           
        }
    }
}
