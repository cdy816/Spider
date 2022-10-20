using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.BeckhoffDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class BeckhoffDriverDevelopViewModel : ViewModelBase
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public BeckhoffDriver.AdsData Model { get; set; }

        /// <summary>
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
                    OnPropertyChanged("PublishVisibility");
                    OnPropertyChanged("ScanText");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ScanText
        {
            get
            {
                return Model.Model == WorkMode.Active ? Res.Get("ScanCircle") + ":" : Res.Get("PublishCircle") + ":";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Visibility PublishVisibility
        {
            get
            {
                return Model.Model == WorkMode.Passivity ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
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
        public int AmsPort
        {
            get
            {
                return Model.AmsPort;
            }
            set
            {
                if (Model.AmsPort != value)
                {
                    Model.AmsPort = value;
                    OnPropertyChanged("AmsPort");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public string TargetAmsNetID
        {
            get
            {
                return Model.TargetAmsNetID;
            }
            set
            {
                if (Model.TargetAmsNetID != value)
                {
                    Model.TargetAmsNetID = value;
                    OnPropertyChanged("TargetAmsNetID");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public bool UseAutoAmsNetID
        {
            get
            {
                return Model.UseAutoAmsNetID;
            }
            set
            {
                if (Model.UseAutoAmsNetID != value)
                {
                    Model.UseAutoAmsNetID = value;
                    OnPropertyChanged("UseAutoAmsNetID");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public string SenderAMSNetId
        {
            get
            {
                return Model.SenderAMSNetId;
            }
            set
            {
                if (Model.SenderAMSNetId != value)
                {
                    Model.SenderAMSNetId =  value;
                    OnPropertyChanged("SenderAMSNetId");
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
