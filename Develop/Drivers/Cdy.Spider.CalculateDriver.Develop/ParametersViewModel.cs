using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cdy.Spider.CalculateDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class ParametersViewModel : WindowViewModelBase
    {

        #region ... Variables  ...
        
        /// <summary>
        /// 
        /// </summary>
        private List<ParameterDesignViewModelBase> mParameters = new List<ParameterDesignViewModelBase>();

        private Dictionary<string, ParameterDesignViewModelBase> mParameterDesinger = new Dictionary<string, ParameterDesignViewModelBase>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public ParametersViewModel()
        {
            InnerInit();
            DefaultWidth = 600;
            DefaultHeight = 400;
            Title = Res.Get("Parameter");
        }
        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public List<ParameterDesignViewModelBase> Parameters
        {
            get
            {
                return mParameters;
            }
            set
            {
                if (mParameters != value)
                {
                    mParameters = value;
                    OnPropertyChanged("Parameters");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paras"></param>
        public void Init(List<Parameter> paras)
        {
            foreach(var vv in paras)
            {
                if(mParameterDesinger.ContainsKey(vv.DesignType))
                {
                    mParameters.Add(mParameterDesinger[vv.DesignType].New(vv));
                }
                else
                {
                    mParameters.Add(new ParameterDesignViewModelBase(vv));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InnerInit()
        {
            mParameterDesinger.Add("tag", new TagParameterDesignViewModel());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Parameter> GetResult()
        {
            return mParameters.Select(e => e.Model).ToList();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 
    /// </summary>
    public class ParameterDesignViewModelBase : ViewModelBase
    {
        private Parameter mModel;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public ParameterDesignViewModelBase(Parameter model)
        {
            mModel = model;
        }
        /// <summary>
        /// 
        /// </summary>
        public ParameterDesignViewModelBase()
        {

        }

        /// <summary>
            /// 
            /// </summary>
        public Parameter Model
        {
            get
            {
                return mModel;
            }
            set
            {
                if (mModel != value)
                {
                    mModel = value;
                    OnPropertyChanged("Model");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public string Name
        {
            get
            {
                return mModel.Name;
            }
            set
            {
                if (mModel.Name != value)
                {
                    mModel.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public string Desc
        {
            get
            {
                return mModel.Desc;
            }
            set
            {
                if (mModel.Desc != value)
                {
                    mModel.Desc = value;
                    OnPropertyChanged("Desc");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string Type
        {
            get
            {
                return mModel.Type;
            }
            set
            {
                if (mModel.Type != value)
                {
                    mModel.Type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Value
        {
            get
            {
                return mModel.Value;
            }
            set
            {
                if (mModel.Value != value)
                {
                    mModel.Value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual ParameterDesignViewModelBase New(Parameter model)
        {
            return new ParameterDesignViewModelBase(model);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public class TagParameterDesignViewModel : ParameterDesignViewModelBase
    {

        #region ... Variables  ...
        
        private ICommand mTagBrowseCommand;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public TagParameterDesignViewModel()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public TagParameterDesignViewModel(Parameter model):base(model)
        {

        }
        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public ICommand TagBrowseCommand
        {
            get
            {
                if(mTagBrowseCommand==null)
                {
                    mTagBrowseCommand = new RelayCommand(() => { 
                        
                    });
                }
                return mTagBrowseCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Value { get => base.Value; set {
                
                if(string.IsNullOrEmpty(value) || value.StartsWith("Tag."))
                base.Value = value;
                else
                {
                    OnPropertyChanged("Value");
                }
            } }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override ParameterDesignViewModelBase New(Parameter model)
        {
            return new TagParameterDesignViewModel(model);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

}
