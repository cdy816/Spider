using Cdy.Spider;
using System;

namespace Cdy.Api.SpiderMqtt.Develop
{
    public class SpiderMqttDevelop : Cdy.Spider.ApiDevelopBase
    {

        #region ... Variables  ...
        private Cdy.Api.SpiderMqtt.MqttApiData mData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override ApiData Data { get => mData; set => mData = value as Cdy.Api.SpiderMqtt.MqttApiData; }

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "SpiderMqttApi";
        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Config()
        {
            if (mData == null) mData = new MqttApiData() { Port = 1803 };
            return new ApiConfigViewModel() { Model = mData };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ApiData CreatNewData()
        {
            return new MqttApiData() { Port = 1803 };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IApiDevelop NewApi()
        {
            return new SpiderMqttDevelop() ;
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...





    }
}
