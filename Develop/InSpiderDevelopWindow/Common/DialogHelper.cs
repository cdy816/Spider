//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/3/17 17:16:04.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace InSpiderDevelopWindow
{
    public static class DialogHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Window GetActiveWindow()
        {
            if (Application.Current != null)
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.IsActive)
                    {
                        return w;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="topMost"></param>
        /// <param name="titleBar"></param>
        /// <param name="showInCenter"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void Show(this WindowViewModelBase model, bool topMost = false, bool titleBar = true, bool showInCenter = true, int x = int.MinValue, int y = int.MinValue)
        {
            CustomWindowBase cwb = new CustomWindowBase();
            cwb.Owner = GetActiveWindow();
            if (showInCenter)
            {
                if (cwb.Owner != null)
                {
                    cwb.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                }
                else
                {
                    cwb.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
            }
            else
            {
                cwb.WindowStartupLocation = WindowStartupLocation.Manual;
                cwb.Left = x;
                cwb.Top = y;
            }

            cwb.DataContext = model;
            if (model.MinHeight != null)
            {
                cwb.MinHeight = model.MinHeight.Value;
            }
            if (model.MinWidth != null)
            {
                cwb.MinWidth = model.MinWidth.Value;
            }

            if (model.DefaultHeight != null)
            {
                if (titleBar)
                {
                    if (model.IsOkCancel)
                    {

                        cwb.Height = model.DefaultHeight.Value + 70;
                    }
                    else
                    {
                        cwb.Height = model.DefaultHeight.Value + 10;
                    }
                }
                cwb.SizeToContent = SizeToContent.Manual;
            }

            if (model.DefaultWidth != null)
            {
                if (titleBar)
                    cwb.Width = model.DefaultWidth.Value + 16;
                cwb.SizeToContent = SizeToContent.Manual;
            }
            cwb.Topmost = topMost;

            if (!titleBar)
            {
                cwb.WindowStyle = WindowStyle.None;
            }
            model.ActiveWindow = cwb;

            cwb.Show();
        }

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="model"></param>
        /// <param name="topMost">是否顶层窗口</param>
        /// <param name="titleBar">是否需要显示TitleBar</param>
        /// <returns></returns>
        public static bool? ShowDialog(this WindowViewModelBase model, bool topMost = false, bool titleBar = true)
        {
            CustomWindowBase cwb = new CustomWindowBase();
            cwb.Owner = GetActiveWindow();
            if (cwb.Owner != null)
            {
                cwb.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
            {
                cwb.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //cwb.Topmost = true;
            }

            cwb.DataContext = model;
            if (model.MinHeight != null)
            {
                cwb.MinHeight = model.MinHeight.Value;
            }
            if (model.MinWidth != null)
            {
                cwb.MinWidth = model.MinWidth.Value;
            }

            if (model.DefaultHeight != null)
            {
                if (titleBar)
                {
                    if (model.IsOkCancel)
                    {

                        cwb.Height = model.DefaultHeight.Value + 70;
                    }
                    else
                    {
                        cwb.Height = model.DefaultHeight.Value + 10;
                    }
                }
                cwb.SizeToContent = SizeToContent.Manual;
            }

            if (model.DefaultWidth != null)
            {
                if (titleBar)
                    cwb.Width = model.DefaultWidth.Value + 16;
                cwb.SizeToContent = SizeToContent.Manual;
            }
            cwb.Topmost = topMost;

            if (!titleBar)
            {
                cwb.WindowStyle = WindowStyle.None;
            }

            return cwb.ShowDialog().Value;
        }

    }
}
