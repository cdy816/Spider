//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2022/9/4 16:40:44.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider.DevelopCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Cdy.Spider.Siemens.Develop
{
    public class SiemensS7RegistorConfigModel : ViewModelBase, IRegistorConfigModel
    {

        #region ... Variables  ...

        private string mRegistor;

        private ICommand mHelpCommand;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        public SiemensS7RegistorConfigModel()
        {
            
        }

        #endregion ...Constructor...

        #region ... Properties ...

        public List<string> Areas
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand HelpCommand
        {
            get
            {
                mHelpCommand = new RelayCommand(() => { 
                
                });
                return mHelpCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TagType TagType
        {
            get;
            set;
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
        /// <param name="registor"></param>
        public void FreshRegistor(string registor)
        {
            this.Registor = registor;
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

        public void OnDisActived()
        {
            UpdateRegistor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void UpdateRegistor()
        {
            UpdateRegistorCallBack?.Invoke(mRegistor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Config()
        {
            return null;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 
    /// </summary>
    public class AreasProvider : ISuggestionProvider
    {
        public List<string> Areas;

        public AreasProvider()
        {
            Areas = new List<string>() {"AI", "AIX", "AIB", "AIW", "AID","AQ", "AQX", "AQB", "AQW", "AQD","I", "IX", "IB", "IW", "ID","Q", "QX", "QB", "QW", "QD","M", "MX", "MB", "MW", "MD", "D", "DB", "DBX", "DBB", "DBW", "DBD", "T", "C", "V", "VB", "VW", "VD", "VX" };
        }
        public IEnumerable GetSuggestions(string filter)
        {
            return Areas.Where(x => x.Contains(filter));
        }
    }

}
