using Cdy.Spider.Common;
using Cdy.Spider.CustomDriver;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.FujiDriver
{
    public class FujiSPBHelper
    {
        /// <summary>
        /// 将int数据转换成SPB可识别的标准的数据内容，例如 2转换为0200 , 200转换为0002
        /// </summary>
        /// <param name="address">等待转换的数据内容</param>
        /// <returns>转换之后的数据内容</returns>
        public static string AnalysisIntegerAddress(int address)
        {
            string text = address.ToString("D4");
            return text.Substring(2) + text.Substring(0, 2);
        }

        /// <summary>
        /// 计算指令的和校验码
        /// </summary>
        /// <param name="data">指令</param>
        /// <returns>校验之后的信息</returns>
        public static string CalculateAcc(string data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            int num = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                num += (int)bytes[i];
            }
            return num.ToString("X4").Substring(2);
        }

        /// <summary>
        /// 创建一条读取的指令信息，需要指定一些参数，单次读取最大105个字
        /// </summary>
        /// <param name="station">PLC的站号</param>
        /// <param name="address">地址信息</param>
        /// <param name="length">数据长度</param>
        /// <returns>是否成功的结果对象</returns>
        public static byte[] BuildReadCommand(byte station, string address, ushort length)
        {
            station = (byte)DataExtend.ExtractParameter(ref address, "s", (int)station);
            FujiSPBAddress operateResult = FujiSPBAddress.ParseFrom(address);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
               return BuildReadCommand(station, operateResult, length);
            }
        }

        /// <summary>
        /// 创建一条读取的指令信息，需要指定一些参数，单次读取最大105个字
        /// </summary>
        /// <param name="station">PLC的站号</param>
        /// <param name="address">地址信息</param>
        /// <param name="length">数据长度</param>
        /// <returns>是否成功的结果对象</returns>
        public static byte[] BuildReadCommand(byte station, FujiSPBAddress address, ushort length)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(':');
            stringBuilder.Append(station.ToString("X2"));
            stringBuilder.Append("09");
            stringBuilder.Append("FFFF");
            stringBuilder.Append("00");
            stringBuilder.Append("00");
            stringBuilder.Append(address.GetWordAddress());
            stringBuilder.Append(FujiSPBHelper.AnalysisIntegerAddress((int)length));
            stringBuilder.Append("\r\n");
            return (Encoding.ASCII.GetBytes(stringBuilder.ToString()));
        }

        /// <summary>
        /// 创建一条读取多个地址的指令信息，需要指定一些参数，单次读取最大105个字
        /// </summary>
        /// <param name="station">PLC的站号</param>
        /// <param name="address">地址信息</param>
        /// <param name="length">数据长度</param>
        /// <param name="isBool">是否位读取</param>
        /// <returns>是否成功的结果对象</returns>
        public static byte[] BuildReadCommand(byte station, string[] address, ushort[] length, bool isBool)
        {
            if (address == null || length == null)
            {
                return null;
            }
            else
            {
                if (address.Length != length.Length)
                {
                    return null;
                }
                else
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(':');
                    stringBuilder.Append(station.ToString("X2"));
                    stringBuilder.Append((6 + address.Length * 4).ToString("X2"));
                    stringBuilder.Append("FFFF");
                    stringBuilder.Append("00");
                    stringBuilder.Append("04");
                    stringBuilder.Append("00");
                    stringBuilder.Append(address.Length.ToString("X2"));
                    for (int i = 0; i < address.Length; i++)
                    {
                        station = (byte)DataExtend.ExtractParameter(ref address[i], "s", (int)station);
                        FujiSPBAddress operateResult = FujiSPBAddress.ParseFrom(address[i]);
                        if (operateResult==null)
                        {
                            return null;
                        }
                        stringBuilder.Append(operateResult.TypeCode);
                        stringBuilder.Append(length[i].ToString("X2"));
                        stringBuilder.Append(FujiSPBHelper.AnalysisIntegerAddress(operateResult.AddressStart));
                    }
                    stringBuilder[1] = station.ToString("X2")[0];
                    stringBuilder[2] = station.ToString("X2")[1];
                    stringBuilder.Append("\r\n");
                   return (Encoding.ASCII.GetBytes(stringBuilder.ToString()));
                }
            }
        }

        /// <summary>
        /// 创建一条别入byte数据的指令信息，需要指定一些参数，按照字单位，单次写入最大103个字
        /// </summary>
        /// <param name="station">站号</param>
        /// <param name="address">地址</param>
        /// <param name="value">数组值</param>
        /// <returns>是否创建成功</returns>
        public static byte[] BuildWriteByteCommand(byte station, string address, byte[] value)
        {
            station = (byte)DataExtend.ExtractParameter(ref address, "s", (int)station);
            FujiSPBAddress operateResult = FujiSPBAddress.ParseFrom(address);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(':');
                stringBuilder.Append(station.ToString("X2"));
                stringBuilder.Append("00");
                stringBuilder.Append("FFFF");
                stringBuilder.Append("01");
                stringBuilder.Append("00");
                stringBuilder.Append(operateResult.GetWordAddress());
                stringBuilder.Append(FujiSPBHelper.AnalysisIntegerAddress(value.Length / 2));
                stringBuilder.Append(value.ToHexString());
                stringBuilder[3] = ((stringBuilder.Length - 5) / 2).ToString("X2")[0];
                stringBuilder[4] = ((stringBuilder.Length - 5) / 2).ToString("X2")[1];
                stringBuilder.Append("\r\n");
                return (Encoding.ASCII.GetBytes(stringBuilder.ToString()));
            }
        }

        /// <summary>
        /// 创建一条别入byte数据的指令信息，需要指定一些参数，按照字单位，单次写入最大103个字
        /// </summary>
        /// <param name="station">站号</param>
        /// <param name="address">地址</param>
        /// <param name="value">数组值</param>
        /// <returns>是否创建成功</returns>
        // Token: 0x0600115D RID: 4445 RVA: 0x0006F9B4 File Offset: 0x0006DBB4
        public static byte[] BuildWriteBoolCommand(byte station, string address, bool value)
        {
            station = (byte)DataExtend.ExtractParameter(ref address, "s", (int)station);
            FujiSPBAddress operateResult = FujiSPBAddress.ParseFrom(address);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                bool flag2 = address.StartsWith("X") || address.StartsWith("Y") || address.StartsWith("M") || address.StartsWith("L") || address.StartsWith("TC") || address.StartsWith("CC");
                if (flag2)
                {
                    bool flag3 = address.IndexOf('.') < 0;
                    if (flag3)
                    {
                        operateResult.BitIndex = operateResult.AddressStart % 16;
                        operateResult.AddressStart = (int)((ushort)(operateResult.AddressStart / 16));
                    }
                }
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(':');
                stringBuilder.Append(station.ToString("X2"));
                stringBuilder.Append("00");
                stringBuilder.Append("FFFF");
                stringBuilder.Append("01");
                stringBuilder.Append("02");
                stringBuilder.Append(operateResult.GetWriteBoolAddress());
                stringBuilder.Append(value ? "01" : "00");
                stringBuilder[3] = ((stringBuilder.Length - 5) / 2).ToString("X2")[0];
                stringBuilder[4] = ((stringBuilder.Length - 5) / 2).ToString("X2")[1];
                stringBuilder.Append("\r\n");
                return (Encoding.ASCII.GetBytes(stringBuilder.ToString()));
            }
        }

        /// <summary>
        /// 检查反馈的数据信息，是否包含了错误码，如果没有包含，则返回成功
        /// </summary>
        /// <param name="content">原始的报文返回</param>
        /// <returns>是否成功的结果对象</returns>
        public static byte[] CheckResponseData(byte[] content)
        {
            bool flag = content[0] != 58;
            if (content[0] != 58)
            {
                return null;
            }
            else
            {
                if (Encoding.ASCII.GetString(content, 9, 2)!="00")
                {
                    return null;
                }
                else
                {
                    if (content[content.Length - 2] == 13 && content[content.Length - 1] == 10)
                    {
                        content = content.RemoveLast(2);
                    }
                    return (content.RemoveBegin(11));
                }
            }
        }

        /// <summary>
        /// 批量读取PLC的数据，以字为单位，支持读取X,Y,L,M,D,TN,CN,TC,CC,R,W具体的地址范围需要根据PLC型号来确认，地址可以携带站号信息，例如：s=2;D100<br />
        /// Read PLC data in batches, in units of words. Supports reading X, Y, L, M, D, TN, CN, TC, CC, R, W. 
        /// The specific address range needs to be confirmed according to the PLC model, The address can carry station number information, for example: s=2;D100
        /// </summary>
        /// <param name="device">PLC设备通信对象</param>
        /// <param name="station">当前的站号信息</param>
        /// <param name="address">地址信息</param>
        /// <param name="length">数据长度</param>
        /// <returns>读取结果信息</returns>
        /// <remarks>
        /// 单次读取的最大的字数为105，如果读取的字数超过这个值，请分批次读取。
        /// </remarks>
        public static byte[] Read(IReadWriteDevice device, byte station, string address, ushort length,out bool res)
        {
            byte[] operateResult = FujiSPBHelper.BuildReadCommand(station, address, length);
            if (operateResult==null)
            {
                res = false;
                return null;
            }
            else
            {
                byte[] operateResult2 = device.ReadFromCoreServer(operateResult);
                if (operateResult2==null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    byte[] operateResult3 = FujiSPBHelper.CheckResponseData(operateResult2);
                    if (operateResult3==null)
                    {
                        res = false;
                        return null;
                    }
                    else
                    {
                        res = true;
                        return (Encoding.ASCII.GetString(operateResult3.RemoveBegin(4)).ToHexBytes());
                    }
                }
            }
        }

        /// <summary>
        /// 批量写入PLC的数据，以字为单位，也就是说最少2个字节信息，支持读取X,Y,L,M,D,TN,CN,TC,CC,R具体的地址范围需要根据PLC型号来确认，地址可以携带站号信息，例如：s=2;D100<br />
        /// The data written to the PLC in batches, in units of words, that is, a minimum of 2 bytes of information. It supports reading X, Y, L, M, D, TN, CN, TC, CC, and R. 
        /// The specific address range needs to be based on PLC model to confirm, The address can carry station number information, for example: s=2;D100
        /// </summary>
        /// <param name="device">PLC设备通信对象</param>
        /// <param name="station">当前的站号信息</param>
        /// <param name="address">地址信息，举例，D100，R200，TN100，CN200</param>
        /// <param name="value">数据值</param>
        /// <returns>是否写入成功</returns>
        /// <remarks>
        /// 单次写入的最大的字数为103个字，如果写入的数据超过这个长度，请分批次写入
        /// </remarks>
        public static object Write(IReadWriteDevice device, byte station, string address, byte[] value, out bool res)
        {
            byte[] operateResult = FujiSPBHelper.BuildWriteByteCommand(station, address, value);
            if (operateResult == null)
            {
                res = false;
                return null;
            }
            else
            {
                var operateResult2 = device.ReadFromCoreServer(operateResult);
                if (operateResult2 == null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    res = FujiSPBHelper.CheckResponseData(operateResult2) != null;
                    return res;
                }
            }
        }

        /// <summary>
        /// 批量读取PLC的Bool数据，以位为单位，支持读取X,Y,L,M,D,TN,CN,TC,CC,R,W，例如 M100, 如果是寄存器地址，可以使用D10.12来访问第10个字的12位，地址可以携带站号信息，例如：s=2;M100<br />
        /// Read PLC's Bool data in batches, in units of bits, support reading X, Y, L, M, D, TN, CN, TC, CC, R, W, such as M100, if it is a register address, 
        /// you can use D10. 12 to access the 12 bits of the 10th word, the address can carry station number information, for example: s=2;M100
        /// </summary>
        /// <param name="device">PLC设备通信对象</param>
        /// <param name="station">当前的站号信息</param>
        /// <param name="address">地址信息，举例：M100, D10.12</param>
        /// <param name="length">读取的bool长度信息</param>
        /// <returns>Bool[]的结果对象</returns>
        public static bool[] ReadBool(IReadWriteDevice device, byte station, string address, ushort length,out bool res)
        {
            byte station2 = (byte)DataExtend.ExtractParameter(ref address, "s", (int)station);
            FujiSPBAddress operateResult = FujiSPBAddress.ParseFrom(address);
            if (operateResult==null)
            {
                res=false;
                return null;
            }
            else
            {
                bool flag2 = address.StartsWith("X") || address.StartsWith("Y") || address.StartsWith("M") || address.StartsWith("L") || address.StartsWith("TC") || address.StartsWith("CC");
                if (flag2)
                {
                    if (address.IndexOf('.') < 0)
                    {
                        operateResult.BitIndex = operateResult.AddressStart % 16;
                        operateResult.AddressStart = (int)((ushort)(operateResult.AddressStart / 16));
                    }
                }
                ushort length2 = (ushort)((operateResult.GetBitIndex() + (int)length - 1) / 16 - operateResult.GetBitIndex() / 16 + 1);
                var operateResult2 = FujiSPBHelper.BuildReadCommand(station2, operateResult, length2);
                if (operateResult2==null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    var operateResult3 = device.ReadFromCoreServer(operateResult2);
                    if (operateResult3==null)
                    {
                        res = false;
                        return null;
                    }
                    else
                    {
                        var operateResult4 = FujiSPBHelper.CheckResponseData(operateResult3);
                        if (operateResult4==null)
                        {
                            res = false;
                            return null;
                        }
                        else
                        {
                            res = true;
                           return (Encoding.ASCII.GetString(operateResult4.RemoveBegin(4)).ToHexBytes().ToBoolArray().SelectMiddle(operateResult.BitIndex, (int)length));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 写入一个Bool值到一个地址里，地址可以是线圈地址，也可以是寄存器地址，例如：M100, D10.12，地址可以携带站号信息，例如：s=2;D10.12<br />
        /// Write a Bool value to an address. The address can be a coil address or a register address, for example: M100, D10.12. 
        /// The address can carry station number information, for example: s=2;D10.12
        /// </summary>
        /// <param name="device">PLC设备通信对象</param>
        /// <param name="station">当前的站号信息</param>
        /// <param name="address">地址信息，举例：M100, D10.12</param>
        /// <param name="value">写入的bool值</param>
        /// <returns>是否写入成功的结果对象</returns>
        public static object Write(IReadWriteDevice device, byte station, string address, bool value,out bool res)
        {
            byte[] operateResult = FujiSPBHelper.BuildWriteBoolCommand(station, address, value);
            if (operateResult==null)
            {
                res=false;
                return null;
            }
            else
            {
                byte[] operateResult2 = device.ReadFromCoreServer(operateResult);
                if (operateResult2==null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    res = FujiSPBHelper.CheckResponseData(operateResult2)!=null;
                    return res;
                }
            }
        }
    }
}
