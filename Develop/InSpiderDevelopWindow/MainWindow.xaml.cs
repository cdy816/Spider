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

namespace InSpiderDevelopWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double mOldHeight;
        private double mOldWidth;

        private WindowState mWindowState;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            InitBd();
            this.SizeChanged += MainWindow_SizeChanged;
            this.StateChanged += MainWindow_StateChanged;
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (ServerHelper.Helper.AutoLogin)
                (this.DataContext as MainViewModel).AutoLogin();
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                Max();
            }
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            bd.Clip = new RectangleGeometry() { Rect = new Rect(1, 1, bd.ActualWidth - 2, bd.ActualHeight - 2), RadiusX = 5, RadiusY = 5 };
        }

        private void InitBd()
        {
            Border bd = new Border();
            Grid.SetRowSpan(bd, 3);
            bd.BorderThickness = new Thickness(2);
            bd.BorderBrush = Brushes.DarkGray;
            bd.CornerRadius = new CornerRadius(5);
            bg.Children.Add(bd);
        }

        private void closeB_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as MainViewModel).CheckAndSave();
            this.Close();
        }

        private void minB_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //(sender as Grid).CaptureMouse();
            if (e.ClickCount > 1)
            {
                if (mWindowState == WindowState.Maximized)
                {
                    Normal();

                }
                else
                {
                    Max();
                }
            }
            else
            {

                if (mWindowState == WindowState.Maximized)
                {

                    double dsx = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.M11;

                    var rec = this.RestoreBounds;
                    var ll = e.GetPosition(this);
                    var dx = ll.X / this.ActualWidth * mOldWidth;
                    var dy = ll.Y / this.ActualHeight * mOldHeight;
                    var pp = this.PointToScreen(ll);

                    pp = new Point(pp.X / dsx, pp.Y / dsx);

                    this.Left = pp.X - dx - 8;
                    this.Top = pp.Y - dy - 4;
                    this.Normal(false);

                }

                this.DragMove();
            }
        }

        private void Max()
        {
            mOldHeight = this.Height;
            mOldWidth = this.Width;
            this.Top = SystemParameters.WorkArea.Top;
            this.Left = SystemParameters.WorkArea.Left;
            this.Height = SystemParameters.WorkArea.Height;
            this.Width = SystemParameters.WorkArea.Width;
            mWindowState = WindowState.Maximized;
        }

        private void Normal(bool iscenter = true)
        {
            this.Height = mOldHeight;
            this.Width = mOldWidth;
            mWindowState = WindowState.Normal;
            if (iscenter)
            {
                this.Left = (SystemParameters.WorkArea.Width - this.Width) / 2;
                this.Top = (SystemParameters.WorkArea.Height - this.Height) / 2;
            }
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
           // (sender as Grid).ReleaseMouseCapture();
        }

        private void maxB_Click(object sender, RoutedEventArgs e)
        {
            if(WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //(sender as TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
                (sender as TextBox).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void TextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((sender as TextBox).IsVisible)
            {
                (sender as TextBox).SelectAll();
                (sender as TextBox).Focus();

            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
            (sender as TextBox).Focus();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            (this.DataContext as MainViewModel).CurrentSelectTreeItem = tv.SelectedItem as TreeItemViewModel;
        }

        private void tv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
            //(this.DataContext as MainViewModel).CurrentSelectTreeItem = null;
        }
    }
}
