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

namespace InSpiderDevelopWindow.View
{
    /// <summary>
    /// ServerUserGroupDetailView.xaml 的交互逻辑
    /// </summary>
    public partial class ServerUserGroupDetailView : UserControl
    {
        public ServerUserGroupDetailView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                (sender as TextBox).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).IsVisible)
            { 
                (sender as TextBox).Focus();
                (sender as TextBox).CaretIndex = (sender as TextBox).Text.Length;
                (sender as TextBox).SelectedText = " ";
                (sender as TextBox).SelectedText = "";
            }
        }
    }
}
