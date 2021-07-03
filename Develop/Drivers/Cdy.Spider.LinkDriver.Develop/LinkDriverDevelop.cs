using System;
using System.Collections.Generic;

namespace Cdy.Spider.LinkDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class LinkDriverDevelop : DriverDevelop
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
        public override DriverData Data { get { return mData; } set { mData = value as LinkDriverData; } }

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "LinkDriver";
        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Config()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override List<string> ListSupportChannels()
        {
            return new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverDevelop NewDriver()
        {
            return new LinkDriverDevelop() { Data = new LinkDriverData() };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public override void CheckTagDeviceInfo(Tagbase tag)
        {
            tag.DeviceInfo = tag.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IRegistorConfigModel RegistorConfig()
        {
            return null;
        }

        protected override DriverData CreatNewData()
        {
            return new LinkDriverData();
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...



    }
}
