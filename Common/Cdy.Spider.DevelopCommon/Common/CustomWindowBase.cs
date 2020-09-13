//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/3/17 17:06:11.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Cdy.Spider.DevelopCommon
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomWindowBase : Window
    {
        #region ... Variables  ...

        //[DllImport("user32.dll")]
        //static extern int GetWindowLong(IntPtr hwnd, int index);

        //[DllImport("user32.dll")]
        //static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        //[DllImport("user32.dll")]
        //static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
        //           int x, int y, int width, int height, uint flags);

        //[DllImport("user32.dll")]
        //static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

        //private const int GWL_EXSTYLE = -20;
        //private const int WS_EX_DLGMODALFRAME = 0x0001;
        //private const int SWP_NOSIZE = 0x0001;
        //private const int SWP_NOMOVE = 0x0002;
        //private const int SWP_NOZORDER = 0x0004;
        //private const int SWP_FRAMECHANGED = 0x0020;
        //private const uint WM_SETICON = 0x0080;

        //private const int GWL_STYLE = -16;
        //private const int WS_MAXIMIZEBOX = 0x00010000;
        //private const int WS_MINIMIZEBOX = 0x00020000;

        private ContentControl mContentHost;

        private Grid mHead;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomWindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= (CustomWindowBase_Loaded);
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
            var bdv = (this.GetTemplateChild("bdb") as Border);
            var bdh = this.GetTemplateChild("rtb") as Border;

            var bdvh = this.GetTemplateChild("rbdb") as Border;

            bdv.MouseLeftButtonDown += bdv_MouseLeftButtonDown;
            bdv.MouseMove += Bdv_MouseMove;
            bdv.MouseLeftButtonUp += Bd_MouseLeftButtonUp;


            bdh.MouseLeftButtonDown += bdh_MouseLeftButtonDown;
            bdh.MouseMove += Bdh_MouseMove;
            bdh.MouseLeftButtonUp += Bd_MouseLeftButtonUp;

            bdvh.MouseLeftButtonDown += Bdvh_MouseLeftButtonDown;
            bdvh.MouseMove += Bdvh_MouseMove;
            bdvh.MouseLeftButtonUp += Bd_MouseLeftButtonUp;
        }

        private void Bdvh_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var vpp = e.GetPosition(this);
                var ddy = vpp.Y;
                var ddx = vpp.X;
                this.Width += (ddx - dx);
                this.Height += (ddy - dy);
                dy = ddy;
                dx = ddx;
            }
        }

        private void Bdvh_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (sender as Border).CaptureMouse();
            var vpp = e.GetPosition(this);
            dy = vpp.Y;
            dx = vpp.X;
            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bd_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            (sender as Border).ReleaseMouseCapture();
            e.Handled = true;
        }

        private double dx,dy;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bdv_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var ddy = e.GetPosition(this).Y;
                this.Height += (ddy - dy);
                dy = ddy;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bdh_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var ddx = e.GetPosition(this).X;
                this.Width += (ddx - dx);
                dx = ddx;
            }
        }

        private void bdv_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            (sender as Border).CaptureMouse();
            dy =  e.GetPosition(this).Y;
            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdh_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            (sender as Border).CaptureMouse();
            dx = e.GetPosition(this).X;
            e.Handled = true;
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
