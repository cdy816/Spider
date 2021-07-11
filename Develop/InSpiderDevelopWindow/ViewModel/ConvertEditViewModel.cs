
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
        public ConvertEditViewModel(Tagbase tag)
        {
            Tag = tag;
            Init();
            DefaultWidth = 400;
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
}
