using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReadWriteFxLinks
    {
        /// <summary>
        /// PLC的当前的站号，需要根据实际的值来设定，默认是0<br />
        /// The current station number of the PLC needs to be set according to the actual value. The default is 0.
        /// </summary>
        byte Station { get; set; }

        /// <summary>
        /// 报文等待时间，单位10ms，设置范围为0-15<br />
        /// Message waiting time, unit is 10ms, setting range is 0-15
        /// </summary>
        byte WaittingTime { get; set; }

        /// <summary>
        /// 是否启动和校验<br />
        /// Whether to start and sum verify
        /// </summary>
        bool SumCheck { get; set; }

        /// <summary>
        /// 当前的PLC的Fxlinks协议格式，通常是格式1，或是格式4，所以此处可以设置1，或者是4<br />
        /// The current PLC Fxlinks protocol format is usually format 1 or format 4, so it can be set to 1 or 4 here
        /// </summary>
        int Format { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="send"></param>
        /// <returns></returns>
        byte[] ReadFromCoreServer(byte[] send);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        byte[] Read(string address, ushort length, out bool res);
    }
}
