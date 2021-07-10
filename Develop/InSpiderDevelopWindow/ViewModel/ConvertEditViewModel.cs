
using Cdy.Spider;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
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
        public ConvertEditViewModel()
        {
            Init();
            DefaultWidth = 400;
            DefaultHeight = 200;
        }

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
            mItems.Add(new LinearConvertViewModel() { Model = new LinerConvert() });
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



}
