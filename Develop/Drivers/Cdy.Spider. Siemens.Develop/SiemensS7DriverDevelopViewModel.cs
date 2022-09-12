using Cdy.Spider.DevelopCommon;
using Cdy.Spider.SiemensDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.Siemens.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class SiemensS7DriverDevelopViewModel : ViewModelBase
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
        public SiemensS7DriverData Model { get; set; }

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
        public int Slot
        {
            get
            {
                return Model.Slot;
            }
            set
            {
                if (Model.Slot != value)
                {
                    Model.Slot = value;
                    OnPropertyChanged("Slot");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int Rack
        {
            get
            {
                return Model.Rack;
            }
            set
            {
                if (Model.Rack != value)
                {
                    Model.Rack = value;
                    OnPropertyChanged("Rack");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int ConnectionType
        {
            get
            {
                return Model.ConnectionType;
            }
            set
            {
                if (Model.ConnectionType != value)
                {
                    Model.ConnectionType = value;
                    OnPropertyChanged("ConnectionType");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int LocalTSAP
        {
            get
            {
                return Model.LocalTSAP;
            }
            set
            {
                if (Model.LocalTSAP != value)
                {
                    Model.LocalTSAP = value;
                    OnPropertyChanged("LocalTSAP");
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

        /// <summary>
            /// 
            /// </summary>
        public int PlcType
        {
            get
            {
                return Model.PlcType-1;
            }
            set
            {
                if (Model.PlcType != value+1)
                {
                    Model.PlcType = value+1;
                    OnPropertyChanged("PlcType");
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
