using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cdy.Spider.WebApiServer
{
    public class ServiceMapManager
    {

        #region ... Variables  ...
        
        private Dictionary<string, Func<string, string>> mServerMaps = new Dictionary<string, Func<string, string>>();
        public static ServiceMapManager Manager = new ServiceMapManager();
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
        /// <param name="deviceName"></param>
        /// <param name="callback"></param>
        public void RegistorServerMap(string deviceName,Func<string,string> callback)
        {
            if(!mServerMaps.ContainsKey(deviceName))
            {
                mServerMaps.Add(deviceName, callback);
            }
            else
            {
                mServerMaps[deviceName] = callback;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        public Func<string,string> GetService(string deviceName)
        {
            if(mServerMaps.ContainsKey(deviceName))
            {
                return mServerMaps[deviceName];
            }
            return null;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...


    }
}
