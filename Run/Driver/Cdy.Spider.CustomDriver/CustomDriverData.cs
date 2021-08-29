using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        private string mVariableExpress = "";

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 变量定义
        /// </summary>
        public string VariableExpress
        {
            get
            {
                return mVariableExpress;
            }
            set
            {
                if (mVariableExpress != value)
                {
                    mVariableExpress = value;
                }
            }
        }


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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
            var re = base.SaveToXML();
            XElement xx = new XElement("VariableExpress");
            xx.SetValue(VariableExpress);
            re.Add(xx);

            xx = new XElement("OnReceiveDataFunExpress");
            xx.SetValue(OnReceiveDataFunExpress);
            re.Add(xx);

            xx = new XElement("OnSetTagValueToDeviceFunExpress");
            xx.SetValue(OnSetTagValueToDeviceFunExpress);
            re.Add(xx);

            xx = new XElement("OnTimerFunExpress");
            xx.SetValue(OnTimerFunExpress);
            re.Add(xx);

            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);
            if(xe.Element("VariableExpress") !=null)
            {
                this.VariableExpress = xe.Element("VariableExpress").Value;
            }
            if (xe.Element("OnReceiveDataFunExpress") != null)
            {
                this.OnReceiveDataFunExpress = xe.Element("OnReceiveDataFunExpress").Value;
            }
            if (xe.Element("OnSetTagValueToDeviceFunExpress") != null)
            {
                this.OnSetTagValueToDeviceFunExpress = xe.Element("OnSetTagValueToDeviceFunExpress").Value;
            }
            if (xe.Element("OnTimerFunExpress") != null)
            {
                this.OnTimerFunExpress = xe.Element("OnTimerFunExpress").Value;
            }
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
