//==============================================================
//  Copyright (C) 2020 Chongdaoyang Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/25 21:14:10 .
//  Version 1.0
//  CDYWORK
//==============================================================

using Cdy.Api.Mars;
using Cdy.Spider;
using Cdy.Spider.DevelopCommon;
using InSpiderDevelop;
using Microsoft.CodeAnalysis;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;

namespace InSpiderDevelopWindow.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ProtocolItem
    {
        public string Name { get; set; }
        public string Desc { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DeviceDetailViewModel:ViewModelBase, IModeSwitch, IDeviceDevelopService
    {

        #region ... Variables  ...

        public static DataGrid mGrid;

        private System.Collections.ObjectModel.ObservableCollection<TagViewModel> mTags = new System.Collections.ObjectModel.ObservableCollection<TagViewModel>();

        private IDeviceDevelop mModel;

        private ICommand mAddCommand;
        private ICommand mRemoveCommand;
        private ICommand mImportCommand;
        private ICommand mExportCommand;
        private ICommand mShareChannelCommand;
        private ICommand mConfigDatabaseCommand;
        private ICommand mConfigDriverRegisorCommand;

        private ICommand mCopyCommand;
        private ICommand mCellCopyCommand;
        private ICommand mPasteCommand;
        private ICommand mCellPasteCommand;

        private ICommand mAddFromDatabaseCommand;

        private ICommand mStartMonitCommand;

        private static List<TagViewModel> mCopyTags = new List<TagViewModel>();

        private Dictionary<string, string> mFilters = new Dictionary<string, string>();

        private bool mEnableFilter = true;

        private Tuple<TagViewModel, int> mPropertyCopy;

        private DataGridSelectionUnit mSelectMode = DataGridSelectionUnit.FullRow;

        private TagViewModel mCurrentSelectTag;

        private int mTagCount = 0;

        private bool mIsLoaded = false;

        private int mPerPageCount = 200;

        private int mCurrentPage = 0;

        private string mFilterRegistorName;
        private bool mRegistorFilterEnable;
        private int mFilterType=-1;
        private string mFilterKeyName;
        private bool mTagTypeFilterEnable;

        private int mDirectionFilter=-1;
        private bool mDirectionFilterEnable;

        private List<int> mIdCach = new List<int>();

        static List<string> mChannelList = new List<string>();
        static List<ProtocolItem> mProtocolList = new List<ProtocolItem>();

        private ICollectionView mChannelView;

        private MachineDocument mMachineModel;

        private string mProtocolName;
        private string mChannelName;

        private object mDriverConfig;
        private object mChannelConfig;

        private string mShareChannel;

        private List<string> mSupportChannels = new List<string>();

        private IDriverDevelop mDriver;

        private int mSelectIndex=1;

        private bool mIsMonitMode = false;

        private ProtocolItem mSelectProtocol;

        private bool mIsDirty = false;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        static DeviceDetailViewModel()
        {
            mProtocolList.Add(new ProtocolItem());
            mProtocolList.AddRange(ServiceLocator.Locator.Resolve<IDriverFactory>().ListDevelopInstance().Select(e=>new ProtocolItem() { Name = e.TypeName, Desc = e.Desc }));
            mChannelList.Insert(0, "");
            mChannelList.AddRange(ServiceLocator.Locator.Resolve<ICommChannelFactory2>().ListDevelopInstance().Select(e => e.TypeName));
        }

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
            /// 
            /// </summary>
        public bool IsDirty
        {
            get
            {
                return mIsDirty;
            }
            set
            {
                if (mIsDirty != value)
                {
                    mIsDirty = value;
                    if(mMachineModel!=null&&value)
                    {
                        mMachineModel.IsDirty = true;
                    }
                    OnPropertyChanged("IsDirty");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool IsMonitMode
        {
            get
            {
                return mIsMonitMode;
            }
            set
            {
                if (mIsMonitMode != value)
                {
                    mIsMonitMode = value;
                    OnPropertyChanged("IsMonitMode");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public  ICommand StartMonitCommand
        {
            get
            {
                if(mStartMonitCommand==null)
                {
                    mStartMonitCommand = new RelayCommand(() => { 
                        if(!mIsMonitMode)
                        {
                            StartRealDataMonitor();
                        }
                        else
                        {
                            StopRealDataMonitor();
                        }
                    });
                }
                return mStartMonitCommand;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ICommand ConfigDriverRegisorCommand
        {
            get
            {
                if(mConfigDriverRegisorCommand==null)
                {
                    mConfigDriverRegisorCommand = new RelayCommand(() => {
                        try
                        {
                            var rc = (Model as DeviceDevelop).Driver.RegistorConfig();
                            rc.Service = this;

                            var vtags = rc.Config();
                            if (vtags != null)
                            {
                                int i = 0;
                                foreach (var vv in vtags)
                                {
                                    if (i < this.grid.SelectedItems.Count)
                                    {
                                        (this.grid.SelectedItems[i] as TagViewModel).DeviceInfo = vv;
                                        IsDirty= true;
                                    }
                                    i++;
                                }
                            }
                        }
                        catch
                        {

                        }

                    });
                }
                return mConfigDriverRegisorCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Visibility IsChannelVisiable
        {
            get
            {
                return mSupportChannels.Count > 0 ? Visibility.Visible : Visibility.Hidden;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int SelectIndex
        {
            get
            {
                return mSelectIndex;
            }
            set
            {
                if (mSelectIndex != value)
                {
                    mSelectIndex = value;
                    CheckChannelAndDriverChanged();
                    OnPropertyChanged("SelectIndex");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ICommand ConfigDatabaseCommand
        {
            get
            {
                if(mConfigDatabaseCommand==null)
                {
                    mConfigDatabaseCommand = new RelayCommand(() => {
                        var vtags = mMachineModel.Api.Api.ConfigMutiTags();
                        if (vtags != null)
                        {
                            int i = 0;
                            foreach (var vv in vtags)
                            {
                                if (i < this.grid.SelectedItems.Count)
                                {
                                    (this.grid.SelectedItems[i] as TagViewModel).DatabaseName = vv;
                                    IsDirty = true;
                                }
                                i++;
                            }
                        }
                    },()=> { return mMachineModel != null && mMachineModel.Api != null; });
                }
                return mConfigDatabaseCommand;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ICollectionView ChannelView
        {
            get
            {
                if(mChannelView==null)
                {
                    mChannelView = CollectionViewSource.GetDefaultView(mChannelList);
                    mChannelView.Filter = new Predicate<object>((e) => 
                    {
                        return mSupportChannels.Contains(e)||e.Equals("");
                    });
                }
                return mChannelView;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public List<ProtocolItem> ProtocolList
        {
            get
            {
                return mProtocolList;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] TagTypeList
        {
            get
            {
                return TagViewModel.mTagTypeList;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] DirectionList { get { return TagViewModel.mDataTranseDirection; }  }

        /// <summary>
        /// 
        /// </summary>
        public string FilterRegistorName
        {
            get
            {
                return mFilterRegistorName;
            }
            set
            {
                if (mFilterRegistorName != value)
                {
                    mFilterRegistorName = value;
                    NewQueryTags();
                    OnPropertyChanged("FilterRegistorName");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool RegistorFilterEnable
        {
            get
            {
                return mRegistorFilterEnable;
            }
            set
            {
                if (mRegistorFilterEnable != value)
                {
                    mRegistorFilterEnable = value;
                    NewQueryTags();
                    if (!value) mFilterRegistorName = string.Empty;
                    OnPropertyChanged("RegistorFilterEnable");
                    OnPropertyChanged("FilterRegistorName");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int FilterType
        {
            get
            {
                return mFilterType;
            }
            set
            {
                if (mFilterType != value)
                {
                    mFilterType = value;
                    NewQueryTags();
                }
                OnPropertyChanged("FilterType");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool TagTypeFilterEnable
        {
            get
            {
                return mTagTypeFilterEnable;
            }
            set
            {
                if (mTagTypeFilterEnable != value)
                {
                    mTagTypeFilterEnable = value;
                    if (!value)
                    {
                        mFilterType = -1;
                        NewQueryTags();
                    }
                }
                OnPropertyChanged("TagTypeFilterEnable");
                OnPropertyChanged("FilterType");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string FilterKeyName
        {
            get
            {
                return mFilterKeyName;
            }
            set
            {
                if (mFilterKeyName != value)
                {
                    mFilterKeyName = value;
                    NewQueryTags();
                }
                OnPropertyChanged("FilterKeyName");
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public int DirectionFilter
        {
            get
            {
                return mDirectionFilter;
            }
            set
            {
                if (mDirectionFilter != value)
                {
                    mDirectionFilter = value;
                    NewQueryTags();
                    OnPropertyChanged("DirectionFilter");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public bool DirectionFilterEnable
        {
            get
            {
                return mDirectionFilterEnable;
            }
            set
            {
                if (mDirectionFilterEnable != value)
                {
                    mDirectionFilterEnable = value;
                    if (!value)
                    {
                        mDirectionFilter = -1;
                        NewQueryTags();
                    }
                    OnPropertyChanged("DirectionFilterEnable");
                    OnPropertyChanged("DirectionFilter");
                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TagViewModel> Tags
        {
            get
            {
                return mTags;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDeviceDevelop Model
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
                    OnPropertyChanged("Model");
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public MachineDocument MachineModel
        {
            get
            {
                return mMachineModel;
            }
            set
            {
                if (mMachineModel != value)
                {
                    mMachineModel = value;
                    OnPropertyChanged("MachineModel");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool RowSelectMode
        {
            get
            {
                return mSelectMode == DataGridSelectionUnit.FullRow;
            }
            set
            {
                mSelectMode = DataGridSelectionUnit.FullRow;
                OnPropertyChanged("RowSelectMode");
                OnPropertyChanged("CellSelectMode");
                OnPropertyChanged("SelectMode");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CellSelectMode
        {
            get
            {
                return mSelectMode == DataGridSelectionUnit.CellOrRowHeader;
            }
            set
            {
                mSelectMode = DataGridSelectionUnit.CellOrRowHeader;
                OnPropertyChanged("CellSelectMode");
                OnPropertyChanged("RowSelectMode");
                OnPropertyChanged("SelectMode");
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public DataGridSelectionUnit SelectMode
        {
            get
            {
                return mSelectMode;
            }
            set
            {
                if (mSelectMode != value)
                {
                    mSelectMode = value;
                    OnPropertyChanged("SelectMode");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand CopyCommand
        {
            get
            {
                if (mCopyCommand == null)
                {
                    mCopyCommand = new RelayCommand(() => {
                        CopyTag();
                    }, () => { return RowSelectMode; });
                }
                return mCopyCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand CellCopyCommand
        {
            get
            {
                if (mCellCopyCommand == null)
                {
                    mCellCopyCommand = new RelayCommand(() => {
                        CopyTagProperty();
                    }, () => { return CellSelectMode && SelectedCells != null && SelectedCells.Count() > 0; });
                }
                return mCellCopyCommand;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ICommand PasteCommand
        {
            get
            {
                if (mPasteCommand == null)
                {
                    mPasteCommand = new RelayCommand(() => {
                        PasteTag();
                    }, () => { return mCopyTags.Count > 0; });
                }
                return mPasteCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand CellPasteCommand
        {
            get
            {
                if (mCellPasteCommand == null)
                {
                    mCellPasteCommand = new RelayCommand(() => {
                        PasteTagProperty();
                    }, () => { return CanPasteTagProperty(); });
                }
                return mCellPasteCommand;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ICommand AddCommand
        {
            get
            {
                if (mAddCommand == null)
                {
                    mAddCommand = new RelayCommand(() => {
                        NewTag();
                    });
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
                        RemoveTag();
                    }, () => { return CurrentSelectTag != null || (SelectedCells != null && SelectedCells.Count > 0); });
                }
                return mRemoveCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand AddFromDatabaseCommand
        {
            get
            {
                if(mAddFromDatabaseCommand== null)
                {
                    mAddFromDatabaseCommand = new RelayCommand(() =>
                    {
                        ProcessFromDatabaseCommand();
                    });
                }
                return mAddFromDatabaseCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ExportCommand
        {
            get
            {
                if (mExportCommand == null)
                {
                    mExportCommand = new RelayCommand(() => {
                        ExportToFile();
                    });
                }
                return mExportCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ImportCommand
        {
            get
            {
                if (mImportCommand == null)
                {
                    mImportCommand = new RelayCommand(() => {
                        ImportFromFile();
                    });
                }
                return mImportCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ShareChannelCommand
        {
            get
            {
                if(mShareChannelCommand==null)
                {
                    mShareChannelCommand = new RelayCommand(() => {

                        SelectShareChannel();
                    },()=> { return !string.IsNullOrEmpty(this.Model.Data.ChannelName); });
                }
                return mShareChannelCommand;
            }
        }



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
        public int TagCount
        {
            get
            {
                return mTagCount;
            }
            set
            {
                if (mTagCount != value)
                {
                    mTagCount = value;
                    OnPropertyChanged("TagCount");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<DataGridCellInfo> SelectedCells { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DataGrid grid { get; set; }


        /// <summary>
            /// 
            /// </summary>
        public bool EnableFilter
        {
            get
            {
                return mEnableFilter;
            }
            set
            {
                if (mEnableFilter != value)
                {
                    mEnableFilter = value;
                    OnPropertyChanged("EnableFilter");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public ProtocolItem SelectProtocol
        {
            get
            {
                return mSelectProtocol;
            }
            set
            {
                if (mSelectProtocol != value)
                {
                    mSelectProtocol = value;
                    ProtocolName = value != null ? value.Name : "";
                    OnPropertyChanged("SelectProtocol");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string ProtocolName
        {
            get
            {
                return mProtocolName;
            }
            set
            {
                if (mProtocolName != value)
                {
                    mProtocolName = value;
                    ChangedProtocol(value);
                    OnPropertyChanged("ProtocolName");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public string ChannelName
        {
            get
            {
                return mChannelName;
            }
            set
            {
                if (mChannelName != value)
                {
                    mChannelName = value;
                    ChangedChannel(value);
                    OnPropertyChanged("ChannelName");
                }
            }
        }

       

        /// <summary>
        /// 
        /// </summary>
        public object DriverConfig
        {
            get
            {
                return mDriverConfig;
            }
            set
            {
                if (mDriverConfig != value)
                {
                    mDriverConfig = value;
                    OnPropertyChanged("DriverConfig");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public object ChannelConfig
        {
            get
            {
                return mChannelConfig;
            }
            set
            {
                if (mChannelConfig != value)
                {
                    mChannelConfig = value;
                    OnPropertyChanged("ChannelConfig");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ShareChannel
        {
            get
            {
                return mShareChannel;
            }
            set
            {
                if (mShareChannel != value)
                {
                    mShareChannel = value;
                    OnPropertyChanged("ShareChannel");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public string[] RegistorList
        {
            get
            {
                return TagViewModel.mRegistorList;
            }
        }


        private string mDriverConfigString = "";
        private string mChannelConfigString = "";
        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        private void ProcessFromDatabaseCommand()
        {
            TagBrowserViewModel tbm;
            if (ServerHelper.Helper.AutoLogin)
            {
                tbm = new TagBrowserViewModel() { ServerAddress = MarsApiDevelop.Server,IsWorkStandard=false, ServerUserName = MarsApiDevelop.User, ServerPassword = MarsApiDevelop.Password, CurrentDatabase = MarsApiDevelop.Database };
            }
            else
            {
                tbm = new TagBrowserViewModel();
            }
            if (tbm.ShowDialog().Value)
            {
                foreach(var vv in tbm.GetSelectedTags())
                {
                    string sname = vv.FullName.Replace(tbm.CurrentDatabase + ".", "");
                    NewTag(sname, (TagType)(Enum.Parse(typeof(TagType),vv.Type)), vv.Desc);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void NewTag(string name,TagType type,string desc)
        {
            string sname = CheckAndNewName(name);
            Tagbase tag = null;
            switch (type)
            {
                case TagType.Bool:
                    tag = new BoolTag() { Name= sname };
                    break;
                case TagType.Byte:
                    tag = new ByteTag() { Name= sname };
                    break;
                case TagType.Short:
                    tag = new ShortTag() { Name= sname };
                    break;
                case TagType.UShort:
                    tag = new UShortTag() { Name= sname };
                    break;
                case TagType.Int:
                        tag = new IntTag() { Name= sname };
                    break;
                case TagType.UInt:
                    tag = new UIntTag() { Name= sname };
                    break;
                case TagType.Long:
                    tag = new LongTag() { Name= sname };
                    break;
                case TagType.ULong:
                    tag = new ULongTag() { Name= sname };
                    break;
                case TagType.Float:
                    tag = new FloatTag() { Name= sname };
                    break;
                case TagType.Double:
                    tag = new DoubleTag() { Name = sname }; ;
                    break;
                case TagType.String:
                    tag = new StringTag() { Name= sname };
                    break;
                case TagType.DateTime:
                    tag = new DateTimeTag() { Name= sname };
                    break;
            }
            if (tag == null) return;
            tag.DatabaseName = sname;
            tag.DataTranseDirection = DataTransType.Both;

            TagViewModel vtag = new TagViewModel() { Model = tag, Document = mModel.Data, Machine = this.MachineModel, Parent = this };
            mDriver?.CheckTagDeviceInfo(vtag.Model);
            if (mModel.Data.AppendTag(vtag.Model))
            {
                mTags.Add(vtag);
                CurrentSelectTag = vtag;
                CachTagModelId(vtag.Model);
            }
            IsDirty = true;
            TagCount++;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newProtocol"></param>
        private void ChangedProtocol(string newProtocol)
        {
            mMachineModel.Driver.RemoveDriver(Model.FullName);
            var dd = ServiceLocator.Locator.Resolve<IDriverFactory>().GetDevelopInstance(newProtocol);
            if (dd != null)
            {
                dd.Name = Model.FullName;
                mMachineModel.Driver.AddDriver(dd);
                DriverConfig = dd.Config();

                foreach(var vv in (mModel as DeviceDevelop).Data.Tags)
                {
                    dd.CheckTagDeviceInfo(vv.Value);
                }

                foreach(var vv in mTags)
                {
                    vv.FreshDeviceInfo();
                }
            }
            else
            {
                DriverConfig = null;
            }
            UpdateSupportChannel(dd);
            (Model as DeviceDevelop).Driver = dd;
            mDriver = dd;

            if (mDriver != null)
            {
                TagViewModel.mRegistorList = mDriver.SupportRegistors;
            }
            else
            {
                TagViewModel.mRegistorList = null;
            }
            IsDirty = true;
            ResetTagEditMode();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetTagEditMode()
        {
            foreach(var vv in mTags)
            {
                vv.ResetEditModel();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newchannel"></param>
        private void ChangedChannel(string newchannel)
        {
            mMachineModel.Channel.RemoveChannel(Model.Data.ChannelName);
            var dd = ServiceLocator.Locator.Resolve<ICommChannelFactory2>().GetDevelopIntance(newchannel);
            if (dd != null)
            {
                dd.Name = string.IsNullOrEmpty(Model.Data.ChannelName) ? mMachineModel.Channel.GetAvaiableName(newchannel) : Model.Data.ChannelName;
                mMachineModel.Channel.AddChannel(dd);

                (Model as DeviceDevelop).Channel = dd;
                (Model as DeviceDevelop).Data.ChannelName = dd.Name;

                ChannelConfig = dd.Config();
            }
            if(string.IsNullOrEmpty(newchannel))
            {
                (Model as DeviceDevelop).Data.ChannelName = "";
                (Model as DeviceDevelop).Channel = null;
                ChannelConfig = null;
            }
            UpdateShareChannelText();
            IsDirty = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        private void UpdateSupportChannel(IDriverDevelop driver)
        {
            mSupportChannels.Clear();

            if (driver == null)
            {
                var vlist = ServiceLocator.Locator.Resolve<ICommChannelFactory2>().ListDevelopInstance();
                foreach (var vv in vlist)
                {
                    mSupportChannels.Add(vv.Data.Type.ToString());
                }
                ChannelView.Refresh();
                return;
            }
            var ss = driver.ListSupportChannels();
            if (ss!=null)
            {
                var vlist = ServiceLocator.Locator.Resolve<ICommChannelFactory2>().ListDevelopInstance();
                if(vlist!=null && vlist.Count>0)
                {
                    foreach(var vv in vlist)
                    {
                        if(ss.Contains(vv.TypeName))
                        {
                            mSupportChannels.Add(vv.TypeName);
                        }
                    }
                }
            }
            //else
            //{

            //    mSupportChannels.AddRange(mChannelList);
            //}

            ChannelView.Refresh();
            OnPropertyChanged("IsChannelVisiable");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void SelectShareChannel()
        {
            ShareDeviceSelectViewModel smm = new ShareDeviceSelectViewModel();
            var names = mMachineModel.Device.ListAllDevices().Where(e => e != this.Model).Where(e => e.Data.ChannelName == this.Model.Data.ChannelName).Select(e => e.FullName);
            smm.SetDevices(names.ToList(), mMachineModel.Device.ListAllDevices().Where(e=>e !=this.Model).Select(e => e.FullName).ToList());
            if(smm.ShowDialog().Value)
            {
                var sels = smm.GetSelectDevice();
                foreach (var vss in sels)
                {
                    var vdd = mMachineModel.Device.GetDevice(vss);
                    //vdd.Data.ChannelName = this.Model.Data.ChannelName;
                    (vdd as DeviceDevelop).Channel = (this.Model as DeviceDevelop).Channel;
                }

                foreach(var vvs in names.Where(e=> !sels.Contains(e)))
                {
                    var vdd = mMachineModel.Device.GetDevice(vvs);
                    //vdd.Data.ChannelName = string.Empty;
                    (vdd as DeviceDevelop).Channel = null;
                }
                IsDirty = true;
            }
            UpdateShareChannelText();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateShareChannelText()
        {
            if (string.IsNullOrEmpty(this.Model.Data.ChannelName))
            {
                ShareChannel = string.Empty;
                return;
            }

            var names = mMachineModel.Device.ListAllDevices().Where(e => e.Data.ChannelName == this.Model.Data.ChannelName && e!=this.Model).Select(e => e.FullName);
            StringBuilder sb = new StringBuilder();
            foreach(var vv in names)
            {
                sb.Append(vv + ",");
            }
            sb.Length = sb.Length > 0 ? sb.Length - 1 : sb.Length;
            ShareChannel = sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        private void CachTagModelId(Tagbase tag)
        {
            if(!mIdCach.Contains(tag.Id))
            {
                mIdCach.Add(tag.Id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void NewQueryTags()
        {
            EnableFilter = false;
            
            Task.Run(() => {
                ReLoad();
                Application.Current?.Dispatcher.Invoke(new Action(() => {
                    EnableFilter = true;
                }));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            if (!mIsLoaded) mIsLoaded = true;
            else return;
            mTags.Clear();
            mIdCach.Clear();

            mCurrentPage = 0;


            var channel = mMachineModel.Channel.GetChannel(mModel.Data.ChannelName);
            if(channel!=null)
            {
                mChannelName = channel.TypeName;
                ChannelConfig = channel.Config();
                mChannelConfigString = channel.Save().ToString();
            }
            else
            {
                ChannelConfig = null;
                mChannelName = string.Empty;
                mChannelConfigString = string.Empty;
            }
            OnPropertyChanged("ChannelName");

            var driver = mMachineModel.Driver.GetDriver(mModel.FullName);
            if(driver!=null)
            {
                mProtocolName = driver.TypeName;
                DriverConfig = driver.Config();
                foreach(var vv in mModel.Data.Tags)
                {
                    driver.CheckTagDeviceInfo(vv.Value);
                }
                var vitems = this.ProtocolList.Where(e=>e.Name == mProtocolName);
                if(vitems.Any())
                {
                    this.mSelectProtocol =vitems.FirstOrDefault();
                }
                else
                {
                    this.mSelectProtocol = this.ProtocolList[0];
                }

                mDriverConfigString = driver.Save().ToString();
            }
            else
            {
                DriverConfig = null;
                mProtocolName = string.Empty;
                this.mSelectProtocol = this.ProtocolList[0];
                mDriverConfigString = "";
            }
            OnPropertyChanged("ProtocolName");
            OnPropertyChanged("SelectProtocol");
            UpdateShareChannelText();
            UpdateSupportChannel(driver);
            mDriver = driver;
            if (mDriver != null)
            {
                TagViewModel.mRegistorList = mDriver.SupportRegistors;
            }
            else
            {
                TagViewModel.mRegistorList = null;
            }

            Task.Run(() =>
            {
                var vpp = GetTag(mCurrentPage);
                if (vpp != null)
                {
                    Application.Current?.Dispatcher.Invoke(new Action(() =>
                    {
                        foreach (var vv in vpp)
                        {
                            mTags.Add(new TagViewModel() { Model = vv, Document = mModel.Data, Machine = this.MachineModel, Parent = this });
                        }
                        
                    }));
                   
                }
            });
            TagCount = mModel.Data.Tags.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool Filter(object obj)
        {
            KeyValuePair<int, Tagbase> kvals = (KeyValuePair<int, Tagbase>)obj;
            bool re = true;
            
            if(!string.IsNullOrEmpty(mFilterKeyName))
            {
                re &= (kvals.Value.Name.Contains(mFilterKeyName) || kvals.Value.DatabaseName.Contains(mFilterKeyName) || kvals.Value.DeviceInfo.Contains(mFilterKeyName));
            }

            if(mDirectionFilterEnable)
            {
                re &= DirectionFilter == (int)kvals.Value.DataTranseDirection;
            }

            if(mRegistorFilterEnable && !string.IsNullOrEmpty(mFilterRegistorName))
            {
                re &= kvals.Value.DeviceInfo.Contains(mFilterRegistorName);
            }

            if(mTagTypeFilterEnable)
            {
                re &= (int)kvals.Value.Type == mFilterType;
            }

            return re;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public IEnumerable<Tagbase> GetTag(int page)
        {
            int count = mModel.Data.Tags.Where(e => Filter(e)).Count();
            //Application.Current?.Dispatcher.Invoke(new Action(() =>
            //{
            //    count = mFilterView.Count;
            //}));
          
            int pagecount = count / mPerPageCount;
            pagecount = count % mPerPageCount > 0 ? pagecount + 1 : pagecount;

            if(page<pagecount)
            {
                int icount = mPerPageCount;
                if((page+1)*mPerPageCount> count)
                {
                    icount = count - page * mPerPageCount;
                }
                return mModel.Data.Tags.Where(e=>Filter(e)).Skip(page * mPerPageCount).Take(icount).Select(e=>e.Value);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReLoad()
        {
            foreach (var vv in mTags) vv.Dispose();
            Application.Current?.Dispatcher.Invoke(new Action(() =>
            {
                mTags.Clear();
            }));
            mIdCach.Clear();
            mCurrentPage = -1;
            ContinueLoad();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanContinueLoadData()
        {
            var count = mModel.Data.Tags.Where(e => Filter(e)).Count();
            int pagecount = count / mPerPageCount;
            pagecount = count % mPerPageCount > 0 ? pagecount + 1 : pagecount;
            return mCurrentPage < pagecount;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ContinueLoad()
        {
            mCurrentPage++;
            Task.Run(() =>
            {
                var vpp = GetTag(mCurrentPage);
                if (vpp != null)
                {
                    Application.Current?.Dispatcher.Invoke(new Action(() =>
                    {
                        foreach (var vv in vpp)
                        {
                            if (mIdCach.Contains(vv.Id))
                            {
                                mIdCach.Remove(vv.Id);
                                continue;
                            }
                            mTags.Add(new TagViewModel() { Model = vv, Document = mModel.Data, Machine = this.MachineModel, Parent = this });
                        }
                       
                    }));

                    
                }
            });
            TagCount = mModel.Data.Tags.Count;
        }

        private void ExportToFile()
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "csv|*.csv";
            if (ofd.ShowDialog().Value)
            {
                var stream = new StreamWriter(File.Open(ofd.FileName, FileMode.Create, FileAccess.ReadWrite));

                Task.Run(() => {
                    ServiceLocator.Locator.Resolve<IProcessNotify>().BeginShowNotify();
                    int count = 0;
                    foreach (var vv in mModel.Data.Tags.Select(e => new TagViewModel() { Model = e.Value}))
                    {
                        stream.WriteLine(vv.SaveToCSVString());

                        ServiceLocator.Locator.Resolve<IProcessNotify>().ShowNotifyValue(((++count * 1.0 / mModel.Data.Tags.Count) * 100));
                    }
                   
                    stream.Close();
                    ServiceLocator.Locator.Resolve<IProcessNotify>().EndShowNotify();

                    MessageBox.Show(Res.Get("TagExportComplete"));
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ImportFromFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "csv|*.csv";
            List<TagViewModel> ltmp = new List<TagViewModel>();

            if (ofd.ShowDialog().Value)
            {
                var stream = new StreamReader(File.Open(ofd.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite));
                while (!stream.EndOfStream)
                {
                    string sval = stream.ReadLine();
                    if (!string.IsNullOrEmpty(sval))
                    {
                        TagViewModel tm = TagViewModel.LoadFromCSVString(sval);
                        tm.Document = this.mModel.Data;
                        tm.Parent = this;
                        ltmp.Add(tm);
                    }
                }
                stream.Close();
            }

            int mode = 0;
            var mm = new ImportModeSelectViewModel();
            if (mm.ShowDialog().Value)
            {
                mode = mm.Mode;
            }
            else
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            Task.Run(() =>
            {
                ServiceLocator.Locator.Resolve<IProcessNotify>().BeginShowNotify();

                //删除所有，重新添加
                if (mode == 1)
                {
                    mModel.Data.Tags.ClearAll();
                    Application.Current?.Dispatcher.Invoke(new Action(() =>
                    {
                        foreach (var vv in this.mTags)
                        {
                            vv.Dispose();
                        }
                        this.mTags.Clear();
                    }));
                    
                }

                //var tags = mSelectGroupTags.ToDictionary(e => e.RealTagMode.Name);
                int tcount = ltmp.Count;
                int icount = 0;
                bool haserro = false;
                
                foreach (var vv in ltmp)
                {
                    vv.Document = this.Model.Data;
                    mDriver?.CheckTagDeviceInfo(vv.Model);
                    if (mode == 2)
                    {
                        vv.Model.Name = GetNewName();

                        if (!mModel.Data.AppendTag(vv.Model))
                        {
                            sb.AppendLine(string.Format(Res.Get("AddTagFail"), vv.Model.Name));
                            haserro = true;
                        }
                    }
                    else
                    {
                        
                        //更新数据
                        if (!mModel.Data.UpdateOrAdd(vv.Model))
                        {
                            sb.AppendLine(string.Format(Res.Get("AddTagFail"), vv.Model.Name));
                            haserro = true;
                        }
                        else
                        {
                            vv.IsNew = false;
                            vv.IsChanged = false;
                        }
                    }

                    icount++;
                    ServiceLocator.Locator.Resolve<IProcessNotify>().ShowNotifyValue(((icount * 1.0 / tcount) * 100));
                }

                if(icount> 0)
                {
                    IsDirty= true;
                }

                if (haserro)
                {
                    string errofile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(ofd.FileName), "erro.txt");
                    System.IO.File.WriteAllText(errofile, sb.ToString());
                    if (MessageBox.Show(Res.Get("ImportErroMsg"), "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        try
                        {
                            Process.Start(Path.GetDirectoryName(errofile));
                        }
                        catch
                        {

                        }
                    }
                }
                ReLoad();
                Application.Current?.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ServiceLocator.Locator.Resolve<IProcessNotify>().EndShowNotify();
                }));

            });

        }

        /// <summary>
        /// 
        /// </summary>
        private void PasteTagProperty()
        {
            if (mPropertyCopy != null)
            {
                foreach (var vv in SelectedCells)
                {
                    if (vv.Item == mPropertyCopy.Item1)
                    {
                        continue;
                    }
                    else
                    {
                        TagViewModel tm = vv.Item as TagViewModel;
                        switch (mPropertyCopy.Item2)
                        {
                            case 1:
                                tm.Type = mPropertyCopy.Item1.Type;
                                break;
                            case 2:
                                tm.Direction = mPropertyCopy.Item1.Direction;
                                break;
                            case 3:
                                tm.DatabaseName = mPropertyCopy.Item1.DatabaseName;
                                break;
                            case 4:
                                tm.DeviceInfo = mPropertyCopy.Item1.DeviceInfo;
                                break;
                        }

                        IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CanPasteTagProperty()
        {
            if (mPropertyCopy == null || SelectedCells.Count == 0 || mPropertyCopy.Item2 == 0) return false;
            foreach (var vv in SelectedCells)
            {
                if (vv.Column.DisplayIndex != mPropertyCopy.Item2)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void RemoveTag()
        {
            int icount = 0;
            if (CurrentSelectTag != null)
            {
                int ind = this.mTags.IndexOf(CurrentSelectTag);

                List<TagViewModel> ll = new List<TagViewModel>();

                var vtag = CurrentSelectTag;

                foreach (var vv in grid.SelectedItems)
                {
                    ll.Add(vv as TagViewModel);
                }

                foreach (var vvv in ll)
                {
                    if (this.mModel.Data.RemoveTag(vvv.Model.Id))
                    {
                        vtag = vvv;
                        mTags.Remove(vtag);
                        vtag.Dispose();
                        icount++;
                    }
                }


                if (icount == 0)
                {
                    if (this.mModel.Data.RemoveTag(CurrentSelectTag.Model.Id))
                    {
                        mTags.Remove(CurrentSelectTag);
                        vtag.Dispose();
                        icount++;
                    }
                }

                if (ind < mTags.Count)
                {
                    CurrentSelectTag = mTags[ind];
                }
                else
                {
                    CurrentSelectTag = mTags.Count > 0 ? mTags.Last() : null;
                }
            }
            else
            {
                foreach (var vv in SelectedCells.Select(e => e.Item).Distinct().ToArray())
                {
                    var vvt = vv as TagViewModel;
                    if (this.mModel.Data.RemoveTag(vvt.Model.Id))
                    {
                        mTags.Remove(vvt);
                        vvt.Dispose();
                        icount++;
                    }
                }
            }
            IsDirty = true;
            TagCount -= icount;

        }

        /// <summary>
        /// 
        /// </summary>
        private void CopyTag()
        {
            mCopyTags.Clear();

            foreach (var vv in grid.SelectedItems)
            {
                mCopyTags.Add((vv as TagViewModel).Clone());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CopyTagProperty()
        {
            if (this.SelectedCells.Count() > 0)
            {
                var vt = SelectedCells.First();
                var tagproperty = (vt.Item as TagViewModel).Clone();

                mPropertyCopy = new Tuple<TagViewModel, int>(tagproperty, vt.Column.DisplayIndex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void NewTag()
        {
            if (CurrentSelectTag != null)
            {
                var vtag = CurrentSelectTag.Clone();
                vtag.Parent = this;
                vtag.Name = GetNewName();
                vtag.IsNew = true;
                vtag.Document = mModel.Data;
                vtag.Machine = this.MachineModel;
               

                mDriver?.CheckTagDeviceInfo(vtag.Model);

                if (mModel.Data.AppendTag(vtag.Model))
                {
                    mTags.Add(vtag);
                    CurrentSelectTag = vtag;
                    CachTagModelId(vtag.Model);
                }

            }
            else
            {
                var tag = new DoubleTag() { Name = GetNewName() };
                TagViewModel vtag = new TagViewModel() { Model = tag, Document = mModel.Data, Machine = this.MachineModel, Parent = this };
                mDriver?.CheckTagDeviceInfo(vtag.Model);
                if (mModel.Data.AppendTag(vtag.Model))
                {
                    mTags.Add(vtag);
                    CurrentSelectTag = vtag;
                    CachTagModelId(vtag.Model);
                }
            }
            IsDirty = true;
            TagCount++;
        }

        /// <summary>
        /// 获取字符串中的数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>数字</returns>
        public static int GetNumberInt(string str)
        {
            int result = -1;
            if (str != null && str != string.Empty)
            {
                // 正则表达式剔除非数字字符（不包含小数点.）
                str = Regex.Replace(str, @"[^\d.\d]", "");
                // 如果是数字，则转换为decimal类型
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(str))
                            result = int.Parse(str);
                    }
                    catch
                    {

                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string CheckAndNewName(string baseName = "tag")
        {
            if (!mModel.Data.Tags.TagNames.Contains(baseName))
            {
                return baseName;
            }

            string tagName = baseName;

            int number = GetNumberInt(baseName);
            if (number >= 0)
            {
                if (tagName.EndsWith(number.ToString()))
                {
                    tagName = tagName.Substring(0, tagName.IndexOf(number.ToString()));
                }
            }
            string sname = tagName;
            for (int i = 1; i < int.MaxValue; i++)
            {
                tagName = sname + i;
                if (!mModel.Data.Tags.TagNames.Contains(tagName))
                {
                    return tagName;
                }
            }
            return tagName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetNewName(string baseName = "tag")
        {
            string tagName = baseName;

            int number = GetNumberInt(baseName);
            if (number >= 0)
            {
                if (tagName.EndsWith(number.ToString()))
                {
                    tagName = tagName.Substring(0, tagName.IndexOf(number.ToString()));
                }
            }
            string sname = tagName;
            for (int i = 1; i < int.MaxValue; i++)
            {
                tagName = sname + i;
                if (!mModel.Data.Tags.TagNames.Contains(tagName))
                {
                    return tagName;
                }
            }
            return tagName;
        }

        /// <summary>
        /// 
        /// </summary>
        private void PasteTag()
        {
            if (mCopyTags.Count > 0)
            {
                TagViewModel tm = null;
                foreach (var vv in mCopyTags)
                {
                    var vtag = vv.Clone();
                    mDriver?.CheckTagDeviceInfo(vtag.Model);
                    vtag.Parent = this;
                    vtag.Name = GetNewName(vv.Name);
                    vtag.IsNew = true;
                    vtag.Machine = this.MachineModel;
                    vtag.Document = mModel.Data;
                   

                    if (mModel.Data.AppendTag(vtag.Model))
                    {
                        mTags.Add(vtag);
                        CachTagModelId(vtag.Model);
                    }
                }
                if (tm != null)
                    CurrentSelectTag = tm;
                IsDirty = true;
            }
            TagCount += mCopyTags.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public void CheckTagDeviceInfo(Tagbase tag)
        {
            mDriver?.CheckTagDeviceInfo(tag);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Active()
        {
            this.grid = mGrid;
            Init();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeActive()
        {
            TagViewModel.mLastConfigModel?.OnDisActived();
            mGrid = this.grid;
            foreach (var vv in mTags)
            {
                vv.Dispose();
            }
            mTags.Clear();
            mIsLoaded = false;
            this.grid = null;

            CheckChannelAndDriverChanged();


        }

        public void CheckChannelAndDriverChanged()
        {
            var ndrivers = this.mDriver != null ? this.mDriver.Save().ToString() : "";
            if (ndrivers != mDriverConfigString)
            {
                MachineModel.IsDirty = true;
            }
            else
            {
                string mchls = "";
                var channel = mMachineModel.Channel.GetChannel(mModel.Data.ChannelName);
                if (channel != null)
                {
                    mchls = channel.Save().ToString();
                }
                else
                {
                    mchls = "";
                }
                if (mchls != mChannelConfigString)
                {
                    MachineModel.IsDirty = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetConfigServerUrl()
        {
            var data = (Model as DeviceDevelop).Channel.Data;
            if(data is NetworkClientChannelData)
            {
                return (data as NetworkClientChannelData).ServerIp;
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetConfigUserName()
        {
            var data = (Model as DeviceDevelop).Channel.Data;
            if (data is NetworkClientChannelData)
            {
                return (data as NetworkClientChannelData).UserName;
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetConfigPassword()
        {
            var data = (Model as DeviceDevelop).Channel.Data;
            if (data is NetworkClientChannelData)
            {
                return (data as NetworkClientChannelData).Password;
            }
            return string.Empty;
        }

        private bool mIsMonitorStoped = false;
        private Thread mMonitorScan;
        private WebClient mClient;

        private void CheckStartLocal()
        {
            if(MonitorParameter.Parameter.Server.Contains("127.0.0.1")||(MonitorParameter.Parameter.Server.Contains("local")))
            {
                var vss = Process.GetProcessesByName("InSpiderRun");
                if (vss != null && vss.Length > 0)
                {
                    return;
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    var info = new ProcessStartInfo() { FileName = "InSpiderRun.exe" };
                    info.UseShellExecute = true;
                    info.Arguments = "start " + mMachineModel.Name;
                    info.WorkingDirectory = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);
                    Process.Start(info).WaitForExit(1000);
                }
                else
                {
                    var info = new ProcessStartInfo() { FileName = "dotnet" };
                    info.UseShellExecute = true;
                    info.CreateNoWindow = false;
                    info.Arguments = "./InSpiderRun.dll start " + mMachineModel.Name;
                    info.WorkingDirectory = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);
                    Process.Start(info).WaitForExit(1000);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartRealDataMonitor()
        {
            IsMonitMode = true;
            mIsMonitorStoped = false;
            mMonitorScan = new Thread(RealDataMonitorProcess);
            mMonitorScan.IsBackground = true;
            mMonitorScan.Start();
            CheckStartLocal();

            this.grid.Columns[this.grid.Columns.Count-1].Visibility = System.Windows.Visibility.Visible;

        }

        /// <summary>
        /// 
        /// </summary>
        private void StopRealDataMonitor()
        {
            mIsMonitorStoped = true;
            IsMonitMode = false;
            this.grid.Columns[this.grid.Columns.Count - 1].Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        private void RealDataMonitorProcess()
        {
            IEnumerable<TagViewModel> mtagquery=null;
            int pagecount = 200;
            int count = 0;
            int pp;

            while (!mIsMonitorStoped)
            {
                lock (mTags)
                {
                     count = mTags.Count / pagecount;
                     pp = mTags.Count % pagecount;
                    if (pp > 0) count++;
                }

                for (int i = 0; i < count; i++)
                {
                    lock (mTags)
                    {
                        try
                        {
                            var vstart = i * pagecount;
                            var len = pagecount;

                            if (vstart + pagecount > mTags.Count)
                            {
                                len = mTags.Count - vstart;
                            }
                            if (len > 0)
                            {
                                mtagquery = mTags.Skip(vstart).Take(len);
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch
                        {
                            mtagquery = null;
                        }
                    }

                    if(mtagquery!=null)
                    GetRealData(mtagquery);

                    Thread.Sleep(10);
                }

                Thread.Sleep(MonitorParameter.Parameter.ScanCircle);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tags"></param>
        private void GetRealData(IEnumerable<TagViewModel> tags)
        {
            if (tags == null) return;

            var vtags = tags.ToDictionary(e => e.Name);

            var rq = new RealValueRequestByTagName() { Device = this.Model.FullName, Tags = vtags.Keys.ToList(), UserName = MonitorParameter.Parameter.UserName, Password = MonitorParameter.Parameter.Password };
            var sval = Post(MonitorParameter.Parameter.Server+ "/RealValue", "GetTagValues", JsonConvert.SerializeObject(rq));

            if (string.IsNullOrEmpty(sval)) return;

            var result = JsonConvert.DeserializeObject<RealValueResult>(sval);

            if (result != null && result.Result)
            {
                foreach (var vv in result.Value)
                {
                    if(vtags.ContainsKey(vv.TagName))
                    {
                        vtags[vv.TagName].Value = vv.Value!=null?vv.Value.ToString():"";
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sval"></param>
        /// <returns></returns>
        private string Post(string url,string fun, string sval)
        {
            try
            {
                if (mClient == null)
                    mClient = new WebClient();
                mClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                mClient.Encoding = Encoding.UTF8;
                return mClient.UploadString(url + "/" + fun, sval);
            }
            catch(Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetChannelType()
        {
            var data = (Model as DeviceDevelop).Channel.Data;
            if (data !=null)
            {
                return (Model as DeviceDevelop).Channel.TypeName;
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetChannelParameter()
        {
            Dictionary<string, string> re = new Dictionary<string, string>();
            var data = (Model as DeviceDevelop).Channel.Data;
            foreach(var vv in  data.GetType().GetProperties())
            {
                var val = vv.GetValue(data);
                re.Add(vv.Name,val!=null?val.ToString():"");
            }
            return re;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 
    /// </summary>
    public class MonitorParameter:ViewModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        public static MonitorParameter Parameter = new MonitorParameter();

        /// <summary>
        /// 
        /// </summary>
        public MonitorParameter()
        {
            Init();
        }

        private string mServer= "http://127.0.0.1:23232";
        private string mUserName= "Guest";
        private string mPassword= "Guest";

        /// <summary>
        /// 
        /// </summary>
        public string Server { get { return mServer; } set { mServer = value; OnPropertyChanged("Server"); } }
        
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get { return mUserName; } set { mUserName = value; OnPropertyChanged("UserName"); } }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get { return mPassword; } set { mPassword = value; OnPropertyChanged("Password"); } }

        /// <summary>
        /// 
        /// </summary>
        public int ScanCircle { get; set; } = 1000;


        public void Init()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location),"Config", "SpiderMonitorConfig.cfg");
            if(System.IO.File.Exists(sfile))
            {
                XElement xe = XElement.Load(sfile);
                if(xe.Attribute("")!=null)
                {
                    Server = xe.Attribute("Server").Value;
                    Password = xe.Attribute("Password").Value;
                    UserName = xe.Attribute("UserName").Value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Config", "SpiderMonitorConfig.cfg");
            if (System.IO.File.Exists(sfile))
            {
                XElement xx = new XElement("Config");
                xx.SetAttributeValue("Server", Server);
                xx.SetAttributeValue("UserName", UserName);
                xx.SetAttributeValue("Password", Password);
                xx.Save(sfile);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TagViewModel : ViewModelBase
    {

        #region ... Variables  ...
        
        private Tagbase mModel;

        public static string[] mTagTypeList;

        public static string[] mDataTranseDirection;

        public static string[] mRegistorList;

        private ICommand mDatabaseBrowserCommand;

        private ICommand mDatabaseRemoveCommand;

        private IRegistorConfigModel mRegistorConfigModel;

        public static IRegistorConfigModel mLastConfigModel;

        private ICommand mConvertEditCommand;

        private ICommand mConvertRemoveCommand;

        private string mValue = "";

        private byte mQuality = 0;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        static TagViewModel()
        {
            mTagTypeList = Enum.GetNames(typeof(TagType));
            mDataTranseDirection = Enum.GetNames(typeof(DataTransType)).Select(e=>Res.Get(e.ToString())).ToArray();
        }

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public IRegistorConfigModel RegistorConfigModel
        {
            get
            {
                if(mRegistorConfigModel==null)
                {
                    var driver = Machine.Driver.GetDriver(this.Document.Name);
                    this.mRegistorConfigModel = driver?.RegistorConfig();
                    if(mRegistorConfigModel!=null)
                    mRegistorConfigModel.Service = Parent;
                }

                
                if(mLastConfigModel!=mRegistorConfigModel)
                {
                    if (mLastConfigModel != null) mLastConfigModel.OnDisActived();
                    mLastConfigModel = mRegistorConfigModel;
                }

                mRegistorConfigModel?.FreshRegistor(DeviceInfo);
                if (mRegistorConfigModel != null && mRegistorConfigModel.UpdateRegistorCallBack==null)
                {
                    mRegistorConfigModel.UpdateRegistorCallBack = (val) =>
                    {
                        DeviceInfo = val;
                    };
                }
                return mRegistorConfigModel;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public MachineDocument Machine { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string[] RegistorList
        {
            get
            {
                return mRegistorList;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string[] DataTranseDirectionList
        {
            get { return mDataTranseDirection; }
          
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] TagTypeList
        {
            get
            {
                return mTagTypeList;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public DeviceData Document { get; set; }

        /// <summary>
        /// 是否新建
        /// </summary>
        public bool IsNew { get; set; }

        private bool mIsChanged;

        /// <summary>
        /// 
        /// </summary>
        public bool IsChanged
        {
            get { return mIsChanged; }
            set
            {
                mIsChanged = value;
                if (Machine != null&&value)
                {
                    Machine.IsDirty = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Tagbase Model
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
                    OnPropertyChanged("Model");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ConvertString
        {
            get
            {
                return mModel.Conveter != null ? mModel.Conveter.SeriseToString() : string.Empty;
            }
            set
            {
                if (ConvertString != value)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        mModel.Conveter = value.DeSeriseToValueConvert(); ;
                    }
                    else
                    {
                        Convert = null;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IValueConvert Convert
        {
            get
            {
                return mModel != null ? mModel.Conveter : null;
            }
            set
            {
                if (mModel.Conveter != value)
                {
                    mModel.Conveter = value;
                    IsChanged = true;
                }
                OnPropertyChanged("Convert");
                OnPropertyChanged("ConvertString");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ConvertEditCommand
        {
            get
            {
                if (mConvertEditCommand == null)
                {
                    mConvertEditCommand = new RelayCommand(() => {

                        ConvertEditViewModel cmm = new ConvertEditViewModel(this.Model);
                        if (mModel.Conveter != null)
                            cmm.SetSelectConvert(mModel.Conveter.SeriseToString());
                        if (cmm.ShowDialog().Value)
                        {
                            Convert = cmm.CurrentSelectModel.Model;
                        }
                    });
                }
                return mConvertEditCommand;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ICommand ConvertRemoveCommand
        {
            get
            {
                if (mConvertRemoveCommand == null)
                {
                    mConvertRemoveCommand = new RelayCommand(() => {
                        Convert = null;
                    }, () => { return Convert != null; });
                }
                return mConvertRemoveCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return mModel.Name;
            }
            set
            {
                if (mModel != null && mModel.Name != value && value.Length<=64)
                {
                    mModel.Name = value;
                    Parent.CheckTagDeviceInfo(this.Model);
                    OnPropertyChanged("Name");
                    OnPropertyChanged("DeviceInfo");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public string DatabaseName
        {
            get
            {
                return mModel != null ? mModel.DatabaseName : string.Empty;
            }
            set
            {
                if (mModel != null && mModel.DatabaseName != value)
                {
                    mModel.DatabaseName = value;
                    OnPropertyChanged("DatabaseName");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public int Direction
        {
            get
            {
                return (int)mModel.DataTranseDirection;
            }
            set
            {
                if (mModel != null && (int)mModel.DataTranseDirection != value)
                {
                    mModel.DataTranseDirection = (DataTransType)value;
                    IsChanged = true;
                    OnPropertyChanged("Direction");
                    OnPropertyChanged("DirectionString");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DirectionString
        {
            get
            {
                return Res.Get(mModel.DataTranseDirection.ToString());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string DeviceInfo
        {
            get
            {
                return mModel.DeviceInfo;
            }
            set
            {
                if (mModel!=null && mModel.DeviceInfo != value)
                {
                    mModel.DeviceInfo = value;
                    IsChanged = true;
                    OnPropertyChanged("DeviceInfo");
                }
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public int Type
        {
            get
            {
                return (int)mModel.Type;
            }
            set
            {
                if (mModel != null && (int)mModel.Type != value)
                {
                    ChangeTagType((TagType)value);
                    IsChanged = true;
                    OnPropertyChanged("Type");
                    OnPropertyChanged("TypeString");
                    OnPropertyChanged("IsNumberTag");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TypeString
        {
            get
            {
                return mModel.Type.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand DatabaseBrowserCommand
        {
            get
            {
                if(mDatabaseBrowserCommand==null)
                {
                    mDatabaseBrowserCommand = new RelayCommand(() => {

                        string stag = Machine.Api.Api.ConfigTags();
                        if(!string.IsNullOrEmpty(stag))
                        {
                            DatabaseName = stag;
                        }
                    },()=> { return Machine != null && Machine.Api != null; });
                }
                return mDatabaseBrowserCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand DatabaseRemoveCommand
        {
            get
            {
                if(mDatabaseRemoveCommand==null)
                {
                    mDatabaseRemoveCommand = new RelayCommand(() => {
                        DatabaseName = string.Empty;
                    },()=> { return !string.IsNullOrEmpty(DatabaseName); });
                }
                return mDatabaseRemoveCommand;
            }
        }


        /// <summary>
            /// 
            /// </summary>
        public DeviceDetailViewModel Parent
        {
            get;
            set;
        }

        /// <summary>
            /// 
            /// </summary>
        public string Value
        {
            get
            {
                return mValue;
            }
            set
            {
                if (mValue != value)
                {
                    mValue = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public byte Quality
        {
            get
            {
                return mQuality;
            }
            set
            {
                if (mQuality != value)
                {
                    mQuality = value;
                    OnPropertyChanged("Quality");
                }
            }
        }



        #endregion ...Properties...

        #region ... Methods    ...


        /// <summary>
        /// 
        /// </summary>
        public void ResetEditModel()
        {
            mRegistorConfigModel = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void FreshDeviceInfo()
        {
            OnPropertyChanged("DeviceInfo");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagType"></param>
        private void ChangeTagType(TagType tagType)
        {
            Tagbase ntag = null;
            switch (tagType)
            {
                case TagType.Bool:
                    ntag = new BoolTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName,DataTranseDirection = mModel.DataTranseDirection,DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.Byte:
                    ntag = new ByteTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName,DataTranseDirection = mModel.DataTranseDirection,DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.DateTime:
                    ntag = new DateTimeTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName,DataTranseDirection = mModel.DataTranseDirection,DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.Double:
                    ntag = new DoubleTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName,DataTranseDirection = mModel.DataTranseDirection,DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.Float:
                    ntag = new FloatTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName,DataTranseDirection = mModel.DataTranseDirection,DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.Int:
                    ntag = new IntTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName,DataTranseDirection = mModel.DataTranseDirection,DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.Long:
                    ntag = new LongTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName,DataTranseDirection = mModel.DataTranseDirection,DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.Short:
                    ntag = new ShortTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName,DataTranseDirection = mModel.DataTranseDirection,DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.String:
                    ntag = new StringTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName,DataTranseDirection = mModel.DataTranseDirection,DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.UInt:
                    ntag = new UIntTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName,DataTranseDirection = mModel.DataTranseDirection,DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.ULong:
                    ntag = new ULongTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName,DataTranseDirection = mModel.DataTranseDirection,DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.UShort:
                    ntag = new UShortTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName,DataTranseDirection = mModel.DataTranseDirection,DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.IntPoint:
                    ntag = new IntPointTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.IntPoint3:
                    ntag = new IntPoint3Tag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.UIntPoint:
                    ntag = new UIntPointTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.UIntPoint3:
                    ntag = new UIntPoint3Tag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.LongPoint:
                    ntag = new LongPointTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.LongPoint3:
                    ntag = new LongPoint3Tag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.ULongPoint:
                    ntag = new ULongPointTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.ULongPoint3:
                    ntag = new ULongPoint3Tag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                default:
                    break;
            }
            if(ntag!=null)
            {
                Model = ntag;
                Document.RemoveTag(ntag.Id);
                Document.AddTag(ntag);
            }
            IsChanged = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TagViewModel Clone()
        {
            Tagbase ntag = null;
            switch (this.mModel.Type)
            {
                case TagType.Bool:
                    ntag = new BoolTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.Byte:
                    ntag = new ByteTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.DateTime:
                    ntag = new DateTimeTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.Double:
                    ntag = new DoubleTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.Float:
                    ntag = new FloatTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.Int:
                    ntag = new IntTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.Long:
                    ntag = new LongTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.Short:
                    ntag = new ShortTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.String:
                    ntag = new StringTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.UInt:
                    ntag = new UIntTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.ULong:
                    ntag = new ULongTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.UShort:
                    ntag = new UShortTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.IntPoint:
                    ntag = new IntPointTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.IntPoint3:
                    ntag = new IntPoint3Tag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.UIntPoint:
                    ntag = new UIntPointTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.UIntPoint3:
                    ntag = new UIntPoint3Tag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.LongPoint:
                    ntag = new LongPointTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.LongPoint3:
                    ntag = new LongPoint3Tag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.ULongPoint:
                    ntag = new ULongPointTag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                case TagType.ULongPoint3:
                    ntag = new ULongPoint3Tag() { Id = this.mModel.Id, Name = mModel.Name, DatabaseName = mModel.DatabaseName, DataTranseDirection = mModel.DataTranseDirection, DeviceInfo = mModel.DeviceInfo };
                    break;
                default:
                    break;
            }
            if (ntag != null)
            {
                return new TagViewModel() { Model = ntag,Machine=this.Machine };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            Machine = null;
            Document = null;
            mModel = null;
            Parent = null;
            base.Dispose();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string SaveToCSVString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(mModel.Id + ",");
            sb.Append(mModel.Name + ",");
            sb.Append(mModel.Type + ",");
            sb.Append(mModel.DatabaseName + ",");
            sb.Append(mModel.DeviceInfo + ",");
            sb.Append(mModel.DataTranseDirection);
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public static TagViewModel LoadFromCSVString(string val)
        {
            string[] stmp = val.Split(new char[] { ',' });
            TagType tp = (TagType)Enum.Parse(typeof(TagType), stmp[2]);
            var realtag = TagExtends.CreatTag(tp);

            realtag.Id = int.Parse(stmp[0]);
            realtag.Name = stmp[1];
            realtag.DatabaseName = stmp[3];
            realtag.DeviceInfo = stmp[4];
            realtag.DataTranseDirection = (DataTransType)Enum.Parse(typeof(DataTransType), stmp[5]); 
            return new TagViewModel() { Model = realtag };
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 
    /// </summary>
    public class RealValueRequest
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RealValueRequestByTagName
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> Tags { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class RealValueItem
    {
        /// <summary>
        /// 变量名称
        /// </summary>
        public string TagName { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 质量戳
        /// </summary>
        public byte Quality { get; set; }
    }


    public class RealValueResult
    {
        public bool Result { get; set; }

        public string ErroMessage { get; set; }


        public List<RealValueItem> Value { get; set; }
    }

}
