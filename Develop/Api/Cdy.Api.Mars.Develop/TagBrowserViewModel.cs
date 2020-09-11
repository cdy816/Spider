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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Cdy.Spider.DevelopCommon;

namespace Cdy.Api.Mars
{
    /// <summary>
    /// 
    /// </summary>
    public class TagBrowserViewModel:WindowViewModelBase
    {

        #region ... Variables  ...
        
        private string mCurrentDatabase;

        DBDevelopClientWebApi.DevelopServiceHelper mHelper;

        private List<string> mDatabase;

        /// <summary>
        /// 
        /// </summary>
        private ObservableCollection<TagGroupViewModel> mTagGroups = new ObservableCollection<TagGroupViewModel>();

        /// <summary>
        /// 
        /// </summary>
        private ObservableCollection<TagViewModel> mTags = new ObservableCollection<TagViewModel>();

        private string mCurrentGroup;

        private TagViewModel mCurrentSelectTag;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        public TagBrowserViewModel()
        {
            Title = Res.Get("TagBrowser");
            DefaultWidth = 1024;
            DefaultHeight = 768;
        }

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public TagViewModel CurrentSelectTag
        {
            get
            {
                return mCurrentSelectTag;
            }
            set
            {
                if (mCurrentSelectTag != value)
                {
                    mCurrentSelectTag = value;
                    OnPropertyChanged("CurrentSelectTag");
                }
            }
        }


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
                    mCurrentGroup = "";
                    UpdateTagGroup();
                    UpdateTags();
                    OnPropertyChanged("CurrentDatabase");
                }
            }
        }

        public List<string> Databases 
        { 
            get { return mDatabase; }
            set 
            { 
                mDatabase = value;
                OnPropertyChanged("Databases");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CurrentGroup
        {
            get
            {
                return mCurrentGroup;
            }
            set
            {
                if (mCurrentGroup != value)
                {
                    mCurrentGroup = value;
                    OnPropertyChanged("CurrentGroup");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DataGrid Grid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SelectTagName
        {
            get
            {
                return Grid.SelectedItem != null ? (Grid.SelectedItem as TagViewModel).FullName : string.Empty;
            }
        }


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
            Task.Run(() => { QueryGroups(); });
        }

        /// <summary>
        /// 
        /// </summary>
        private void QueryGroups()
        {
            Application.Current?.Dispatcher.Invoke(() => {
                this.mTagGroups.Clear();
            });

            var vv = mHelper.GetTagGroup(CurrentDatabase);
            if (vv != null)
            {
                foreach (var vvv in vv.Where(e => string.IsNullOrEmpty(e.Parent)))
                {
                    Application.Current?.Dispatcher.Invoke(() => {
                        TagGroupViewModel groupViewModel = new TagGroupViewModel() { Name = vvv.Name, Database = CurrentDatabase };
                        mTagGroups.Add(groupViewModel);
                        groupViewModel.InitData(vv);
                    });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateTags()
        {
            mTags.Clear();
            var tags = mHelper.GetTagByGroup(CurrentDatabase, CurrentGroup, 0);
            if(tags!=null)
            {
                foreach(var vv in tags)
                {
                    mTags.Add(new TagViewModel() { Name = vv.Item1.Name, Desc = vv.Item1.Desc,Type = vv.Item1.Type.ToString() });
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool CanOKCommandProcess()
        {
            return Grid != null && Grid.SelectedItem != null;
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

        /// <summary>
        /// 
        /// </summary>
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
        public string FullName
        {
            get
            {
                return Parent != null ? Parent.FullName + "." + this.Name : this.Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TagGroupViewModel Parent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Database { get; set; }

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
                    OnPropertyChanged("IsExpended");
                }
            }
        }


        #endregion ...Properties...

        #region ... Methods    ...

        public void InitData(List<DBDevelopClientWebApi.TagGroup> groups)
        {
            foreach (var vv in groups.Where(e => e.Parent == this.FullName))
            {
                TagGroupViewModel groupViewModel = new TagGroupViewModel() { Name = vv.Name, Database = Database };
                groupViewModel.Parent = this;
                this.Children.Add(groupViewModel);
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
        public string Group { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        public string FullName
        {
            get
            {
                return string.IsNullOrEmpty(Group) ? Name : Group + "." + Name;
            }
        }


        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

}
