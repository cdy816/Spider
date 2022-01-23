//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/3/28 10:54:26.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Timers;
using Cdy.Spider;
using InSpiderDevelopWindow.ViewModel;
using InSpiderDevelop;
using System.Windows.Interop;

namespace InSpiderDevelopWindow
{
    public class MainViewModel : ViewModelBase, IProcessNotify
    {

        #region ... Variables  ...

        //private ICommand mLoginCommand;

        private string mDatabase = string.Empty;

        private ICommand mSaveCommand;

        private ICommand mAddGroupCommand;

        private ICommand mAddCommand;
        
        private ICommand mRemoveCommand;

        private ICommand mCancelCommand;

        private ICommand mCopyCommand;

        private ICommand mPasteCommand;

        private ICommand mReNameCommand;

        private ICommand mMonitorCommand;

        private ICommand mMonitorSettingCommand;

        private TreeItemViewModel mCurrentSelectTreeItem;

        private System.Collections.ObjectModel.ObservableCollection<TreeItemViewModel> mItems = new System.Collections.ObjectModel.ObservableCollection<TreeItemViewModel>();

        private ViewModelBase mContentViewModel;

        private bool mIsCanOperate = true;

        private double mProcessNotify;

        private Visibility mNotifyVisiblity = Visibility.Hidden;

        //private bool mIsLogin;

        //private bool mIsDatabaseRunning;

        //private System.Timers.Timer mCheckRunningTimer;

        private SpiderInfoViewModel infoModel;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        public MainViewModel()
        {
            ServiceLocator.Locator.Registor<IProcessNotify>(this);
            infoModel = new SpiderInfoViewModel();
            mContentViewModel = infoModel;
            Init();
        }

        
        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public ICommand MonitorSettingCommand
        {
            get
            {
                if(mMonitorSettingCommand == null)
                {
                    mMonitorSettingCommand = new RelayCommand(() => {
                        MonitorSettingViewModel mm = new MonitorSettingViewModel();
                        mm.Server = MonitorParameter.Parameter.Server;
                        mm.UserName = MonitorParameter.Parameter.UserName;
                        mm.Password = MonitorParameter.Parameter.Password;
                        mm.ScanCircle = MonitorParameter.Parameter.ScanCircle;
                        if(mm.ShowDialog().Value)
                        {
                            MonitorParameter.Parameter.Server = mm.Server;
                            MonitorParameter.Parameter.UserName = mm.UserName;
                            MonitorParameter.Parameter.Password = mm.Password;
                            MonitorParameter.Parameter.ScanCircle = mm.ScanCircle;
                            MonitorParameter.Parameter.Save();
                        }
                    });
                }
                return mMonitorSettingCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool IsMonitor
        {
            get
            {
                if(ContentViewModel is DeviceDetailViewModel)
                {
                    return (ContentViewModel as DeviceDetailViewModel).IsMonitMode;
                }
                else
                {
                    return false;
                }
            }
        }

        public string MonitorString
        {
            get 
            { 
                return !IsMonitor?Res.Get("Monitor"):Res.Get("Stop"); 
            }
            set
            {
                ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand MonitorCommand
        {
            get
            {
                if(mMonitorCommand==null)
                {
                    mMonitorCommand = new RelayCommand(() => {
                        (ContentViewModel as DeviceDetailViewModel).StartMonitCommand.Execute(null);
                        OnPropertyChanged("MonitorString");
                    },()=> { return ContentViewModel is DeviceDetailViewModel; });
                }
                return mMonitorCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ReNameCommand
        {
            get
            {
                if(mReNameCommand==null)
                {
                    mReNameCommand = new RelayCommand(() => {
                        CurrentSelectTreeItem.ReNameCommand.Execute(null);
                    },()=> { return CurrentSelectTreeItem != null && CurrentSelectTreeItem.CanRemove(); });
                }
                return mReNameCommand;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                if(mCancelCommand==null)
                {
                    mCancelCommand = new RelayCommand(() => { 
                        if(MessageBox.Show(Res.Get("canceltosavemsg"),"",MessageBoxButton.YesNo)== MessageBoxResult.Yes)
                        {
                            Reload();
                        }
                    });
                }
                return mCancelCommand;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public string MainwindowTitle
        {
            get
            {
                return string.IsNullOrEmpty(Database) ? Res.Get("MainwindowTitle"): Res.Get("MainwindowTitle")+"--"+this.Database;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string Database
        {
            get
            {
                return mDatabase;
            }
            set
            {
                if (mDatabase != value)
                {
                    mDatabase = value;
                    OnPropertyChanged("Database");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public Visibility NotifyVisiblity
        {
            get
            {
                return mNotifyVisiblity;
            }
            set
            {
                if (mNotifyVisiblity != value)
                {
                    mNotifyVisiblity = value;
                    OnPropertyChanged("NotifyVisiblity");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public double ProcessNotify
        {
            get
            {
                return mProcessNotify;
            }
            set
            {
                if (mProcessNotify != value)
                {
                    mProcessNotify = value;
                    OnPropertyChanged("ProcessNotify");
                    OnPropertyChanged("ProcessNotifyPercent");
                }
            }
        }

        public double ProcessNotifyPercent
        {
            get
            {
                return mProcessNotify / 100;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public bool IsCanOperate
        {
            get
            {
                return mIsCanOperate;
            }
            set
            {
                if (mIsCanOperate != value)
                {
                    mIsCanOperate = value;
                    OnPropertyChanged("IsCanOperate");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ViewModelBase ContentViewModel
        {
            get
            {
                return mContentViewModel;
            }
            set
            {
                if(mContentViewModel!=value)
                {
                    mContentViewModel = value;
                    OnPropertyChanged("ContentViewModel");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Collections.ObjectModel.ObservableCollection<TreeItemViewModel> Items
        {
            get
            {
                return mItems;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ICommand AddGroupCommand
        {
            get
            {
                if (mAddGroupCommand == null)
                {
                    mAddGroupCommand = new RelayCommand(() => {
                        (CurrentSelectTreeItem).IsExpanded = true;
                        Application.Current?.Dispatcher.BeginInvoke(new Action(() => {
                            (CurrentSelectTreeItem).AddGroupCommand.Execute(null);
                        }));
                        
                    },()=> { return mCurrentSelectTreeItem != null && mCurrentSelectTreeItem.CanAddGroup(); });
                }
                return mAddGroupCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand AddCommand
        {
            get
            {
                if(mAddCommand==null)
                {
                    mAddCommand = new RelayCommand(() => {
                        if (mCurrentSelectTreeItem != null)
                        {
                            (CurrentSelectTreeItem).IsExpanded = true;
                            (CurrentSelectTreeItem).AddCommand.Execute(null);
                        }
                        else
                        {
                            AddMachine();
                        }

                    }, () => { return mCurrentSelectTreeItem==null || (mCurrentSelectTreeItem != null && mCurrentSelectTreeItem.CanAddChild()); });
                }
                return mAddCommand;
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
                        (CurrentSelectTreeItem).RemoveCommand.Execute(null);
                    },()=> { return CurrentSelectTreeItem != null && CurrentSelectTreeItem.CanRemove() ; });
                }
                return mRemoveCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                if(mSaveCommand==null)
                {
                    mSaveCommand = new RelayCommand(() => {

                        if (DevelopManager.Manager.Save())
                        {
                            MessageBox.Show(Res.Get("SaveSucessfull"));
                        }
                        else
                        {
                            MessageBox.Show(Res.Get("Savefailed"), Res.Get("erro"), MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        
                    }, () => { return string.IsNullOrEmpty(Database); });
                }
                return mSaveCommand;
            }
        }

        public ICommand PasteCommand
        {
            get
            {
                if(mPasteCommand==null)
                {
                    mPasteCommand = new RelayCommand(() => {
                        CurrentSelectTreeItem.PasteCommand.Execute(null);
                    },
                    () => 
                    {
                        return CurrentSelectTreeItem != null && CurrentSelectTreeItem.PasteCommand.CanExecute(null); 
                    });
                }
                return mPasteCommand;
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
                    mCopyCommand = new RelayCommand(() =>
                    {
                        CurrentSelectTreeItem.CopyCommand.Execute(null);
                    },
                    () =>
                    { 
                        return CurrentSelectTreeItem != null && CurrentSelectTreeItem.CopyCommand.CanExecute(null);
                    });
                }
                return mCopyCommand;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public TreeItemViewModel CurrentSelectTreeItem
        {
            get
            {
                return mCurrentSelectTreeItem;
            }
            set
            {
                if (mCurrentSelectTreeItem != value)
                {
                    if(mCurrentSelectTreeItem!=null)
                    {
                        mCurrentSelectTreeItem.IsSelected = false;
                    }

                    mCurrentSelectTreeItem = value;

                    
                    SelectContentModel();
                    OnPropertyChanged("CurrentSelectGroup");
                }
            }
        }



        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void AddMachine()
        {
            string sname = DevelopManager.Manager.ListMachinesNames().GetAvaiableName("project");
            var mm = DevelopManager.Manager.NewMachine(sname);
            var vmm = new MachineViewModel() { Model = mm,Parent=this };
            mItems.Add(vmm);
            vmm.IsExpanded = true;
            vmm.IsSelected = true;
            vmm.IsEdit = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void RemoveMachine(MachineViewModel model)
        {
            model.Parent = null;
            DevelopManager.Manager.Remove(model.Name);
            mItems.Remove(model);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            mItems.Clear();
            ValueConvertManager.manager.Init();
            DevelopManager.Manager.Load();
            foreach(var vv in DevelopManager.Manager.ListMachines())
            {
                mItems.Add(new MachineViewModel { Model = vv,Parent=this});
            }

            if(mItems.Count>0)
            {
                CurrentSelectTreeItem = mItems[0];
                CurrentSelectTreeItem.IsExpanded = true;
                CurrentSelectTreeItem.IsSelected = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reload()
        {
            mItems.Clear();
            ValueConvertManager.manager.Init();
            DevelopManager.Manager.ReLoad();
            foreach (var vv in DevelopManager.Manager.ListMachines())
            {
                mItems.Add(new MachineViewModel { Model = vv, Parent = this });
            }

            if (mItems.Count > 0)
            {
                CurrentSelectTreeItem = mItems[0];
                CurrentSelectTreeItem.IsExpanded = true;
                CurrentSelectTreeItem.IsSelected = true;
            }
        }


        private void SelectContentModel()
        {
            if (ContentViewModel is IModeSwitch)
            {
                (ContentViewModel as IModeSwitch).DeActive();
            }

            if (mCurrentSelectTreeItem != null)
                ContentViewModel = mCurrentSelectTreeItem.GetModel(ContentViewModel);

            if (ContentViewModel == null) ContentViewModel = infoModel;

            if (ContentViewModel is IModeSwitch)
            {
                (ContentViewModel as IModeSwitch).Active();
            }

        }


        /// <summary>
        /// 
        /// </summary>
        public void BeginShowNotify()
        {
            Application.Current?.Dispatcher.BeginInvoke(new Action(() => {

                NotifyVisiblity = Visibility.Visible;
            }));
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void ShowNotifyValue(double val)
        {
            Application.Current?.Dispatcher.BeginInvoke(new Action(() => {
                if (val > 100)
                {
                    ProcessNotify = 100;
                }
                else
                {
                    ProcessNotify = val;
                }
            }));
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void EndShowNotify()
        {
            Application.Current?.Dispatcher.BeginInvoke(new Action(() => {
                NotifyVisiblity = Visibility.Hidden;
                ProcessNotify = 0;
            }));
           
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //public interface ITagGroupAdd
    //{
    //    bool AddGroup(string parent);
    //}

    /// <summary>
    /// 
    /// </summary>
    public interface IProcessNotify
    {
        /// <summary>
        /// 
        /// </summary>
        void BeginShowNotify();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        void ShowNotifyValue(double val);
        /// <summary>
        /// 
        /// </summary>
        void EndShowNotify();
    }


}
