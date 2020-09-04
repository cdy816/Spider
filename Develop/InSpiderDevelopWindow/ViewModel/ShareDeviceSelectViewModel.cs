//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/9/3 11:40:16.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace InSpiderDevelopWindow.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ShareDeviceSelectViewModel:WindowViewModelBase
    {

        #region ... Variables  ...
        
        private System.Collections.ObjectModel.ObservableCollection<ShareDeviceItem> mItems = new System.Collections.ObjectModel.ObservableCollection<ShareDeviceItem>();

        private ICollectionView mDataView;

        private ICommand mClearCommand;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        
        public ShareDeviceSelectViewModel()
        {
            Title = Res.Get("ShareDeviceSelectTitle");
            DefaultWidth = 1024;
            DefaultHeight = 600;
            IsEnableMax = false;
        }

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public ICollectionView DataView
        {
            get
            {
                return mDataView;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ClearCommand
        {
            get
            {
                if(mClearCommand==null)
                {
                    mClearCommand = new RelayCommand(() => { 
                        foreach(var vv in this.mItems)
                        {
                            vv.IsChecked = false;
                        }
                    });
                }
                return mClearCommand;
            }
        }



        #endregion ...Properties...

        #region ... Methods    ...
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedItems"></param>
        /// <param name="allItems"></param>
        public void SetDevices(List<string> selectedItems,List<string> allItems)
        {
            foreach(var vv in allItems)
            {
                ShareDeviceItem sitem = new ShareDeviceItem() { Name = vv };
                if(selectedItems.Contains(vv))
                {
                    sitem.IsChecked = true;
                }
                mItems.Add(sitem);
            }
            mDataView = CollectionViewSource.GetDefaultView(mItems);
            mDataView.SortDescriptions.Add(new SortDescription() { PropertyName = "IsChecked", Direction = ListSortDirection.Descending });
            mDataView.SortDescriptions.Add(new SortDescription() { PropertyName = "NamePrev", Direction = ListSortDirection.Ascending });
            mDataView.SortDescriptions.Add(new SortDescription() { PropertyName = "NameAfter", Direction = ListSortDirection.Ascending });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetSelectDevice()
        {
            return mItems.Where(e => e.IsChecked).Select(e=>e.Name).ToList();
        }
        
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    public class ShareDeviceItem : ViewModelBase
    {

        #region ... Variables  ...
        private bool mIsChecked;
        private string mName;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
            /// 
            /// </summary>
        public bool IsChecked
        {
            get
            {
                return mIsChecked;
            }
            set
            {
                if (mIsChecked != value)
                {
                    mIsChecked = value;
                    OnPropertyChanged("IsChecked");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string NamePrev { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int NameAfter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get { return mName; } set { mName = value; ParseName(value); } }

        #endregion ...Properties...

        #region ... Methods    ...


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void ParseName(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                NameAfter = GetNumberInt(value);
                NamePrev = value.Replace(NameAfter.ToString(), "");
            }
            else
            {
                NameAfter = 0;
                NamePrev = "";
            }
        }

        /// <summary>
        /// 获取字符串中的数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>数字</returns>
        public int GetNumberInt(string str)
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

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

}
