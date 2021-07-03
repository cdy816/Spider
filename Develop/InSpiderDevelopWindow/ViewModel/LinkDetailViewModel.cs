//==============================================================
//  Copyright (C) 2020 Chongdaoyang Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/25 21:14:48 .
//  Version 1.0
//  CDYWORK
//==============================================================

using Cdy.Spider;
using InSpiderDevelop;
using System;
using System.Collections.Generic;
using System.Text;

namespace InSpiderDevelopWindow.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class LinkDetailViewModel : ViewModelBase
    {

        #region ... Variables  ...
        
        private ILinkDevelop mModel;

        private List<string> mApis;

        private string mSelectApiType;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        public LinkDetailViewModel()
        {
            mApis = ServiceLocator.Locator.Resolve<ILinkFactory>()?.ListDevelopLinks();
            mApis.Insert(0, "");
        }



        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public LinkTreeViewModel Parent
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        public ILinkDevelop Model
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
                    if (value != null)
                        this.mSelectApiType = Model.TypeName;
                    OnPropertyChanged("Model");
                    OnPropertyChanged("ConfigModel");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object ConfigModel
        {
            get
            {
                return mModel?.Config();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public List<string> Apis
        {
            get
            {
                return mApis;
            }
            internal set
            {
                mApis = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SelectApiType 
        {
            get { return mSelectApiType; } 
            set 
            {
                if(mSelectApiType!=value)
                {
                    mSelectApiType = value;
                    if (!string.IsNullOrEmpty(value))
                    {
                        this.mModel = (ServiceLocator.Locator.Resolve<ILinkFactory>().GetDevelopInstance(value) as ILinkDevelopForFactory).NewApi();
                        Parent.UpdateDataModel(this.mModel);
                    }
                    else
                    {
                        this.mModel = null;
                        Parent.UpdateDataModel(null);
                    }
                    OnPropertyChanged("Model");
                    OnPropertyChanged("ConfigModel");
                }
                OnPropertyChanged("SelectApiType");
            }
        }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
