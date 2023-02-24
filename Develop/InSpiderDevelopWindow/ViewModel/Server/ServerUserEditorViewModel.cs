//==============================================================
//  Copyright (C) 2020 Chongdaoyang Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/6/18 14:11:56 .
//  Version 1.0
//  CDYWORK
//==============================================================

using InSpiderDevelopServerClientAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace InSpiderDevelopWindow.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerUserEditorViewModel:ViewModelBase
    {

        #region ... Variables  ...

        private string mPassword;

        private string mNewPassword;

        private string mConfirmPassword;

        private ICommand mModifyCommand;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public ICommand ModifyCommand
        {
            get
            {
                if(mModifyCommand==null)
                {
                    mModifyCommand = new RelayCommand(() => {
                        if (DevelopServiceHelper.Helper.ModifyUserPassword(UserName == null ? "" : UserName, mPassword == null ? "" : mPassword, mNewPassword == null ? "" : mNewPassword))
                        {
                            System.Windows.MessageBox.Show(Res.Get("SetPasswordSeccussful"));
                        }
                        else
                        {
                            System.Windows.MessageBox.Show(Res.Get("SetPasswordfail"));
                        }
                    },()=> { return mNewPassword == mConfirmPassword; });
                }
                return mModifyCommand;
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public string ConfirmPassword
        {
            get
            {
                return mConfirmPassword;
            }
            set
            {
                if (mConfirmPassword != value)
                {
                    mConfirmPassword = value;
                    OnPropertyChanged("ConfirmPassword");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string NewPassword
        {
            get
            {
                return mNewPassword;
            }
            set
            {
                if (mNewPassword != value)
                {
                    mNewPassword = value;
                    OnPropertyChanged("NewPassword");
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
                return CurrentUserManager.Manager.UserName;
            }
            set
            {
                ;
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


        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
