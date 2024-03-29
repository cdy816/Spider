﻿using System;
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
            var vlink = ServiceLocator.Locator.Resolve<ILink>();
            if (vlink != null)
                vlink.RegistorValueUpdateCallBack(this.Device.Name, UpdateValues);
            else
            {
                LoggerService.Service.Warn(this.mData.Name,"ILink Service is null.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        private void UpdateValues(Dictionary<string,object> values)
        {
            if (values == null)
            {
                this.UpdateAllTagQualityToCommBad();
            }
            else
            {
                foreach (var vv in values)
                {
                    this.UpdateValue(vv.Key, vv.Value);
                }
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
