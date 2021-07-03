using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MQTTServer
{
    /// <summary>
    /// 
    /// </summary>
    public class MQTTServerManager
    {

        #region ... Variables  ...

        private Dictionary<int, MQTTServer> mCachedServer = new Dictionary<int, MQTTServer>();

        /// <summary>
        /// 
        /// </summary>
        public static MQTTServerManager Manager = new MQTTServerManager();
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
        /// <returns></returns>
        public MQTTServer GetServer(int port)
        {
            lock (mCachedServer)
            {
                if (mCachedServer.ContainsKey(port))
                {
                    return mCachedServer[port];
                }
                else
                {
                    MQTTServer ss = new MQTTServer() { Port = port };
                    ss.Init();
                    mCachedServer.Add(port, ss);
                    return ss;
                }
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
