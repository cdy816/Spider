//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/9/1 16:29:31.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider.ModbusDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class ModbusSeriseDriverDevelopViewModel : ViewModelBase
    {

        #region ... Variables  ...
        static string[] mEightFormates;
        static string[] mFourFormates;
        static string[] mStringEncodings;
        static string[] mModbusTypes;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        static ModbusSeriseDriverDevelopViewModel()
        {
            mEightFormates = Enum.GetNames( typeof(EightValueFormate));
            mFourFormates = Enum.GetNames(typeof(FourValueFormate));
            mStringEncodings = Enum.GetNames(typeof(StringEncoding));

            mModbusTypes = Enum.GetNames(typeof(ModbusSeriseType));

        }

        #endregion ...Constructor...

        #region ... Properties ...
        
        /// <summary>
        /// 
        /// </summary>
        public ModbusSeriseDriverData Model { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string[] EightFormates
        {
            get
            {
                return mEightFormates;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] FourFormates
        {
            get
            {
                return mFourFormates;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] StringEncodings
        {
            get
            {
                return mStringEncodings;
            }
        }

        public string[] ModbusTypes
        {
            get
            {
                return mModbusTypes;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get
            {
                return Model.Id;
            }
            set
            {
                Model.Id = value;
                OnPropertyChanged("Id");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ModbusType
        {
            get
            {
                return (int)Model.Type;
            }
            set
            {
                if ((int)Model.Type != value)
                {
                    Model.Type = (ModbusSeriseType) value;
                    OnPropertyChanged("ModbusType");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int IntFormate
        {
            get
            {
                return (int)Model.IntFormate;
            }
            set
            {
                Model.IntFormate = (FourValueFormate)value;
                OnPropertyChanged("IntFormate");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int FloatFormate
        {
            get
            {
                return (int)Model.FloatFormate;
            }
            set
            {
                Model.FloatFormate = (FourValueFormate)value;
                OnPropertyChanged("FloatFormate");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int LongFormate
        {
            get
            {
                return (int)Model.LongFormate;
            }
            set
            {
                Model.LongFormate = (EightValueFormate)value;
                OnPropertyChanged("LongFormate");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int DoubleFormate
        {
            get
            {
                return (int)Model.DoubleFormate;
            }
            set
            {
                Model.DoubleFormate = (EightValueFormate)value;
                OnPropertyChanged("DoubleFormate");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int StringEncoding
        {
            get
            {
                return (int)Model.StringEncoding;
            }
            set
            {
                Model.StringEncoding = (Spider.StringEncoding)value;
                OnPropertyChanged("StringEncoding");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ushort PackageLen
        {
            get
            {
                return Model.PackageLen;
            }
            set
            {
                if (Model.PackageLen != value)
                {
                    Model.PackageLen = value;
                    OnPropertyChanged("PackageLen");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Visibility ScanCircleVisibility
        {
            get
            {
                return Model.Model == WorkMode.Active ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
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

        #endregion ...Properties...

        #region ... Methods    ...





        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
