using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.OmronDriver
{
    public class OmronFinsNetHelper
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...

        

        /// <summary>
        /// 根据读取的地址，长度，是否位读取创建Fins协议的核心报文<br />
        /// According to the read address, length, whether to read the core message that creates the Fins protocol
        /// </summary>
        /// <param name="address">地址，具体格式请参照示例说明</param>
        /// <param name="length">读取的数据长度</param>
        /// <param name="isBit">是否使用位读取</param>
        /// <param name="splitLength">读取的长度切割，默认500</param>
        /// <returns>带有成功标识的Fins核心报文</returns>
        public static List<byte[]> BuildReadCommand(string address, ushort length, bool isBit, int splitLength = 500)
        {
           var operateResult = OmronFinsAddress.ParseFrom(address, length,out bool result);
            if (!result)
            {
                return null;
            }
            else
            {
                List<byte[]> list = new List<byte[]>();
                int[] array = DataExtend.SplitIntegerToArray((int)length, isBit ? int.MaxValue : splitLength);
                for (int i = 0; i < array.Length; i++)
                {
                    byte[] array2 = new byte[8];
                    array2[0] = 1;
                    array2[1] = 1;
                    if (isBit)
                    {
                        array2[2] = operateResult.BitCode;
                    }
                    else
                    {
                        array2[2] = operateResult.WordCode;
                    }
                    array2[3] = (byte)(operateResult.AddressStart / 16 / 256);
                    array2[4] = (byte)(operateResult.AddressStart / 16 % 256);
                    array2[5] = (byte)(operateResult.AddressStart % 16);
                    array2[6] = (byte)(array[i] / 256);
                    array2[7] = (byte)(array[i] % 256);
                    list.Add(array2);
                    operateResult.AddressStart += (isBit ? array[i] : (array[i] * 16));
                }
                return list;
            }
        }

        /// <summary>
        /// 根据写入的地址，数据，是否位写入生成Fins协议的核心报文<br />
        /// According to the written address, data, whether the bit is written to generate the core message of the Fins protocol
        /// </summary>
        /// <param name="address">地址内容，具体格式请参照示例说明</param>
        /// <param name="value">实际的数据</param>
        /// <param name="isBit">是否位数据</param>
        /// <returns>带有成功标识的Fins核心报文</returns>
        public static byte[] BuildWriteWordCommand(string address, byte[] value, bool isBit)
        {
            var operateResult = OmronFinsAddress.ParseFrom(address, 0,out bool result);
            if (!result)
            {
                return null;
            }
            else
            {
                byte[] array = new byte[8 + value.Length];
                array[0] = 1;
                array[1] = 2;
                if (isBit)
                {
                    array[2] = operateResult.BitCode;
                }
                else
                {
                    array[2] = operateResult.WordCode;
                }
                array[3] = (byte)(operateResult.AddressStart / 16 / 256);
                array[4] = (byte)(operateResult.AddressStart / 16 % 256);
                array[5] = (byte)(operateResult.AddressStart % 16);
                if (isBit)
                {
                    array[6] = (byte)(value.Length / 256);
                    array[7] = (byte)(value.Length % 256);
                }
                else
                {
                    array[6] = (byte)(value.Length / 2 / 256);
                    array[7] = (byte)(value.Length / 2 % 256);
                }
                value.CopyTo(array, 8);
                return array;
            }
        }


        /// <summary>
        /// 验证欧姆龙的Fins-TCP返回的数据是否正确的数据，如果正确的话，并返回所有的数据内容<br />
        /// Verify that the data returned by Omron's Fins-TCP is correct data, if correct, and return all data content
        /// </summary>
        /// <param name="response">来自欧姆龙返回的数据内容</param>
        /// <returns>带有是否成功的结果对象</returns>
        public static byte[] ResponseValidAnalysis(byte[] response,out string err)
        {
            if (response.Length >= 16)
            {
                int num = BitConverter.ToInt32(new byte[]
                {
                    response[15],
                    response[14],
                    response[13],
                    response[12]
                }, 0);
                bool flag2 = num > 0;
                if (num > 0)
                {
                    err=GetStatusDescription(num);
                    return null;
                }
                else
                {
                    return OmronFinsNetHelper.UdpResponseValidAnalysis(response.AsSpan(16),out err);
                }
            }
            else
            {
                err = "OmronReceiveDataError";
                return null;
            }
        }

        // HslCommunication.Profinet.Omron.OmronFinsNetHelper
        /// <summary>
        /// 验证欧姆龙的Fins-Udp返回的数据是否正确的数据，如果正确的话，并返回所有的数据内容<br />
        /// Verify that the data returned by Omron's Fins-Udp is correct data, if correct, and return all data content
        /// </summary>
        /// <param name="response">来自欧姆龙返回的数据内容</param>
        /// <returns>带有是否成功的结果对象</returns>
        public static byte[] UdpResponseValidAnalysis(Span<byte> response,out string err)
        {
            if (response.Length >= 14)
            {
                int num = (int)response[12] * 256 + (int)response[13];
                bool flag2 = (response[10] == 1 & response[11] == 1) || (response[10] == 1 & response[11] == 4) || (response[10] == 2 & response[11] == 1) || (response[10] == 3 & response[11] == 6) || (response[10] == 5 & response[11] == 1) || (response[10] == 5 & response[11] == 2) || (response[10] == 6 & response[11] == 1) || (response[10] == 6 & response[11] == 32) || (response[10] == 7 & response[11] == 1) || (response[10] == 9 & response[11] == 32) || (response[10] == 33 & response[11] == 2) || (response[10] == 34 & response[11] == 2);
                if (flag2)
                {
                    byte[] array = new byte[response.Length - 14];
                    if (array.Length != 0)
                    {
                        Array.Copy(response.ToArray(), 14, array, 0, array.Length);
                    }
                    else
                    {
                        err = OmronFinsNetHelper.GetStatusDescription(num);
                        return null;
                    }

                    err = OmronFinsNetHelper.GetStatusDescription(num);
                    return array;
                }
                else
                {
                    err = OmronFinsNetHelper.GetStatusDescription(num);
                    return null;

                }
            }
            else
            {
                err = "OmronReceiveDataError";
                return null;
               // result = new OperateResult<byte[]>(StringResources.Language.OmronReceiveDataError);
            }
        }

        /// <summary>
        /// 根据欧姆龙返回的错误码，获取错误信息的字符串描述文本<br />
        /// According to the error code returned by Omron, get the string description text of the error message
        /// </summary>
        /// <param name="err">错误码</param>
        /// <returns>文本描述</returns>
        public static string GetStatusDescription(int err)
        {
            string result;
            switch (err)
            {
                case 0:
                    result = "通讯正常";
                    break;
                case 1:
                    result = "消息头不是FINS";
                    break;
                case 2:
                    result = "数据长度太长";
                    break;
                case 3:
                    result = "该命令不支持";
                    break;
                default:
                    switch (err)
                    {
                        case 32:
                            result = "超过连接上限";
                            break;
                        case 33:
                            result = "指定的节点已经处于连接中";
                            break;
                        case 34:
                            result = "尝试去连接一个受保护的网络节点，该节点还未配置到PLC中";
                            break;
                        case 35:
                            result = "当前客户端的网络节点超过正常范围";
                            break;
                        case 36:
                            result = "当前客户端的网络节点已经被使用";
                            break;
                        case 37:
                            result = "所有的网络节点已经被使用";
                            break;
                        default:
                            result = "未知错误";
                            break;
                    }
                    break;
            }
            return result;
        }


        /// <summary>
        /// 从欧姆龙PLC中读取想要的数据，返回读取结果，读取长度的单位为字，地址格式为"D100","C100","W100","H100","A100"<br />
        /// Read the desired data from the Omron PLC and return the read result. The unit of the read length is word. The address format is "D100", "C100", "W100", "H100", "A100"
        /// </summary>
        /// <param name="omron">PLC设备的连接对象</param>
        /// <param name="address">读取地址，格式为"D100","C100","W100","H100","A100"</param>
        /// <param name="length">读取的数据长度</param>
        /// <param name="splits">分割信息</param>
        /// <returns>带成功标志的结果数据对象</returns>
        public static byte[] Read(NetworkDeviceProxyBase omron, string address, ushort length, int splits)
        {
            List<byte[]> operateResult = BuildReadCommand(address, length, false, splits);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                List<byte> list = new List<byte>();
                for (int i = 0; i < operateResult.Count; i++)
                {
                    byte[] operateResult2 = omron.ReadFromCoreServer(operateResult[i]);
                    if (operateResult2==null)
                    {
                        return null;
                    }
                    list.AddRange(operateResult2);
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// 向PLC写入数据，数据格式为原始的字节类型，地址格式为"D100","C100","W100","H100","A100"<br />
        /// Write data to PLC, the data format is the original byte type, and the address format is "D100", "C100", "W100", "H100", "A100"
        /// </summary>
        /// <param name="omron">PLC设备的连接对象</param>
        /// <param name="address">初始地址</param>
        /// <param name="value">原始的字节数据</param>
        /// <returns>结果</returns>
        public static object Write(NetworkDeviceProxyBase omron, string address, byte[] value)
        {
            byte[] operateResult = OmronFinsNetHelper.BuildWriteWordCommand(address, value, false);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                return omron.ReadFromCoreServer(operateResult);
            }
        }

        /// <summary>
        /// 从欧姆龙PLC中批量读取位软元件，地址格式为"D100.0","C100.0","W100.0","H100.0","A100.0"<br />
        /// Read bit devices in batches from Omron PLC with address format "D100.0", "C100.0", "W100.0", "H100.0", "A100.0"
        /// </summary>
        /// <param name="omron">PLC设备的连接对象</param>
        /// <param name="address">读取地址，格式为"D100","C100","W100","H100","A100"</param>
        /// <param name="length">读取的长度</param>
        /// <param name="splits">分割信息</param>
        /// <returns>带成功标志的结果数据对象</returns>
        public static bool[] ReadBool(NetworkDeviceProxyBase omron, string address, ushort length, int splits)
        {
            List<byte[]> operateResult = OmronFinsNetHelper.BuildReadCommand(address, length, true, splits);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                List<bool> list = new List<bool>();
                for (int i = 0; i < operateResult.Count; i++)
                {
                    var operateResult2 = omron.ReadFromCoreServer(operateResult[i]);
                    if (operateResult2==null)
                    {
                        return null;
                    }
                    list.AddRange(from m in operateResult2  select m > 0);
                }
               return list.ToArray();
            }
        }

        /// <summary>
        /// 向PLC中位软元件写入bool数组，返回是否写入成功，比如你写入D100,values[0]对应D100.0，地址格式为"D100.0","C100.0","W100.0","H100.0","A100.0"<br />
        /// Write the bool array to the PLC's median device and return whether the write was successful. For example, if you write D100, values [0] corresponds to D100.0 
        /// and the address format is "D100.0", "C100.0", "W100. 0 "," H100.0 "," A100.0 "
        /// </summary>
        /// <param name="omron">PLC设备的连接对象</param>
        /// <param name="address">要写入的数据地址</param>
        /// <param name="values">要写入的实际数据，可以指定任意的长度</param>
        /// <returns>返回写入结果</returns>
        public static byte[] Write(NetworkDeviceProxyBase omron, string address, bool[] values)
        {
            byte[] operateResult = OmronFinsNetHelper.BuildWriteWordCommand(address, (from m in values select m ? (byte)1 : (byte)0).ToArray<byte>(), true);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                return omron.ReadFromCoreServer(operateResult);
            }
        }


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
