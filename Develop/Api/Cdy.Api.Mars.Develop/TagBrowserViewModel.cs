//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/9/8 9:29:38.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Cdy.Spider.DevelopCommon;

namespace Cdy.Api.Mars.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class TagBrowserViewModel:ViewModelBase
    {

        #region ... Variables  ...
        
        private string mCurrentDatabase;

        DBDevelopClientWebApi.DevelopServiceHelper mHelper;

        private List<string> mDatabase;

        private ObservableCollection<TagGroupViewModel> mTagGroups = new ObservableCollection<TagGroupViewModel>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public MarsApiDevelop ApiDevelop { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CurrentDatabase
        {
            get
            {
                return mCurrentDatabase;
            }
            set
            {
                if (mCurrentDatabase != value)
                {
                    mCurrentDatabase = value;
                    OnPropertyChanged("CurrentDatabase");
                }
            }
        }

        public List<string> Databases { get { return mDatabase; } set { mDatabase = value; OnPropertyChanged("Databases"); } }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            try
            {
                mHelper = new DBDevelopClientWebApi.DevelopServiceHelper();
                mHelper.Server = ApiDevelop.Data.ServerIp + ":" + ApiDevelop.Data.Port;
                if (!mHelper.Server.StartsWith("http://"))
                {
                    mHelper.Server = "http://" + mHelper.Server;
                }
                if (mHelper.Login(ApiDevelop.Data.UserName, ApiDevelop.Data.Password))
                {
                    Databases = mHelper.QueryDatabase().Select(e => e.Name).ToList();
                }
                else
                {
                    MessageBox.Show("Logging server failed!");
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateTagGroup()
        {
            mTagGroups.Clear();
            if(!string.IsNullOrEmpty(CurrentDatabase))
            {
                var database = mHelper.GetTagGroup(CurrentDatabase);
                if(database!=null)
                {
                    foreach(var vvv in database)
                    {
                        mTagGroups.Add(new TagGroupViewModel() { Name = vvv.Name });
                    }
                }
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 
    /// </summary>
    public class TagGroupViewModel : ViewModelBase
    {

        #region ... Variables  ...
        
        private bool mIsSelected;

        private bool mIsExpended;

        private ObservableCollection<TagGroupViewModel> mChildren = new ObservableCollection<TagGroupViewModel>();

        private bool mIsInited = false;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TagGroupViewModel> Children
        {
            get
            {
                return mChildren;
            }
        }


        /// <summary>
        /// 
        /// </summary>

        public string Name { get; set; }

        /// <summary>
            /// 
            /// </summary>
        public bool IsSelected
        {
            get
            {
                return mIsSelected;
            }
            set
            {
                if (mIsSelected != value)
                {
                    mIsSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public bool IsExpended
        {
            get
            {
                return mIsExpended;
            }
            set
            {
                if (mIsExpended != value)
                {
                    mIsExpended = value;
                    LoadChild();
                    OnPropertyChanged("IsExpended");
                }
            }
        }


        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        private void LoadChild()
        {
            if(!mIsInited)
            {
                mIsInited = true;
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 
    /// </summary>
    public class TagViewModel : ViewBase
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }
        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

}
