using InSpiderDevelopWindow.ViewModel;
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
    /// DeviceDetailView.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceDetailView : UserControl
    {
        DeviceDetailViewModel mModel;
        /// <summary>
        /// 
        /// </summary>
        public DeviceDetailView()
        {
            InitializeComponent();
            this.Loaded += DeviceDetailView_Loaded;
        }

        private void DeviceDetailView_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= DeviceDetailView_Loaded;
            mModel = (this.DataContext as DeviceDetailViewModel);
            mModel.grid = this.dg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="ll"></param>
        private void GetVisualParent(DependencyObject target,List<string> ll)
        {
            var parent = LogicalTreeHelper.GetParent(target);
            if(parent!=null)
            {
                ll.Add(parent.ToString());
                GetVisualParent(parent, ll);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (((sender as DataGrid).SelectionUnit == DataGridSelectionUnit.CellOrRowHeader)|| ((sender as DataGrid).SelectionUnit == DataGridSelectionUnit.Cell))
            {
                (sender as DataGrid).EndInit();
                (sender as DataGrid).BeginEdit();
            }
            mModel.SelectedCells = (sender as DataGrid).SelectedCells;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kwinput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                (sender as FrameworkElement).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            List<string> ll = new List<string>();
            GetVisualParent(e.OriginalSource as DependencyObject, ll);
            if (ll.Contains("System.Windows.Controls.Primitives.Popup")) return;

            var dscoll = (e.ExtentHeight - e.ViewportHeight);

            if(dscoll>0 && (dscoll - e.VerticalOffset)/dscoll < 0.25)
            {
                if (mModel.CanContinueLoadData())
                    mModel.ContinueLoad();
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
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            if ((sender as ComboBox).IsVisible)
            {
                (sender as ComboBox).Focus();
            }
        }
    }
}
