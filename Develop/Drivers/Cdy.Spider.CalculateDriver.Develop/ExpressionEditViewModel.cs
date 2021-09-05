using Cdy.Spider.CalculateExpressEditor;
using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace Cdy.Spider.CalculateDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class ExpressionEditViewModel: WindowViewModelBase
    {

        #region ... Variables  ...
        
        private string mExpresse = "";

        private RoslynCodeEditor mExpressEditor;

        private System.Collections.ObjectModel.ObservableCollection<CommandItem> mCommands = new System.Collections.ObjectModel.ObservableCollection<CommandItem>();

        private System.Collections.ObjectModel.ObservableCollection<ScriptItem> mScripts = new System.Collections.ObjectModel.ObservableCollection<ScriptItem>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public ExpressionEditViewModel()
        {
            Title = Res.Get("Expresse");
            DefaultWidth = 800;
            DefaultHeight = 600;
            Init();
        }
        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public System.Collections.ObjectModel.ObservableCollection<CommandItem> Commands
        {
            get
            {
                return mCommands;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public System.Collections.ObjectModel.ObservableCollection<ScriptItem> Scripts
        {
            get
            {
                return mScripts;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public RoslynCodeEditor ExpressEditor
        {
            get
            {
                return mExpressEditor;
            }
            set
            {
                if (mExpressEditor != value)
                {
                    mExpressEditor = value;
                    //mExpressEditor.Text = mExpresse;
                    OnPropertyChanged("ExpressEditor");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string Expresse
        {
            get
            {
                return mExpresse;
            }
            set
            {
                if (mExpresse != value)
                {
                    mExpresse = value;
                    //if(mExpressEditor!=null)
                    //mExpressEditor.Text = value;
                    OnPropertyChanged("Expresse");
                }
            }
        }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        private void Init()
        {
            LoadFunction();
            LoadScript();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadFunction()
        {
            mCommands.Clear();
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Command.cfg");
            if(System.IO.File.Exists(sfile))
            {
                XElement xe = XElement.Load(sfile);
                foreach(var vv in xe.Elements())
                {
                    mCommands.Add(new CommandItem().LoadFromXML(vv));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadScript()
        {
            mScripts.Clear();
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Script.cfg");
            if (System.IO.File.Exists(sfile))
            {
                XElement xe = XElement.Load(sfile);
                foreach (var vv in xe.Elements())
                {
                    mScripts.Add(new ScriptItem().LoadFromXML(vv));
                }
               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetExpressResult()
        {
            return mExpressEditor.Text;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 脚本片段
    /// </summary>
    public class ScriptItem
    {
        private ICommand mInsertCommand;
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICommand InsertCommand
        {
            get
            {
                if(mInsertCommand==null)
                {
                    mInsertCommand = new RelayCommand(() => { 
                        
                    });
                }
                return mInsertCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public ScriptItem LoadFromXML(XElement xe)
        {
            return this;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class CommandItem
    {
        private ICommand mInsertCommand;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 函数定义
        /// </summary>
        public Function Function { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICommand InsertCommand
        {
            get
            {
                if (mInsertCommand == null)
                {
                    mInsertCommand = new RelayCommand(() => {

                    });
                }
                return mInsertCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public CommandItem LoadFromXML(XElement xe)
        {
            return this;
        }
    }

    /// <summary>
    /// 函数
    /// </summary>
    public class Function
    {
        /// <summary>
        /// 函数体
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public List<Parameter> Parameters { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public void LoadFromXML(XElement xe)
        {

        }

    }
    /// <summary>
    /// 参数
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 配置的值
        /// </summary>
        public string Value { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public void LoadFromXML(XElement xe)
        {

        }

    }


}
