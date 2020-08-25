//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/4/4 17:34:05.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace InSpiderDevelopWindow
{
    public class TreeItemViewModel: ViewModelBase, ITreeItemViewModel
    {

        #region ... Variables  ...
        internal string mName="";
        private bool mIsSelected = false;
        private bool mIsExpanded = false;
        private bool mIsEdit;
        private ICommand mAddCommand;
        private ICommand mRenameCommand;
        private ICommand mRemoveCommand;
        private ICommand mCopyCommand;
        private ICommand mPasteCommand;
        private TreeItemViewModel mParent;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        public ICommand AddCommand
        {
            get
            {
                if(mAddCommand==null)
                {
                    mAddCommand = new RelayCommand(() => {
                        Add();
                    },()=> { return CanAddChild(); });
                }
                return mAddCommand;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ICommand CopyCommand
        {
            get
            {
                if(mCopyCommand==null)
                {
                    mCopyCommand = new RelayCommand(() => { Copy(); },()=> { return CanCopy(); });
                }
                return mCopyCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand PasteCommand
        {
            get
            {
                if(mPasteCommand==null)
                {
                    mPasteCommand = new RelayCommand(() => { Paste(); },()=> { return CanPaste(); });
                }
                return mPasteCommand;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public virtual string Name
        {
            get
            {
                return mName;
            }
            set
            {
                if (mName != value && !string.IsNullOrEmpty(value))
                {
                    string oldName = mName;
                    if(OnRename(oldName, value))
                    {
                        mName = value;
                    }
                }
                OnPropertyChanged("Name");
                IsEdit = false;
            }
        }

        //public string Database { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ReNameCommand
        {
            get
            {
                if (mRenameCommand == null)
                {
                    mRenameCommand = new RelayCommand(() => {
                        IsEdit = true;
                    },()=> { return CanReName(); });
                }
                return mRenameCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand RemoveCommand
        {
            get
            {
                if (mRemoveCommand == null)
                {
                    mRemoveCommand = new RelayCommand(() => {
                        Remove();
                    },()=> { return CanRemove(); });
                }
                return mRemoveCommand;
            }
        }

        public bool IsEdit
        {
            get
            {
                return mIsEdit;
            }
            set
            {
                if (mIsEdit != value)
                {
                    mIsEdit = value;
                }
                OnPropertyChanged("IsEdit");
            }
        }

        /// <summary>
        /// 被选中
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
        /// 展开
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return mIsExpanded;
            }
            set
            {
                if (mIsExpanded != value)
                {
                    mIsExpanded = value;
                    OnIsExpended();
                    OnPropertyChanged("IsExpanded");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TreeItemViewModel Parent
        {
            get
            {
                return mParent;
            }
            set
            {
                if (mParent != value)
                {
                    mParent = value;
                    OnPropertyChanged("Parent");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual string FullName { get { return Parent != null ? Parent.FullName + "." + this.Name : this.Name; } }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnIsExpended()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ViewModelBase GetModel(ViewModelBase mode)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Add()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Copy()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Paste()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool CanCopy()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool CanPaste()
        {
            return false;
        }


        public virtual bool CanAddChild()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool CanReName()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Remove()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool CanRemove()
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public virtual bool OnRename(string oldName, string newName)
        {
            return true;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }


    public class HasChildrenTreeItemViewModel:TreeItemViewModel
    {

        #region ... Variables  ...
        private System.Collections.ObjectModel.ObservableCollection<TreeItemViewModel> mChildren = new System.Collections.ObjectModel.ObservableCollection<TreeItemViewModel>();

        private bool mIsLoaded = false;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        

        /// <summary>
        /// 
        /// </summary>
        public System.Collections.ObjectModel.ObservableCollection<TreeItemViewModel> Children
        {
            get
            {
                return mChildren;
            }
        }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void PreLoadChildForExpend(bool value)
        {
            if (value) mChildren.Add(new TreeItemViewModel());
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnIsExpended()
        {
            if(!mIsLoaded)
            {
                mChildren.Clear();
                LoadData();
                mIsLoaded = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void LoadData()
        {

        }

        public override void Dispose()
        {
            foreach(var vv in Children)
            {
                vv.Dispose();
            }
            base.Dispose();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

         
    public interface ITreeItemViewModel
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
        string Name { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
