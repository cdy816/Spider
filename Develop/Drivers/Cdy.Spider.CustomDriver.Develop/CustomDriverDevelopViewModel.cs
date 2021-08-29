using Cdy.Spider.CalculateExpressEditor;
using Cdy.Spider.DevelopCommon;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RoslynPad.Roslyn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Cdy.Spider.CustomDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomDriverDevelopViewModel : ViewModelBase
    {

        #region ... Variables  ...
        private string mOnVariableExpresse = "";
        private string mOnReceiveDataExpress = "";
        private string mOnTimerProcessExpress = "";
        private string mOnSetValueToDeviceExpress = "";

        private RoslynHost mHost;

        public const string mOnReceiveDataTemplate =
@"using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Buffers;
using System.Threading;
using Cdy.Spider.CustomDriver;
namespace Cdy.Spider
{
public class Driver:Cdy.Spider.CustomDriver.CustomImp
{
$VariableBody$
public override object OnReceiveData(string key, object data)
{
$OnReceiveDataBody$
}
}
}";

        public const string mOnVariableDefineTemplate =
@"using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Buffers;
using System.Threading;
using Cdy.Spider.CustomDriver;
namespace Cdy.Spider
{
public class Driver:Cdy.Spider.CustomDriver.CustomImp
{
$VariableBody$
}
}";

        public const string mOnInitTemplate = 
@"using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Buffers;
using System.Threading;
using Cdy.Spider.CustomDriver;
namespace Cdy.Spider
{
public class Driver:Cdy.Spider.CustomDriver.CustomImp
{
public override void Init()
{
$InitBody$
}
}
}";

public const string mOnProcessTimerElapsedTemplate =
@"using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Buffers;
using System.Threading;
using Cdy.Spider.CustomDriver;
namespace Cdy.Spider
{
public class Driver:Cdy.Spider.CustomDriver.CustomImp
{
$VariableBody$
public override void ProcessTimerElapsed()
{
$ProcessTimerElapsed$
}
}
}";

public const string mOnSetValueToDeviceTemplate =
@"using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Buffers;
using System.Threading;
using Cdy.Spider.CustomDriver;
namespace Cdy.Spider
{
public class Driver:Cdy.Spider.CustomDriver.CustomImp
{
$VariableBody$
public override void WriteValue(string deviceInfo, object value, byte valueType)
{
$WriteValue$
}
}
}";

        private CustomDriverData mModel;

        private ICommand mVariableCompileCommand;
        private ICommand mOnReceiveDataCompileCommand;
        private ICommand mOnSetValueToDeviceCommand;
        private ICommand mOnTimerProcessCommand;

        public delegate void CompileMessage(object sender, string msg,int state);

        public event CompileMessage CompileMessageEvent;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public CustomDriverDevelopViewModel()
        {
            InnerInit();
        }
        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public ICommand VariableCompileCommand
        {
            get
            {
                if(mVariableCompileCommand==null)
                {
                    mVariableCompileCommand = new RelayCommand(() => {
                        Compile(InitExpressEditor.Text);
                    });
                }
                return mVariableCompileCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand OnReceiveDataCompileCommand
        {
            get
            {
                if(mOnReceiveDataCompileCommand==null)
                {
                    mOnReceiveDataCompileCommand = new RelayCommand(() => {
                        Compile(OnReceiveDataExpressEditor.Text);
                    });
                }
                return mOnReceiveDataCompileCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand OnTimerProcessCommand
        {
            get
            {
                if(mOnTimerProcessCommand==null)
                {
                    mOnTimerProcessCommand = new RelayCommand(() => {
                        Compile(TimerProcessExpressEditor.Text);
                    });
                }
                return mOnTimerProcessCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand OnSetValueToDeviceCommand
        {
            get
            {
                if(mOnSetValueToDeviceCommand==null)
                {
                    mOnSetValueToDeviceCommand = new RelayCommand(() => {
                        Compile(OnSetValueToDeviceExpressEditor.Text);
                    });
                }
                return mOnSetValueToDeviceCommand;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public CustomDriverData Model { get { return mModel; } set { mModel = value;
                mOnVariableExpresse = mOnVariableDefineTemplate.Replace("$VariableBody$", mModel.VariableExpress);
                mOnReceiveDataExpress = mOnReceiveDataTemplate.Replace("$OnReceiveDataBody$", mModel.OnReceiveDataFunExpress).Replace("$VariableBody$", mModel.VariableExpress.Replace("\r\n","").Replace("\n", ""));
                mOnSetValueToDeviceExpress = mOnSetValueToDeviceTemplate.Replace("$WriteValue$", mModel.OnSetTagValueToDeviceFunExpress).Replace("$VariableBody$", mModel.VariableExpress.Replace("\r\n", "").Replace("\n", ""));
                mOnTimerProcessExpress = mOnProcessTimerElapsedTemplate.Replace("$ProcessTimerElapsed$", mModel.OnTimerFunExpress).Replace("$VariableBody$", mModel.VariableExpress.Replace("\r\n", "").Replace("\n", ""));
            } }



        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Visibility ScanCircleVisibility
        {
            get
            {
                return Model.Model == WorkMode.Active ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int ScanCircle
        {
            get
            {
                return Model.ScanCircle;
            }
            set
            {
                if (Model.ScanCircle != value)
                {
                    Model.ScanCircle = value;
                    OnPropertyChanged("ScanCircle");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public RoslynCodeEditor InitExpressEditor
        {
            get;
            set;
        }


        public RoslynCodeEditor TimerProcessExpressEditor
        {
            get;
            set;
        }


        public RoslynCodeEditor OnReceiveDataExpressEditor
        {
            get;
            set;
        }

        public RoslynCodeEditor OnSetValueToDeviceExpressEditor
        {
            get;
            set;
        }

        // <summary>
        /// 
        /// </summary>
        public int WorkModel
        {
            get
            {
                return (int)Model.Model;
            }
            set
            {
                if ((int)Model.Model != value)
                {
                    Model.Model = (WorkMode)value;
                    OnPropertyChanged("WorkModel");
                    OnPropertyChanged("ScanCircleVisibility");
                }
            }
        }

        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        private void InnerInit()
        {
            List<Assembly> ass = new List<Assembly>();
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
                MetadataReference.CreateFromFile(typeof(CustomImp).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Xml.Linq.XElement).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.MemoryExtensions).Assembly.Location)
                //MetadataReference.CreateFromFile(typeof(Tag).Assembly.Location)
            }), new string[] { "Cdy.Spider" }, ass.Select(e => e.Location).ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitEditor()
        {

            var colors = new ClassificationHighlightColors();
            colors.DefaultBrush.Foreground = new ICSharpCode.AvalonEdit.Highlighting.SimpleHighlightingBrush(Colors.White);
            colors.KeywordBrush.Foreground = new ICSharpCode.AvalonEdit.Highlighting.SimpleHighlightingBrush(Colors.LightBlue);
            colors.StringBrush.Foreground = new ICSharpCode.AvalonEdit.Highlighting.SimpleHighlightingBrush(Colors.OrangeRed);

            InitExpressEditor.Initialize(mHost, colors, AppDomain.CurrentDomain.BaseDirectory, mOnVariableExpresse);
            TimerProcessExpressEditor.Initialize(mHost, colors, AppDomain.CurrentDomain.BaseDirectory, mOnTimerProcessExpress);
            OnReceiveDataExpressEditor.Initialize(mHost, colors, AppDomain.CurrentDomain.BaseDirectory, mOnReceiveDataExpress);
            OnSetValueToDeviceExpressEditor.Initialize(mHost, colors, AppDomain.CurrentDomain.BaseDirectory, mOnSetValueToDeviceExpress);

           

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="org"></param>
        /// <param name="key"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        private string SubString(string org,string key,string template)
        {
            int sindex = template.IndexOf(key);
            int eindex = template.Length-sindex - key.Length;
            string re = org;
            re = re.Substring(sindex);
            re = re.Substring(0, re.Length - eindex);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void UpdateInitExpress()
        {
            Model.VariableExpress= SubString(InitExpressEditor.Text, "$VariableBody$", mOnVariableDefineTemplate);

            mOnReceiveDataExpress = mOnReceiveDataTemplate.Replace("$OnReceiveDataBody$", mModel.OnReceiveDataFunExpress).Replace("$VariableBody$", mModel.VariableExpress.Replace("\r\n", ""));
            mOnSetValueToDeviceExpress = mOnSetValueToDeviceTemplate.Replace("$WriteValue$", mModel.OnSetTagValueToDeviceFunExpress).Replace("$VariableBody$", mModel.VariableExpress.Replace("\r\n", ""));
            mOnTimerProcessExpress = mOnProcessTimerElapsedTemplate.Replace("$ProcessTimerElapsed$", mModel.OnTimerFunExpress).Replace("$VariableBody$", mModel.VariableExpress.Replace("\r\n", ""));

            TimerProcessExpressEditor.Text = mOnTimerProcessExpress;
            OnReceiveDataExpressEditor.Text = mOnReceiveDataExpress;
            OnSetValueToDeviceExpressEditor.Text = mOnSetValueToDeviceExpress;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void UpdateOnReceiveExpressExpress()
        {
            Model.OnReceiveDataFunExpress = SubString(OnReceiveDataExpressEditor.Text, "$OnReceiveDataBody$", mOnReceiveDataTemplate.Replace("$VariableBody$", mModel.VariableExpress.Replace("\r\n", "")));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void UpdateOnSetValueToDeviceExpress()
        {
            Model.OnSetTagValueToDeviceFunExpress = SubString(OnSetValueToDeviceExpressEditor.Text, "$WriteValue$", mOnSetValueToDeviceTemplate.Replace("$VariableBody$", mModel.VariableExpress.Replace("\r\n", "")));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void UpdateOnTimeProcessExpress()
        {
            Model.OnTimerFunExpress = SubString(TimerProcessExpressEditor.Text, "$ProcessTimerElapsed$", mOnProcessTimerElapsedTemplate.Replace("$VariableBody$", mModel.VariableExpress.Replace("\r\n", "")));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sval"></param>
        private void Compile(string sval)
        {
            // 元数据引用
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(System.Object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(DriverRunnerBase).Assembly.Location),
                MetadataReference.CreateFromFile(this.GetType().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(CustomImp).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Xml.Linq.XElement).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.MemoryExtensions).Assembly.Location),
            };

            Assembly.GetEntryAssembly().GetReferencedAssemblies().ToList().ForEach(e => references.Add(MetadataReference.CreateFromFile(Assembly.Load(e).Location)));

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sval);

            var compileOption = new CSharpCompilationOptions(Microsoft.CodeAnalysis.OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true);
            var cc = CSharpCompilation.Create("test", new SyntaxTree[] { syntaxTree }, references, compileOption);

            StringBuilder sb = new StringBuilder();
            bool iserro = false;
            using (var ms = new System.IO.MemoryStream())
            {
                var result = cc.Emit(ms);
                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        sb.AppendLine(string.Format("{0}: {1}", diagnostic.Location.GetLineSpan().StartLinePosition, diagnostic.GetMessage()));
                    }
                    iserro = true;
                }
                else
                {
                    sb.AppendLine("Compile sucessful!");
                }

                CompileMessageEvent?.Invoke(this, sb.ToString(),iserro?2:0);
            }

        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
