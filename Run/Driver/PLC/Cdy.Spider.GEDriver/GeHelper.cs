using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.GEDriver
{
    public class GeHelper
    {
        /// <summary>
        /// 构建一个读取数据的报文信息，需要指定操作的数据代码，读取的参数信息<br />
        /// To construct a message information for reading data, you need to specify the data code of the operation and the parameter information to be read
        /// </summary>
        /// <param name="id">消息号</param>
        /// <param name="code">操作代码</param>
        /// <param name="data">数据参数</param>
        /// <returns>包含是否成功的报文信息</returns>
        public static byte[] BuildReadCoreCommand(long id, byte code, byte[] data)
        {
            byte[] array = new byte[56];
            array[0] = 2;
            array[1] = 0;
            array[2] = BitConverter.GetBytes(id)[0];
            array[3] = BitConverter.GetBytes(id)[1];
            array[4] = 0;
            array[5] = 0;
            array[9] = 1;
            array[17] = 1;
            array[18] = 0;
            array[30] = 6;
            array[31] = 192;
            array[36] = 16;
            array[37] = 14;
            array[40] = 1;
            array[41] = 1;
            array[42] = code;
            data.CopyTo(array, 43);
            return array;
        }

        /// <summary>
        /// 构建一个读取数据的报文命令，需要指定消息号，读取的 GE 地址信息<br />
        /// To construct a message command to read data, you need to specify the message number and read GE address information
        /// </summary>
        /// <param name="id">消息号</param>
        /// <param name="address">GE 的地址</param>
        /// <returns>包含是否成功的报文信息</returns>
        public static byte[] BuildReadCommand(long id, GeSRTPAddress address)
        {
            bool flag = address.DataCode == 10 || address.DataCode == 12 || address.DataCode == 8;
            if (flag)
            {
                address.Length /= 2;
            }
            return GeHelper.BuildReadCoreCommand(id, 4, new byte[]
            {
                address.DataCode,
                BitConverter.GetBytes(address.AddressStart)[0],
                BitConverter.GetBytes(address.AddressStart)[1],
                BitConverter.GetBytes(address.Length)[0],
                BitConverter.GetBytes(address.Length)[1]
            });
        }

        /// <summary>
        /// 构建一个读取数据的报文命令，需要指定消息号，地址，长度，是否位读取，返回完整的报文信息。<br />
        /// To construct a message command to read data, you need to specify the message number, 
        /// address, length, whether to read in bits, and return the complete message information.
        /// </summary>
        /// <param name="id">消息号</param>
        /// <param name="address">地址</param>
        /// <param name="length">读取的长度</param>
        /// <param name="isBit"></param>
        /// <returns>包含是否成功的报文对象</returns>
        public static byte[] BuildReadCommand(long id, string address, ushort length, bool isBit)
        {
            var operateResult = GeSRTPAddress.ParseFrom(address, length, isBit);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                return  GeHelper.BuildReadCommand(id, operateResult);
            }
        }

        /// <inheritdoc cref="M:HslCommunication.Profinet.GE.GeHelper.BuildWriteCommand(System.Int64,System.String,System.Byte[])" />
        // Token: 0x060010B3 RID: 4275 RVA: 0x0006C1F4 File Offset: 0x0006A3F4
        public static byte[] BuildWriteCommand(long id, GeSRTPAddress address, byte[] value)
        {
            int num = (int)address.Length;
            bool flag = address.DataCode == 10 || address.DataCode == 12 || address.DataCode == 8;
            if (flag)
            {
                num /= 2;
            }
            byte[] array = new byte[56 + value.Length];
            array[0] = 2;
            array[1] = 0;
            array[2] = BitConverter.GetBytes(id)[0];
            array[3] = BitConverter.GetBytes(id)[1];
            array[4] = BitConverter.GetBytes(value.Length)[0];
            array[5] = BitConverter.GetBytes(value.Length)[1];
            array[9] = 2;
            array[17] = 2;
            array[18] = 0;
            array[30] = 9;
            array[31] = 128;
            array[36] = 16;
            array[37] = 14;
            array[40] = 1;
            array[41] = 1;
            array[42] = 2;
            array[48] = 1;
            array[49] = 1;
            array[50] = 7;
            array[51] = address.DataCode;
            array[52] = BitConverter.GetBytes(address.AddressStart)[0];
            array[53] = BitConverter.GetBytes(address.AddressStart)[1];
            array[54] = BitConverter.GetBytes(num)[0];
            array[55] = BitConverter.GetBytes(num)[1];
            value.CopyTo(array, 56);
            return array;
        }

        /// <summary>
        /// 构建一个批量写入 byte 数组变量的报文，需要指定消息号，写入的地址，地址参照 <see cref="T:HslCommunication.Profinet.GE.GeSRTPNet" /> 说明。<br />
        /// To construct a message to be written into byte array variables in batches, 
        /// you need to specify the message number and write address. For the address, refer to the description of <see cref="T:HslCommunication.Profinet.GE.GeSRTPNet" />.
        /// </summary>
        /// <param name="id">消息的序号</param>
        /// <param name="address">地址信息</param>
        /// <param name="value">byte数组的原始数据</param>
        /// <returns>包含结果信息的报文内容</returns>
        // Token: 0x060010B4 RID: 4276 RVA: 0x0006C318 File Offset: 0x0006A518
        public static byte[] BuildWriteCommand(long id, string address, byte[] value)
        {
            var operateResult = GeSRTPAddress.ParseFrom(address, (ushort)value.Length, false);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                return GeHelper.BuildWriteCommand(id, operateResult, value);
            }
        }

        /// <summary>
        /// 构建一个批量写入 bool 数组变量的报文，需要指定消息号，写入的地址，地址参照 <see cref="T:HslCommunication.Profinet.GE.GeSRTPNet" /> 说明。<br />
        /// To construct a message to be written into bool array variables in batches, 
        /// you need to specify the message number and write address. For the address, refer to the description of <see cref="T:HslCommunication.Profinet.GE.GeSRTPNet" />.
        /// </summary>
        /// <param name="id">消息的序号</param>
        /// <param name="address">地址信息</param>
        /// <param name="value">bool数组</param>
        /// <returns>包含结果信息的报文内容</returns>
        // Token: 0x060010B5 RID: 4277 RVA: 0x0006C35C File Offset: 0x0006A55C
        public static byte[] BuildWriteCommand(long id, string address, bool[] value)
        {
            var operateResult = GeSRTPAddress.ParseFrom(address, (ushort)value.Length, true);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                bool[] array = new bool[operateResult.AddressStart % 8 + value.Length];
                value.CopyTo(array, operateResult.AddressStart % 8);
                return GeHelper.BuildWriteCommand(id, operateResult, DataExtend.BoolArrayToByte(array));
            }
        }

        /// <summary>
        /// 从PLC返回的数据中，提取出实际的数据内容，最少6个字节的数据。超出实际的数据长度的部分没有任何意义。<br />
        /// From the data returned by the PLC, extract the actual data content, at least 6 bytes of data. The part beyond the actual data length has no meaning.
        /// </summary>
        /// <param name="content">PLC返回的数据信息</param>
        /// <returns>解析后的实际数据内容</returns>
        public static byte[] ExtraResponseContent(byte[] content)
        {
            try
            {
                if (content[0] != 3)
                {
                    return null;
                }
                else
                {
                    //bool flag2 = content[31] == 212;
                    if (content[31] == 212)
                    {
                        ushort num = BitConverter.ToUInt16(content, 42);
                        //bool flag3 = num > 0;
                        if (num > 0)
                        {
                            return null;
                        }
                        else
                        {
                           return (content.SelectMiddle(44, 6));
                        }
                    }
                    else
                    {
                        //bool flag4 = content[31] == 148;
                        if (content[31] == 148)
                        {
                            return (content.RemoveBegin(56));
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 从实际的时间的字节数组里解析出C#格式的时间对象，这个时间可能是时区0的时间，需要自行转化本地时间。<br />
        /// Analyze the time object in C# format from the actual time byte array. 
        /// This time may be the time in time zone 0, and you need to convert the local time yourself.
        /// </summary>
        /// <param name="content">字节数组</param>
        /// <returns>包含是否成功的结果对象</returns>
        public static DateTime ExtraDateTime(byte[] content)
        {
            try
            {
               return (new DateTime(int.Parse(content[5].ToString("X2")) + 2000, int.Parse(content[4].ToString("X2")), int.Parse(content[3].ToString("X2")), int.Parse(content[2].ToString("X2")), int.Parse(content[1].ToString("X2")), int.Parse(content[0].ToString("X2"))));
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// 从实际的时间的字节数组里解析出PLC的程序的名称。<br />
        /// Parse the name of the PLC program from the actual time byte array
        /// </summary>
        /// <param name="content">字节数组</param>
        /// <returns>包含是否成功的结果对象</returns>
        public static string ExtraProgramName(byte[] content,out bool res)
        {
            try
            {
                res = true;
                return (Encoding.UTF8.GetString(content, 18, 16).Trim(new char[1]));
            }
            catch (Exception)
            {
                res=false;
                return String.Empty;
            }
        }
    }

}
