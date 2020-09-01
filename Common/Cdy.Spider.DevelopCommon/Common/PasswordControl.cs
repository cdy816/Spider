using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cdy.Spider.DevelopCommon
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:DBInStudio.Desktop.Common"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:DBInStudio.Desktop.Common;assembly=DBInStudio.Desktop.Common"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:PasswordControl/>
    ///
    /// </summary>
    public class PasswordControl : Control
    {
        static PasswordControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PasswordControl), new FrameworkPropertyMetadata(typeof(PasswordControl)));
        }


        #region ... Variables  ...
        private PasswordBox mpb;
        private TextBox mtb;
        private bool mIgnorNotifyChanged = false;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(PasswordControl), new PropertyMetadata("",new PropertyChangedCallback(PasswordChanged)));


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private static void PasswordChanged(DependencyObject sender,DependencyPropertyChangedEventArgs arg)
        {
            (sender as PasswordControl).ApplyPasswordToPasswordBox();
        }



        /// <summary>
        /// 显示Password
        /// </summary>
        public bool IsShowPassword
        {
            get { return (bool)GetValue(IsShowPasswordProperty); }
            set { SetValue(IsShowPasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShowPassword.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowPasswordProperty =  DependencyProperty.Register("IsShowPassword", typeof(bool), typeof(PasswordControl), new PropertyMetadata(false,new PropertyChangedCallback(IsShowPasswordChanged)));

        private static void IsShowPasswordChanged(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            (sender as PasswordControl).ApplyPasswordBoxVisiable();
        }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        private void ApplyPasswordBoxVisiable()
        {
            if(IsShowPassword)
            {
                mtb.Visibility = Visibility.Visible;
                mpb.Visibility = Visibility.Hidden;
            }
            else
            {
                mtb.Visibility = Visibility.Hidden;
                mpb.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ApplyPasswordToPasswordBox()
        {
            if (!mIgnorNotifyChanged)
            {
                if(mpb!=null)
                mpb.Password = Password;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            mpb = GetTemplateChild("pb") as PasswordBox;
            mtb = GetTemplateChild("tb") as TextBox;
            if (mpb != null)
            {
                mpb.PasswordChanged += Mpb_PasswordChanged;
            }
            ApplyPasswordBoxVisiable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mpb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            mIgnorNotifyChanged = true;
            this.Password = (sender as PasswordBox).Password;
            mIgnorNotifyChanged = false;
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
