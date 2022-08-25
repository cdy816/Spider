//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/3/17 17:06:11.
//  Version 1.0
//  种道洋
//==============================================================

using InSpiderDevelopWindow;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace InSpiderDevelopWindow
{
    internal enum AccentState
    {
        ACCENT_DISABLED = 0,
        ACCENT_ENABLE_GRADIENT = 1,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND = 3,
        ACCENT_INVALID_STATE = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public int AccentFlags;
        public int GradientColor;
        public int AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    internal enum WindowCompositionAttribute
    {
        // ...
        WCA_ACCENT_POLICY = 19
        // ...
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomWindowBase : Window
    {
        #region ... Variables  ...

        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        private ContentControl mContentHost;

        private Grid mHead;
        private Grid mMain;
        private Border mBorder;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        static CustomWindowBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomWindowBase), new FrameworkPropertyMetadata(typeof(CustomWindowBase)));
        }

        /// <summary>
        /// 
        /// </summary>
        public CustomWindowBase()
            : base()
        {
            this.AllowsTransparency = true;
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.CanResizeWithGrip;
            this.Loaded += new RoutedEventHandler(CustomWindowBase_Loaded);
        }



        #endregion ...Constructor...

        #region ... Properties ...


        /// <summary>
        /// 是否确认关闭窗口
        /// </summary>
        public bool IsOK
        {
            get { return (bool)GetValue(IsOKProperty); }
            set { SetValue(IsOKProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsOKProperty = DependencyProperty.Register("IsOK", typeof(bool), typeof(CustomWindowBase), new UIPropertyMetadata(false, new PropertyChangedCallback(OKPropertyChanged)));


        /// <summary>
        /// 是否取消关闭窗口
        /// </summary>
        public bool IsCancel
        {
            get { return (bool)GetValue(IsCancelProperty); }
            set { SetValue(IsCancelProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsCancelProperty = DependencyProperty.Register("IsCancel", typeof(bool), typeof(CustomWindowBase), new UIPropertyMetadata(false, new PropertyChangedCallback(CancelPropertyChanged)));



        /// <summary>
        /// 已经关闭
        /// </summary>
        public bool IsClosed
        {
            get { return (bool)GetValue(IsClosedProperty); }
            set { SetValue(IsClosedProperty, value); }
        }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register("IsClosed", typeof(bool), typeof(CustomWindowBase), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        /// <summary>
        /// 
        /// </summary>
        public bool IsEnableMax
        {
            get { return (bool)GetValue(IsEnableMaxProperty); }
            set { SetValue(IsEnableMaxProperty, value); }
        }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsEnableMaxProperty = DependencyProperty.Register("IsEnableMax", typeof(bool), typeof(CustomWindowBase), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(IsEnableMaxPropertyChanged)));



        /// <summary>
        /// 隐藏窗口
        /// </summary>
        public bool IsHidden
        {
            get { return (bool)GetValue(IsHiddenProperty); }
            set { SetValue(IsHiddenProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsHiddenProperty = DependencyProperty.Register("IsHidden", typeof(bool), typeof(CustomWindowBase), new UIPropertyMetadata(false, new PropertyChangedCallback(IsHiddenPropertyChanged)));

        /// <summary>
        /// 图标
        /// </summary>
        public string IconString
        {
            get { return (string)GetValue(IconStringProperty); }
            set { SetValue(IconStringProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IconStringProperty = DependencyProperty.Register("IconString", typeof(string), typeof(CustomWindowBase), new UIPropertyMetadata(""));

        #endregion ...Properties...

        #region ... Methods    ...
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="window"></param>
        //public static void RemoveIcon(Window window)
        //{
        //    IntPtr hwnd = new WindowInteropHelper(window).Handle;
        //    int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
        //    SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);
        //    SendMessage(hwnd, WM_SETICON, new IntPtr(1), IntPtr.Zero);
        //    SendMessage(hwnd, WM_SETICON, IntPtr.Zero, IntPtr.Zero);
        //}

        ///// <summary>
        ///// Disables the minimizebox.
        ///// </summary>
        ///// <param name="window">The window.</param>
        //public static void DisableMinimizebox(Window window)
        //{
        //    var hwnd = new WindowInteropHelper(window).Handle;
        //    var value = GetWindowLong(hwnd, GWL_STYLE);
        //    SetWindowLong(hwnd, GWL_STYLE, (int)(value & ~WS_MINIMIZEBOX));
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomWindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= (CustomWindowBase_Loaded);
            EnableBlur();
            if (!string.IsNullOrEmpty(IconString))
            {
                this.Icon = new BitmapImage(new Uri(IconString));
            }
            this.Activate();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            mContentHost = this.GetTemplateChild("content_host") as ContentControl;
            mHead = this.GetTemplateChild("head") as Grid;
            mHead.MouseLeftButtonDown += MHead_MouseLeftButtonDown;

            mMain = this.GetTemplateChild("main") as Grid;

            (this.GetTemplateChild("minB") as Button).Click += minB_Click;
            if (this.IsEnableMax)
            {
                (this.GetTemplateChild("maxB") as Button).Click += maxB_Click;
            }
            else
            {
                (this.GetTemplateChild("maxB") as Button).Visibility = Visibility.Collapsed;
            }

            (this.GetTemplateChild("closeB") as Button).Click += closeB_Click;

            mBorder = this.GetTemplateChild("bd") as Border;
            if (mBorder != null)
            {
                mBorder.SizeChanged += MBorder_SizeChanged;
                InitBd();
            }

        }
        private void MBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            mBorder.Clip = new System.Windows.Media.RectangleGeometry() { Rect = new Rect(1, 1, mBorder.ActualWidth - 2, mBorder.ActualHeight - 2), RadiusX = 5, RadiusY = 5 };
        }

        private void InitBd()
        {
            Border bd = new Border();
            Grid.SetRowSpan(bd, 3);
            bd.BorderThickness = new Thickness(2);
            bd.BorderBrush = System.Windows.Media.Brushes.DarkGray;
            bd.CornerRadius = new CornerRadius(5);
            mMain.Children.Add(bd);
        }
        internal void EnableBlur()
        {
            var windowHelper = new WindowInteropHelper(this);

            var accent = new AccentPolicy();
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

            var accentStructSize = Marshal.SizeOf(accent);

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;

            SetWindowCompositionAttribute(windowHelper.Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeB_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void maxB_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void minB_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }



        private void MHead_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1 && IsEnableMax)
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
                else
                {
                    WindowState = WindowState.Maximized;
                }
            }
            else
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private static void IsEnableMaxPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            var sd = (sender as CustomWindowBase);
            if (sd.IsEnableMax)
            {
                sd.ResizeMode = ResizeMode.CanResizeWithGrip;
            }
            else
            {
                sd.ResizeMode = ResizeMode.NoResize;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private static void OKPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as CustomWindowBase).OKClose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private static void CancelPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as CustomWindowBase).CancelClose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private static void IsHiddenPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            if ((bool)arg.NewValue)
            {
                (sender as CustomWindowBase).Hidden();
            }
        }


        /// <summary>
        /// 隐藏窗口
        /// </summary>
        internal void Hidden()
        {
            this.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 确认关闭窗口
        /// </summary>
        internal void OKClose()
        {
            if (System.Windows.Interop.ComponentDispatcher.IsThreadModal && this.IsLoaded)
                DialogResult = true;
            Close();
        }

        /// <summary>
        /// 取消关闭窗口
        /// </summary>
        internal void CancelClose()
        {
            Close();
        }

        /// <summary>
        /// 窗口关闭后回调
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IsClosed = true;
            if (this.DataContext is IDisposable)
            {
                (this.DataContext as IDisposable).Dispose();
            }
            mContentHost.Content = null;
            mContentHost = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (this.DataContext is WindowViewModelBase)
            {
                (this.DataContext as WindowViewModelBase).Active();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            if (this.DataContext is WindowViewModelBase)
            {
                (this.DataContext as WindowViewModelBase).DeActive();
            }
        }


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
