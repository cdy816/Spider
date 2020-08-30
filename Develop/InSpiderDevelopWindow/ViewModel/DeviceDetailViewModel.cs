//==============================================================
//  Copyright (C) 2020 Chongdaoyang Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/25 21:14:10 .
//  Version 1.0
//  CDYWORK
//==============================================================

using Cdy.Spider;
using InSpiderDevelop;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InSpiderDevelopWindow.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class DeviceDetailViewModel:ViewModelBase
    {

        #region ... Variables  ...

        private System.Collections.ObjectModel.ObservableCollection<TagViewModel> mTags = new System.Collections.ObjectModel.ObservableCollection<TagViewModel>();

        private IDeviceDevelop mModel;

        private ICommand mAddCommand;
        private ICommand mRemoveCommand;
        private ICommand mImportCommand;
        private ICommand mExportCommand;

        private ICommand mCopyCommand;
        private ICommand mCellCopyCommand;
        private ICommand mPasteCommand;
        private ICommand mCellPasteCommand;

        private static List<TagViewModel> mCopyTags = new List<TagViewModel>();

        private Dictionary<string, string> mFilters = new Dictionary<string, string>();

        private bool mEnableFilter = true;

        private Tuple<TagViewModel, int> mPropertyCopy;

        private DataGridSelectionUnit mSelectMode = DataGridSelectionUnit.FullRow;

        private TagViewModel mCurrentSelectTag;

        private int mTagCount = 0;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
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


        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        private void Init()
        {
            mTags.Clear();
            TagCount = 0;
            Task.Run(() => { 
                
                foreach(var vv in mModel.Data.Tags)
                {
                    Application.Current?.Dispatcher.Invoke(new Action(() => {
                        mTags.Add(new TagViewModel() { Model = vv.Value });
                    }));
                }
            });
        }

        private void ExportToFile()
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "csv|*.csv";
            if (ofd.ShowDialog().Value)
            {
                var stream = new StreamWriter(File.Open(ofd.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite));

                Task.Run(() => {
                    ServiceLocator.Locator.Resolve<IProcessNotify>().BeginShowNotify();
                    int count = 0;
                    foreach (var vv in mModel.Data.Tags.Select(e => new TagViewModel() { Model = e.Value }))
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
                    mModel.Data.Tags.Clear();
                }

                //var tags = mSelectGroupTags.ToDictionary(e => e.RealTagMode.Name);
                int tcount = ltmp.Count;
                int icount = 0;
                bool haserro = false;
                
                foreach (var vv in ltmp)
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

                    icount++;
                    ServiceLocator.Locator.Resolve<IProcessNotify>().ShowNotifyValue(((icount * 1.0 / tcount) * 100));
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
                Application.Current?.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ServiceLocator.Locator.Resolve<IProcessNotify>().EndShowNotify();
                }));

            });

        }

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

                foreach (var vv in grid.SelectedItems)
                {
                    ll.Add(vv as TagViewModel);
                }

                foreach (var vvv in ll)
                {
                    if (this.mModel.Data.RemoveTag(vvv.Model.Id))
                    {
                        mTags.Remove(CurrentSelectTag);
                        icount++;
                    }
                }


                if (icount == 0)
                {
                    if (this.mModel.Data.RemoveTag(CurrentSelectTag.Model.Id))
                    {
                        mTags.Remove(CurrentSelectTag);
                        icount++;
                    }
                }

                if (ind < mTags.Count)
                {
                    CurrentSelectTag = mTags[ind];
                }
                else
                {
                    CurrentSelectTag = mTags.Last();
                }

                //if (DevelopServiceHelper.Helper.Remove(GroupModel.Database, CurrentSelectTag.RealTagMode.Id))
                //{
                //    SelectGroupTags.Remove(CurrentSelectTag);
                //}
            }
            else
            {
                foreach (var vv in SelectedCells.Select(e => e.Item).Distinct().ToArray())
                {
                    var vvt = vv as TagViewModel;
                    if (this.mModel.Data.RemoveTag(vvt.Model.Id))
                    {
                        mTags.Remove(vvt);
                        icount++;
                    }
                }
            }

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
                mCopyTags.Add(vv as TagViewModel);
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
                vtag.Name = GetNewName();
                vtag.IsNew = true;
                
                if(mModel.Data.AppendTag(vtag.Model))
                {
                    mTags.Add(vtag);
                    CurrentSelectTag = vtag;
                }

            }
            else
            {
                var tag = new DoubleTag() { Name = GetNewName() };
                TagViewModel vtag = new TagViewModel() { Model = tag };
                if (mModel.Data.AppendTag(vtag.Model))
                {
                    mTags.Add(vtag);
                    CurrentSelectTag = vtag;
                }
            }
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
                  
                    vtag.Name = GetNewName(vv.Name);
                    vtag.IsNew = true;
                    if (mModel.Data.AppendTag(vtag.Model))
                    {
                        mTags.Add(vtag);
                    }
                }
                if (tm != null)
                    CurrentSelectTag = tm;
            }
            TagCount += mCopyTags.Count;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 
    /// </summary>
    public class TagViewModel : ViewModelBase
    {

        #region ... Variables  ...
        
        private Tagbae mModel;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public DeviceData Document { get; set; }

        /// <summary>
        /// 是否新建
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsChanged { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Tagbae Model
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
        public string Name
        {
            get
            {
                return mModel.Name;
            }
            set
            {
                if (mModel.Name != value)
                {
                    mModel.Name = value;
                    OnPropertyChanged("Name");
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
                return mModel.DatabaseName;
            }
            set
            {
                if (mModel.DatabaseName != value)
                {
                    mModel.DatabaseName = value;
                    OnPropertyChanged("DatabaseName");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public DataTransType Direction
        {
            get
            {
                return mModel.DataTranseDirection;
            }
            set
            {
                if (mModel.DataTranseDirection != value)
                {
                    mModel.DataTranseDirection = value;
                    OnPropertyChanged("DataTranseDirection");
                }
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
                if (mModel.DeviceInfo != value)
                {
                    mModel.DeviceInfo = value;
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


        #endregion ...Properties...

        #region ... Methods    ...

        private void ChangeTagType(TagType tagType)
        {
            Tagbae ntag = null;
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
            Tagbae ntag = null;
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
                return new TagViewModel() { Model = ntag };
            }
            else
            {
                return null;
            }
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
            sb.Length = sb.Length > 0 ? sb.Length - 1 : sb.Length;
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

}
