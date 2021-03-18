using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cdy.Spider.OpcDriver.Develop
{
    /// <summary>
    /// OpcBrowserView.xaml 的交互逻辑
    /// </summary>
    public partial class OpcBrowserView : UserControl
    {
        public OpcBrowserView()
        {
            InitializeComponent();
            this.Loaded += OpcBrowserView_Loaded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpcBrowserView_Loaded(object sender, RoutedEventArgs e)
        {
            (this.DataContext as OpcBrowserViewModel).GridInstance = dg;
            (this.DataContext as OpcBrowserViewModel).CheckAutoConnect();
        }

        private void dg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                var cmd = (this.DataContext as OpcBrowserViewModel).OKCommand;
                if (cmd.CanExecute(null))
                {
                    cmd.Execute(null);
                }
            }
        }
    }
}
