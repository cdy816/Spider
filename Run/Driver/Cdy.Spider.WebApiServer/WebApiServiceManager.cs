using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cdy.Spider.WebApiServer
{
    public class WebApiServiceManager
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, WebApiServer> mServers = new Dictionary<int, WebApiServer>();

        public static WebApiServiceManager Manager = new WebApiServiceManager();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="enableHttps"></param>
        public WebApiServer AddService(int port,bool enableHttps=true)
        {
            if(!mServers.ContainsKey(port))
            {
                WebApiServer ws = new WebApiServer() { Port = port, UseHttps = enableHttps };
            
                mServers.Add(port, ws);
                return ws;
            }
            else
            {
                return mServers[port];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            foreach(var vv in mServers)
            {
                vv.Value.Start();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            foreach(var vv in mServers)
            {
                vv.Value.Stop();
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
