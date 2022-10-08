using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.DeltaDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class DeltaSerialDriverDevelopViewModel : ViewModelBase
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
        public DeltaDriverData Model { get; set; }

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
        public int Station
        {
            get
            {
                return Model.Station;
            }
            set
            {
                if (Model.Station != value)
                {
                    Model.Station = value;
                    OnPropertyChanged("Station");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public int InovanceSeries
        {
            get
            {
                return (int)Model.Serise;
            }
            set
            {
                if ((int)Model.Serise != value)
                {
                    Model.Serise = (Spider.DeltaDriver.DeltaSeries)  value;
                    OnPropertyChanged("InovanceSeries");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public bool AddressStartWithZero
        {
            get
            {
                return Model.AddressStartWithZero;
            }
            set
            {
                if (Model.AddressStartWithZero != value)
                {
                    Model.AddressStartWithZero = value;
                    OnPropertyChanged("AddressStartWithZero");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public bool IsStringReverse
        {
            get
            {
                return Model.IsStringReverse;
            }
            set
            {
                if (Model.IsStringReverse != value)
                {
                    Model.IsStringReverse = value;
                    OnPropertyChanged("IsStringReverse");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public int DataFormat
        {
            get
            {
                return (int)Model.DataFormate;
            }
            set
            {
                if ((int)Model.DataFormate != value)
                {
                    Model.DataFormate = (Common.DataFormat) value;
                    OnPropertyChanged("DataFormat");
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
