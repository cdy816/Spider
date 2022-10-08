using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver
{
    public class MelsecFxLinksHelper
    {
        /// <summary>
        /// 将当前的报文进行打包，根据和校验的方式以及格式信息来实现打包操作
        /// </summary>
        /// <param name="plc">PLC通信对象</param>
        /// <param name="command">原始的命令数据</param>
        /// <returns>打包后的命令</returns>
        public static byte[] PackCommandWithHeader(IReadWriteFxLinks plc, byte[] command)
        {
            byte[] array = command;
            bool sumCheck = plc.SumCheck;
            if (sumCheck)
            {
                array = new byte[command.Length + 2];
                command.CopyTo(array, 0);
                DataExtend.CalculateAccAndFill(array, 0, 2);
            }
            byte[] result;
            if (plc.Format == 1)
            {
                result = DataExtend.SpliceArray(new byte[][]
                {
                    new byte[]
                    {
                        5
                    },
                    array
                });
            }
            else
            {
                if (plc.Format == 4)
                {
                    result = DataExtend.SpliceArray(new byte[][]
                    {
                        new byte[]
                        {
                            5
                        },
                        array,
                        new byte[]
                        {
                            13,
                            10
                        }
                    });
                }
                else
                {
                    result = DataExtend.SpliceArray(new byte[][]
                    {
                        new byte[]
                        {
                            5
                        },
                        array
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// 创建一条读取的指令信息，需要指定一些参数
        /// </summary>
        /// <param name="station">PLC的站号</param>
        /// <param name="address">地址信息</param>
        /// <param name="length">数据长度</param>
        /// <param name="isBool">是否位读取</param>
        /// <param name="waitTime">等待时间</param>
        /// <returns>是否成功的结果对象</returns>
        public static List<byte[]> BuildReadCommand(byte station, string address, ushort length, bool isBool, byte waitTime = 0)
        {
            MelsecFxLinksAddress operateResult = MelsecFxLinksAddress.ParseFrom(address);
            if (operateResult == null)
            {
                return null;
            }
            else
            {
                int[] array = DataExtend.SplitIntegerToArray(length, isBool ? 256 : 64);
                List<byte[]> list = new List<byte[]>();
                for (int i = 0; i < array.Length; i++)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(station.ToString("X2"));
                    stringBuilder.Append("FF");
                    if (isBool)
                    {
                        stringBuilder.Append("BR");
                    }
                    else
                    {
                        bool flag2 = operateResult.AddressStart >= 10000;
                        if (flag2)
                        {
                            stringBuilder.Append("QR");
                        }
                        else
                        {
                            stringBuilder.Append("WR");
                        }
                    }
                    stringBuilder.Append(waitTime.ToString("X"));
                    stringBuilder.Append(operateResult.ToString());
                    bool flag3 = array[i] == 256;
                    if (flag3)
                    {
                        stringBuilder.Append("00");
                    }
                    else
                    {
                        stringBuilder.Append(array[i].ToString("X2"));
                    }
                    list.Add(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
                    operateResult.AddressStart += array[i];
                }
                return list;
            }
        }

        /// <summary>
        /// 创建一条别入bool数据的指令信息，需要指定一些参数
        /// </summary>
        /// <param name="station">站号</param>
        /// <param name="address">地址</param>
        /// <param name="value">数组值</param>
        /// <param name="waitTime">等待时间</param>
        /// <returns>是否创建成功</returns>
        public static byte[] BuildWriteBoolCommand(byte station, string address, bool[] value, byte waitTime = 0)
        {
            MelsecFxLinksAddress operateResult = MelsecFxLinksAddress.ParseFrom(address);
            if (operateResult == null)
            {
                return null;
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(station.ToString("X2"));
                stringBuilder.Append("FF");
                stringBuilder.Append("BW");
                stringBuilder.Append(waitTime.ToString("X"));
                stringBuilder.Append(operateResult.ToString());
                stringBuilder.Append(value.Length.ToString("X2"));
                for (int i = 0; i < value.Length; i++)
                {
                    stringBuilder.Append(value[i] ? "1" : "0");
                }
                return Encoding.ASCII.GetBytes(stringBuilder.ToString());
            }
        }

        /// <summary>
        /// 创建一条别入byte数据的指令信息，需要指定一些参数，按照字单位
        /// </summary>
        /// <param name="station">站号</param>
        /// <param name="address">地址</param>
        /// <param name="value">数组值</param>
        /// <param name="waitTime">等待时间</param>
        /// <returns>命令报文的结果内容对象</returns>
        public static byte[] BuildWriteByteCommand(byte station, string address, byte[] value, byte waitTime = 0)
        {
            MelsecFxLinksAddress operateResult = MelsecFxLinksAddress.ParseFrom(address);
            if (operateResult == null)
            {
                return null;
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(station.ToString("X2"));
                stringBuilder.Append("FF");
                bool flag2 = operateResult.AddressStart >= 10000;
                if (flag2)
                {
                    stringBuilder.Append("QW");
                }
                else
                {
                    stringBuilder.Append("WW");
                }
                stringBuilder.Append(waitTime.ToString("X"));
                stringBuilder.Append(operateResult.ToString());
                stringBuilder.Append((value.Length / 2).ToString("X2"));
                byte[] array = new byte[value.Length * 2];
                for (int i = 0; i < value.Length / 2; i++)
                {
                    DataExtend.BuildAsciiBytesFrom(BitConverter.ToUInt16(value, i * 2)).CopyTo(array, 4 * i);
                }
                stringBuilder.Append(Encoding.ASCII.GetString(array));
                return Encoding.ASCII.GetBytes(stringBuilder.ToString());
            }
        }

        /// <summary>
        /// 创建读取PLC类型的命令报文
        /// </summary>
        /// <param name="station">站号信息</param>
        /// <param name="waitTime">等待实际</param>
        /// <returns>命令报文的结果内容对象</returns>
        public static byte[] BuildReadPlcType(byte station, byte waitTime = 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(station.ToString("X2"));
            stringBuilder.Append("FF");
            stringBuilder.Append("PC");
            stringBuilder.Append(waitTime.ToString("X"));
            return Encoding.ASCII.GetBytes(stringBuilder.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        internal static uint ComputeStringHash(string s)
        {
            uint num = 0;
            if (s != null)
            {
                num = 2166136261U;
                for (int i = 0; i < s.Length; i++)
                {
                    num = (s[i] ^ num) * 16777619U;
                }
            }
            return num;
        }

        /// <summary>
        /// 从编码中提取PLC的型号信息
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>PLC的型号信息</returns>
        public static string GetPlcTypeFromCode(string code)
        {
            uint num = ComputeStringHash(code);
            if (num <= 752165258U)
            {
                if (num <= 366132926U)
                {
                    if (num <= 234789874U)
                    {
                        if (num != 134124160U)
                        {
                            if (num == 234789874U)
                            {
                                if (code == "92")
                                {
                                    return "A2ACPU";
                                }
                            }
                        }
                        else if (code == "94")
                        {
                            return "A3ACPU";
                        }
                    }
                    else if (num != 251567493U)
                    {
                        if (num != 335455588U)
                        {
                            if (num == 366132926U)
                            {
                                if (code == "F3")
                                {
                                    return "FX3U/FX3UC";
                                }
                            }
                        }
                        else if (code == "98")
                        {
                            return "A0J2HCPU";
                        }
                    }
                    else if (code == "93")
                    {
                        return "A2ACPU-S1";
                    }
                }
                else if (num <= 486601254U)
                {
                    if (num != 382910545U)
                    {
                        if (num != 416465783U)
                        {
                            if (num == 486601254U)
                            {
                                if (code == "8E")
                                {
                                    return "FX0N";
                                }
                            }
                        }
                        else if (code == "F4")
                        {
                            return "FX3G";
                        }
                    }
                    else if (code == "F2")
                    {
                        return "FX1S";
                    }
                }
                else if (num != 503378873U)
                {
                    if (num != 536934111U)
                    {
                        if (num == 752165258U)
                        {
                            if (code == "AB")
                            {
                                return "AJ72P25/R25";
                            }
                        }
                    }
                    else if (code == "8B")
                    {
                        return "AJ72LP25/BR15";
                    }
                }
                else if (code == "8D")
                {
                    return "FX2/FX2C";
                }
            }
            else if (num <= 2382472201U)
            {
                if (num <= 2029995107U)
                {
                    if (num != 2013217488U)
                    {
                        if (num == 2029995107U)
                        {
                            if (code == "9E")
                            {
                                return "FX1N/FX1NC";
                            }
                        }
                    }
                    else if (code == "9D")
                    {
                        return "FX2N/FX2NC";
                    }
                }
                else if (num != 2097105583U)
                {
                    if (num != 2365694582U)
                    {
                        if (num == 2382472201U)
                        {
                            if (code == "84")
                            {
                                return "A3UCPU";
                            }
                        }
                    }
                    else if (code == "85")
                    {
                        return "A4UCPU";
                    }
                }
                else if (code == "9A")
                {
                    return "A2CCPU";
                }
            }
            else if (num <= 2530592872U)
            {
                if (num != 2399249820U)
                {
                    if (num != 2416027439U)
                    {
                        if (num == 2530592872U)
                        {
                            if (code == "A4")
                            {
                                return "A3HCPU/A3MCPU";
                            }
                        }
                    }
                    else if (code == "82")
                    {
                        return "A2USCPU";
                    }
                }
                else if (code == "83")
                {
                    return "A2CPU-S1/A2USCPU-S1";
                }
            }
            else if (num != 2614480967U)
            {
                if (num != 2631258586U)
                {
                    if (num == 2648036205U)
                    {
                        if (code == "A3")
                        {
                            return "A3CPU/A3NCPU";
                        }
                    }
                }
                else if (code == "A2")
                {
                    return "A2CPU/A2NCPU/A2SCPU";
                }
            }
            else if (code == "A1")
            {
                return "A1CPU /A1NCPU";
            }
            return "";
        }



        /// <summary>
        /// 检查PLC的消息反馈是否合法，合法则提取当前的数据信息，当时写入的命令消息时，无任何的数据返回<br />
        /// Check whether the PLC's message feedback is legal. If it is legal, extract the current data information. When the command message is written at that time, no data is returned.
        /// </summary>
        /// <param name="response">从PLC反馈的数据消息</param>
        /// <returns>检查的结果消息</returns>
        public static byte[] CheckPlcResponse(byte[] response)
        {
            try
            {
                if (response[0] == 21)
                {
                    return null;
                }
                else
                {
                    if (response[0] != 2 && response[0] != 6)
                    {
                        return null;
                    }
                    else
                    {
                        if (response[0] == 6)
                        {
                            return new byte[0];
                        }
                        else
                        {
                            int num2 = -1;
                            for (int i = 5; i < response.Length; i++)
                            {
                                bool flag4 = response[i] == 3;
                                if (flag4)
                                {
                                    num2 = i;
                                    break;
                                }
                            }
                            bool flag5 = num2 == -1;
                            if (flag5)
                            {
                                num2 = response.Length;
                            }
                            return response.SelectMiddle(5, num2 - 5);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 批量读取PLC的数据，以字为单位，支持读取X,Y,M,S,D,T,C，具体的地址范围需要根据PLC型号来确认，地址支持动态指定站号，例如：s=2;D100<br />
        /// Read PLC data in batches, in units of words, supports reading X, Y, M, S, D, T, C. 
        /// The specific address range needs to be confirmed according to the PLC model, 
        /// The address supports dynamically specifying the station number, for example: s=2;D100
        /// </summary>
        /// <param name="plc">PLC通信对象</param>
        /// <param name="address">地址信息</param>
        /// <param name="length">数据长度</param>
        /// <returns>读取结果信息</returns>
        public static byte[] Read(IReadWriteFxLinks plc, string address, ushort length, out bool res)
        {
            byte station = (byte)DataExtend.ExtractParameter(ref address, "s", plc.Station);
            List<byte[]> operateResult = BuildReadCommand(station, address, length, false, plc.WaittingTime);
            if (operateResult == null)
            {
                res = false;
                return null;
            }
            else
            {
                List<byte> list = new List<byte>();
                for (int i = 0; i < operateResult.Count; i++)
                {
                    byte[] operateResult2 = plc.ReadFromCoreServer(operateResult[i]);
                    if (operateResult2 == null)
                    {
                        res = false;
                        return null;
                    }
                    var operateResult3 = CheckPlcResponse(operateResult2);
                    if (operateResult3 == null)
                    {
                        res = false;
                        return null;
                    }
                    byte[] array = new byte[operateResult3.Length / 2];
                    for (int j = 0; j < array.Length / 2; j++)
                    {
                        ushort value = Convert.ToUInt16(Encoding.ASCII.GetString(operateResult3, j * 4, 4), 16);
                        BitConverter.GetBytes(value).CopyTo(array, j * 2);
                    }
                    list.AddRange(array);
                }
                res = true;
                return list.ToArray();
            }
        }

        /// <summary>
        /// 批量写入PLC的数据，以字为单位，也就是说最少2个字节信息，支持X,Y,M,S,D,T,C，具体的地址范围需要根据PLC型号来确认，地址支持动态指定站号，例如：s=2;D100<br />
        /// The data written to the PLC in batches is in units of words, that is, at least 2 bytes of information. 
        /// It supports X, Y, M, S, D, T, and C. The specific address range needs to be confirmed according to the PLC model, 
        /// The address supports dynamically specifying the station number, for example: s=2;D100
        /// </summary>
        /// <param name="plc">PLC通信对象</param>
        /// <param name="address">地址信息</param>
        /// <param name="value">数据值</param>
        /// <returns>是否写入成功</returns>
        public static object Write(IReadWriteFxLinks plc, string address, byte[] value, out bool res)
        {
            byte station = (byte)DataExtend.ExtractParameter(ref address, "s", plc.Station);
            byte[] operateResult = BuildWriteByteCommand(station, address, value, plc.WaittingTime);
            if (operateResult == null)
            {
                res = false;
                return null;
            }
            else
            {
                byte[] operateResult2 = plc.ReadFromCoreServer(operateResult);
                if (operateResult2 == null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    byte[] operateResult3 = CheckPlcResponse(operateResult2);
                    if (operateResult3 == null)
                    {
                        res = false;
                        return null;
                    }
                    else
                    {
                        res = true;
                        return true;
                    }
                }
            }
        }



        /// <summary>
        /// 批量读取bool类型数据，支持的类型为X,Y,S,T,C，具体的地址范围取决于PLC的类型，地址支持动态指定站号，例如：s=2;D100<br />
        /// Read bool data in batches. The supported types are X, Y, S, T, C. The specific address range depends on the type of PLC, 
        /// The address supports dynamically specifying the station number, for example: s=2;D100
        /// </summary>
        /// <param name="plc">PLC通信对象</param>
        /// <param name="address">地址信息，比如X10,Y17，注意X，Y的地址是8进制的</param>
        /// <param name="length">读取的长度</param>
        /// <returns>读取结果信息</returns>
        public static bool[] ReadBool(IReadWriteFxLinks plc, string address, ushort length, out bool res)
        {
            byte station = (byte)DataExtend.ExtractParameter(ref address, "s", plc.Station);
            List<byte[]> operateResult = BuildReadCommand(station, address, length, true, plc.WaittingTime);
            if (operateResult == null)
            {
                res = false;
                return null;
            }
            else
            {
                List<bool> list = new List<bool>();
                for (int i = 0; i < operateResult.Count; i++)
                {
                    byte[] operateResult2 = plc.ReadFromCoreServer(operateResult[i]);
                    if (operateResult2 == null)
                    {
                        res = false;
                        return null;
                    }
                    byte[] operateResult3 = CheckPlcResponse(operateResult2);
                    if (operateResult3 == null)
                    {
                        res = false;
                        return null;
                    }
                    list.AddRange((from m in operateResult3
                                   select m == 49).ToArray());
                }
                res = true;
                return list.ToArray();
            }
        }

        /// <summary>
        /// 批量写入bool类型的数组，支持的类型为X,Y,S,T,C，具体的地址范围取决于PLC的类型，地址支持动态指定站号，例如：s=2;D100<br />
        /// Write arrays of type bool in batches. The supported types are X, Y, S, T, C. The specific address range depends on the type of PLC, 
        /// The address supports dynamically specifying the station number, for example: s=2;D100
        /// </summary>
        /// <param name="plc">PLC通信对象</param>
        /// <param name="address">PLC的地址信息</param>
        /// <param name="value">数据信息</param>
        /// <returns>是否写入成功</returns>
        public static object Write(IReadWriteFxLinks plc, string address, bool[] value, out bool res)
        {
            byte station = (byte)DataExtend.ExtractParameter(ref address, "s", plc.Station);
            byte[] operateResult = BuildWriteBoolCommand(station, address, value, plc.WaittingTime);
            if (operateResult == null)
            {
                res = false;
                return false;
            }
            else
            {
                byte[] operateResult2 = plc.ReadFromCoreServer(operateResult);
                if (operateResult2 == null)
                {
                    res = false;
                    return false;
                }
                else
                {
                    byte[] operateResult3 = CheckPlcResponse(operateResult2);
                    if (operateResult3 == null)
                    {
                        res = false;
                        return false;
                    }
                    else
                    {
                        res = true;
                        return true;
                    }
                }
            }
        }

        /// <summary>
        /// 读取PLC的型号信息，可以携带额外的参数信息，指定站号。举例：s=2; 注意：分号是必须的。<br />
        /// Read the PLC model information, you can carry additional parameter information, and specify the station number. Example: s=2; Note: The semicolon is required.
        /// </summary>
        /// <param name="plc">PLC通信对象</param>
        /// <param name="parameter">允许携带的参数信息，例如s=2; 也可以为空</param>
        /// <returns>带PLC型号的结果信息</returns>
        public static string ReadPlcType(IReadWriteFxLinks plc, string parameter, out bool res)
        {
            byte station = (byte)DataExtend.ExtractParameter(ref parameter, "s", plc.Station);
            byte[] operateResult = BuildReadPlcType(station, plc.WaittingTime);
            if (operateResult == null)
            {
                res = false;
                return string.Empty;
            }
            else
            {
                byte[] operateResult2 = plc.ReadFromCoreServer(operateResult);
                if (operateResult2 == null)
                {
                    res = false;
                    return string.Empty;
                }
                else
                {
                    byte[] operateResult3 = CheckPlcResponse(operateResult2);
                    if (operateResult3 == null)
                    {
                        res = false;
                        return string.Empty;
                    }
                    else
                    {
                        res = true;
                        return GetPlcTypeFromCode(Encoding.ASCII.GetString(operateResult2, 5, 2));
                    }
                }
            }
        }
    }
}
