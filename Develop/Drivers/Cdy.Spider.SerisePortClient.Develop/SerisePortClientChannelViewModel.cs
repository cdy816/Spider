using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.SerisePortClient.Develop
{
    public class SerisePortClientChannelViewModel: ViewModelBase
    {

        #region ... Variables  ...
        private SerisePortClientChannelData mModel;

        private int[] mDataSizes = new int[] { 5, 6, 7, 8 };

        public string[] mStopBitses;

        public string[] mPortCheckTypes;

        public string[] mPorts;

        public int[] mBaudRates;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public SerisePortClientChannelViewModel()
        {
            mStopBitses = typeof(Cdy.Spider.StopBits).GetEnumNames();
            mPortCheckTypes = typeof(Cdy.Spider.PortCheckType).GetEnumNames();
            mBaudRates = new int[] { 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200 };
            try
            {
                mPorts = System.IO.Ports.SerialPort.GetPortNames();
            }
            catch
            {

            }
            

        }
        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public int[] BaudRates
        {
            get
            {
                return mBaudRates;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string[] Ports {
            get
            {
                return mPorts;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string[] StopBitses
        {
            get
            {
                return mStopBitses;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] PortCheckTypes
        {
            get
            {
                return mPortCheckTypes;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string PortName
        {
            get
            {
                return mModel.PortName;
            }
            set
            {
                if (mModel.PortName != value)
                {
                    mModel.PortName = value;
                    OnPropertyChanged("PortName");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int BaudRate
        {
            get
            {
                return mModel.BandRate;
            }
            set
            {
                if (mModel.BandRate != value)
                {
                    mModel.BandRate = value;
                    OnPropertyChanged("BaudRate");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public int PortCheckType
        {
            get
            {
                return (int)mModel.Check;
            }
            set
            {
                if ((int)mModel.Check != value)
                {
                    mModel.Check = (Spider.PortCheckType)value;
                    OnPropertyChanged("PortCheckType");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int StopBits
        {
            get
            {
                return (int)mModel.StopBits;
            }
            set
            {
                if ((int)mModel.StopBits != value)
                {
                    mModel.StopBits = (Spider.StopBits) value;
                    OnPropertyChanged("StopBits");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int[] DataSizes
        {
            get
            {
                return mDataSizes;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int DataSize
        {
            get
            {
                return mModel.DataSize;
            }
            set
            {
                if (mModel.DataSize != value)
                {
                    mModel.DataSize = value;
                    OnPropertyChanged("DataSize");
                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public SerisePortClientChannelData Data
        {
            get
            {
                return mModel;
            }
            set
            {
                mModel = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ReTryCount
        {
            get
            {
                return mModel.ReTryCount;
            }
            set
            {
                if (mModel.ReTryCount != value)
                {
                    mModel.ReTryCount = value;
                    OnPropertyChanged("ReTryCount");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ReTryDuration
        {
            get
            {
                return mModel.ReTryDuration;
            }
            set
            {
                if (mModel.ReTryDuration != value)
                {
                    mModel.ReTryDuration = value;
                    OnPropertyChanged("ReTryDuration");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Timeout
        {
            get
            {
                return mModel.Timeout;
            }
            set
            {
                if (mModel.Timeout != value)
                {
                    mModel.Timeout = value;
                    OnPropertyChanged("Timeout");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int DataSendTimeout
        {
            get
            {
                return mModel.DataSendTimeout;
            }
            set
            {
                if (mModel.DataSendTimeout != value)
                {
                    mModel.DataSendTimeout = value;
                    OnPropertyChanged("DataSendTimeout");
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
