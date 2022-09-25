using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.AllenBradleyDriver
{
    /// <summary>
    /// AB PLC的辅助类，用来辅助生成基本的指令信息
    /// </summary>
    public class AllenBradleyHelper
    {
        private static byte[] BuildRequestPathCommand(string address, bool isConnectedAddress = false)
        {
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                string[] array = address.Split(new char[]
                {
                    '.'
                }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < array.Length; i++)
                {
                    string text = string.Empty;
                    int num = array[i].IndexOf('[');
                    int num2 = array[i].IndexOf(']');
                    bool flag = num > 0 && num2 > 0 && num2 > num;
                    if (flag)
                    {
                        text = array[i].Substring(num + 1, num2 - num - 1);
                        array[i] = array[i].Substring(0, num);
                    }
                    memoryStream.WriteByte(145);
                    byte[] bytes = Encoding.UTF8.GetBytes(array[i]);
                    memoryStream.WriteByte((byte)bytes.Length);
                    memoryStream.Write(bytes, 0, bytes.Length);
                    bool flag2 = bytes.Length % 2 == 1;
                    if (flag2)
                    {
                        memoryStream.WriteByte(0);
                    }
                    bool flag3 = !string.IsNullOrEmpty(text);
                    if (flag3)
                    {
                        string[] array2 = text.Split(new char[]
                        {
                            ','
                        }, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < array2.Length; j++)
                        {
                            int num3 = Convert.ToInt32(array2[j]);
                            bool flag4 = num3 < 256 && !isConnectedAddress;
                            if (flag4)
                            {
                                memoryStream.WriteByte(40);
                                memoryStream.WriteByte((byte)num3);
                            }
                            else
                            {
                                memoryStream.WriteByte(41);
                                memoryStream.WriteByte(0);
                                memoryStream.WriteByte(BitConverter.GetBytes(num3)[0]);
                                memoryStream.WriteByte(BitConverter.GetBytes(num3)[1]);
                            }
                        }
                    }
                }
                result = memoryStream.ToArray();
            }
            return result;
        }

        /// <summary>
        /// 从生成的报文里面反解出实际的数据地址，不支持结构体嵌套，仅支持数据，一维数组，不支持多维数据
        /// </summary>
        /// <param name="pathCommand">地址路径报文</param>
        /// <returns>实际的地址</returns>
        // Token: 0x06001364 RID: 4964 RVA: 0x0007CA54 File Offset: 0x0007AC54
        public static string ParseRequestPathCommand(byte[] pathCommand)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < pathCommand.Length; i++)
            {
                bool flag = pathCommand[i] == 145;
                if (flag)
                {
                    string text = Encoding.UTF8.GetString(pathCommand, i + 2, (int)pathCommand[i + 1]).TrimEnd(new char[1]);
                    stringBuilder.Append(text);
                    int num = 2 + text.Length;
                    bool flag2 = text.Length % 2 == 1;
                    if (flag2)
                    {
                        num++;
                    }
                    bool flag3 = pathCommand.Length > num + i;
                    if (flag3)
                    {
                        bool flag4 = pathCommand[i + num] == 40;
                        if (flag4)
                        {
                            stringBuilder.Append(string.Format("[{0}]", pathCommand[i + num + 1]));
                        }
                        else
                        {
                            bool flag5 = pathCommand[i + num] == 41;
                            if (flag5)
                            {
                                stringBuilder.Append(string.Format("[{0}]", BitConverter.ToUInt16(pathCommand, i + num + 2)));
                            }
                        }
                    }
                    stringBuilder.Append(".");
                }
            }
            bool flag6 = stringBuilder[stringBuilder.Length - 1] == '.';
            if (flag6)
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 获取枚举PLC数据信息的指令
        /// </summary>
        /// <param name="startInstance">实例的起始地址</param>
        /// <returns>结果数据</returns>
        // Token: 0x06001365 RID: 4965 RVA: 0x0007CB94 File Offset: 0x0007AD94
        public static byte[] BuildEnumeratorCommand(uint startInstance)
        {
            return new byte[]
            {
                85,
                3,
                32,
                107,
                37,
                0,
                BitConverter.GetBytes(startInstance)[0],
                BitConverter.GetBytes(startInstance)[1],
                3,
                0,
                1,
                0,
                2,
                0,
                8,
                0
            };
        }

        /// <summary>
        /// 获取枚举PLC的局部变量的数据信息的指令
        /// </summary>
        /// <param name="startInstance">实例的起始地址</param>
        /// <returns>结果命令数据</returns>
        // Token: 0x06001366 RID: 4966 RVA: 0x0007CC08 File Offset: 0x0007AE08
        public static byte[] BuildEnumeratorProgrameMainCommand(uint startInstance)
        {
            byte[] array = new byte[38];
            array[0] = 85;
            array[1] = 14;
            array[2] = 145;
            array[3] = 19;
            Encoding.ASCII.GetBytes("Program:MainProgram").CopyTo(array, 4);
            array[23] = 0;
            array[24] = 32;
            array[25] = 107;
            array[26] = 37;
            array[27] = 0;
            array[28] = BitConverter.GetBytes(startInstance)[0];
            array[29] = BitConverter.GetBytes(startInstance)[1];
            array[30] = 3;
            array[31] = 0;
            array[32] = 1;
            array[33] = 0;
            array[34] = 2;
            array[35] = 0;
            array[36] = 8;
            array[37] = 0;
            return array;
        }

        /// <summary>
        /// 获取获得结构体句柄的命令
        /// </summary>
        /// <param name="symbolType">包含地址的信息</param>
        /// <returns>命令数据</returns>
        // Token: 0x06001367 RID: 4967 RVA: 0x0007CCB0 File Offset: 0x0007AEB0
        public static byte[] GetStructHandleCommand(ushort symbolType)
        {
            byte[] array = new byte[18];
            symbolType &= 4095;
            array[0] = 3;
            array[1] = 3;
            array[2] = 32;
            array[3] = 108;
            array[4] = 37;
            array[5] = 0;
            array[6] = BitConverter.GetBytes(symbolType)[0];
            array[7] = BitConverter.GetBytes(symbolType)[1];
            array[8] = 4;
            array[9] = 0;
            array[10] = 4;
            array[11] = 0;
            array[12] = 5;
            array[13] = 0;
            array[14] = 2;
            array[15] = 0;
            array[16] = 1;
            array[17] = 0;
            return array;
        }

        /// <summary>
        /// 获取结构体内部数据结构的方法
        /// </summary>
        /// <param name="symbolType">地址</param>
        /// <param name="structHandle">句柄</param>
        /// <param name="offset">偏移量地址</param>
        /// <returns>指令</returns>
        public static byte[] GetStructItemNameType(ushort symbolType, AbStructHandle structHandle, int offset)
        {
            byte[] array = new byte[14];
            symbolType &= 4095;
            byte[] bytes = BitConverter.GetBytes(structHandle.TemplateObjectDefinitionSize * 4U - 21U);
            array[0] = 76;
            array[1] = 3;
            array[2] = 32;
            array[3] = 108;
            array[4] = 37;
            array[5] = 0;
            array[6] = BitConverter.GetBytes(symbolType)[0];
            array[7] = BitConverter.GetBytes(symbolType)[1];
            array[8] = BitConverter.GetBytes(offset)[0];
            array[9] = BitConverter.GetBytes(offset)[1];
            array[10] = BitConverter.GetBytes(offset)[2];
            array[11] = BitConverter.GetBytes(offset)[3];
            array[12] = bytes[0];
            array[13] = bytes[1];
            return array;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="session"></param>
        /// <param name="commandSpecificData"></param>
        /// <param name="senderContext"></param>
        /// <returns></returns>
        public static byte[] PackRequestHeader(ushort command, uint session, byte[] commandSpecificData, byte[] senderContext = null)
        {
            bool flag = commandSpecificData == null;
            if (flag)
            {
                commandSpecificData = new byte[0];
            }
            byte[] array = new byte[commandSpecificData.Length + 24];
            Array.Copy(commandSpecificData, 0, array, 24, commandSpecificData.Length);
            BitConverter.GetBytes(command).CopyTo(array, 0);
            BitConverter.GetBytes(session).CopyTo(array, 4);
            bool flag2 = senderContext != null;
            if (flag2)
            {
                senderContext.CopyTo(array, 12);
            }
            BitConverter.GetBytes((ushort)commandSpecificData.Length).CopyTo(array, 2);
            return array;
        }

        /// <summary>
        /// 将CommandSpecificData的命令，打包成可发送的数据指令
        /// </summary>
        /// <param name="command">实际的命令暗号</param>
        /// <param name="error">错误号信息</param>
        /// <param name="session">当前会话的id</param>
        /// <param name="commandSpecificData">CommandSpecificData命令</param>
        /// <returns>最终可发送的数据命令</returns>
        // Token: 0x0600136A RID: 4970 RVA: 0x0007CE5C File Offset: 0x0007B05C
        public static byte[] PackRequestHeader(ushort command, uint error, uint session, byte[] commandSpecificData)
        {
            byte[] array = AllenBradleyHelper.PackRequestHeader(command, session, commandSpecificData, null);
            BitConverter.GetBytes(error).CopyTo(array, 8);
            return array;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pccc"></param>
        /// <returns></returns>
        private static byte[] PackExecutePCCC(byte[] pccc)
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.WriteByte(75);
            memoryStream.WriteByte(2);
            memoryStream.WriteByte(32);
            memoryStream.WriteByte(103);
            memoryStream.WriteByte(36);
            memoryStream.WriteByte(1);
            memoryStream.WriteByte(7);
            memoryStream.WriteByte(9);
            memoryStream.WriteByte(16);
            memoryStream.WriteByte(11);
            memoryStream.WriteByte(70);
            memoryStream.WriteByte(165);
            memoryStream.WriteByte(193);
            memoryStream.Write(pccc);
            byte[] array = memoryStream.ToArray();
            BitConverter.GetBytes(4105).CopyTo(array, 7);
            BitConverter.GetBytes(3248834059U).CopyTo(array, 9);
            return array;
        }

        /// <summary>
        /// 打包一个PCCC的读取的命令报文
        /// </summary>
        /// <param name="tns">请求序号信息</param>
        /// <param name="address">请求的文件地址，地址示例：N7:1</param>
        /// <param name="length">请求的字节长度</param>
        /// <returns>PCCC的读取报文信息</returns>
        public static byte[] PackExecutePCCCRead(int tns, string address, ushort length)
        {
            return BuildProtectedTypedLogicalReadWithThreeAddressFields(tns, address, length);
        }

        /// <summary>
        /// 打包一个PCCC的写入的命令报文
        /// </summary>
        /// <param name="tns">请求序号信息</param>
        /// <param name="address">请求的文件地址，地址示例：N7:1</param>
        /// <param name="value">写入的原始数据信息</param>
        /// <returns>PCCC的写入报文信息</returns>
        public static byte[] PackExecutePCCCWrite(int tns, string address, byte[] value)
        {
            return BuildProtectedTypedLogicalWriteWithThreeAddressFields(tns, address, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tns"></param>
        /// <param name="address"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static byte[] PackExecutePCCCWrite(int tns, string address, int bitIndex, bool value)
        {
            return BuildProtectedTypedLogicalMaskWithThreeAddressFields(tns, address, bitIndex, value);
        }

        private static void AddLengthToMemoryStream(MemoryStream ms, ushort value)
        {
            bool flag = value < 255;
            if (flag)
            {
                ms.WriteByte((byte)value);
            }
            else
            {
                ms.WriteByte(byte.MaxValue);
                ms.WriteByte(BitConverter.GetBytes(value)[0]);
                ms.WriteByte(BitConverter.GetBytes(value)[1]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tns"></param>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] BuildProtectedTypedLogicalReadWithThreeAddressFields(int tns, string address, ushort length)
        {
            AllenBradleySLCAddress operateResult = AllenBradleySLCAddress.ParseFrom(address,out bool res);
            if (!res || operateResult==null)
            {
                return null;
            }
            else
            {
                MemoryStream memoryStream = new MemoryStream();
                memoryStream.WriteByte(15);
                memoryStream.WriteByte(0);
                memoryStream.WriteByte(BitConverter.GetBytes(tns)[0]);
                memoryStream.WriteByte(BitConverter.GetBytes(tns)[1]);
                memoryStream.WriteByte(162);
                memoryStream.WriteByte(BitConverter.GetBytes(length)[0]);
                AddLengthToMemoryStream(memoryStream, operateResult.DbBlock);
                memoryStream.WriteByte(operateResult.DataCode);
                AddLengthToMemoryStream(memoryStream, (ushort)operateResult.AddressStart);
                AddLengthToMemoryStream(memoryStream, 0);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// 构建0F-A2命令码的报文读取指令，用来读取文件数据。适用 Micro-Logix1000,SLC500,SLC 5/03,SLC 5/04, PLC-5，地址示例：N7:1<br />
        /// Construct a message read instruction of 0F-A2 command code to read file data. Applicable to Micro-Logix1000, SLC500, SLC 5/03, SLC 5/04, PLC-5, address example: N7:1
        /// </summary>
        /// <param name="dstNode">目标节点号</param>
        /// <param name="srcNode">原节点号</param>
        /// <param name="tns">消息号</param>
        /// <param name="address">PLC的地址信息</param>
        /// <param name="length">读取的数据长度</param>
        /// <returns>初步的报文信息</returns>
        /// <remarks>
        /// 对于SLC 5/01或SLC 5/02而言，一次最多读取82个字节。对于 03 或是 04 为225，236字节取决于是否应用DF1驱动
        /// </remarks>
        // Token: 0x0600135B RID: 4955 RVA: 0x0007C008 File Offset: 0x0007A208
        public static byte[] BuildProtectedTypedLogicalReadWithThreeAddressFields(byte dstNode, byte srcNode, int tns, string address, ushort length)
        {
            byte[] operateResult = BuildProtectedTypedLogicalReadWithThreeAddressFields(tns, address, length);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
               return (DataExtend.SpliceArray<byte>(new byte[][]
                {
                    new byte[]
                    {
                        dstNode,
                        srcNode
                    },
                    operateResult
                }));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tns"></param>
        /// <param name="address"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] BuildProtectedTypedLogicalWriteWithThreeAddressFields(int tns, string address, byte[] data)
        {
            AllenBradleySLCAddress operateResult = AllenBradleySLCAddress.ParseFrom(address,out bool res);
            if (!res || operateResult==null)
            {
                return null;
            }
            else
            {
                MemoryStream memoryStream = new MemoryStream();
                memoryStream.WriteByte(15);
                memoryStream.WriteByte(0);
                memoryStream.WriteByte(BitConverter.GetBytes(tns)[0]);
                memoryStream.WriteByte(BitConverter.GetBytes(tns)[1]);
                memoryStream.WriteByte(170);
                memoryStream.WriteByte(BitConverter.GetBytes(data.Length)[0]);
                AddLengthToMemoryStream(memoryStream, operateResult.DbBlock);
                memoryStream.WriteByte(operateResult.DataCode);
                AddLengthToMemoryStream(memoryStream, (ushort)operateResult.AddressStart);
                AddLengthToMemoryStream(memoryStream, 0);
                memoryStream.Write(data);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// 构建0F-AB的掩码写入的功能
        /// </summary>
        /// <param name="tns">消息号</param>
        /// <param name="address">PLC的地址信息</param>
        /// <param name="bitIndex">位索引信息</param>
        /// <param name="value">通断值</param>
        /// <returns>命令报文</returns>
        public static byte[] BuildProtectedTypedLogicalMaskWithThreeAddressFields(int tns, string address, int bitIndex, bool value)
        {
            int value2 = 1 << bitIndex;
            AllenBradleySLCAddress operateResult = AllenBradleySLCAddress.ParseFrom(address, out bool res);
            if (!res || operateResult==null)
            {
                return null;
            }
            else
            {
                MemoryStream memoryStream = new MemoryStream();
                memoryStream.WriteByte(15);
                memoryStream.WriteByte(0);
                memoryStream.WriteByte(BitConverter.GetBytes(tns)[0]);
                memoryStream.WriteByte(BitConverter.GetBytes(tns)[1]);
                memoryStream.WriteByte(171);
                memoryStream.WriteByte(2);
                AddLengthToMemoryStream(memoryStream, operateResult.DbBlock);
                memoryStream.WriteByte(operateResult.DataCode);
                AddLengthToMemoryStream(memoryStream, (ushort)operateResult.AddressStart);
                AddLengthToMemoryStream(memoryStream, 0);
                memoryStream.WriteByte(BitConverter.GetBytes(value2)[0]);
                memoryStream.WriteByte(BitConverter.GetBytes(value2)[1]);
                if (value)
                {
                    memoryStream.WriteByte(BitConverter.GetBytes(value2)[0]);
                    memoryStream.WriteByte(BitConverter.GetBytes(value2)[1]);
                }
                else
                {
                    memoryStream.WriteByte(0);
                    memoryStream.WriteByte(0);
                }
              return (memoryStream.ToArray());
            }
        }

        /// <summary>
        /// 构建0F-AA命令码的写入读取指令，用来写入文件数据。适用 Micro-Logix1000,SLC500,SLC 5/03,SLC 5/04, PLC-5，地址示例：N7:1<br />
        /// Construct a write and read command of 0F-AA command code to write file data. Applicable to Micro-Logix1000, SLC500, SLC 5/03, SLC 5/04, PLC-5, address example: N7:1
        /// </summary>
        /// <param name="dstNode">目标节点号</param>
        /// <param name="srcNode">原节点号</param>
        /// <param name="tns">消息号</param>
        /// <param name="address">PLC的地址信息</param>
        /// <param name="data">写入的数据内容</param>
        /// <returns>初步的报文信息</returns>
        /// <remarks>
        /// 对于SLC 5/01或SLC 5/02而言，一次最多读取82个字节。对于 03 或是 04 为225，236字节取决于是否应用DF1驱动
        /// </remarks>
        public static byte[] BuildProtectedTypedLogicalWriteWithThreeAddressFields(byte dstNode, byte srcNode, int tns, string address, byte[] data)
        {
            byte[] operateResult = BuildProtectedTypedLogicalWriteWithThreeAddressFields(tns, address, data);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                return (DataExtend.SpliceArray<byte>(new byte[][]
                {
                    new byte[]
                    {
                        dstNode,
                        srcNode
                    },
                    operateResult
                }));
            }
        }
        /// <summary>
        /// 打包生成一个请求读取数据的节点信息，CIP指令信息
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="length">指代数组的长度</param>
        /// <param name="isConnectedAddress">是否是连接模式下的地址，默认为false</param>
        /// <returns>CIP的指令信息</returns>
        // Token: 0x0600136F RID: 4975 RVA: 0x0007D008 File Offset: 0x0007B208
        public static byte[] PackRequsetRead(string address, int length, bool isConnectedAddress = false)
        {
            byte[] array = new byte[1024];
            int num = 0;
            array[num++] = 76;
            num++;
            byte[] array2 = AllenBradleyHelper.BuildRequestPathCommand(address, isConnectedAddress);
            array2.CopyTo(array, num);
            num += array2.Length;
            array[1] = (byte)((num - 2) / 2);
            array[num++] = BitConverter.GetBytes(length)[0];
            array[num++] = BitConverter.GetBytes(length)[1];
            byte[] array3 = new byte[num];
            Array.Copy(array, 0, array3, 0, num);
            return array3;
        }

        /// <summary>
        /// 打包生成一个请求读取数据片段的节点信息，CIP指令信息
        /// </summary>
        /// <param name="address">节点的名称 -&gt; Tag Name</param>
        /// <param name="startIndex">起始的索引位置，以字节为单位 -&gt; The initial index position, in bytes</param>
        /// <param name="length">读取的数据长度，一次通讯总计490个字节 -&gt; Length of read data, a total of 490 bytes of communication</param>
        /// <returns>CIP的指令信息</returns>
        // Token: 0x06001370 RID: 4976 RVA: 0x0007D088 File Offset: 0x0007B288
        public static byte[] PackRequestReadSegment(string address, int startIndex, int length)
        {
            byte[] array = new byte[1024];
            int num = 0;
            array[num++] = 82;
            num++;
            byte[] array2 = AllenBradleyHelper.BuildRequestPathCommand(address, false);
            array2.CopyTo(array, num);
            num += array2.Length;
            array[1] = (byte)((num - 2) / 2);
            array[num++] = BitConverter.GetBytes(length)[0];
            array[num++] = BitConverter.GetBytes(length)[1];
            array[num++] = BitConverter.GetBytes(startIndex)[0];
            array[num++] = BitConverter.GetBytes(startIndex)[1];
            array[num++] = BitConverter.GetBytes(startIndex)[2];
            array[num++] = BitConverter.GetBytes(startIndex)[3];
            byte[] array3 = new byte[num];
            Array.Copy(array, 0, array3, 0, num);
            return array3;
        }

        /// <summary>
        /// 根据指定的数据和类型，生成对应的数据
        /// </summary>
        /// <param name="address">地址信息</param>
        /// <param name="typeCode">数据类型</param>
        /// <param name="value">字节值</param>
        /// <param name="length">如果节点为数组，就是数组长度</param>
        /// <param name="isConnectedAddress">是否为连接模式的地址</param>
        /// <returns>CIP的指令信息</returns>
        // Token: 0x06001371 RID: 4977 RVA: 0x0007D144 File Offset: 0x0007B344
        public static byte[] PackRequestWrite(string address, ushort typeCode, byte[] value, int length = 1, bool isConnectedAddress = false)
        {
            byte[] array = new byte[1024];
            int num = 0;
            array[num++] = 77;
            num++;
            byte[] array2 = AllenBradleyHelper.BuildRequestPathCommand(address, isConnectedAddress);
            array2.CopyTo(array, num);
            num += array2.Length;
            array[1] = (byte)((num - 2) / 2);
            array[num++] = BitConverter.GetBytes(typeCode)[0];
            array[num++] = BitConverter.GetBytes(typeCode)[1];
            array[num++] = BitConverter.GetBytes(length)[0];
            array[num++] = BitConverter.GetBytes(length)[1];
            value.CopyTo(array, num);
            num += value.Length;
            byte[] array3 = new byte[num];
            Array.Copy(array, 0, array3, 0, num);
            return array3;
        }

        /// <summary>
        /// 分析地址数据信息里的位索引的信息，例如a[10]  返回 a 和 10 索引，如果没有指定索引，就默认为0
        /// </summary>
        /// <param name="address">数据地址</param>
        /// <param name="arrayIndex">位索引</param>
        /// <returns>地址信息</returns>
        public static string AnalysisArrayIndex(string address, out int arrayIndex)
        {
            arrayIndex = 0;
            bool flag = !address.EndsWith("]");
            string result;
            if (flag)
            {
                result = address;
            }
            else
            {
                int num = address.LastIndexOf('[');
                bool flag2 = num < 0;
                if (flag2)
                {
                    result = address;
                }
                else
                {
                    address = address.Remove(address.Length - 1);
                    arrayIndex = int.Parse(address.Substring(num + 1));
                    address = address.Substring(0, num);
                    result = address;
                }
            }
            return result;
        }

        /// <summary>
        /// 写入Bool数据的基本指令信息
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        /// <returns>报文信息</returns>
        public static byte[] PackRequestWrite(string address, bool value)
        {
            int num;
            address = AllenBradleyHelper.AnalysisArrayIndex(address, out num);
            address = address + "[" + (num / 32).ToString() + "]";
            int value2 = 0;
            int value3 = -1;
            if (value)
            {
                value2 = 1 << num;
            }
            else
            {
                value3 = ~(1 << num);
            }
            byte[] array = new byte[1024];
            int num2 = 0;
            array[num2++] = 78;
            num2++;
            byte[] array2 = AllenBradleyHelper.BuildRequestPathCommand(address, false);
            array2.CopyTo(array, num2);
            num2 += array2.Length;
            array[1] = (byte)((num2 - 2) / 2);
            array[num2++] = 4;
            array[num2++] = 0;
            BitConverter.GetBytes(value2).CopyTo(array, num2);
            num2 += 4;
            BitConverter.GetBytes(value3).CopyTo(array, num2);
            num2 += 4;
            byte[] array3 = new byte[num2];
            Array.Copy(array, 0, array3, 0, num2);
            return array3;
        }

        /// <summary>
        /// 将所有的cip指定进行打包操作。
        /// </summary>
        /// <param name="portSlot">PLC所在的面板槽号</param>
        /// <param name="cips">所有的cip打包指令信息</param>
        /// <returns>包含服务的信息</returns>
        public static byte[] PackCommandService(byte[] portSlot, params byte[][] cips)
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.WriteByte(178);
            memoryStream.WriteByte(0);
            memoryStream.WriteByte(0);
            memoryStream.WriteByte(0);
            memoryStream.WriteByte(82);
            memoryStream.WriteByte(2);
            memoryStream.WriteByte(32);
            memoryStream.WriteByte(6);
            memoryStream.WriteByte(36);
            memoryStream.WriteByte(1);
            memoryStream.WriteByte(10);
            memoryStream.WriteByte(240);
            memoryStream.WriteByte(0);
            memoryStream.WriteByte(0);
            int num = 0;
            bool flag = cips.Length == 1;
            if (flag)
            {
                memoryStream.Write(cips[0], 0, cips[0].Length);
                num += cips[0].Length;
            }
            else
            {
                memoryStream.WriteByte(10);
                memoryStream.WriteByte(2);
                memoryStream.WriteByte(32);
                memoryStream.WriteByte(2);
                memoryStream.WriteByte(36);
                memoryStream.WriteByte(1);
                num += 8;
                memoryStream.Write(BitConverter.GetBytes((ushort)cips.Length), 0, 2);
                ushort num2 = (ushort)(2 + 2 * cips.Length);
                num += 2 * cips.Length;
                for (int i = 0; i < cips.Length; i++)
                {
                    memoryStream.Write(BitConverter.GetBytes(num2), 0, 2);
                    num2 = (ushort)((int)num2 + cips[i].Length);
                }
                for (int j = 0; j < cips.Length; j++)
                {
                    memoryStream.Write(cips[j], 0, cips[j].Length);
                    num += cips[j].Length;
                }
            }
            bool flag2 = portSlot != null;
            if (flag2)
            {
                memoryStream.WriteByte((byte)((portSlot.Length + 1) / 2));
                memoryStream.WriteByte(0);
                memoryStream.Write(portSlot, 0, portSlot.Length);
                bool flag3 = portSlot.Length % 2 == 1;
                if (flag3)
                {
                    memoryStream.WriteByte(0);
                }
            }
            byte[] array = memoryStream.ToArray();
            BitConverter.GetBytes((short)num).CopyTo(array, 12);
            BitConverter.GetBytes((short)(array.Length - 4)).CopyTo(array, 2);
            return array;
        }

        /// <summary>
        /// 将所有的cip指定进行打包操作。
        /// </summary>
        /// <param name="portSlot">PLC所在的面板槽号</param>
        /// <param name="cips">所有的cip打包指令信息</param>
        /// <returns>包含服务的信息</returns>
        public static byte[] PackCleanCommandService(byte[] portSlot, params byte[][] cips)
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.WriteByte(178);
            memoryStream.WriteByte(0);
            memoryStream.WriteByte(0);
            memoryStream.WriteByte(0);
            bool flag = cips.Length == 1;
            if (flag)
            {
                memoryStream.Write(cips[0], 0, cips[0].Length);
            }
            else
            {
                memoryStream.WriteByte(10);
                memoryStream.WriteByte(2);
                memoryStream.WriteByte(32);
                memoryStream.WriteByte(2);
                memoryStream.WriteByte(36);
                memoryStream.WriteByte(1);
                memoryStream.Write(BitConverter.GetBytes((ushort)cips.Length), 0, 2);
                ushort num = (ushort)(2 + 2 * cips.Length);
                for (int i = 0; i < cips.Length; i++)
                {
                    memoryStream.Write(BitConverter.GetBytes(num), 0, 2);
                    num = (ushort)((int)num + cips[i].Length);
                }
                for (int j = 0; j < cips.Length; j++)
                {
                    memoryStream.Write(cips[j], 0, cips[j].Length);
                }
            }
            memoryStream.WriteByte((byte)((portSlot.Length + 1) / 2));
            memoryStream.WriteByte(0);
            memoryStream.Write(portSlot, 0, portSlot.Length);
            bool flag2 = portSlot.Length % 2 == 1;
            if (flag2)
            {
                memoryStream.WriteByte(0);
            }
            byte[] array = memoryStream.ToArray();
            BitConverter.GetBytes((short)(array.Length - 4)).CopyTo(array, 2);
            return array;
        }

        /// <summary>
        /// 打包一个读取所有特性数据的报文信息，需要传入slot
        /// </summary>
        /// <param name="portSlot">站号信息</param>
        /// <param name="sessionHandle">会话的ID信息</param>
        /// <returns>最终发送的报文数据</returns>
        // Token: 0x06001376 RID: 4982 RVA: 0x0007D6B8 File Offset: 0x0007B8B8
        public static byte[] PackCommandGetAttributesAll(byte[] portSlot, uint sessionHandle)
        {
            byte[] commandSpecificData = AllenBradleyHelper.PackCommandSpecificData(new byte[][]
            {
                new byte[4],
                AllenBradleyHelper.PackCommandService(portSlot, new byte[][]
                {
                    new byte[]
                    {
                        1,
                        2,
                        32,
                        1,
                        36,
                        1
                    }
                })
            });
            return AllenBradleyHelper.PackRequestHeader(111, sessionHandle, commandSpecificData, null);
        }

        /// <summary>
        /// 根据数据创建反馈的数据信息
        /// </summary>
        /// <param name="data">反馈的数据信息</param>
        /// <param name="isRead">是否是读取</param>
        /// <returns>数据</returns>
        public static byte[] PackCommandResponse(byte[] data, bool isRead)
        {
            bool flag = data == null;
            byte[] result;
            if (flag)
            {
                byte[] array = new byte[6];
                array[2] = 4;
                result = array;
            }
            else
            {
                byte[][] array2 = new byte[2][];
                int num = 0;
                byte[] array3 = new byte[6];
                array3[0] = (byte)(isRead ? 204 : 205);
                array2[num] = array3;
                array2[1] = data;
                result = DataExtend.SpliceArray<byte>(array2);
            }
            return result;
        }

        /// <summary>
        /// 生成读取直接节点数据信息的内容
        /// </summary>
        /// <param name="service">cip指令内容</param>
        /// <returns>最终的指令值</returns>
        public static byte[] PackCommandSpecificData(params byte[][] service)
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.WriteByte(0);
            memoryStream.WriteByte(0);
            memoryStream.WriteByte(0);
            memoryStream.WriteByte(0);
            memoryStream.WriteByte(10);
            memoryStream.WriteByte(0);
            memoryStream.WriteByte(BitConverter.GetBytes(service.Length)[0]);
            memoryStream.WriteByte(BitConverter.GetBytes(service.Length)[1]);
            for (int i = 0; i < service.Length; i++)
            {
                memoryStream.Write(service[i], 0, service[i].Length);
            }
            return memoryStream.ToArray();
        }

        /// <summary>
        /// 将所有的cip指定进行打包操作。
        /// </summary>
        /// <param name="command">指令信息</param>
        /// <param name="code">服务器的代号信息</param>
        /// <param name="isConnected">是否基于连接的服务</param>
        /// <returns>包含服务的信息</returns>
        // Token: 0x06001379 RID: 4985 RVA: 0x0007D7F8 File Offset: 0x0007B9F8
        public static byte[] PackCommandSingleService(byte[] command, ushort code = 178, bool isConnected = false)
        {
            bool flag = command == null;
            if (flag)
            {
                command = new byte[0];
            }
            byte[] array = isConnected ? new byte[6 + command.Length] : new byte[4 + command.Length];
            array[0] = BitConverter.GetBytes(code)[0];
            array[1] = BitConverter.GetBytes(code)[1];
            array[2] = BitConverter.GetBytes(array.Length - 4)[0];
            array[3] = BitConverter.GetBytes(array.Length - 4)[1];
            command.CopyTo(array, isConnected ? 6 : 4);
            return array;
        }

        /// <summary>
        /// 向PLC注册会话ID的报文<br />
        /// Register a message with the PLC for the session ID
        /// </summary>
        /// <param name="senderContext">发送的上下文信息</param>
        /// <returns>报文信息 -&gt; Message information </returns>
        // Token: 0x0600137A RID: 4986 RVA: 0x0007D878 File Offset: 0x0007BA78
        public static byte[] RegisterSessionHandle(byte[] senderContext = null)
        {
            byte[] array = new byte[4];
            array[0] = 1;
            byte[] commandSpecificData = array;
            return AllenBradleyHelper.PackRequestHeader(101, 0U, commandSpecificData, senderContext);
        }

        /// <summary>
        /// 获取卸载一个已注册的会话的报文<br />
        /// Get a message to uninstall a registered session
        /// </summary>
        /// <param name="sessionHandle">当前会话的ID信息</param>
        /// <returns>字节报文信息 -&gt; BYTE message information </returns>
        // Token: 0x0600137B RID: 4987 RVA: 0x0007D8A0 File Offset: 0x0007BAA0
        public static byte[] UnRegisterSessionHandle(uint sessionHandle)
        {
            return AllenBradleyHelper.PackRequestHeader(102, sessionHandle, new byte[0], null);
        }

        /// <summary>
        /// 初步检查返回的CIP协议的报文是否正确<br />
        /// Initially check whether the returned CIP protocol message is correct
        /// </summary>
        /// <param name="response">CIP的报文信息</param>
        /// <returns>是否正确的结果信息</returns>
        public static bool CheckResponse(byte[] response)
        {
            int num = BitConverter.ToInt32(response, 8);
            if (num == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 从PLC反馈的数据解析，返回解析后的数据内容，数据类型（在多项数据返回中无效），以及是否有更多的数据
        /// </summary>
        /// <param name="response">PLC的反馈数据</param>
        /// <param name="isRead">是否是返回的操作</param>
        /// <returns>带有结果标识的最终数据</returns>
        public static Tuple<byte[], ushort, bool> ExtractActualData(byte[] response, bool isRead)
        {
            List<byte> list = new List<byte>();
            int num = 38;
            bool value = false;
            ushort value2 = 0;
            ushort num2 = BitConverter.ToUInt16(response, 38);
            bool flag = BitConverter.ToInt32(response, 40) == 138;
            if (!flag)
            {
                byte b = response[num + 4];
                byte b2 = b;
                byte b3 = b2;
                if (b3 <= 10)
                {
                    switch (b3)
                    {
                        case 0:
                            break;
                        default:
                            return null;
                    }
                    bool flag2 = response[num + 2] == 205 || response[num + 2] == 211;
                    if (flag2)
                    {
                        return new Tuple<byte[], ushort, bool>(list.ToArray(), value2, value);
                    }
                    bool flag3 = response[num + 2] == 204 || response[num + 2] == 210;
                    if (flag3)
                    {
                        for (int i = num + 8; i < num + 2 + (int)num2; i++)
                        {
                            list.Add(response[i]);
                        }
                        value2 = BitConverter.ToUInt16(response, num + 6);
                    }
                    else
                    {
                        bool flag4 = response[num + 2] == 213;
                        if (flag4)
                        {
                            for (int j = num + 6; j < num + 2 + (int)num2; j++)
                            {
                                list.Add(response[j]);
                            }
                        }
                    }
                    return new Tuple<byte[], ushort, bool>(list.ToArray(), value2, value);
                }
                else
                {
                    if (b3 == 19)
                    {
                        return null;
                    }
                    switch (b3)
                    {
                        case 28:
                            return null;
                        case 29:
                        case 31:
                            break;
                        case 30:
                        case 32:
                            return null;
                        default:
                            if (b3 == 38)
                            {
                                return null;
                            }
                            break;
                    }
                }

            }
            num = 44;
            int num3 = (int)BitConverter.ToUInt16(response, num);
            int k = 0;
            while (k < num3)
            {
                int num4 = (int)BitConverter.ToUInt16(response, num + 2 + k * 2) + num;
                int num5 = (k == num3 - 1) ? response.Length : ((int)BitConverter.ToUInt16(response, num + 4 + k * 2) + num);
                ushort num6 = BitConverter.ToUInt16(response, num4 + 2);
                ushort num7 = num6;
                ushort num8 = num7;
                if (num8 <= 19)
                {
                    switch (num8)
                    {
                        case 0:
                            break;
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                            return null;
                        case 6:
                            if (response[num + 2] == 210 || response[num + 2] == 204)
                            {
                                return null;
                            }
                            break;
                        default:
                            return null;
                    }
                    if (isRead)
                    {
                        for (int l = num4 + 6; l < num5; l++)
                        {
                            list.Add(response[l]);
                        }
                    }
                    k++;
                    continue;
                }
                if (num8 == 28 || num8 == 30 || num8 == 38)
                {
                    return null;
                }
                //if (num8 == 30)
                //{
                //    return new OperateResult<byte[], ushort, bool>
                //    {
                //        ErrorCode = (int)num6,
                //        Message = StringResources.Language.AllenBradley1E
                //    };
                //}
                //if (num8 == 38)
                //{
                //    return new OperateResult<byte[], ushort, bool>
                //    {
                //        ErrorCode = (int)num6,
                //        Message = StringResources.Language.AllenBradley26
                //    };
                //}
            //IL_24C:
            //    return new OperateResult<byte[], ushort, bool>
            //    {
            //        ErrorCode = (int)num6,
            //        Message = StringResources.Language.UnknownError
            //    };
            }
            //IL_56C:
            return new Tuple<byte[], ushort, bool>(list.ToArray(), value2, value);
        }

        /// <summary>
        /// 从PLC里读取当前PLC的型号信息<br />
        /// Read the current PLC model information from the PLC
        /// </summary>
        /// <param name="plc">PLC对象</param>
        /// <returns>型号数据信息</returns>
        public static string ReadPlcType(NetworkDeviceProxyBase plc,out bool res)
        {
            byte[] send = "00 00 00 00 00 00 02 00 00 00 00 00 b2 00 06 00 01 02 20 01 24 01".ToHexBytes();
            byte[] operateResult = plc.ReadFromCoreServer(send);
            if (operateResult==null)
            {
                res = false;
                return String.Empty;
            }
            else
            {
                bool flag2 = operateResult.Length > 59;
                if (flag2)
                {
                    res = true;
                    return Encoding.UTF8.GetString(operateResult, 59, (int)operateResult[58]);
                }
                else
                {
                    res = false;
                    return String.Empty;
                }
            }
        }


        /// <summary>
        /// 读取指定地址的日期数据，最小日期为 1970年1月1日，当PLC的变量类型为 "Date" 和 "TimeAndDate" 时，都可以用本方法读取。<br />
        /// Read the date data of the specified address. The minimum date is January 1, 1970. When the PLC variable type is "Date" and "TimeAndDate", this method can be used to read.
        /// </summary>
        /// <param name="plc">当前的通信对象信息</param>
        /// <param name="address">PLC里变量的地址</param>
        /// <returns>日期结果对象</returns>
        public static DateTime ReadDate(NetworkDeviceProxyBase plc, string address,out bool res)
        {
            long operateResult = plc.ReadInt64(address,out res);
            if (!res)
            {
                return DateTime.MinValue;
            }
            else
            {
                long value = operateResult / 100L;
                DateTime dateTime = new DateTime(1970, 1, 1);
                return dateTime.AddTicks(value);
            }
        }

        /// <summary>
        /// 使用日期格式（Date）将指定的数据写入到指定的地址里，PLC的地址类型变量必须为 "Date"，否则写入失败。<br />
        /// Use the date format (Date) to write the specified data to the specified address. The PLC address type variable must be "Date", otherwise the writing will fail.
        /// </summary>
        /// <param name="plc">当前的通信对象信息</param>
        /// <param name="address">PLC里变量的地址</param>
        /// <param name="date">时间信息</param>
        /// <returns>是否写入成功</returns>
        public static object WriteDate(AllenBradleyCIPNetProxy plc, string address, DateTime date,out bool res)
        {
            long value = (date.Date - new DateTime(1970, 1, 1)).Ticks * 100L;
            return plc.WriteTag(address, 8, plc.ByteTransform.TransByte(value), 1,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plc"></param>
        /// <param name="address"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static object WriteTimeAndDate(AllenBradleyCIPNetProxy plc, string address, DateTime date, out bool res)
        {
            long value = (date - new DateTime(1970, 1, 1)).Ticks * 100L;
            return plc.WriteTag(address, 10, plc.ByteTransform.TransByte(value), 1,out res);
        }

        /// <summary>
        /// 读取指定地址的时间数据，最小时间为 0，如果获取秒，可以访问 <see cref="P:System.TimeSpan.TotalSeconds" />，当PLC的变量类型为 "Time" 和 "TimeOfDate" 时，都可以用本方法读取。<br />
        /// Read the time data of the specified address. The minimum time is 0. If you get seconds, you can access <see cref="P:System.TimeSpan.TotalSeconds" />. 
        /// When the PLC variable type is "Time" and "TimeOfDate", you can use this Method to read.
        /// </summary>
        /// <param name="plc">当前的通信对象信息</param>
        /// <param name="address">PLC里变量的地址</param>
        /// <returns>时间的结果对象</returns>
        public static TimeSpan ReadTime(NetworkDeviceProxyBase plc, string address, out bool res)
        {
            long operateResult = plc.ReadInt64(address,out res);
            if (!res)
            {
                return new TimeSpan();
            }
            else
            {
                long value = operateResult / 100L;
                return TimeSpan.FromTicks(value);
            }
        }

        /// <summary>
        /// 使用时间格式（TIME）将时间数据写入到PLC中指定的地址里去，PLC的地址类型变量必须为 "TIME"，否则写入失败。<br />
        /// Use the time format (TIME) to write the time data to the address specified in the PLC. The PLC address type variable must be "TIME", otherwise the writing will fail.
        /// </summary>
        /// <param name="plc">当前的通信对象信息</param>
        /// <param name="address">PLC里变量的地址</param>
        /// <param name="time">时间参数变量</param>
        /// <returns>是否写入成功</returns>
        // Token: 0x06001384 RID: 4996 RVA: 0x0007E14C File Offset: 0x0007C34C
        public static object WriteTime(AllenBradleyCIPNetProxy plc, string address, TimeSpan time,out bool res)
        {
            return plc.WriteTag(address, 9, plc.ByteTransform.TransByte(time.Ticks * 100L), 1,out res);
        }

        /// <summary>
        /// 使用时间格式（TimeOfDate）将时间数据写入到PLC中指定的地址里去，PLC的地址类型变量必须为 "TimeOfDate"，否则写入失败。<br />
        /// Use the time format (TimeOfDate) to write the time data to the address specified in the PLC. The PLC address type variable must be "TimeOfDate", otherwise the writing will fail.
        /// </summary>
        /// <param name="plc">当前的通信对象信息</param>
        /// <param name="address">PLC里变量的地址</param>
        /// <param name="timeOfDate">时间参数变量</param>
        /// <returns>是否写入成功</returns>
        public static object WriteTimeOfDate(AllenBradleyCIPNetProxy plc, string address, TimeSpan timeOfDate, out bool res)
        {
            return plc.WriteTag(address, 11, plc.ByteTransform.TransByte(timeOfDate.Ticks * 100L), 1,out res);
        }


        /// <summary>
        /// CIP命令中PCCC命令相关的信息
        /// </summary>
        // Token: 0x040004C7 RID: 1223
        public const byte CIP_Execute_PCCC = 75;

        /// <summary>
        /// CIP命令中的读取数据的服务
        /// </summary>
        // Token: 0x040004C8 RID: 1224
        public const byte CIP_READ_DATA = 76;

        /// <summary>
        /// CIP命令中的写数据的服务
        /// </summary>
        // Token: 0x040004C9 RID: 1225
        public const int CIP_WRITE_DATA = 77;

        /// <summary>
        /// CIP命令中的读并写的数据服务
        /// </summary>
        // Token: 0x040004CA RID: 1226
        public const int CIP_READ_WRITE_DATA = 78;

        /// <summary>
        /// CIP命令中的读片段的数据服务
        /// </summary>
        // Token: 0x040004CB RID: 1227
        public const int CIP_READ_FRAGMENT = 82;

        /// <summary>
        /// CIP命令中的写片段的数据服务
        /// </summary>
        // Token: 0x040004CC RID: 1228
        public const int CIP_WRITE_FRAGMENT = 83;

        /// <summary>
        /// CIP指令中读取数据的列表
        /// </summary>
        // Token: 0x040004CD RID: 1229
        public const byte CIP_READ_LIST = 85;

        /// <summary>
        /// CIP命令中的对数据读取服务
        /// </summary>
        // Token: 0x040004CE RID: 1230
        public const int CIP_MULTIREAD_DATA = 4096;

        /// <summary>
        /// 日期的格式
        /// </summary>
        // Token: 0x040004CF RID: 1231
        public const ushort CIP_Type_DATE = 8;

        /// <summary>
        /// 时间的格式
        /// </summary>
        // Token: 0x040004D0 RID: 1232
        public const ushort CIP_Type_TIME = 9;

        /// <summary>
        /// 日期时间格式，最完整的时间格式
        /// </summary>
        // Token: 0x040004D1 RID: 1233
        public const ushort CIP_Type_TimeAndDate = 10;

        /// <summary>
        /// 一天中的时间格式
        /// </summary>
        // Token: 0x040004D2 RID: 1234
        public const ushort CIP_Type_TimeOfDate = 11;

        /// <summary>
        /// bool型数据，一个字节长度
        /// </summary>
        // Token: 0x040004D3 RID: 1235
        public const ushort CIP_Type_Bool = 193;

        /// <summary>
        /// byte型数据，一个字节长度，SINT
        /// </summary>
        // Token: 0x040004D4 RID: 1236
        public const ushort CIP_Type_Byte = 194;

        /// <summary>
        /// 整型，两个字节长度，INT
        /// </summary>
        // Token: 0x040004D5 RID: 1237
        public const ushort CIP_Type_Word = 195;

        /// <summary>
        /// 长整型，四个字节长度，DINT
        /// </summary>
        // Token: 0x040004D6 RID: 1238
        public const ushort CIP_Type_DWord = 196;

        /// <summary>
        /// 特长整型，8个字节，LINT
        /// </summary>
        // Token: 0x040004D7 RID: 1239
        public const ushort CIP_Type_LInt = 197;

        /// <summary>
        /// Unsigned 8-bit integer, USINT
        /// </summary>
        // Token: 0x040004D8 RID: 1240
        public const ushort CIP_Type_USInt = 198;

        /// <summary>
        /// Unsigned 16-bit integer, UINT
        /// </summary>
        // Token: 0x040004D9 RID: 1241
        public const ushort CIP_Type_UInt = 199;

        /// <summary>
        ///  Unsigned 32-bit integer, UDINT
        /// </summary>
        // Token: 0x040004DA RID: 1242
        public const ushort CIP_Type_UDint = 200;

        /// <summary>
        ///  Unsigned 64-bit integer, ULINT
        /// </summary>
        // Token: 0x040004DB RID: 1243
        public const ushort CIP_Type_ULint = 201;

        /// <summary>
        /// 实数数据，四个字节长度
        /// </summary>
        // Token: 0x040004DC RID: 1244
        public const ushort CIP_Type_Real = 202;

        /// <summary>
        /// 实数数据，八个字节的长度
        /// </summary>
        // Token: 0x040004DD RID: 1245
        public const ushort CIP_Type_Double = 203;

        /// <summary>
        /// 结构体数据，不定长度
        /// </summary>
        // Token: 0x040004DE RID: 1246
        public const ushort CIP_Type_Struct = 204;

        /// <summary>
        /// 字符串数据内容
        /// </summary>
        // Token: 0x040004DF RID: 1247
        public const ushort CIP_Type_String = 208;

        /// <summary>
        ///  Bit string, 8 bits, BYTE,
        /// </summary>
        // Token: 0x040004E0 RID: 1248
        public const ushort CIP_Type_D1 = 209;

        /// <summary>
        /// Bit string, 16-bits, WORD
        /// </summary>
        // Token: 0x040004E1 RID: 1249
        public const ushort CIP_Type_D2 = 210;

        /// <summary>
        /// Bit string, 32 bits, DWORD
        /// </summary>
        // Token: 0x040004E2 RID: 1250
        public const ushort CIP_Type_D3 = 211;

        /// <summary>
        /// Bit string, 64 bits LWORD
        /// </summary>
        // Token: 0x040004E3 RID: 1251
        public const ushort CIP_Type_D4 = 212;

        /// <summary>
        /// 二进制数据内容
        /// </summary>
        // Token: 0x040004E4 RID: 1252
        public const ushort CIP_Type_BitArray = 211;

        /// <summary>
        /// 连接方的厂商标识
        /// </summary>
        // Token: 0x040004E5 RID: 1253
        public const ushort OriginatorVendorID = 4105;

        /// <summary>
        /// 连接方的序列号
        /// </summary>
        // Token: 0x040004E6 RID: 1254
        public const uint OriginatorSerialNumber = 3248834059U;
    }

    /// <summary>
    /// 结构体的句柄信息
    /// </summary>
    public class AbStructHandle
    {
        /// <summary>
        /// 实例化一个默认的对象<br />
        /// instantiate a default object
        /// </summary>
        public AbStructHandle()
        {
        }

        /// <summary>
        /// 使用原始字节的数据，索引信息来实例化一个对象<br />
        /// Instantiate an object with raw bytes of data, index information
        /// </summary>
        /// <param name="source">原始字节数据</param>
        /// <param name="index">起始的偏移索引</param>
        public AbStructHandle(byte[] source, int index)
        {
            this.ReturnCount = BitConverter.ToUInt16(source, index);
            this.TemplateObjectDefinitionSize = BitConverter.ToUInt32(source, index + 6);
            this.TemplateStructureSize = BitConverter.ToUInt32(source, index + 14);
            this.MemberCount = BitConverter.ToUInt16(source, index + 22);
            this.StructureHandle = BitConverter.ToUInt16(source, index + 28);
        }

        /// <summary>
        /// 返回项数
        /// </summary>
        /// <remarks>
        /// Count of Items returned
        /// </remarks>
        public ushort ReturnCount { get; set; }

        /// <summary>
        /// 结构体定义大小
        /// </summary>
        /// <remarks>
        /// This is the number of structure members
        /// </remarks>
        public uint TemplateObjectDefinitionSize { get; set; }

        /// <summary>
        /// 使用读取标记服务读取结构时在线路上传输的字节数
        /// </summary>
        /// <remarks>
        /// This is the number of bytes of the structure data
        /// </remarks>
        public uint TemplateStructureSize { get; set; }

        /// <summary>
        /// 成员数量
        /// </summary>
        /// <remarks>
        /// This is the number of structure members
        /// </remarks>
        public ushort MemberCount { get; set; }

        /// <summary>
        /// 结构体的handle
        /// </summary>
        /// <remarks>
        /// This is the Tag Type Parameter used in Read/Write Tag service
        /// </remarks>
        public ushort StructureHandle { get; set; }
    }
}
