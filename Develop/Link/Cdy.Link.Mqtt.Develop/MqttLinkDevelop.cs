using Cdy.Spider;
using System;

namespace Cdy.Link.Mqtt.Develop
{
    public class MqttLinkDevelop : Cdy.Spider.LinkDevelopBase
    {

        #region ... Variables  ...
        private Cdy.Link.Mqtt.MqttLinkData mData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        public MqttLinkDevelop()
        {
            mData = new MqttLinkData();
        }
        #endregion ...Constructor...

        #region ... Properties ...
        public override LinkData Data { get => mData; set => mData = value as MqttLinkData; }

        public override string TypeName => "MqttLink";

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Config()
        {
            if (mData == null) mData = new MqttLinkData() { ServerPort = 1803 };
            return new ApiConfigViewModel() { Model = mData };
        }

        protected override LinkData CreatNewData()
        {
            return new MqttLinkData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ILinkDevelop NewApi()
        {
            return new MqttLinkDevelop() { Data = new MqttLinkData() };
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...





    }
}
