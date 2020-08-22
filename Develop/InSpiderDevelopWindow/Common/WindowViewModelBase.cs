//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/3/17 17:02:43.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace InSpiderDevelopWindow
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class WindowViewModelBase : INotifyPropertyChanged, IDisposable
    {
        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private ICommand mCancelCommand;

        /// <summary>
        /// 
        /// </summary>
        private ICommand mOKCommand;

        /// <summary>
        /// 
        /// </summary>
        private string mTitle;


        /// <summary>
        /// 
        /// </summary>
        private string mIcon;

        /// <summary>
        /// 
        /// </summary>
        private bool mIsCancel;

        /// <summary>
        /// 
        /// </summary>
        private bool mIsOK;

        /// <summary>
        /// 
        /// </summary>
        private bool mIsClosed;

        /// <summary>
        /// 
        /// </summary>
        private bool mIsEnableMax = true;

        /// <summary>
        /// 
        /// </summary>
        private bool mIsHidden;

        private string mMessage;

        private bool mIsOkCancel = true;

        private double? mMinWidth;

        private double? mMinHeight;

        private double? mDefaultWidth;

        private double? mDefaultHeight;

        private bool mIsEnableDefault = true;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public bool IsEnableDefault
        {
            get
            {
                return mIsEnableDefault;
            }
            set
            {
                if (mIsEnableDefault != value)
                {
                    mIsEnableDefault = value;
                    OnPropertyChanged("IsEnableDefault");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public double? MinWidth
        {
            get
            {
                return mMinWidth;
            }
            set
            {
                if (mMinWidth != value)
                {
                    mMinWidth = value;
                    OnPropertyChanged("MinWidth");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public double? MinHeight
        {
            get
            {
                return mMinHeight;
            }
            set
            {
                if (mMinHeight != value)
                {
                    mMinHeight = value;
                    OnPropertyChanged("MinHeight");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double? DefaultHeight
        {
            get
            {
                return mDefaultHeight;
            }
            set
            {
                if (mDefaultHeight != value)
                {
                    mDefaultHeight = value;
                    OnPropertyChanged("DefaultHeight");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double? DefaultWidth
        {
            get
            {
                return mDefaultWidth;
            }
            set
            {
                if (mDefaultWidth != value)
                {
                    mDefaultWidth = value;
                    OnPropertyChanged("DefaultWidth");
                }
            }
        }


        /// <summary>
        /// 是否是包含OK 和 Cancel的标准模式
        /// </summary>
        public bool IsOkCancel
        {
            get
            {
                return mIsOkCancel;
            }
            set
            {
                if (mIsOkCancel != value)
                {
                    mIsOkCancel = value;
                    OnPropertyChanged("IsOkCancel");
                }
            }
        }


        /// <summary>
        /// 图标
        /// </summary>
        public String Icon
        {
            get
            {
                return mIcon;
            }
            set
            {
                if (mIcon != value)
                {
                    mIcon = value;
                }
            }
        }


        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get
            {
                return mTitle;
            }
            set
            {
                if (mTitle != value)
                {
                    mTitle = value;
                    OnPropertyChanged("Title");
                }
            }
        }


        /// <summary>
        /// 确定
        /// </summary>
        public bool IsOK
        {
            get
            {
                return mIsOK;
            }
            protected set
            {
                if (mIsOK != value)
                {
                    mIsOK = value;
                    OnPropertyChanged("IsOK");
                }
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        public bool IsCancel
        {
            get
            {
                return mIsCancel;
            }
            private set
            {
                if (mIsCancel != value)
                {
                    mIsCancel = value;
                    OnPropertyChanged("IsCancel");
                }
            }
        }

        /// <summary>
        /// 是能最大化按钮
        /// </summary>
        public bool IsEnableMax
        {
            get
            {
                return mIsEnableMax;
            }
            set
            {
                if (mIsEnableMax != value)
                {
                    mIsEnableMax = value;
                    OnPropertyChanged("IsEnableMax");
                }
            }
        }

        /// <summary>
        /// 状态栏提示信息
        /// </summary>
        public string Message
        {
            get
            {
                return mMessage;
            }
            set
            {
                if (mMessage != value)
                {
                    mMessage = value;
                    OnPropertyChanged("Message");
                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public virtual ICommand CancelCommand
        {
            get
            {
                if (mCancelCommand == null)
                {
                    mCancelCommand = new RelayCommand(() =>
                    {
                        if (CancelCommandProcess())
                        {
                            IsCancel = true;
                        }
                    }, CanCancelCommandProcess);
                }
                return mCancelCommand;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual ICommand OKCommand
        {
            get
            {
                if (mOKCommand == null)
                {
                    mOKCommand = new RelayCommand(() =>
                    {
                        if (OKCommandProcess())
                        {
                            IsOK = true;
                        }
                    }, CanOKCommandProcess);
                }
                return mOKCommand;
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public bool IsClosed
        {
            get
            {
                return mIsClosed;
            }
            set
            {
                if (mIsClosed != value)
                {
                    mIsClosed = value;
                    ClosedProcess();
                    OnPropertyChanged("IsClosed");
                }
            }
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public bool IsHidden
        {
            get
            {
                return mIsHidden;
            }
            set
            {
                if (mIsHidden != value)
                {
                    mIsHidden = value;
                    OnPropertyChanged("IsHidden");
                }
            }
        }

        /// <summary>
        /// 当前激活的窗口实例引用
        /// </summary>
        public System.Windows.Window ActiveWindow { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 隐藏窗口
        /// </summary>
        public void Hidden()
        {
            IsHidden = true;
        }

        /// <summary>
        /// 关闭处理
        /// </summary>
        protected virtual void ClosedProcess()
        {

        }

        /// <summary>
        /// 确定处理函数
        /// </summary>
        /// <returns></returns>
        protected virtual bool OKCommandProcess()
        {
            return true;
        }

        /// <summary>
        /// 取消处理函数
        /// </summary>
        /// <returns></returns>
        protected virtual bool CancelCommandProcess()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanOKCommandProcess()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanCancelCommandProcess()
        {
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            ActiveWindow = null;
        }

        /// <summary>
        /// 激活
        /// </summary>
        public virtual void Active()
        {

        }

        /// <summary>
        /// 取消激活
        /// </summary>
        public virtual void DeActive()
        {

        }

        #endregion ...Methods...

        #region ... Interfaces ...



        #endregion ...Interfaces...

        #region INotifyPropertyChanged 成员

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}
