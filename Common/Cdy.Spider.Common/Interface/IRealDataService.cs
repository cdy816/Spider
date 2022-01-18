using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRealDataService
    {

        /// <summary>
        /// 枚举变量
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        IEnumerable<string> ListTags(string device);

        /// <summary>
        /// 获取变量的全部值
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        Dictionary<string, Tuple<object, byte>> GetDeviceALLTagValues(string device);

        /// <summary>
        /// 获取变量的值
        /// </summary>
        /// <param name="device"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        Dictionary<string, Tuple<object, byte>> GetTagValues(string device, IEnumerable<string> tags);
    }
}
