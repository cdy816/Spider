using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver
{
    /// <summary>
    /// 三菱PLC，二进制的辅助类对象
    /// </summary>
    public class McBinaryHelper
    {
        /// <summary>
        /// 将MC协议的核心报文打包成一个可以直接对PLC进行发送的原始报文
        /// </summary>
        /// <param name="mcCore">MC协议的核心报文</param>
        /// <param name="networkNumber">网络号</param>
        /// <param name="networkStationNumber">网络站号</param>
        /// <returns>原始报文信息</returns>
        public static byte[] PackMcCommand(byte[] mcCore, byte networkNumber = 0, byte networkStationNumber = 0)
        {
            byte[] array = new byte[11 + mcCore.Length];
            array[0] = 80;
            array[1] = 0;
            array[2] = networkNumber;
            array[3] = byte.MaxValue;
            array[4] = byte.MaxValue;
            array[5] = 3;
            array[6] = networkStationNumber;
            array[7] = (byte)((array.Length - 9) % 256);
            array[8] = (byte)((array.Length - 9) / 256);
            array[9] = 10;
            array[10] = 0;
            mcCore.CopyTo(array, 11);
            return array;
        }

        /// <summary>
        /// 检查从MC返回的数据是否是合法的。
        /// </summary>
        /// <param name="content">数据内容</param>
        /// <returns>是否合法</returns>
        // Token: 0x06000E08 RID: 3592 RVA: 0x00058F94 File Offset: 0x00057194
        public static bool CheckResponseContentHelper(byte[] content)
        {
            ushort num = BitConverter.ToUInt16(content, 9);
            if (num > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 从三菱地址，是否位读取进行创建读取的MC的核心报文<br />
        /// From the Mitsubishi address, whether to read the core message of the MC for creating and reading
        /// </summary>
        /// <param name="isBit">是否进行了位读取操作</param>
        /// <param name="addressData">三菱Mc协议的数据地址</param>
        /// <returns>带有成功标识的报文对象</returns>
        // Token: 0x06000E09 RID: 3593 RVA: 0x00058FCC File Offset: 0x000571CC
        public static byte[] BuildReadMcCoreCommand(McAddressData addressData, bool isBit)
        {
            return new byte[]
            {
                1,
                4,
                isBit ? (byte)1 : (byte)0,
                0,
                BitConverter.GetBytes(addressData.AddressStart)[0],
                BitConverter.GetBytes(addressData.AddressStart)[1],
                BitConverter.GetBytes(addressData.AddressStart)[2],
                (byte)addressData.McDataType.DataCode,
                (byte)(addressData.Length % 256),
                (byte)(addressData.Length / 256)
            };
        }

        /// <summary>
        /// 以字为单位，创建数据写入的核心报文
        /// </summary>
        /// <param name="addressData">三菱Mc协议的数据地址</param>
        /// <param name="value">实际的原始数据信息</param>
        /// <returns>带有成功标识的报文对象</returns>
        // Token: 0x06000E0A RID: 3594 RVA: 0x00059060 File Offset: 0x00057260
        public static byte[] BuildWriteWordCoreCommand(McAddressData addressData, byte[] value)
        {
            bool flag = value == null;
            if (flag)
            {
                value = new byte[0];
            }
            byte[] array = new byte[10 + value.Length];
            array[0] = 1;
            array[1] = 20;
            array[2] = 0;
            array[3] = 0;
            array[4] = BitConverter.GetBytes(addressData.AddressStart)[0];
            array[5] = BitConverter.GetBytes(addressData.AddressStart)[1];
            array[6] = BitConverter.GetBytes(addressData.AddressStart)[2];
            array[7] = (byte)addressData.McDataType.DataCode;
            array[8] = (byte)(value.Length / 2 % 256);
            array[9] = (byte)(value.Length / 2 / 256);
            value.CopyTo(array, 10);
            return array;
        }

        /// <summary>
        /// 以位为单位，创建数据写入的核心报文
        /// </summary>
        /// <param name="addressData">三菱Mc协议的数据地址</param>
        /// <param name="value">原始的bool数组数据</param>
        /// <returns>带有成功标识的报文对象</returns>
        // Token: 0x06000E0B RID: 3595 RVA: 0x00059108 File Offset: 0x00057308
        public static byte[] BuildWriteBitCoreCommand(McAddressData addressData, bool[] value)
        {
            bool flag = value == null;
            if (flag)
            {
                value = new bool[0];
            }
            byte[] array = MelsecHelper.TransBoolArrayToByteData(value);
            byte[] array2 = new byte[10 + array.Length];
            array2[0] = 1;
            array2[1] = 20;
            array2[2] = 1;
            array2[3] = 0;
            array2[4] = BitConverter.GetBytes(addressData.AddressStart)[0];
            array2[5] = BitConverter.GetBytes(addressData.AddressStart)[1];
            array2[6] = BitConverter.GetBytes(addressData.AddressStart)[2];
            array2[7] = (byte)addressData.McDataType.DataCode;
            array2[8] = (byte)(value.Length % 256);
            array2[9] = (byte)(value.Length / 256);
            array.CopyTo(array2, 10);
            return array2;
        }

        /// <summary>
        /// 从三菱扩展地址，是否位读取进行创建读取的MC的核心报文
        /// </summary>
        /// <param name="isBit">是否进行了位读取操作</param>
        /// <param name="extend">扩展指定</param>
        /// <param name="addressData">三菱Mc协议的数据地址</param>
        /// <returns>带有成功标识的报文对象</returns>
        // Token: 0x06000E0C RID: 3596 RVA: 0x000591B4 File Offset: 0x000573B4
        public static byte[] BuildReadMcCoreExtendCommand(McAddressData addressData, ushort extend, bool isBit)
        {
            return new byte[]
            {
                1,
                4,
                isBit ? (byte)129 : (byte)128,
                0,
                0,
                0,
                BitConverter.GetBytes(addressData.AddressStart)[0],
                BitConverter.GetBytes(addressData.AddressStart)[1],
                BitConverter.GetBytes(addressData.AddressStart)[2],
                (byte)addressData.McDataType.DataCode,
                0,
                0,
                BitConverter.GetBytes(extend)[0],
                BitConverter.GetBytes(extend)[1],
                249,
                (byte)(addressData.Length % 256),
                (byte)(addressData.Length / 256)
            };
        }

        /// <summary>
        /// 按字为单位随机读取的指令创建
        /// </summary>
        /// <param name="address">地址数组</param>
        /// <returns>指令</returns>
        // Token: 0x06000E0D RID: 3597 RVA: 0x00059284 File Offset: 0x00057484
        public static byte[] BuildReadRandomWordCommand(McAddressData[] address)
        {
            byte[] array = new byte[6 + address.Length * 4];
            array[0] = 3;
            array[1] = 4;
            array[2] = 0;
            array[3] = 0;
            array[4] = (byte)address.Length;
            array[5] = 0;
            for (int i = 0; i < address.Length; i++)
            {
                array[i * 4 + 6] = BitConverter.GetBytes(address[i].AddressStart)[0];
                array[i * 4 + 7] = BitConverter.GetBytes(address[i].AddressStart)[1];
                array[i * 4 + 8] = BitConverter.GetBytes(address[i].AddressStart)[2];
                array[i * 4 + 9] = (byte)address[i].McDataType.DataCode;
            }
            return array;
        }

        /// <summary>
        /// 随机读取的指令创建
        /// </summary>
        /// <param name="address">地址数组</param>
        /// <returns>指令</returns>
        // Token: 0x06000E0E RID: 3598 RVA: 0x0005932C File Offset: 0x0005752C
        public static byte[] BuildReadRandomCommand(McAddressData[] address)
        {
            byte[] array = new byte[6 + address.Length * 6];
            array[0] = 6;
            array[1] = 4;
            array[2] = 0;
            array[3] = 0;
            array[4] = (byte)address.Length;
            array[5] = 0;
            for (int i = 0; i < address.Length; i++)
            {
                array[i * 6 + 6] = BitConverter.GetBytes(address[i].AddressStart)[0];
                array[i * 6 + 7] = BitConverter.GetBytes(address[i].AddressStart)[1];
                array[i * 6 + 8] = BitConverter.GetBytes(address[i].AddressStart)[2];
                array[i * 6 + 9] = (byte)address[i].McDataType.DataCode;
                array[i * 6 + 10] = (byte)(address[i].Length % 256);
                array[i * 6 + 11] = (byte)(address[i].Length / 256);
            }
            return array;
        }

        /// <summary>
        /// 创建批量读取标签的报文数据信息
        /// </summary>
        /// <param name="tags">标签名</param>
        /// <param name="lengths">长度信息</param>
        /// <returns>报文名称</returns>
        public static byte[] BuildReadTag(string[] tags, ushort[] lengths)
        {
            if (tags.Length != lengths.Length)
            {
                return null;
            }
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.WriteByte(26);
            memoryStream.WriteByte(4);
            memoryStream.WriteByte(0);
            memoryStream.WriteByte(0);
            memoryStream.WriteByte(BitConverter.GetBytes(tags.Length)[0]);
            memoryStream.WriteByte(BitConverter.GetBytes(tags.Length)[1]);
            memoryStream.WriteByte(0);
            memoryStream.WriteByte(0);
            for (int i = 0; i < tags.Length; i++)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(tags[i]);
                memoryStream.WriteByte(BitConverter.GetBytes(bytes.Length / 2)[0]);
                memoryStream.WriteByte(BitConverter.GetBytes(bytes.Length / 2)[1]);
                memoryStream.Write(bytes, 0, bytes.Length);
                memoryStream.WriteByte(1);
                memoryStream.WriteByte(0);
                memoryStream.WriteByte(BitConverter.GetBytes(lengths[i] * 2)[0]);
                memoryStream.WriteByte(BitConverter.GetBytes(lengths[i] * 2)[1]);
            }
            byte[] result = memoryStream.ToArray();
            memoryStream.Dispose();
            return result;
        }

        /// <summary>
        /// 读取本站缓冲寄存器的数据信息，需要指定寄存器的地址，和读取的长度
        /// </summary>
        /// <param name="address">寄存器的地址</param>
        /// <param name="length">数据长度</param>
        /// <returns>结果内容</returns>
        public static byte[] BuildReadMemoryCommand(string address, ushort length)
        {
            byte[] result = null;
            try
            {
                uint value = uint.Parse(address);
                result = new byte[]
                {
                    19,
                    6,
                    0,
                    0,
                    BitConverter.GetBytes(value)[0],
                    BitConverter.GetBytes(value)[1],
                    BitConverter.GetBytes(value)[2],
                    BitConverter.GetBytes(value)[3],
                    (byte)(length % 256),
                    (byte)(length / 256)
                };
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 构建读取智能模块的命令，需要指定模块编号，起始地址，读取的长度，注意，该长度以字节为单位。
        /// </summary>
        /// <param name="module">模块编号</param>
        /// <param name="address">智能模块的起始地址</param>
        /// <param name="length">读取的字长度</param>
        /// <returns>报文的结果内容</returns>
        // Token: 0x06000E11 RID: 3601 RVA: 0x000595D8 File Offset: 0x000577D8
        public static byte[] BuildReadSmartModule(ushort module, string address, ushort length)
        {
            byte[] result = null;
            try
            {
                uint value = uint.Parse(address);
                result = new byte[]
                {
                    1,
                    6,
                    0,
                    0,
                    BitConverter.GetBytes(value)[0],
                    BitConverter.GetBytes(value)[1],
                    BitConverter.GetBytes(value)[2],
                    BitConverter.GetBytes(value)[3],
                    (byte)(length % 256),
                    (byte)(length / 256),
                    BitConverter.GetBytes(module)[0],
                    BitConverter.GetBytes(module)[1]
                };
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 解析出标签读取的数据内容
        /// </summary>
        /// <param name="content">返回的数据信息</param>
        /// <returns>解析结果</returns>
        public static byte[] ExtraTagData(byte[] content)
        {
            byte[] result = null;
            try
            {
                int num = BitConverter.ToUInt16(content, 0);
                int num2 = 2;
                List<byte> list = new List<byte>(20);
                for (int i = 0; i < num; i++)
                {
                    int num3 = BitConverter.ToUInt16(content, num2 + 2);
                    list.AddRange(DataExtend.ArraySelectMiddle(content, num2 + 4, num3));
                    num2 += 4 + num3;
                }
                result = list.ToArray();
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <inheritdoc cref="M:HslCommunication.Profinet.Melsec.Helper.IReadWriteMc.ExtractActualData(System.Byte[],System.Boolean)" />
        public static byte[] ExtractActualDataHelper(byte[] response, bool isBit)
        {
            byte[] result;
            if (isBit)
            {
                byte[] array = new byte[response.Length * 2];
                for (int i = 0; i < response.Length; i++)
                {
                    bool flag = (response[i] & 16) == 16;
                    if (flag)
                    {
                        array[i * 2] = 1;
                    }
                    bool flag2 = (response[i] & 1) == 1;
                    if (flag2)
                    {
                        array[i * 2 + 1] = 1;
                    }
                }
                result = array;
            }
            else
            {
                result = response;
            }
            return result;
        }

        /// <summary>
        /// <b>[商业授权]</b> 读取PLC的标签信息，需要传入标签的名称，读取的字长度，标签举例：A; label[1]; bbb[10,10,10]<br />
        /// <b>[Authorization]</b> To read the label information of the PLC, you need to pass in the name of the label, 
        /// the length of the word read, and an example of the label: A; label [1]; bbb [10,10,10]
        /// </summary>
        /// <param name="mc">MC协议通信对象</param>
        /// <param name="tags">标签名</param>
        /// <param name="length">读取长度</param>
        /// <returns>是否成功</returns>
        /// <remarks>
        ///  不可以访问局部标签。<br />
        ///  不可以访问通过GX Works2设置的全局标签。<br />
        ///  为了访问全局标签，需要通过GX Works3的全局标签设置编辑器将“来自于外部设备的访问”的设置项目置为有效。(默认为无效。)<br />
        ///  以ASCII代码进行数据通信时，由于需要从UTF-16将标签名转换为ASCII代码，因此报文容量将增加
        /// </remarks>
        public static byte[] ReadTags(IReadWriteMc mc, string[] tags, ushort[] length)
        {
            byte[] send = BuildReadTag(tags, length);
            byte[] operateResult = mc.ReadFromCoreServer(send);
            if (operateResult == null)
            {
                return null;
            }
            else
            {
                return mc.ExtractActualData(operateResult, false);
            }
        }


    }
}
