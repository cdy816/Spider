using Cdy.Spider.CalculateExpressEditor;
using RoslynPad.Roslyn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.CustomDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomDriverDevelop : DriverDevelop
    {

        #region ... Variables  ...
        private CustomDriverData mData;
      

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>

        public override DriverData Data 
        {
            get => mData;
            set {
                mData = value as CustomDriverData;
              
            } }
        
        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "CustomDriver";

        /// <summary>
        /// 
        /// </summary>
        public RoslynCodeEditor InitExpressEditor
        {
            get;
            set;
        }


        public RoslynCodeEditor TimerProcessExpressEditor
        {
            get;
            set;
        }


        public RoslynCodeEditor OnReceiveDataExpressEditor
        {
            get;
            set;
        }

        public RoslynCodeEditor OnSetValueToDeviceExpressEditor
        {
            get;
            set;
        }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override List<string> ListSupportChannels()
        {
            return new List<string>() { "MQTTClient", "MQTTServer", "CoapClient", "CoapServer", "WebAPIClient", "WebAPIServer" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverDevelop NewDriver()
        {
            return new CustomDriverDevelop() { Data = new CustomDriverData() };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override DriverData CreatNewData()
        {
            return new CustomDriverData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Config()
        {
            return new CustomDriverDevelopViewModel() { Model = mData };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IRegistorConfigModel RegistorConfig()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public override void CheckTagDeviceInfo(Tagbase tag)
        {
            tag.DeviceInfo = tag.Name;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...







    }
}
