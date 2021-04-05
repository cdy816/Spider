using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.ModbusDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class ModbusMutiConfigViewModel: WindowViewModelBase
    {

        #region ... Variables  ...
        private static string[] mRegistorTypes = new string[] { "Coil statue", "Input statue", "Input registor", "hold registor" };
        private static List<string> mInnerRegistorTypes = new List<string> { "cs", "is", "ir", "hr" };

        private static int mRegistorType;

        private int mStartAddress;

        private int mDataLen=1;

        private int mRepeat=1;

        private static int mStartAddressCach = 0;
        private static int mDataLenCach = 1;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public ModbusMutiConfigViewModel()
        {
            Title = Res.Get("ModbusMutiConfigTitle");
            DefaultHeight = 80;
            DefaultWidth = 780;
            mStartAddress = mStartAddressCach;
            mDataLen = mDataLenCach;
            
        }
        #endregion ...Constructor...

        #region ... Properties ...

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
                    if(mRegistorType == 0 || mRegistorType==1)
                    {
                        DataLen = 1;
                    }
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
                if (mDataLen != value && value>0)
                {
                    if (mRegistorType == 0 || mRegistorType == 1)
                    {
                        mDataLen = 1;
                    }
                    else
                    {
                        mDataLen = value;
                    }
                    OnPropertyChanged("DataLen");
                }
            }
        }


        /// <summary>
            /// 
            /// </summary>
        public int Repeat
        {
            get
            {
                return mRepeat;
            }
            set
            {
                if (mRepeat != value)
                {
                    mRepeat = value;
                    OnPropertyChanged("Repeat");
                }
            }
        }



        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetConfigRegistor()
        {
            List<string> ltmp = new List<string>();
            int addr = 0;
            for(int i=0;i<mRepeat;i++)
            {
                addr = mStartAddress + mDataLen * i;
                ltmp.Add(mInnerRegistorTypes[RegistorType] + ":" + addr + ":" + mDataLen);
            }
            mStartAddressCach = addr+mDataLen;
            mDataLenCach = mDataLen;
            return ltmp;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
