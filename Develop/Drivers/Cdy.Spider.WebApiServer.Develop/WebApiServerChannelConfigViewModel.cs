using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.WebApiServer.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiServerChannelConfigViewModel : ViewModelBase
    {

        #region ... Variables  ...

        /// <summary>
        /// 
        /// </summary>
        private WebApiServerChannelData mModel;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public WebApiServerChannelData Model
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
        public bool UseHttps
        {
            get
            {
                return mModel.UseHttps;
            }
            set
            {
                if (mModel.UseHttps != value)
                {
                    mModel.UseHttps = value;
                    OnPropertyChanged("UseHttps");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int Port
        {
            get
            {
                return mModel.Port;
            }
            set
            {
                if (mModel.Port != value)
                {
                    mModel.Port = value;
                    OnPropertyChanged("Port");
                }
            }
        }


        #endregion ...Properties...

        #region ... Methods    ...



        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
