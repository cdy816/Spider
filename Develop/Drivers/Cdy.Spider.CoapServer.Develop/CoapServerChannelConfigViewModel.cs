using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.CoapServer.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class CoapServerChannelConfigViewModel : ViewModelBase
    {

        #region ... Variables  ...

        /// <summary>
        /// 
        /// </summary>
        private CoapServer.CoapServerChannelData mModel;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public CoapServer.CoapServerChannelData Model
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

        #endregion ...Properties...

        #region ... Methods    ...

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
                    OnPropertyChanged("MessageInfo");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string MessageInfo
        {
            get
            {
                return "Url: coap://127.0.0.1:" + Port;
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
