using Cdy.Spider.DevelopCommon;
using Cdy.Spider.FujiDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.Fuji.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class FujiSPHDriverDevelopViewModel : ViewModelBase
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
        public FujiSPHDriverData Model { get; set; }

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
        public int ConnectionID
        {
            get
            {
                return Model.ConnectionID;
            }
            set
            {
                if (Model.ConnectionID != value)
                {
                    Model.ConnectionID = value;
                    OnPropertyChanged("ConnectionID");
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
