using Cdy.Spider.DevelopCommon;
using Cdy.Spider.MelsecDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.Melsec.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class MelsecA3DriverDevelopViewModel : ViewModelBase
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
        public MelsecA3DriverData Model { get; set; }

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
        public byte Formate
        {
            get
            {
                return (byte)(Model.Formate-1);
            }
            set
            {
                Model.Formate = (byte)(value + 1);
                OnPropertyChanged("Formate");
            }
        }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
