using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.BeckhoffDriver
{
    public enum AmsTcpHeaderFlags : ushort
    {
        /// <summary>
        /// AmsCommand (AMS_TCP_PORT_AMS_CMD, 0x0000)
        /// </summary>
        Command,
        /// <summary>
        /// Port Close command (AMS_TCP_PORT_CLOSE, 0x0001)
        /// </summary>
        PortClose,
        /// <summary>
        /// Port connect command (AMS_TCP_PORT_CONNECT, 0x1000)
        /// </summary>
        PortConnect = 4096,
        /// <summary>
        /// Router Notification (AMS_TCP_PORT_ROUTER_NOTE, 0x1001)
        /// </summary>
        RouterNotification,
        /// <summary>
        /// Get LocalNetId header
        /// </summary>
        GetLocalNetId
    }

    /// <summary>
    /// 
    /// </summary>
    public class AdsHelper
    {
        /// <summary>
        /// 根据命令码ID，消息ID，数据信息组成AMS的命令码
        /// </summary>
        /// <param name="commandId">命令码ID</param>
        /// <param name="data">数据内容</param>
        /// <returns>打包之后的数据信息，没有填写AMSNetId的Target和Source内容</returns>
        public static byte[] BuildAmsHeaderCommand(ushort commandId, byte[] data)
        {
            if (data == null)
            {
                data = new byte[0];
            }
            byte[] array = new byte[32 + data.Length];
            array[16] = BitConverter.GetBytes(commandId)[0];
            array[17] = BitConverter.GetBytes(commandId)[1];
            array[18] = 4;
            array[19] = 0;
            array[20] = BitConverter.GetBytes(data.Length)[0];
            array[21] = BitConverter.GetBytes(data.Length)[1];
            array[22] = BitConverter.GetBytes(data.Length)[2];
            array[23] = BitConverter.GetBytes(data.Length)[3];
            array[24] = 0;
            array[25] = 0;
            array[26] = 0;
            array[27] = 0;
            data.CopyTo(array, 32);
            return AdsHelper.PackAmsTcpHelper(AmsTcpHeaderFlags.Command, array);
        }

        /// <summary>
        /// 构建读取设备信息的命令报文
        /// </summary>
        /// <returns>报文信息</returns>
        public static byte[] BuildReadDeviceInfoCommand()
        {
            return AdsHelper.BuildAmsHeaderCommand(1, null);
        }

        /// <summary>
        /// 构建读取状态的命令报文
        /// </summary>
        /// <returns>报文信息</returns>
        public static byte[] BuildReadStateCommand()
        {
            return AdsHelper.BuildAmsHeaderCommand(4, null);
        }

        /// <summary>
        /// 构建写入状态的命令报文
        /// </summary>
        /// <param name="state">Ads state</param>
        /// <param name="deviceState">Device state</param>
        /// <param name="data">Data</param>
        /// <returns>报文信息</returns>
        public static byte[] BuildWriteControlCommand(short state, short deviceState, byte[] data)
        {
            if (data == null)
            {
                data = new byte[0];
            }
            byte[] array = new byte[8 + data.Length];
            return AdsHelper.BuildAmsHeaderCommand(5, DataExtend.SpliceArray<byte>(new byte[][]
            {
                BitConverter.GetBytes(state),
                BitConverter.GetBytes(deviceState),
                BitConverter.GetBytes(data.Length),
                data
            }));
        }

        /// <summary>
        /// 构建写入的指令信息
        /// </summary>
        /// <param name="address">地址信息</param>
        /// <param name="length">数据长度</param>
        /// <param name="isBit">是否是位信息</param>
        /// <returns>结果内容</returns>
        public static byte[] BuildReadCommand(string address, int length, bool isBit)
        {
            Tuple<uint, uint> operateResult = AdsHelper.AnalysisAddress(address, isBit);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                byte[] array = new byte[12];
                BitConverter.GetBytes(operateResult.Item1).CopyTo(array, 0);
                BitConverter.GetBytes(operateResult.Item2).CopyTo(array, 4);
                BitConverter.GetBytes(length).CopyTo(array, 8);
                return AdsHelper.BuildAmsHeaderCommand(2, array);
            }
        }

        /// <summary>
        /// 构建批量读取的指令信息，不能传入读取符号数据，只能传入读取M,I,Q,i=0x0001信息
        /// </summary>
        /// <param name="address">地址信息</param>
        /// <param name="length">数据长度</param>
        /// <returns>结果内容</returns>
        public static byte[] BuildReadCommand(string[] address, ushort[] length)
        {
            byte[] array = new byte[12 * address.Length];
            int num = 0;
            for (int i = 0; i < address.Length; i++)
            {
                Tuple<uint, uint> operateResult = AdsHelper.AnalysisAddress(address[i], false);
                if (operateResult==null)
                {
                    return null;
                }
                BitConverter.GetBytes(operateResult.Item1).CopyTo(array, 12 * i);
                BitConverter.GetBytes(operateResult.Item2).CopyTo(array, 12 * i + 4);
                BitConverter.GetBytes((int)length[i]).CopyTo(array, 12 * i + 8);
                num += (int)length[i];
            }
            return AdsHelper.BuildReadWriteCommand("ig=0xF080;0", num, false, array);
        }

        /// <summary>
        /// 构建写入的指令信息
        /// </summary>
        /// <param name="address">地址信息</param>
        /// <param name="length">数据长度</param>
        /// <param name="isBit">是否是位信息</param>
        /// <param name="value">写入的数值</param>
        /// <returns>结果内容</returns>
        public static byte[] BuildReadWriteCommand(string address, int length, bool isBit, byte[] value)
        {
            Tuple<uint, uint> operateResult = AdsHelper.AnalysisAddress(address, isBit);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                byte[] array = new byte[16 + value.Length];
                BitConverter.GetBytes(operateResult.Item1).CopyTo(array, 0);
                BitConverter.GetBytes(operateResult.Item2).CopyTo(array, 4);
                BitConverter.GetBytes(length).CopyTo(array, 8);
                BitConverter.GetBytes(value.Length).CopyTo(array, 12);
                value.CopyTo(array, 16);
                return AdsHelper.BuildAmsHeaderCommand(9, array);
            }
        }

        /// <summary>
        /// 构建批量写入的指令代码，不能传入读取符号数据，只能传入读取M,I,Q,i=0x0001信息
        /// </summary>
        /// <remarks>
        /// 实际没有调试通
        /// </remarks>
        /// <param name="address">地址列表信息</param>
        /// <param name="value">写入的数据值信息</param>
        /// <returns>命令报文</returns>
        public static byte[] BuildWriteCommand(string[] address, List<byte[]> value)
        {
            MemoryStream memoryStream = new MemoryStream();
            int num = 0;
            for (int i = 0; i < address.Length; i++)
            {
                Tuple<uint, uint> operateResult = AdsHelper.AnalysisAddress(address[i], false);
                if (operateResult == null)
                {
                    return null;
                }
                memoryStream.Write(BitConverter.GetBytes(operateResult.Item1));
                memoryStream.Write(BitConverter.GetBytes(operateResult.Item2));
                memoryStream.Write(BitConverter.GetBytes(value[i].Length));
                memoryStream.Write(value[i]);
                num += value[i].Length;
            }
            return AdsHelper.BuildReadWriteCommand("ig=0xF081;0", num, false, memoryStream.ToArray());
        }

        /// <summary>
        /// 构建写入的指令信息
        /// </summary>
        /// <param name="address">地址信息</param>
        /// <param name="value">数据</param>
        /// <param name="isBit">是否是位信息</param>
        /// <returns>结果内容</returns>
        public static byte[] BuildWriteCommand(string address, byte[] value, bool isBit)
        {
            Tuple<uint, uint> operateResult = AdsHelper.AnalysisAddress(address, isBit);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                byte[] array = new byte[12 + value.Length];
                BitConverter.GetBytes(operateResult.Item1).CopyTo(array, 0);
                BitConverter.GetBytes(operateResult.Item2).CopyTo(array, 4);
                BitConverter.GetBytes(value.Length).CopyTo(array, 8);
                value.CopyTo(array, 12);
                return AdsHelper.BuildAmsHeaderCommand(3, array);
            }
        }

        /// <summary>
        /// 构建写入的指令信息
        /// </summary>
        /// <param name="address">地址信息</param>
        /// <param name="value">数据</param>
        /// <param name="isBit">是否是位信息</param>
        /// <returns>结果内容</returns>
        public static byte[] BuildWriteCommand(string address, bool[] value, bool isBit)
        {
            Tuple<uint, uint> operateResult = AdsHelper.AnalysisAddress(address, isBit);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                byte[] array = (from m in value select m ? (byte)1 : (byte)0).ToArray<byte>();
                byte[] array2 = new byte[12 + array.Length];
                BitConverter.GetBytes(operateResult.Item1).CopyTo(array2, 0);
                BitConverter.GetBytes(operateResult.Item2).CopyTo(array2, 4);
                BitConverter.GetBytes(array.Length).CopyTo(array2, 8);
                array.CopyTo(array2, 12);
                return AdsHelper.BuildAmsHeaderCommand(3, array2);
            }

        }

        /// <summary>
        /// 构建释放句柄的报文信息，当获取了变量的句柄后，这个句柄就被释放
        /// </summary>
        /// <param name="handle">句柄信息</param>
        /// <returns>报文的结果内容</returns>
        public static byte[] BuildReleaseSystemHandle(uint handle)
        {
            byte[] array = new byte[16];
            BitConverter.GetBytes(61446).CopyTo(array, 0);
            BitConverter.GetBytes(4).CopyTo(array, 8);
            BitConverter.GetBytes(handle).CopyTo(array, 12);
            return AdsHelper.BuildAmsHeaderCommand(3, array);
        }

        /// <summary>
        /// 检查从PLC的反馈的数据报文是否正确
        /// </summary>
        /// <param name="response">反馈报文</param>
        /// <returns>检查结果</returns>
        public static bool CheckResponse(byte[] response)
        {
            try
            {
                int num = BitConverter.ToInt32(response, 30);
                bool flag = num > 0;
                if (num > 0)
                {
                    return false;
                }
                if (response.Length >= 42)
                {
                    int num2 = BitConverter.ToInt32(response, 38);
                    if (num2 != 0)
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 将实际的包含AMS头报文和数据报文的命令，打包成实际可发送的命令
        /// </summary>
        /// <param name="headerFlags">命令头信息</param>
        /// <param name="command">命令信息</param>
        /// <returns>结果信息</returns>
        public static byte[] PackAmsTcpHelper(AmsTcpHeaderFlags headerFlags, byte[] command)
        {
            byte[] array = new byte[6 + command.Length];
            BitConverter.GetBytes((ushort)headerFlags).CopyTo(array, 0);
            BitConverter.GetBytes(command.Length).CopyTo(array, 2);
            command.CopyTo(array, 6);
            return array;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private static int CalculateAddressStarted(string address)
        {
            //bool flag = address.IndexOf('.') < 0;
            int result;
            if (address.IndexOf('.') < 0)
            {
                result = Convert.ToInt32(address);
            }
            else
            {
                string[] array = address.Split(new char[]
                {
                    '.'
                });
                result = Convert.ToInt32(array[0]) * 8 + DataExtend.CalculateBitStartIndex(array[1]);
            }
            return result;
        }

        /// <summary>
        /// 分析当前的地址信息，根据结果信息进行解析出真实的偏移地址
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="isBit">是否位访问</param>
        /// <returns>结果内容</returns>
        public static Tuple<uint, uint> AnalysisAddress(string address, bool isBit)
        {
            //Tuple<uint, uint> operateResult = new Tuple<uint, uint>();
            try
            {
                bool flag = address.StartsWith("i=") || address.StartsWith("I=");
                if (flag)
                {
                    return new Tuple<uint, uint>(61445U, uint.Parse(address.Substring(2)));
                    //operateResult.Content1 = 61445U;
                    //operateResult.Content2 = uint.Parse(address.Substring(2));
                }
                else if (address.StartsWith("s=") || address.StartsWith("S="))
                {
                    return new Tuple<uint, uint>(61443U, 0U);
                }
                else if(address.StartsWith("ig=") || address.StartsWith("IG="))
                {
                    address = address.ToUpper();
                    return new Tuple<uint, uint>((uint)DataExtend.ExtractParameter(ref address, "IG", 0), uint.Parse(address));
                }
                else
                {
                    char c = address[0];
                    char c2 = c;
                    if (c2 <= 'Q')
                    {
                        if (c2 == 'I')
                        {
                            if (isBit)
                            {
                                return new Tuple<uint, uint>(61473U, (uint)AdsHelper.CalculateAddressStarted(address.Substring(1)));
                            }
                            else
                            {
                                return new Tuple<uint, uint>(61472U, uint.Parse(address.Substring(1)));
                            }
                        }
                        if (c2 != 'M')
                        {
                            if (c2 != 'Q')
                            {
                                return null;
                            }
                            if (isBit)
                            {
                                return new Tuple<uint, uint>(61489U, (uint)AdsHelper.CalculateAddressStarted(address.Substring(1)));
                            }
                            else
                            {
                                return new Tuple<uint, uint>(61488U, uint.Parse(address.Substring(1)));
                            }
                        }
                    }
                    else
                    {
                        if (c2 == 'i')
                        {
                            if (isBit)
                            {
                                return new Tuple<uint, uint>(61473U, (uint)AdsHelper.CalculateAddressStarted(address.Substring(1)));
                            }
                            else
                            {
                                return new Tuple<uint, uint>(61472U, uint.Parse(address.Substring(1)));
                            }
                        }
                        if (c2 != 'm')
                        {
                            if (c2 != 'q')
                            {
                                return null;
                            }
                            if (isBit)
                            {
                                return new Tuple<uint, uint>(61489U, (uint)AdsHelper.CalculateAddressStarted(address.Substring(1)));
                            }
                            else
                            {
                                return new Tuple<uint, uint>(61488U, uint.Parse(address.Substring(1)));
                            }
                        }
                    }
                    if (isBit)
                    {
                        return new Tuple<uint, uint>(16417U, (uint)AdsHelper.CalculateAddressStarted(address.Substring(1)));
                    }
                    else
                    {
                        return new Tuple<uint, uint>(16416U, uint.Parse(address.Substring(1)));
                    }
                }
                
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// 将字符串名称转变为ADS协议可识别的字节数组
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>字节数组</returns>
        public static byte[] StrToAdsBytes(string value)
        {
            return DataExtend.SpliceArray<byte>(new byte[][]
            {
                Encoding.ASCII.GetBytes(value),
                new byte[1]
            });
        }

        /// <summary>
        /// 将字符串的信息转换为AMS目标的地址
        /// </summary>
        /// <param name="amsNetId">目标信息</param>
        /// <returns>字节数组</returns>
        public static byte[] StrToAMSNetId(string amsNetId)
        {
            string text = amsNetId;
            //bool flag = amsNetId.IndexOf(':') > 0;
            byte[] array;
            if (amsNetId.IndexOf(':') > 0)
            {
                array = new byte[8];
                string[] array2 = amsNetId.Split(new char[]
                {
                    ':'
                }, StringSplitOptions.RemoveEmptyEntries);
                text = array2[0];
                array[6] = BitConverter.GetBytes(int.Parse(array2[1]))[0];
                array[7] = BitConverter.GetBytes(int.Parse(array2[1]))[1];
            }
            else
            {
                array = new byte[6];
            }
            string[] array3 = text.Split(new char[]
            {
                '.'
            }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < array3.Length; i++)
            {
                array[i] = byte.Parse(array3[i]);
            }
            return array;
        }

        /// <summary>
        /// 根据byte数组信息提取出字符串格式的AMSNetId数据信息，方便日志查看
        /// </summary>
        /// <param name="data">原始的报文数据信息</param>
        /// <param name="index">起始的节点信息</param>
        /// <returns>Ams节点号信息</returns>
        public static string GetAmsNetIdString(byte[] data, int index)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(data[index]);
            stringBuilder.Append(".");
            stringBuilder.Append(data[index + 1]);
            stringBuilder.Append(".");
            stringBuilder.Append(data[index + 2]);
            stringBuilder.Append(".");
            stringBuilder.Append(data[index + 3]);
            stringBuilder.Append(".");
            stringBuilder.Append(data[index + 4]);
            stringBuilder.Append(".");
            stringBuilder.Append(data[index + 5]);
            stringBuilder.Append(":");
            stringBuilder.Append(BitConverter.ToUInt16(data, index + 6));
            return stringBuilder.ToString();
        }

    }

}
