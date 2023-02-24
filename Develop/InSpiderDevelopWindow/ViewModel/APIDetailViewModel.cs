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
    public class APIDetailViewModel:ViewModelBase, IModeSwitch
    {

        #region ... Variables  ...
        
        private IApiDevelop mModel;

        private List<string> mApis;

        private string mSelectApiType;

        private bool mIsChanged = false;

        private MachineDocument mMachineModel;

        private string mOldDataString;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        public APIDetailViewModel()
        {
            mApis = ServiceLocator.Locator.Resolve<IApiFactory>()?.ListDevelopApis();
        }



        #endregion ...Constructor...

        #region ... Properties ...


        /// <summary>
        /// 
        /// </summary>
        public bool IsChanged
        {
            get
            {
                return false;
            }
            set
            {
                if (value)
                {
                    Parent.MachineModel.IsDirty = true;
                    OnPropertyChanged("IsChanged");
                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public APITreeViewModel Parent
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        public IApiDevelop Model
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
                    this.mModel = (ServiceLocator.Locator.Resolve<IApiFactory>().GetDevelopInstance(value) as IApiDevelopForFactory).NewApi();
                    Parent.UpdateDataModel(this.mModel);
                    OnPropertyChanged("Model");
                    OnPropertyChanged("ConfigModel");
                    IsChanged= true;
                }
                OnPropertyChanged("SelectApiType");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Active()
        {
            if(this.Model!=null)
            mOldDataString=this.Model.Save().ToString();
            else
            {
                mOldDataString = "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeActive()
        {
            string nstr = this.Model != null ? this.Model.Save().ToString() : "";
            if (nstr != mOldDataString)
            {
                IsChanged = true;
            }
        }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
