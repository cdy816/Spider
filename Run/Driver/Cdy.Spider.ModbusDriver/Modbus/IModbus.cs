using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.ModbusDriver
{
    public interface IModbus
    {
        bool AddressStartWithZero { get; set; }

        byte Station { get; set; }

        DataFormat DataFormat { get; set; }

        bool IsStringReverse { get; set; }

        IByteTransform ByteTransform { get; set; }

        /// <summary>
        /// 将当前的地址信息转换成Modbus格式的地址，如果转换失败，返回失败的消息。默认不进行任何的转换。<br />
        /// Convert the current address information into a Modbus format address. If the conversion fails, a failure message will be returned. No conversion is performed by default.
        /// </summary>
        /// <param name="address">传入的地址</param>
        /// <param name="modbusCode">Modbus的功能码</param>
        /// <returns>转换之后Modbus的地址</returns>
        string TranslateToModbusAddress(string address, byte modbusCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="send"></param>
        /// <returns></returns>
        byte[] ReadFromCoreServer(byte[] send);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="send"></param>
        /// <returns></returns>
        byte[] ReadFromCoreServer(IEnumerable<byte[]> send);

        byte[] Read(string address, ushort length,out bool res);
    }
}
