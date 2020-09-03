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
            mDataView.SortDescriptions.Add(new SortDescription() { PropertyName = "Name", Direction = ListSortDirection.Ascending });
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


        public string Name { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

}
