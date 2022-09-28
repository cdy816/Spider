using Cdy.Spider;
using System;

namespace Cdy.Api.OpcUAServer.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class OpcUAServerDevelop : Cdy.Spider.ApiDevelopBase
    {

        #region ... Variables  ...
        private OPCUAApiData mData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override ApiData Data { get => mData; set => mData = value as OPCUAApiData; }

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "OpcUAServer";
        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Config()
        {
            if (mData == null) mData = new OPCUAApiData() { Port = 24440 };
            return new ApiConfigViewModel() { Model = mData };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ApiData CreatNewData()
        {
            return new OPCUAApiData() { Port = 24440 };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IApiDevelop NewApi()
        {
            return new OpcUAServerDevelop() ;
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...





    }
}
