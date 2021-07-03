using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Cdy.Spider
{
    public class LinkDriver : DriverRunnerBase
    {

        #region ... Variables  ...
        private LinkDriverData mData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "LinkDriver";

        public override DriverData Data => mData;

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new LinkDriverData();
            mData.LoadFromXML(xe);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Prepare()
        {
            ServiceLocator.Locator.Resolve<ILink>().RegistorValueUpdateCallBack(this.Device.Name, UpdateValues);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        private void UpdateValues(Dictionary<string,object> values)
        {
            foreach(var vv in values)
            {
                this.UpdateValue(vv.Key, vv.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        public override void WriteValue(string deviceInfo, object value, byte valueType)
        {
            var dd = ServiceLocator.Locator.Resolve<ILink>();
            if (dd != null)
            {
                dd.WriteValue(this.Device.Name, deviceInfo, value, valueType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverRuntime NewApi()
        {
            return new LinkDriver();
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...



    }
}
