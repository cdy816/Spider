using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver
{
    /// <summary>
    /// MC协议的辅助类对象，提供了MC协议的读写操作的基本支持
    /// </summary>
    public class McHelper
    {
        /// <summary>
        /// 返回按照字单位读取的最低的长度信息
        /// </summary>
        /// <param name="type">MC协议的类型</param>
        /// <returns>长度信息</returns>
        public static int GetReadWordLength(McType type)
        {
            int result;
            if (type == McType.McBinary || type == McType.McRBinary)
            {
                result = 950;
            }
            else
            {
                result = 460;
            }
            return result;
        }

        /// <summary>
        /// 返回按照位单位读取的最低的长度信息
        /// </summary>
        /// <param name="type">MC协议的类型</param>
        /// <returns>长度信息</returns>
        public static int GetReadBoolLength(McType type)
        {
            int result;
            if (type == McType.McBinary || type == McType.McRBinary)
            {
                result = 7168;
            }
            else
            {
                result = 3584;
            }
            return result;
        }

        /// 初步支持普通的数据地址之外，还额外支持高级的地址写法，以下是示例（适用于MC协议为二进制和ASCII格式）：<br />
        /// </remarks>
        public static byte[] Read(IReadWriteMc mc, string address, ushort length, out bool res)
        {
            bool flag = mc.McType == McType.McBinary && address.StartsWith("s=") || address.StartsWith("S=");
            byte[] result;
            if (flag)
            {
                result = McBinaryHelper.ReadTags(mc, new string[]
                {
                    address.Substring(2)
                }, new ushort[]
                {
                    length
                });
            }
            else
            {
                bool flag2 = (mc.McType == McType.McBinary || mc.McType == McType.MCAscii) && Regex.IsMatch(address, "ext=[0-9]+;", RegexOptions.IgnoreCase);
                if (flag2)
                {
                    string value = Regex.Match(address, "ext=[0-9]+;").Value;
                    ushort extend = ushort.Parse(Regex.Match(value, "[0-9]+").Value);
                    result = ReadExtend(mc, extend, address.Substring(value.Length), length, out res);
                }
                else
                {
                    bool flag3 = (mc.McType == McType.McBinary || mc.McType == McType.MCAscii) && Regex.IsMatch(address, "mem=", RegexOptions.IgnoreCase);
                    if (flag3)
                    {
                        result = ReadMemory(mc, address.Substring(4), length, out res);
                    }
                    else
                    {
                        bool flag4 = (mc.McType == McType.McBinary || mc.McType == McType.MCAscii) && Regex.IsMatch(address, "module=[0-9]+;", RegexOptions.IgnoreCase);
                        if (flag4)
                        {
                            string value2 = Regex.Match(address, "module=[0-9]+;").Value;
                            ushort module = ushort.Parse(Regex.Match(value2, "[0-9]+").Value);
                            result = ReadSmartModule(mc, module, address.Substring(value2.Length), (ushort)(length * 2), out res);
                        }
                        else
                        {
                            McAddressData operateResult = mc.McAnalysisAddress(address, length);
                            if (operateResult == null)
                            {
                                res = false;
                                return null;
                            }
                            else
                            {
                                List<byte> list = new List<byte>();
                                ushort num = 0;
                                while (num < length)
                                {
                                    ushort num2 = (ushort)Math.Min(length - num, GetReadWordLength(mc.McType));
                                    operateResult.Length = num2;
                                    byte[] send = mc.McType == McType.McBinary ? McBinaryHelper.BuildReadMcCoreCommand(operateResult, false) : mc.McType == McType.MCAscii ? McAsciiHelper.BuildAsciiReadMcCoreCommand(operateResult, false) : mc.McType == McType.McRBinary ? MelsecMcRNetProxy.BuildReadMcCoreCommand(operateResult, false) : null;
                                    byte[] operateResult2 = mc.ReadFromCoreServer(send);
                                    if (operateResult2 == null)
                                    {
                                        res = false;
                                        return operateResult2;
                                    }
                                    list.AddRange(mc.ExtractActualData(operateResult2, false));
                                    num += num2;
                                    bool flag7 = operateResult.McDataType.DataType == 0;
                                    if (flag7)
                                    {
                                        operateResult.AddressStart += num2;
                                    }
                                    else
                                    {
                                        operateResult.AddressStart += num2 * 16;
                                    }
                                }

                                result = list.ToArray();
                            }
                        }
                    }
                }
            }
            res = true;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Write(IReadWriteMc mc, string address, byte[] value)
        {
            McAddressData operateResult = mc.McAnalysisAddress(address, 0);
            if (operateResult == null)
            {
                return false;
            }
            else
            {
                byte[] send = mc.McType == McType.McBinary ? McBinaryHelper.BuildWriteWordCoreCommand(operateResult, value) : mc.McType == McType.MCAscii ? McAsciiHelper.BuildAsciiWriteWordCoreCommand(operateResult, value) : mc.McType == McType.McRBinary ? MelsecMcRNetProxy.BuildWriteWordCoreCommand(operateResult, value) : null;
                var operateResult2 = mc.ReadFromCoreServer(send);
                if (operateResult2 == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <inheritdoc cref="M:HslCommunication.Core.IReadWriteNet.ReadBool(System.String)" />
        /// <remarks>
        /// 对于X,Y类型的地址，有两种表示方式，十六进制和八进制，默认16进制，比如输入 X10 是16进制的，如果想要输入8进制的地址，地址补0操作，例如 X010
        /// </remarks>
        // Token: 0x06000E1B RID: 3611 RVA: 0x00059C1C File Offset: 0x00057E1C
        public static bool ReadBool(IReadWriteMc mc, string address)
        {
            return ByteTransformHelper.GetResultFromArray(ReadBool(mc, address, 1, out bool res), out res);
        }

        /// <inheritdoc cref="M:HslCommunication.Core.IReadWriteNet.ReadBool(System.String,System.UInt16)" />
        /// <remarks>
        /// 当读取的长度过大时，会自动进行切割，对于二进制格式，切割长度为7168，对于ASCII格式协议来说，切割长度则是3584<br />
        /// 对于X,Y类型的地址，有两种表示方式，十六进制和八进制，默认16进制，比如输入 X10 是16进制的，如果想要输入8进制的地址，地址补0操作，例如 X010
        /// </remarks>
        public static bool[] ReadBool(IReadWriteMc mc, string address, ushort length, out bool res)
        {
            bool flag = address.IndexOf('.') > 0;
            bool[] result;
            if (flag)
            {
                string[] array = address.SplitDot();
                int num = 0;
                try
                {
                    num = Convert.ToInt32(array[1]);
                }
                catch (Exception ex)
                {
                    res = false;
                    return null;
                }
                ushort length2 = (ushort)((length + num + 15) / 16);
                byte[] operateResult = mc.Read(array[0], length2, out res);
                if (!res)
                {
                    res = false;
                    return null;
                }
                else
                {
                    res = true;
                    return operateResult.ToBoolArray().SelectMiddle(num, length);
                }
            }
            else
            {
                McAddressData operateResult2 = mc.McAnalysisAddress(address, length);
                if (operateResult2 == null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    List<bool> list = new List<bool>();
                    ushort num2 = 0;
                    while (num2 < length)
                    {
                        ushort num3 = (ushort)Math.Min(length - num2, GetReadBoolLength(mc.McType));
                        operateResult2.Length = num3;
                        byte[] send = mc.McType == McType.McBinary ? McBinaryHelper.BuildReadMcCoreCommand(operateResult2, true) : mc.McType == McType.MCAscii ? McAsciiHelper.BuildAsciiReadMcCoreCommand(operateResult2, true) : mc.McType == McType.McRBinary ? MelsecMcRNetProxy.BuildReadMcCoreCommand(operateResult2, true) : null;
                        byte[] operateResult3 = mc.ReadFromCoreServer(send);
                        if (operateResult3 == null)
                        {
                            res = false;
                            return null;
                        }
                        list.AddRange((from m in mc.ExtractActualData(operateResult3, true)
                                       select m == 1).Take(num3).ToArray());
                        num2 += num3;
                        operateResult2.AddressStart += num3;
                    }
                    res = true;
                    return list.ToArray();
                }
            }
        }

        /// 当读取的长度过大时，会自动进行切割，对于二进制格式，切割长度为7168，对于ASCII格式协议来说，切割长度则是3584<br />
        /// 对于X,Y类型的地址，有两种表示方式，十六进制和八进制，默认16进制，比如输入 X10 是16进制的，如果想要输入8进制的地址，地址补0操作，例如 X010
        public static bool Write(IReadWriteMc mc, string address, bool[] values)
        {
            McAddressData operateResult = mc.McAnalysisAddress(address, 0);
            if (operateResult == null)
            {
                return false;
            }
            else
            {
                byte[] send = mc.McType == McType.McBinary ? McBinaryHelper.BuildWriteBitCoreCommand(operateResult, values) : mc.McType == McType.MCAscii ? McAsciiHelper.BuildAsciiWriteBitCoreCommand(operateResult, values) : mc.McType == McType.McRBinary ? MelsecMcRNetProxy.BuildWriteBitCoreCommand(operateResult, values) : null;
                byte[] operateResult2 = mc.ReadFromCoreServer(send);
                if (operateResult2 == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }







        /// <summary>
        /// 随机读取PLC的数据信息，可以跨地址，跨类型组合，但是每个地址只能读取一个word，也就是2个字节的内容。收到结果后，需要自行解析数据<br />
        /// Randomly read PLC data information, which can be combined across addresses and types, but each address can only read one word, 
        /// which is the content of 2 bytes. After receiving the results, you need to parse the data yourself
        /// </summary>
        /// <param name="mc">MC协议通信对象</param>
        /// <param name="address">所有的地址的集合</param>
        /// <remarks>
        /// 访问安装有 Q 系列 C24/E71 的站 QCPU 上位站 经由 Q 系列兼容网络系统 MELSECNET/H MELSECNET/10 Ethernet 的 QCPU 其他站 时
        /// 访问点数········1≦ 字访问点数 双字访问点数 ≦192
        /// <br />
        /// 访问 QnACPU 其他站 经由 QnA 系列兼容网络系统 MELSECNET/10 Ethernet 的 Q/QnACPU 其他站 时访问点数········1≦ 字访问点数 双字访问点数 ≦96
        /// <br />
        /// 访问上述以外的 PLC CPU 其他站 时访问点数········1≦字访问点数≦10
        /// </remarks>
        /// <example>
        /// <returns>结果</returns>
        public static byte[] ReadRandom(IReadWriteMc mc, string[] address, out bool res)
        {
            McAddressData[] array = new McAddressData[address.Length];
            for (int i = 0; i < address.Length; i++)
            {
                McAddressData operateResult = McAddressData.ParseMelsecFrom(address[i], 1);
                if (operateResult == null)
                {
                    res = false;
                    return null;
                }
                array[i] = operateResult;
            }
            byte[] send = mc.McType == McType.McBinary ? McBinaryHelper.BuildReadRandomWordCommand(array) : mc.McType == McType.MCAscii ? McAsciiHelper.BuildAsciiReadRandomWordCommand(array) : null;
            byte[] operateResult2 = mc.ReadFromCoreServer(send);
            if (operateResult2 == null)
            {
                res = false;
                return null;
            }
            res = true;
            return mc.ExtractActualData(operateResult2, false);
        }

        /// <summary>
        /// 随机读取PLC的数据信息，可以跨地址，跨类型组合，每个地址是任意的长度。收到结果后，需要自行解析数据，目前只支持字地址，比如D区，W区，R区，不支持X，Y，M，B，L等等<br />
        /// Read the data information of the PLC randomly. It can be combined across addresses and types. Each address is of any length. After receiving the results, 
        /// you need to parse the data yourself. Currently, only word addresses are supported, such as D area, W area, R area. X, Y, M, B, L, etc
        /// </summary>
        /// <param name="mc">MC协议通信对象</param>
        /// <param name="address">所有的地址的集合</param>
        /// <param name="length">每个地址的长度信息</param>
        /// <remarks>
        /// 实际测试不一定所有的plc都可以读取成功，具体情况需要具体分析
        /// <br />
        /// 1 块数按照下列要求指定 120 ≧ 字软元件块数 + 位软元件块数
        /// <br />
        /// 2 各软元件点数按照下列要求指定 960 ≧ 字软元件各块的合计点数 + 位软元件各块的合计点数
        /// </remarks>
        /// <returns>结果</returns>
        public static byte[] ReadRandom(IReadWriteMc mc, string[] address, ushort[] length, out bool res)
        {
            byte[] result;
            if (length.Length != address.Length)
            {
                res = false;
                return null;
            }
            else
            {
                McAddressData[] array = new McAddressData[address.Length];
                for (int i = 0; i < address.Length; i++)
                {
                    McAddressData operateResult = McAddressData.ParseMelsecFrom(address[i], length[i]);
                    if (operateResult == null)
                    {
                        res = false;
                        return null;
                    }
                    array[i] = operateResult;
                }
                byte[] send = mc.McType == McType.McBinary ? McBinaryHelper.BuildReadRandomCommand(array) : mc.McType == McType.MCAscii ? McAsciiHelper.BuildAsciiReadRandomCommand(array) : null;
                byte[] operateResult2 = mc.ReadFromCoreServer(send);
                if (operateResult2 == null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    res = true;
                    return mc.ExtractActualData(operateResult2, false);
                }
            }
        }

        /// <summary>
        /// 随机读取PLC的数据信息，可以跨地址，跨类型组合，但是每个地址只能读取一个word，也就是2个字节的内容。收到结果后，自动转换为了short类型的数组<br />
        /// Randomly read PLC data information, which can be combined across addresses and types, but each address can only read one word, 
        /// which is the content of 2 bytes. After receiving the result, it is automatically converted to an array of type short.
        /// </summary>
        /// <param name="mc">MC协议的通信对象</param>
        /// <param name="address">所有的地址的集合</param>
        /// <remarks>
        /// 访问安装有 Q 系列 C24/E71 的站 QCPU 上位站 经由 Q 系列兼容网络系统 MELSECNET/H MELSECNET/10 Ethernet 的 QCPU 其他站 时
        /// 访问点数········1≦ 字访问点数 双字访问点数 ≦192
        ///
        /// 访问 QnACPU 其他站 经由 QnA 系列兼容网络系统 MELSECNET/10 Ethernet 的 Q/QnACPU 其他站 时访问点数········1≦ 字访问点数 双字访问点数 ≦96
        ///
        /// 访问上述以外的 PLC CPU 其他站 时访问点数········1≦字访问点数≦10
        /// </remarks>
        /// <returns>包含是否成功的结果对象</returns>
        public static short[] ReadRandomInt16(IReadWriteMc mc, string[] address, out bool res)
        {
            byte[] operateResult = ReadRandom(mc, address, out res);
            if (operateResult == null)
            {
                res = false;
                return null;
            }
            else
            {
                res = true;
                return mc.ByteTransform.TransInt16(operateResult, 0, address.Length);
            }
        }

        /// <summary>
        /// 随机读取PLC的数据信息，可以跨地址，跨类型组合，但是每个地址只能读取一个word，也就是2个字节的内容。收到结果后，自动转换为了ushort类型的数组<br />
        /// Randomly read PLC data information, which can be combined across addresses and types, but each address can only read one word, 
        /// which is the content of 2 bytes. After receiving the result, it is automatically converted to an array of type ushort.
        /// </summary>
        /// <param name="mc">MC协议的通信对象</param>
        /// <param name="address">所有的地址的集合</param>
        /// <remarks>
        /// 访问安装有 Q 系列 C24/E71 的站 QCPU 上位站 经由 Q 系列兼容网络系统 MELSECNET/H MELSECNET/10 Ethernet 的 QCPU 其他站 时
        /// 访问点数········1≦ 字访问点数 双字访问点数 ≦192
        ///
        /// 访问 QnACPU 其他站 经由 QnA 系列兼容网络系统 MELSECNET/10 Ethernet 的 Q/QnACPU 其他站 时访问点数········1≦ 字访问点数 双字访问点数 ≦96
        ///
        /// 访问上述以外的 PLC CPU 其他站 时访问点数········1≦字访问点数≦10
        /// </remarks>
        /// <returns>包含是否成功的结果对象</returns>
        public static ushort[] ReadRandomUInt16(IReadWriteMc mc, string[] address, out bool res)
        {
            byte[] operateResult = ReadRandom(mc, address, out res);
            if (operateResult == null)
            {
                res = false;
                return null;
            }
            else
            {
                res = true;
                return mc.ByteTransform.TransUInt16(operateResult, 0, address.Length);
            }
        }


        /// <summary>
        /// </summary>
        /// <remarks>
        /// 本指令不可以访问下述缓冲存储器:<br />
        /// 1. 本站(SLMP对应设备)上安装的智能功能模块<br />
        /// 2. 其它站缓冲存储器<br />
        /// </remarks>
        /// <param name="mc">MC通信对象</param>
        /// <param name="address">偏移地址</param>
        /// <param name="length">读取长度</param>
        /// <returns>读取的内容</returns>
        public static byte[] ReadMemory(IReadWriteMc mc, string address, ushort length, out bool res)
        {
            byte[] operateResult = mc.McType == McType.McBinary ? McBinaryHelper.BuildReadMemoryCommand(address, length) : mc.McType == McType.MCAscii ? McAsciiHelper.BuildAsciiReadMemoryCommand(address, length) : null;
            if (operateResult == null)
            {
                res = false;
                return null;
            }
            else
            {
                var operateResult2 = mc.ReadFromCoreServer(operateResult);
                if (operateResult2 == null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    res = true;
                    return mc.ExtractActualData(operateResult2, false);
                }
            }
        }



        /// <summary>
        /// 读取智能模块的数据信息，需要指定模块地址，偏移地址，读取的字节长度<br />
        ///  To read the extended data information, you need to enter the extended value information in addition to the original address and length information
        /// </summary>
        /// <param name="mc">MC通信对象</param>
        /// <param name="module">模块地址</param>
        /// <param name="address">地址</param>
        /// <param name="length">数据长度</param>
        /// <returns>返回结果</returns>
        public static byte[] ReadSmartModule(IReadWriteMc mc, ushort module, string address, ushort length, out bool res)
        {
            byte[] operateResult = mc.McType == McType.McBinary ? McBinaryHelper.BuildReadSmartModule(module, address, length) : mc.McType == McType.MCAscii ? McAsciiHelper.BuildAsciiReadSmartModule(module, address, length) : null;
            if (operateResult == null)
            {
                res = false;
                return null;
            }
            else
            {
                byte[] operateResult2 = mc.ReadFromCoreServer(operateResult);
                if (operateResult2 == null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    res = true;
                    return mc.ExtractActualData(operateResult2, false);
                }
            }
        }


        /// <summary>
        ///  读取扩展的数据信息，需要在原有的地址，长度信息之外，输入扩展值信息<br />
        ///  To read the extended data information, you need to enter the extended value information in addition to the original address and length information
        /// </summary>
        /// <param name="mc">MC通信对象</param>
        /// <param name="extend">扩展信息</param>
        /// <param name="address">地址</param>
        /// <param name="length">数据长度</param>
        /// <returns>返回结果</returns>
        // Token: 0x06000E2E RID: 3630 RVA: 0x0005A5A0 File Offset: 0x000587A0
        public static byte[] ReadExtend(IReadWriteMc mc, ushort extend, string address, ushort length, out bool res)
        {
            McAddressData operateResult = mc.McAnalysisAddress(address, length);
            if (operateResult == null)
            {
                res = false;
                return null;
            }
            else
            {
                byte[] send = mc.McType == McType.McBinary ? McBinaryHelper.BuildReadMcCoreExtendCommand(operateResult, extend, false) : mc.McType == McType.MCAscii ? McAsciiHelper.BuildAsciiReadMcCoreExtendCommand(operateResult, extend, false) : null;
                byte[] operateResult2 = mc.ReadFromCoreServer(send);
                if (operateResult2 == null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    res = true;
                    return mc.ExtractActualData(operateResult2, false);
                }
            }
        }



        ///// <summary>
        ///// 远程Run操作<br />
        ///// Remote Run Operation
        ///// </summary>
        ///// <param name="mc">MC协议通信对象</param>
        ///// <returns>是否成功</returns>
        //// Token: 0x06000E30 RID: 3632 RVA: 0x0005A6B8 File Offset: 0x000588B8
        //public static OperateResult RemoteRun(IReadWriteMc mc)
        //{
        //    return (mc.McType == McType.McBinary) ? mc.ReadFromCoreServer(new byte[]
        //    {
        //        1,
        //        16,
        //        0,
        //        0,
        //        1,
        //        0,
        //        0,
        //        0
        //    }) : ((mc.McType == McType.MCAscii) ? mc.ReadFromCoreServer(Encoding.ASCII.GetBytes("1001000000010000")) : new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction));
        //}

        ///// <summary>
        ///// 远程Stop操作<br />
        ///// Remote Stop operation
        ///// </summary>
        ///// <param name="mc">MC协议通信对象</param>
        ///// <returns>是否成功</returns>
        //// Token: 0x06000E31 RID: 3633 RVA: 0x0005A71C File Offset: 0x0005891C
        //public static OperateResult RemoteStop(IReadWriteMc mc)
        //{
        //    return (mc.McType == McType.McBinary) ? mc.ReadFromCoreServer(new byte[]
        //    {
        //        2,
        //        16,
        //        0,
        //        0,
        //        1,
        //        0
        //    }) : ((mc.McType == McType.MCAscii) ? mc.ReadFromCoreServer(Encoding.ASCII.GetBytes("100200000001")) : new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction));
        //}

        ///// <summary>
        ///// 远程Reset操作<br />
        ///// Remote Reset Operation
        ///// </summary>
        ///// <param name="mc">MC协议通信对象</param>
        ///// <returns>是否成功</returns>
        //// Token: 0x06000E32 RID: 3634 RVA: 0x0005A780 File Offset: 0x00058980
        //public static OperateResult RemoteReset(IReadWriteMc mc)
        //{
        //    return (mc.McType == McType.McBinary) ? mc.ReadFromCoreServer(new byte[]
        //    {
        //        6,
        //        16,
        //        0,
        //        0,
        //        1,
        //        0
        //    }) : ((mc.McType == McType.MCAscii) ? mc.ReadFromCoreServer(Encoding.ASCII.GetBytes("100600000001")) : new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction));
        //}

        /// <summary>
        /// 读取PLC的型号信息，例如 Q02HCPU<br />
        /// Read PLC model information, such as Q02HCPU
        /// </summary>
        /// <param name="mc">MC协议通信对象</param>
        /// <returns>返回型号的结果对象</returns>
        public static string ReadPlcType(IReadWriteMc mc, out bool res)
        {
            byte[] operateResult;
            if (mc.McType != McType.McBinary)
            {
                operateResult = mc.McType == McType.MCAscii ? mc.ReadFromCoreServer(Encoding.ASCII.GetBytes("01010000")) : Encoding.ASCII.GetBytes("NotSupportedFunction");
            }
            else
            {
                byte[] array = new byte[4];
                array[0] = 1;
                array[1] = 1;
                operateResult = mc.ReadFromCoreServer(array);
            }
            if (operateResult == null)
            {
                res = false;
                return string.Empty;
            }
            else
            {
                res = true;
                return Encoding.ASCII.GetString(operateResult, 0, 16).TrimEnd(new char[0]);
            }
        }

        ///// <summary>
        ///// LED 熄灭 出错代码初始化<br />
        ///// LED off Error code initialization
        ///// </summary>
        ///// <param name="mc">MC协议通信对象</param>
        ///// <returns>是否成功</returns>
        //// Token: 0x06000E34 RID: 3636 RVA: 0x0005A880 File Offset: 0x00058A80
        //public static OperateResult ErrorStateReset(IReadWriteMc mc)
        //{
        //    OperateResult result;
        //    if (mc.McType != McType.McBinary)
        //    {
        //        result = ((mc.McType == McType.MCAscii) ? mc.ReadFromCoreServer(Encoding.ASCII.GetBytes("16170000")) : new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction));
        //    }
        //    else
        //    {
        //        byte[] array = new byte[4];
        //        array[0] = 23;
        //        array[1] = 22;
        //        result = mc.ReadFromCoreServer(array);
        //    }
        //    return result;
        //}


    }

}
