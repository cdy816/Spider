using Cdy.Spider.CalculateExpressEditor;
using Microsoft.CodeAnalysis;
using RoslynPad.Roslyn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace Cdy.Spider.CalculateDriver.Develop
{
    /// <summary>
    /// ExpressionEditView.xaml 的交互逻辑
    /// </summary>
    public partial class ExpressionEditView : UserControl
    {

        private RoslynHost mHost;
        public ExpressionEditView()
        {
            InitializeComponent();
            this.Loaded += ExpressionEditView_Loaded;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpressionEditView_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= ExpressionEditView_Loaded;
            (this.DataContext as ExpressionEditViewModel).ExpressEditor = rc;
            Init();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Init()
        {
            List<Assembly> ass = new List<Assembly>();
            //ass.Add(typeof(Cdy.Spider.CalculateDriver.CalculateDriver).Assembly);
            ass.Add(typeof(Cdy.Spider.CalculateExpressEditor.AvalonEditExtensions).Assembly);
            ass.Add(typeof(Cdy.Spider.CalculateDriver.Develop.CalculateDriverConfigModel).Assembly);

            if (CalculateExtend.extend.ExtendDlls.Count > 0)
            {
                ass.AddRange(CalculateExtend.extend.ExtendDlls.Select(e => Assembly.LoadFile(e)));
            }

            mHost = new RoslynHost(ass.ToArray(), RoslynHostReferences.NamespaceDefault.With(new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Text.RegularExpressions.Regex).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Linq.Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Tag).Assembly.Location)
            }).With(ass.Select(e=> MetadataReference.CreateFromFile(e.Location))),new string[] { "Cdy.Spider" });

            var colors = new ClassificationHighlightColors();
            colors.DefaultBrush.Foreground = new  ICSharpCode.AvalonEdit.Highlighting.SimpleHighlightingBrush(Colors.White);
            colors.KeywordBrush.Foreground = new ICSharpCode.AvalonEdit.Highlighting.SimpleHighlightingBrush(Colors.LightBlue);
            colors.StringBrush.Foreground = new ICSharpCode.AvalonEdit.Highlighting.SimpleHighlightingBrush(Colors.OrangeRed);

            rc.Initialize(mHost, colors, Directory.GetCurrentDirectory(), (this.DataContext as ExpressionEditViewModel).Expresse);
        }

    }
}
