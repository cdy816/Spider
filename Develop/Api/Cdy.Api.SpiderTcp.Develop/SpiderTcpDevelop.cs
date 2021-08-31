using Cdy.Spider;
using System;

namespace Cdy.Api.SpiderTcp.Develop
{
    public class SpiderTcpDevelop : Cdy.Spider.ApiDevelopBase
    {

        #region ... Variables  ...
        private TcpApiData mData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public override ApiData Data { get => mData; set { mData = value as TcpApiData; } }

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "SpiderTcpApi";

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Config()
        {
            if(mData==null) mData = new TcpApiData() { Port = 10000, UserName = "Admin", Password = "Admin", ServerIp = "127.0.0.1", Type = ApiData.TransType.Timer, Circle = 2000 };
            return new ApiConfigViewModel() { Model = mData };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ApiData CreatNewData()
        {
            return new TcpApiData() { Port = 10000,UserName= "Admin", Password="Admin",ServerIp="127.0.0.1",Type = ApiData.TransType.Timer,Circle=2000 };
        }

        public override IApiDevelop NewApi()
        {
            return new SpiderTcpDevelop();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...





    }
}
