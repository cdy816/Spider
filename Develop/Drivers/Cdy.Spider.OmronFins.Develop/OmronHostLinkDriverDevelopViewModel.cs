using Cdy.Spider.DevelopCommon;
using Cdy.Spider.OmronDriver.HostLink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.OmronFins.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class OmronHostLinkDriverDevelopViewModel : ViewModelBase
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
        public OmronHostLinkDriverData Model { get; set; }

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
                return Model.Model == WorkMode.Active ? Res.Get("ScanCircle")+":" : Res.Get("PublishCircle")+":";
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
        public int SA2
        {
            get
            {
                return Model.SA2;
            }
            set
            {
                if (Model.SA2 != value)
                {
                    Model.SA2 = value;
                    OnPropertyChanged("SA2");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int DA2
        {
            get
            {
                return Model.DA2;
            }
            set
            {
                if (Model.DA2 != value)
                {
                    Model.DA2 = value;
                    OnPropertyChanged("DA2");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int SID
        {
            get
            {
                return Model.SID;
            }
            set
            {
                if (Model.SID != value)
                {
                    Model.SID = value;
                    OnPropertyChanged("SID");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public int UnitNumber
        {
            get
            {
                return Model.UnitNumber;
            }
            set
            {
                if (Model.UnitNumber != value)
                {
                    Model.UnitNumber = value;
                    OnPropertyChanged("UnitNumber");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int DataFormate
        {
            get
            {
                return (int)Model.DataFormate;
            }
            set
            {
                if ((int)Model.DataFormate != value)
                {
                    Model.DataFormate = (Common.DataFormat)value;
                    OnPropertyChanged("DataFormate");
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
