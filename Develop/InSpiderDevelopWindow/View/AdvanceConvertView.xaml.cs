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
    /// AdvanceConvertView.xaml 的交互逻辑
    /// </summary>
    public partial class AdvanceConvertView : UserControl
    {
        public AdvanceConvertView()
        {
            InitializeComponent();
            this.Loaded += AdvanceConvertView_Loaded;
        }

        private void AdvanceConvertView_Loaded(object sender, RoutedEventArgs e)
        {
            (this.DataContext as InSpiderDevelopWindow.ViewModel.AdvanceConvertViewModel).ExpressEditor = exp;
            (this.DataContext as InSpiderDevelopWindow.ViewModel.AdvanceConvertViewModel).CallBackExpressEditor = expback;
            (this.DataContext as InSpiderDevelopWindow.ViewModel.AdvanceConvertViewModel).InitEditor();
            exp.LostFocus += Exp_LostFocus;
            expback.LostFocus += Expback_LostFocus;
        }

        private void Expback_LostFocus(object sender, RoutedEventArgs e)
        {
            if(this.DataContext!=null)
            (this.DataContext as InSpiderDevelopWindow.ViewModel.AdvanceConvertViewModel).CallBackExpress = expback.Text;
        }

        private void Exp_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                (this.DataContext as InSpiderDevelopWindow.ViewModel.AdvanceConvertViewModel).Express = exp.Text;
        }
    }
}
