using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDeviceDevelopService
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 获取通道类型
        /// </summary>
        /// <returns></returns>
        string GetChannelType();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetChannelParameter();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetConfigServerUrl();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetConfigUserName();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetConfigPassword();

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
