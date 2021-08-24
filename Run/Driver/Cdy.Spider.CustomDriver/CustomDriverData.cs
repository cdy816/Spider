using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomDriverData: DriverData
    {

        #region ... Variables  ...
        
        private string mOnReceiveDataFunExpress = "";
        private string mOnSetTagValueToDeviceFunExpress = "";
        private string mOnTimerFunExpress = "";

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 初始化
        /// </summary>
        public string OnInitFunExpress { get; set; }

        /// <summary>
        /// 被动接受数据表达式
        /// </summary>
        public string OnReceiveDataFunExpress
        {
            get
            {
                return mOnReceiveDataFunExpress;
            }
            set
            {
                if (mOnReceiveDataFunExpress != value)
                {
                    mOnReceiveDataFunExpress = value;
                }
            }
        }

        /// <summary>
        ///  响应数据库下发数据到设备
        /// </summary>
        public string OnSetTagValueToDeviceFunExpress
        {
            get
            {
                return mOnSetTagValueToDeviceFunExpress;
            }
            set
            {
                if (mOnSetTagValueToDeviceFunExpress != value)
                {
                    mOnSetTagValueToDeviceFunExpress = value;
                }
            }
        }

        /// <summary>
        /// 响应定时触发执行表达式
        /// </summary>
        public string OnTimerFunExpress
        {
            get
            {
                return mOnTimerFunExpress;
            }
            set
            {
                if (mOnTimerFunExpress != value)
                {
                    mOnTimerFunExpress = value;
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
