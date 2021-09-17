
using Cdy.Spider;
using Cdy.Spider.CalculateExpressEditor;
using Microsoft.CodeAnalysis;
using RoslynPad.Roslyn;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InSpiderDevelopWindow.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvertEditViewModel: WindowViewModelBase
    {

        private ConvertViewModel mCurrentSelectModel;

        /// <summary>
        /// 
        /// </summary>
        private System.Collections.ObjectModel.Collection<ConvertViewModel> mItems = new System.Collections.ObjectModel.Collection<ConvertViewModel>();

        /// <summary>
        /// 
        /// </summary>
        static ConvertEditViewModel()
        {
            ValueConvertManager.manager.Init();
        }

        /// <summary>
        /// 
        /// </summary>
        public ConvertEditViewModel(Tagbase tag)
        {
            Tag = tag;
            Init();
            DefaultWidth = 500;
            DefaultHeight = 200;
            Title = Res.Get("Convert");
        }

        /// <summary>
        /// 
        /// </summary>
        public Tagbase Tag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Collections.ObjectModel.Collection<ConvertViewModel> Items
        {
            get
            {
                return mItems;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ConvertViewModel CurrentSelectModel
        {
            get
            {
                return mCurrentSelectModel;
            }
            set
            {
                mCurrentSelectModel = value;
                OnPropertyChanged("CurrentSelectModel");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Init()
        {
            if (new LinerConvert().SupportTag(Tag))
            {
                mItems.Add(new LinearConvertViewModel() { Model = new LinerConvert() });
            }
            if (new NumberToBitConvert().SupportTag(Tag))
                mItems.Add(new NumberToBitConvertViewModel() { Model = new NumberToBitConvert() });
            if (new BitInvertConvert().SupportTag(Tag))
                mItems.Add(new BitInvertConvertViewModel() { Model = new BitInvertConvert() });
            if (new StringFormateConvert().SupportTag(Tag))
                mItems.Add(new StringFormatConvertViewModel() { Model = new StringFormateConvert() });

            if (new AdvanceConvert().SupportTag(Tag))
                mItems.Add(new AdvanceConvertViewModel() { Model = new AdvanceConvert() });

            CurrentSelectModel = mItems.First();
        }

        public void SetSelectConvert(string cstring)
        {
            var cc = cstring.DeSeriseToValueConvert();
            if (cc == null) return;

            foreach(var vv in Items)
            {
                if(vv.Name == cc.Name)
                {
                    CurrentSelectModel = vv;
                    CurrentSelectModel.Model = cc;
                }
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class ConvertViewModel : ViewModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get { return Model.Name; } }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get { return Res.Get(Model.Name); } }

        /// <summary>
        /// 
        /// </summary>
        public IValueConvert  Model { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class LinearConvertViewModel : ConvertViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public double K
        {
            get
            {
                return (Model as LinerConvert).K;
            }
            set
            {
                var mm = Model as LinerConvert;
                if(mm.K!=value)
                {
                    mm.K = value;
                    OnPropertyChanged("K");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double T
        {
            get
            {
                return (Model as LinerConvert).T;
            }
            set
            {
                var mm = Model as LinerConvert;
                if (mm.T != value)
                {
                    mm.T = value;
                    OnPropertyChanged("T");
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class NumberToBitConvertViewModel: ConvertViewModel
    {
        /// <summary>
            /// 
            /// </summary>
        public byte Index
        {
            get
            {
                return (Model as NumberToBitConvert).Index;
            }
            set
            {
                if ((Model as NumberToBitConvert).Index != value)
                {
                    (Model as NumberToBitConvert).Index = value;
                    OnPropertyChanged("Index");
                }
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class BitInvertConvertViewModel : ConvertViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Enable
        {
            get
            {
                return (Model as BitInvertConvert).Enable;
            }
            set
            {
                if ((Model as BitInvertConvert).Enable != value)
                {
                    (Model as BitInvertConvert).Enable = value;
                    OnPropertyChanged("Enable");
                }
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class StringFormatConvertViewModel : ConvertViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Formate
        {
            get
            {
                return (Model as StringFormateConvert).Formate;
            }
            set
            {
                if ((Model as StringFormateConvert).Formate != value)
                {
                    (Model as StringFormateConvert).Formate = value;
                    OnPropertyChanged("Formate");
                }
            }
        }

    }

    public class AdvanceConvertViewModel : ConvertViewModel
    {
        private RoslynHost mHost;

        public const string mOnVariableDefineTemplate =
@"using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Buffers;
using System.Threading;
using Cdy.Spider;
namespace Cdy.Spider
{
public class Driver:Cdy.Spider.ConvertExecuteContext
{
public void Init(){
$VariableBody$
}}
}";

        /// <summary>
        /// 
        /// </summary>
        public RoslynCodeEditor ExpressEditor
        {
            get;
            set;
        }

        public RoslynCodeEditor CallBackExpressEditor
        {
            get;
            set;
        }

        public AdvanceConvertViewModel()
        {
            InnerInit();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InnerInit()
        {
            List<Assembly> ass = new List<Assembly>();
            ass.Add(typeof(Cdy.Spider.CalculateExpressEditor.AvalonEditExtensions).Assembly);
            ass.Add(typeof(Cdy.Spider.Tagbase).Assembly);

            if (CalculateExtend.extend.ExtendDlls.Count > 0)
            {
                ass.AddRange(CalculateExtend.extend.ExtendDlls.Where(e=> System.IO.File.Exists(e)).Select(e => Assembly.LoadFile(e)));
            }
            
            mHost = new RoslynHost(ass.ToArray(), RoslynHostReferences.NamespaceDefault.With(new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Text.RegularExpressions.Regex).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Linq.Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Cdy.Spider.ConvertExecuteContext).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Xml.Linq.XElement).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.MemoryExtensions).Assembly.Location)
                //MetadataReference.CreateFromFile(typeof(Tag).Assembly.Location)
            }), new string[] { "Cdy.Spider" }, ass.Select(e => e.Location).ToArray());
        }


        public void InitEditor()
        {

            var colors = new ClassificationHighlightColors();
            colors.DefaultBrush.Foreground = new ICSharpCode.AvalonEdit.Highlighting.SimpleHighlightingBrush(System.Windows.Media.Colors.White);
            colors.KeywordBrush.Foreground = new ICSharpCode.AvalonEdit.Highlighting.SimpleHighlightingBrush(System.Windows.Media.Colors.LightBlue);
            colors.StringBrush.Foreground = new ICSharpCode.AvalonEdit.Highlighting.SimpleHighlightingBrush(System.Windows.Media.Colors.OrangeRed);

            ExpressEditor.Initialize(mHost, colors, AppDomain.CurrentDomain.BaseDirectory, mOnVariableDefineTemplate.Replace("$VariableBody$", string.IsNullOrEmpty((Model as AdvanceConvert).Express)?"return Value;": (Model as AdvanceConvert).Express));
            CallBackExpressEditor.Initialize(mHost, colors, AppDomain.CurrentDomain.BaseDirectory, mOnVariableDefineTemplate.Replace("$VariableBody$", string.IsNullOrEmpty((Model as AdvanceConvert).ConvertBackExpress)?"return Value;": (Model as AdvanceConvert).ConvertBackExpress));

        }

        /// <summary>
        /// 
        /// </summary>
        public string Express
        {
            get
            {
                return (Model as AdvanceConvert).Express;
            }
            set
            {
                (Model as AdvanceConvert).Express = SubString(ExpressEditor.Text, "$VariableBody$", mOnVariableDefineTemplate);
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public string CallBackExpress
        {
            get
            {
                return (Model as AdvanceConvert).ConvertBackExpress;
            }
            set
            {
                (Model as AdvanceConvert).ConvertBackExpress = SubString(CallBackExpressEditor.Text, "$VariableBody$", mOnVariableDefineTemplate);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="org"></param>
        /// <param name="key"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        private string SubString(string org, string key, string template)
        {
            int sindex = template.IndexOf(key);
            int eindex = template.Length - sindex - key.Length;
            string re = org;
            re = re.Substring(sindex);
            re = re.Substring(0, re.Length - eindex);
            return re;
        }

    }
}
