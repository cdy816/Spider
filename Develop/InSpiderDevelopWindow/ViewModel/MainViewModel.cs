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
using InSpiderDevelopServerClientAPI;
using RoslynPad.Roslyn.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Cdy.Api.Mars;

namespace InSpiderDevelopWindow
{
    public class MainViewModel : ViewModelBase, IProcessNotify
    {

        #region ... Variables  ...

        //private ICommand mLoginCommand;

        private string mCurrentMachine = string.Empty;

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

        private ICommand mStartCommand;

        private ICommand mStopCommand;

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

        private ICommand mLogoutCommand;

        private ICommand mLoginCommand;

        private ICommand mPublishCommand;

        private bool mIsLogin = false;

        private bool mIsMachineRunning = false;

        private System.Timers.Timer mCheckRunningTimer;

        private ServerSecurityTreeViewModel sec;

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
            mCheckRunningTimer = new System.Timers.Timer(1000);
          

            DevelopServiceHelper.Helper.OfflineCallBack = new Action(() => {
               
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Logout();
                });

                MessageBox.Show(Res.Get("serveroffline"));
            });

           
        }


        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public string Solution { get; set; } = "";

        public ICommand PublishCommand
        {
            get { 
                if(mPublishCommand == null)
                {
                    mPublishCommand = new RelayCommand(() => {
                        Publish(mCurrentMahineDoc);
                    }, () => { return IsLogin && mCurrentMahineDoc != null; });
                }
                return mPublishCommand; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMachineRunning
        {
            get
            {
                return mIsMachineRunning;
            }
            set
            {
                if (mIsMachineRunning != value)
                {
                    mIsMachineRunning = value;
                    OnPropertyChanged("IsMachineRunning");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ICommand StartCommand
        {
            get
            {
                if (mStartCommand == null)
                {
                    mStartCommand = new RelayCommand(() => {
                        CheckAndSaveMachine(mCurrentMahineDoc);
                        IsMachineRunning = DevelopServiceHelper.Helper.StartMachine(Solution, mCurrentMachine);
                    }, () => { return IsLogin && mCurrentMahineDoc!=null && !IsMachineRunning; });
                }
                return mStartCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand StopCommand
        {
            get
            {
                if (mStopCommand == null)
                {
                    mStopCommand = new RelayCommand(() => {
                        IsMachineRunning = !DevelopServiceHelper.Helper.StopMachine(Solution, mCurrentMachine);

                    }, () => { return IsLogin && mCurrentMahineDoc != null && IsMachineRunning; });
                }
                return mStopCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand LogoutCommand
        {
            get
            {
                if (mLogoutCommand == null)
                {
                    mLogoutCommand = new RelayCommand(() => {
                        CheckAndSave();
                        Logout();
                    }, () => { return IsLogin; });
                }
                return mLogoutCommand;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ICommand LoginCommand
        {
            get
            {
                if (mLoginCommand == null)
                {
                    mLoginCommand = new RelayCommand(() => {
                        Login();
                    });
                }
                return mLoginCommand;
            }
        }

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
                        if (mm.ShowDialog().Value)
                        {
                            MonitorParameter.Parameter.Server = mm.Server;
                            MonitorParameter.Parameter.UserName = mm.UserName;
                            MonitorParameter.Parameter.Password = mm.Password;
                            MonitorParameter.Parameter.ScanCircle = mm.ScanCircle;
                            MonitorParameter.Parameter.Save();
                        }
                    }, () =>{ return IsLogin; });
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
                    },()=> { return IsLogin && ContentViewModel is DeviceDetailViewModel&& IsMachineRunning; });
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
                    }, () => { return IsLogin; });
                }
                return mCancelCommand;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public string MainWindowTitle
        {
            get
            {
                return string.IsNullOrEmpty(Solution) ? Res.Get("MainWindowTitle"): Res.Get("MainWindowTitle") +"--"+this.Solution;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string CurrentMachine
        {
            get
            {
                return mCurrentMachine;
            }
            set
            {
                if (mCurrentMachine != value)
                {
                    mCurrentMachine = value;
                    Task.Run(() => {
                        IsMachineRunning = DevelopServiceHelper.Helper.IsMachineRunning(this.Solution, mCurrentMachine);
                    });
                   
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

                    }, () => { return IsLogin && mCurrentSelectTreeItem ==null || (mCurrentSelectTreeItem != null && mCurrentSelectTreeItem.CanAddChild()); });
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
                        UpdateMachine();
                        //mCurrentMahineDoc.IsDirty=false;
                        if (DevelopServiceHelper.Helper.Save(CurrentMachine))
                        {
                            MessageBox.Show(Res.Get("SaveSucessfull"));
                        }
                        else
                        {
                            MessageBox.Show(Res.Get("Savefailed"), Res.Get("erro"), MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }, () => { return IsLogin && !string.IsNullOrEmpty(CurrentMachine); });
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

        /// <summary>
        /// 
        /// </summary>
        public bool IsLogin
        {
            get
            {
                return mIsLogin;
            }
            set
            {
                if (mIsLogin != value)
                {
                    mIsLogin = value;
                    OnPropertyChanged("IsLogin");
                    OnPropertyChanged("IsLoginOut");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLoginOut
        {
            get
            {
                return !IsLogin;
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
        }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="machine"></param>
        private void Publish(MachineDocument machine)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "zip file|*.zip";
            if (ofd.ShowDialog().Value)
            {
                string sname = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(ofd.FileName), "tmp" + DateTime.Now.Ticks);

                if(!System.IO.Directory.Exists(sname))
                    System.IO.Directory.CreateDirectory(sname);

                CopyFile(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location),sname);

                var sdata = System.IO.Path.Combine(sname, "Data",this.mCurrentMahineDoc.Name);
                System.IO.Directory.CreateDirectory(sdata);

                this.mCurrentMahineDoc.Save(sdata);

                System.IO.Compression.ZipFile.CreateFromDirectory(sname, ofd.FileName);

                System.IO.Directory.Delete(sname, true);

                MessageBox.Show(Res.Get("completely"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="todirectory"></param>
        private void CopyFile(string directory,string todirectory)
        {
            foreach(var vv in System.IO.Directory.EnumerateFiles(directory))
            {
                if(IsFileFilter(vv))
                System.IO.File.Copy(vv, System.IO.Path.Combine(todirectory,System.IO.Path.GetFileName(vv)), true);
            }

            foreach (var vv in System.IO.Directory.EnumerateDirectories(directory))
            {
                var dname = new DirectoryInfo(vv).Name;
                if (IsDirectFilter(dname))
                {
                    string sname = System.IO.Path.Combine(todirectory, dname);
                    if (!System.IO.Directory.Exists(sname))
                    {
                        System.IO.Directory.CreateDirectory(sname);
                    }
                    CopyFile(vv, sname);
                }
            }

        }

        private bool IsFileFilter(string sfile)
        {
            if (sfile.EndsWith(".Develop.dll") || sfile.EndsWith(".Develop.resources.dll") || sfile.EndsWith("_b") || sfile.EndsWith("Develop.cfg") || sfile.EndsWith(".pdb") || sfile.EndsWith(".xml") || sfile== "InSpiderDevelopWindow.dll" || sfile== "InSpiderStudioServer.exe") return false;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsDirectFilter(string name)
        {
            if (name == "Data" || name== "CustomTemplate"||name== "TagBrowserCach") return false;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MCheckRunningTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            mCheckRunningTimer.Elapsed -= MCheckRunningTimer_Elapsed;
            if (!string.IsNullOrEmpty(mCurrentMachine))
            {
                var isrunning = DevelopServiceHelper.Helper.IsMachineRunning(this.Solution,mCurrentMachine);
                Application.Current?.Dispatcher.BeginInvoke(new Action(() => {
                    if (IsMachineRunning != isrunning)
                    {
                        IsMachineRunning = isrunning;
                        CommandManager.InvalidateRequerySuggested();
                    }
                }), null);
            }
            mCheckRunningTimer.Elapsed += MCheckRunningTimer_Elapsed;
        }

        public void CheckAndSave()
        {
            if (CheckMachineIsDirty())
            {
                if (MessageBox.Show(Res.Get("saveprompt"), "",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    SaveAllMachine();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        private void CheckAndSaveMachine(MachineDocument doc)
        {
            if(doc.IsDirty)
            {
                UpdateMachine(doc);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CheckMachineIsDirty()
        {
            foreach (var vv in DevelopManager.Manager.ListMachines(this.Solution))
            {
                if(vv.IsDirty)
                {
                    return true;
                }
            }
            return false;
        }

        private void SaveAllMachine()
        {
            foreach (var vv in DevelopManager.Manager.ListMachines(this.Solution))
            {
                if (vv.IsDirty)
                {
                    UpdateMachine(vv);

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartCheckDatabaseRunning()
        {
            mCheckRunningTimer.Elapsed += MCheckRunningTimer_Elapsed;
            mCheckRunningTimer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void StopCheckDatabaseRunning()
        {
            mCheckRunningTimer.Elapsed -= MCheckRunningTimer_Elapsed;
            mCheckRunningTimer.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Login()
        {
            LoginViewModel login = new LoginViewModel();
            if (login.ShowDialog().Value)
            {
                CurrentUserManager.Manager.UserName = login.UserName;
                CurrentUserManager.Manager.Password = login.Password;

                MonitorParameter.Parameter.Server= "http://"+ login.Server+":23232";


                //ServerHelper.Helper.Database = CurrentMachine;
                OnPropertyChanged("UserName");
                OnPropertyChanged("MainwindowTitle");
                IsLogin = true;

                Init();
                StartCheckDatabaseRunning();
            }
        }

        public void AutoLogin()
        {
            LoginViewModel login = new LoginViewModel();
            login.Server = ServerHelper.Helper.Server;
            login.UserName = ServerHelper.Helper.UserName;
            login.Password= ServerHelper.Helper.Password;
            Solution = ServerHelper.Helper.Solution;

          
            if (login.Login())
            {
                CurrentUserManager.Manager.UserName = login.UserName;
                CurrentUserManager.Manager.Password = login.Password;

                MonitorParameter.Parameter.Server = "http://" + login.Server + ":23232";


                //ServerHelper.Helper.Database = CurrentMachine;
                OnPropertyChanged("UserName");
                OnPropertyChanged("MainwindowTitle");
                IsLogin = true;

                Init();
                StartCheckDatabaseRunning();

                //实现数据库的自动连接
                MarsApiDevelop.Server = "http://" + login.Server+ ":9000";
                MarsApiDevelop.User = login.UserName;
                MarsApiDevelop.Password = login.Password;
                MarsApiDevelop.Database = Solution;
            }
        }

        private void Logout()
        {
            IsLogin = false;
            StopCheckDatabaseRunning();

            CurrentUserManager.Manager.UserName = string.Empty;
            OnPropertyChanged("UserName");
            if (ContentViewModel != null)
            {

                ContentViewModel.Dispose();
            }

            ContentViewModel = infoModel;
            CurrentMachine = string.Empty;

           

            mItems.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddMachine()
        {
            string sname = DevelopManager.Manager.ListMachinesNames(this.Solution).GetAvaiableName("project");
            sname = DevelopServiceHelper.Helper.NewMachine(Solution,sname) ;
            if (!string.IsNullOrEmpty(sname))
            {
                var mm = DevelopManager.Manager.NewMachine("",sname);
                var vmm = new MachineViewModel() { Model = mm, Parent = this };
                mProject.Children.Add(vmm);
                vmm.IsExpanded = true;
                vmm.IsSelected = true;
                vmm.IsEdit = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void UpdateMachine()
        {
            var model = DevelopManager.Manager.Machines[""][CurrentMachine];
            UpdateMachine(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        private void UpdateMachine(MachineDocument model)
        {
            Dictionary<string, string> dtmp = new Dictionary<string, string>();
            dtmp.Add("Api", model.Api.SaveToString());
            dtmp.Add("Channel", model.Channel.SaveToString());
            dtmp.Add("Device", model.Device.SaveToString());
            dtmp.Add("Driver", model.Driver.SaveToString());
            dtmp.Add("Link", model.Link.SaveToString());
            if(DevelopServiceHelper.Helper.UpdateMachine(Solution, model.Name,dtmp))
            {
                model.IsDirty= false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void RemoveMachine(MachineViewModel model)
        {
            model.Parent = null;
            if (DevelopServiceHelper.Helper.RemoveMachine(Solution, model.Name))
            {
                DevelopManager.Manager.Remove("", model.Name);
                mProject.Children.Remove(model);
                //mItems.Remove(model);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public bool RenameMachine(MachineViewModel model, string oldName,string newName)
        {
            if (DevelopServiceHelper.Helper.ReNameMachine(Solution, oldName,newName))
            {
                DevelopManager.Manager.ReName("",oldName,newName);
                return true;
            }
            return false;
        }

        private ProjectItemViewModel mProject;


        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            mItems.Clear();

            mProject = new ProjectItemViewModel() { Parent = this };

            mItems.Add(mProject);

            if (!ServerHelper.Helper.AutoLogin)
            {
                var sec = new ServerSecurityTreeViewModel();
                sec.Children.Add(new ServerUserEditorTreeViewModel());
                if (DevelopServiceHelper.Helper.IsAdmin())
                {
                    sec.Children.Add(new ServerUserManagerTreeViewModel());
                }
                mItems.Add(sec);
            }

            ValueConvertManager.manager.Init();

            DevelopManager.Manager.Clear();
            try
            {
                foreach (var vv in DevelopServiceHelper.Helper.LoadMachines(this.Solution))
                {
                    MachineDocument md = new MachineDocument() { Name = vv.Key };
                    md.Api = new APIDocument().LoadFromString(vv.Value["Api"]);
                    using (Context context = new Context())
                    {
                        md.Driver = new DriverDocument().LoadFromString(vv.Value["Driver"], context);
                        md.Channel = new ChannelDocument().LoadFromString(vv.Value["Channel"], context);
                        md.Device = new DeviceDocument().LoadFromString(vv.Value["Device"], context);
                        md.Link = new LinkDocument().LoadFromString(vv.Value["Link"]);
                    }
                    DevelopManager.Manager.Add(md);
                }

                foreach (var vv in DevelopManager.Manager.ListMachines(""))
                {
                    mProject.Children.Add(new MachineViewModel { Model = vv, Parent = this });
                }

                if (mItems.Count > 0)
                {
                    CurrentSelectTreeItem = mItems[0];
                    CurrentSelectTreeItem.IsExpanded = true;
                    CurrentSelectTreeItem.IsSelected = true;
                }

                mProject.IsExpanded= true;
            }
            catch
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reload()
        {
            if (DevelopServiceHelper.Helper.Cancel())
            {
                Init();
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
            GetCurrentMachine();
        }

        private MachineDocument mCurrentMahineDoc;

        private void GetCurrentMachine()
        {
            var vv = GetToRoot(mCurrentSelectTreeItem);
            if(vv is MachineViewModel)
            {
                CurrentMachine = vv.Name;
                mCurrentMahineDoc = (vv as MachineViewModel).Model;
            }
            else
            {
                CurrentMachine = "";
                mCurrentMahineDoc = null;
            }
        }

        private TreeItemViewModel GetToRoot(TreeItemViewModel item)
        {
            if(item == null) return null;
            if(item is MachineViewModel) return item;
            else
            {
                return GetToRoot(item.Parent);
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
