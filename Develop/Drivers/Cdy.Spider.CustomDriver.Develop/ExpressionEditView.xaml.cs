using Cdy.Spider.CalculateExpressEditor;
using Microsoft.CodeAnalysis;
using RoslynPad.Roslyn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Immutable;

namespace Cdy.Spider.CustomDriver.Develop
{
    /// <summary>
    /// ExpressionEditView.xaml 的交互逻辑
    /// </summary>
    public partial class ExpressionEditView : UserControl
    {

        private RoslynHost mHost;
        /// <summary>
        /// 
        /// </summary>
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
            (this.DataContext as ExpressionEditViewModel).ExpressEditor = rcInit;
            Init();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Init()
        {
            List<Assembly> ass = new List<Assembly>();
            //ass.Add(typeof(Cdy.Spider.Tag).Assembly);
            ass.Add(typeof(Cdy.Spider.CalculateExpressEditor.AvalonEditExtensions).Assembly);
            ass.Add(typeof(Cdy.Spider.CustomDriver.CustomDriver).Assembly);

            if (CalculateExtend.extend.ExtendDlls.Count > 0)
            {
                ass.AddRange(CalculateExtend.extend.ExtendDlls.Select(e => Assembly.LoadFile(e)));
            }

            mHost = new RoslynHost(ass.ToArray(), RoslynHostReferences.NamespaceDefault.With(new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Text.RegularExpressions.Regex).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Linq.Enumerable).Assembly.Location),
                //MetadataReference.CreateFromFile(typeof(Tag).Assembly.Location)
            }),new string[] { "Cdy.Spider" }, ass.Select(e=>e.Location).ToArray());


            //mHost.DefaultReferences.AddRange(ass.Select(e => mHost.CreateMetadataReference(e.Location)));

            //mHost.DefaultReferences.Add(mHost.CreateMetadataReference(typeof(Tag).Assembly.Location));


            var colors = new ClassificationHighlightColors();
            colors.DefaultBrush.Foreground = new  ICSharpCode.AvalonEdit.Highlighting.SimpleHighlightingBrush(Colors.White);
            colors.KeywordBrush.Foreground = new ICSharpCode.AvalonEdit.Highlighting.SimpleHighlightingBrush(Colors.LightBlue);
            colors.StringBrush.Foreground = new ICSharpCode.AvalonEdit.Highlighting.SimpleHighlightingBrush(Colors.OrangeRed);

            rcInit.Initialize(mHost, colors, AppDomain.CurrentDomain.BaseDirectory, (this.DataContext as ExpressionEditViewModel).Expresse);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetInitExpress()
        {
            return string.Empty;
        }

    }
}
