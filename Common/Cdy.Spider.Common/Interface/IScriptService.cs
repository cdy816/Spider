using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public interface IScriptService
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
        /// 编译
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        string Compile(string exp,object context);

        /// <summary>
        /// 运行脚本
        /// </summary>
        /// <param name="id">编译返回的Id</param>
        /// <returns></returns>
        object RunById(string id, object context);


        /// <summary>
        /// 运行脚本内容
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        object Run(string exp, object context);


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
