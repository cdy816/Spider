using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.OmronDriver
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHostLink
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        byte ICF { get; set; }

        byte DA2 { get; set; }

        byte SA2 { get; set; }

        byte SID { get; set; }

        byte ResponseWaitTime { get; set; }

        byte UnitNumber { get; set; }

        int ReadSplits { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 将当前的数据报文发送到设备去，具体使用什么通信方式取决于设备信息，然后从设备接收数据回来，并返回给调用者。<br />
        /// Send the current data message to the device, the specific communication method used depends on the device information, and then receive the data back from the device and return it to the caller.
        /// </summary>
        /// <param name="send">发送的完整的报文信息</param>
        /// <returns>接收的完整的报文信息</returns>
        /// <remarks>
        /// 本方法用于实现本组件还未实现的一些报文功能，例如有些modbus服务器会有一些特殊的功能码支持，需要收发特殊的报文，详细请看示例
        /// </remarks>
        byte[] ReadFromCoreServer(byte[] send);


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
