using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.FujiDriver
{
    public interface IReadWriteDevice
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="send"></param>
        /// <returns></returns>
        byte[] ReadFromCoreServer(byte[] send);
    }
}
