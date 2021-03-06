﻿//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/9/17 16:40:44.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Text;

namespace Cdy.Spider.ModbusDriver.Develop
{
    public class ModbusRegistorConfigModel : ViewModelBase, IRegistorConfigModel
    {

        #region ... Variables  ...

        private int mRegistorType = 0;

        private int mStartAddress = 0;

        private int mDataLen = 0;

        private string mRegistor = string.Empty;

        private static string[] mRegistorTypes = new string[] { "Coil statue","Input statue","Input registor","hold registor"};

        private static List<string> mInnerRegistorTypes = new List<string> { "cs", "is", "ir", "hr" };

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public TagType TagType
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        public string[] RegistorTypes
        {
            get
            {
                return mRegistorTypes;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int RegistorType
        {
            get
            {
                return mRegistorType;
            }
            set
            {
                if (mRegistorType != value)
                {
                    mRegistorType = value;
                    UpdateRegistor();
                    OnPropertyChanged("RegistorType");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public int StartAddress
        {
            get
            {
                return mStartAddress;
            }
            set
            {
                if (mStartAddress != value)
                {
                    mStartAddress = value;
                    UpdateRegistor();
                    OnPropertyChanged("StartAddress");
                }
            }
        }



        /// <summary>
            /// 
            /// </summary>
        public int DataLen
        {
            get
            {
                return mDataLen;
            }
            set
            {
                if (mDataLen != value)
                {
                    mDataLen = value;
                    UpdateRegistor();
                    OnPropertyChanged("DataLen");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public Action<string> UpdateRegistorCallBack { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDeviceDevelopService Service { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void UpdateRegistor()
        {
            string stmp = mInnerRegistorTypes[RegistorType] + ":" + StartAddress + ":" + DataLen;
            UpdateRegistorCallBack?.Invoke(stmp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        public void ParseRegistorInfo(string info)
        {
            string[] ss = info.Split(new char[] { ':' });
            if (ss.Length == 3)
            {
                RegistorType = mInnerRegistorTypes.IndexOf(ss[0]);
                StartAddress = int.Parse(ss[1]);
                DataLen = int.Parse(ss[2]);
            }
            else
            {
                RegistorType = 0;
                DataLen = GetDataSize();    
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int GetDataSize()
        {
            switch (TagType)
            {
                case TagType.Bool:
                case TagType.Byte:
                case TagType.Short:
                case TagType.UShort:
                    return 1;
                case TagType.Int:
                case TagType.UInt:
                case TagType.Float:
                    return 2;
                case TagType.Long:
                case TagType.ULong:
                case TagType.Double:
                case TagType.DateTime:
                    return 4;
            }
            return 1;
        }

        public void OnDisActived()
        {
            UpdateRegistor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registor"></param>
        public void FreshRegistor(string registor)
        {
            ParseRegistorInfo(registor);
            OnPropertyChanged("Registor");
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            UpdateRegistorCallBack = null;
            base.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Config()
        {
            ModbusMutiConfigViewModel mm = new ModbusMutiConfigViewModel();
            if(mm.ShowDialog().Value)
            {
                return mm.GetConfigRegistor();
            }
            return null;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...



        

        
    }
}
