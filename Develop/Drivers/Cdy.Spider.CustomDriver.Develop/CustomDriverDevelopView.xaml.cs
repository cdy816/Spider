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

namespace Cdy.Spider.CustomDriver.Develop
{
    /// <summary>
    /// CustomDriverDevelopView.xaml 的交互逻辑
    /// </summary>
    public partial class CustomDriverDevelopView : UserControl
    {
        public CustomDriverDevelopView()
        {
            InitializeComponent();
            this.Loaded += CustomDriverDevelopView_Loaded;
            this.Unloaded += CustomDriverDevelopView_Unloaded;
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomDriverDevelopView_Unloaded(object sender, RoutedEventArgs e)
        {
            if(this.DataContext!=null)
            (this.DataContext as CustomDriverDevelopViewModel).CompileMessageEvent -= CustomDriverDevelopView_CompileMessageEvent;
        }

        private void CustomDriverDevelopView_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= CustomDriverDevelopView_Loaded;
            (this.DataContext as CustomDriverDevelopViewModel).InitExpressEditor = rcInit;
            (this.DataContext as CustomDriverDevelopViewModel).OnReceiveDataExpressEditor = rcOnReceive;
            (this.DataContext as CustomDriverDevelopViewModel).OnSetValueToDeviceExpressEditor = rcSetDataToDevice;
            (this.DataContext as CustomDriverDevelopViewModel).TimerProcessExpressEditor = rcTimerProcess;
            (this.DataContext as CustomDriverDevelopViewModel).InitEditor();

            rcInit.LostFocus += RcInit_LostFocus;
            rcOnReceive.LostFocus += RcOnReceive_LostFocus;
            rcSetDataToDevice.LostFocus += RcSetDataToDevice_LostFocus;
            rcTimerProcess.LostFocus += RcTimerProcess_LostFocus;
            (this.DataContext as CustomDriverDevelopViewModel).CompileMessageEvent += CustomDriverDevelopView_CompileMessageEvent;
        }

        private void CustomDriverDevelopView_CompileMessageEvent(object sender, string msg,int state)
        {
            if(cmsg.Document.Blocks.Count>100)
            {
                cmsg.SelectAll();
                cmsg.Selection.Text = "";
            }
            string mm = (state == 0 ? "Info" : "Erro") + " " + DateTime.Now.ToString() + "  " + msg;
            Paragraph para = new Paragraph();
            para.Inlines.Add(new Run() { Text = mm, Foreground = state == 0 ? Brushes.White : Brushes.Red });
            cmsg.Document.Blocks.Add(para);
            cmsg.ScrollToEnd();
        }

        private void RcTimerProcess_LostFocus(object sender, RoutedEventArgs e)
        {
            (this.DataContext as CustomDriverDevelopViewModel).UpdateOnTimeProcessExpress();
        }

        private void RcSetDataToDevice_LostFocus(object sender, RoutedEventArgs e)
        {
            (this.DataContext as CustomDriverDevelopViewModel).UpdateOnSetValueToDeviceExpress();
        }

        private void RcOnReceive_LostFocus(object sender, RoutedEventArgs e)
        {
            (this.DataContext as CustomDriverDevelopViewModel).UpdateOnReceiveExpressExpress();
        }

        private void RcInit_LostFocus(object sender, RoutedEventArgs e)
        {
            (this.DataContext as CustomDriverDevelopViewModel).UpdateInitExpress();
        }
    }
}
