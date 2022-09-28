using Cdy.Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Api.OpcUAServer
{
    /// <summary>
    /// 
    /// </summary>
    public class OPCUAApiRuntime : Cdy.Spider.ApiBase
    {

        #region ... Variables  ...
        private OPCUAApiData mData;
        private OPCServer mServer;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
        /// <summary>
        /// 
        /// </summary>
        public override ApiData Data => mData;

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "OpcUAServer";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new OPCUAApiData();
            mData.LoadFromXML(xe);
            base.Load(xe);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            base.Start();
            mServer = new OPCServer() { UserName = mData.UserName, Password = mData.Password, Port = mData.Port, NoneSecurityMode = mData.NoneSecurityMode };
            mServer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Stop()
        {
            base.Stop();
            mServer.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IApi NewApi()
        {
            return new OPCUAApiRuntime();
        }
    }
}
