using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InSpiderDevelopServer
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMachineManager
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
        /// 启动
        /// </summary>
        /// <param name="name"></param>
        bool Start(string solution,string name);

        /// <summary>
        /// 停止运行
        /// </summary>
        /// <param name="name"></param>
        bool Stop(string solution, string name);

        /// <summary>
        /// 是否处于运行中
        /// </summary>
        /// <param name="name"></param>
        bool IsRunning(string solution, string name);

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
