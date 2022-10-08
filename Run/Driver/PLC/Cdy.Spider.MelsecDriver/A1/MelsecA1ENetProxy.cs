using Cdy.Spider.Common;
using Cdy.Spider.MelsecDriver.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver
{
    /// <summary>
    /// 三菱PLC通讯协议，采用A兼容1E帧协议实现，使用二进制码通讯，请根据实际型号来进行选取<
    /// </summary>
    public class MelsecA1ENetProxy : NetworkDeviceProxyBase
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        public MelsecA1ENetProxy(DriverRunnerBase driver) : base(driver)
        {
            WordLength = 1;
            ByteTransform = new RegularByteTransform();
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public byte PLCNumber { get; set; } = byte.MaxValue;
        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override byte[] Read(string address, ushort length, out bool res)
        {
            List<byte[]> operateResult = BuildReadCommand(address, length, false, PLCNumber);
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
                    byte[] operateResult2 = ReadFromCoreServer(operateResult[i]);
                    if (operateResult2 == null || !CheckResponseLegal(operateResult2))
                    {
                        res = false;
                        return null;
                    }
                    //var operateResult3 = CheckResponseLegal(operateResult2);
                    //if (!operateResult3)
                    //{
                    //    res = false;
                    //    return null;
                    //}
                    byte[] operateResult4 = ExtractActualData(operateResult2, false);
                    if (operateResult4 == null)
                    {
                        res = false;
                        return null;
                    }
                    list.AddRange(operateResult4);
                }
                res = true;
                return list.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override object Write(string address, byte[] value, out bool result)
        {
            byte[] operateResult = BuildWriteWordCommand(address, value, PLCNumber);
            if (operateResult == null)
            {
                result = false;
                return null;
            }
            else
            {
                byte[] operateResult2 = ReadFromCoreServer(operateResult);
                if (operateResult2 == null)
                {
                    result = false;
                    return null;
                }
                else
                {
                    var operateResult3 = CheckResponseLegal(operateResult2);
                    if (!operateResult3)
                    {
                        result = false;
                        return false;
                    }
                    else
                    {
                        result = true;
                        return true;
                    }
                }
            }
        }

        /// <summary>
        /// 数组信息，需要指定地址和长度，地址示例M100，S100，B1A，如果是X,Y, X017就是8进制地址，Y10就是16进制地址。
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override bool[] ReadBool(string address, ushort length, out bool res)
        {
            List<byte[]> operateResult = BuildReadCommand(address, length, true, PLCNumber);
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
                    byte[] operateResult2 = ReadFromCoreServer(operateResult[i]);
                    if (operateResult2 == null || !CheckResponseLegal(operateResult2))
                    {
                        res = false;
                        return null;
                    }
                    var operateResult4 = ExtractActualData(operateResult2, true);
                    if (operateResult4 == null)
                    {
                        res = false;
                        return null;
                    }
                    list.AddRange(operateResult4);
                }
                res = true;
                return (from m in list select m == 1).Take(length).ToArray();
            }
        }

        /// <summary>
        /// 数组数据，返回是否成功，地址示例M100，S100，B1A，如果是X,Y, X017就是8进制地址，Y10就是16进制地址。
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, bool[] value, out bool res)
        {
            byte[] operateResult = BuildWriteBoolCommand(address, value, PLCNumber);
            if (operateResult == null)
            {
                res = false;
                return false;
            }
            else
            {
                byte[] operateResult2 = ReadFromCoreServer(operateResult);
                if (operateResult2 == null)
                {
                    res = false;
                    return false;
                }
                else
                {
                    res = CheckResponseLegal(operateResult2);
                    return res;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override INetMessage GetNewNetMessage()
        {
            return new MelsecA1EBinaryMessage();
        }

        /// <summary>
        /// 从PLC反馈的数据中提取出实际的数据内容，需要传入反馈数据，是否位读取
        /// </summary>
        /// <param name="response">反馈的数据内容</param>
        /// <param name="isBit">是否位读取</param>
        /// <returns>解析后的结果对象</returns>
        public static byte[] ExtractActualData(byte[] response, bool isBit)
        {
            byte[] result;
            if (isBit)
            {
                byte[] array = new byte[(response.Length - 2) * 2];
                for (int i = 2; i < response.Length; i++)
                {
                    //bool flag = (response[i] & 16) == 16;
                    if ((response[i] & 16) == 16)
                    {
                        array[(i - 2) * 2] = 1;
                    }
                    //bool flag2 = (response[i] & 1) == 1;
                    if ((response[i] & 1) == 1)
                    {
                        array[(i - 2) * 2 + 1] = 1;
                    }
                }
                result = array;
            }
            else
            {
                result = response.RemoveBegin(2);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static bool CheckResponseLegal(byte[] response)
        {
            bool flag = response.Length < 2;
            if (response.Length < 2)
            {
                return false;
            }
            else
            {
                //bool flag2 = response[1] == 0;
                if (response[1] == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 根据类型地址长度确认需要读取的指令头
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="length">长度</param>
        /// <param name="isBit">指示是否按照位成批的读出</param>
        /// <param name="plcNumber">PLC编号</param>
        /// <returns>带有成功标志的指令数据</returns>
        public static List<byte[]> BuildReadCommand(string address, ushort length, bool isBit, byte plcNumber)
        {
            Tuple<MelsecA1EDataType, int> operateResult = MelsecHelper.McA1EAnalysisAddress(address);
            if (operateResult == null)
            {
                return null;
            }
            else
            {
                byte b = isBit ? (byte)0 : (byte)1;
                int[] array = DataExtend.SplitIntegerToArray(length, isBit ? 256 : 64);
                List<byte[]> list = new List<byte[]>();
                for (int i = 0; i < array.Length; i++)
                {
                    list.Add(new byte[]
                    {
                        b,
                        plcNumber,
                        10,
                        0,
                        BitConverter.GetBytes(operateResult.Item2)[0],
                        BitConverter.GetBytes(operateResult.Item2)[1],
                        BitConverter.GetBytes(operateResult.Item2)[2],
                        BitConverter.GetBytes(operateResult.Item2)[3],
                        BitConverter.GetBytes(operateResult.Item1.DataCode)[0],
                        BitConverter.GetBytes(operateResult.Item1.DataCode)[1],
                        BitConverter.GetBytes(array[i])[0],
                        BitConverter.GetBytes(array[i])[1]
                    });
                    //operateResult.Content2 += array[i];
                }
                return list;
            }
        }

        /// <summary>
        /// 根据类型地址以及需要写入的数据来生成指令头
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="value">数据值</param>
        /// <param name="plcNumber">PLC编号</param>
        /// <returns>带有成功标志的指令数据</returns>
        public static byte[] BuildWriteWordCommand(string address, byte[] value, byte plcNumber)
        {
            Tuple<MelsecA1EDataType, int> operateResult = MelsecHelper.McA1EAnalysisAddress(address);
            if (operateResult == null)
            {
                return null;
            }
            else
            {
                byte[] array = new byte[12 + value.Length];
                array[0] = 3;
                array[1] = plcNumber;
                array[2] = 10;
                array[3] = 0;
                array[4] = BitConverter.GetBytes(operateResult.Item2)[0];
                array[5] = BitConverter.GetBytes(operateResult.Item2)[1];
                array[6] = BitConverter.GetBytes(operateResult.Item2)[2];
                array[7] = BitConverter.GetBytes(operateResult.Item2)[3];
                array[8] = BitConverter.GetBytes(operateResult.Item1.DataCode)[0];
                array[9] = BitConverter.GetBytes(operateResult.Item1.DataCode)[1];
                array[10] = BitConverter.GetBytes(value.Length / 2)[0];
                array[11] = BitConverter.GetBytes(value.Length / 2)[1];
                Array.Copy(value, 0, array, 12, value.Length);
                return array;
            }
        }

        /// <summary>
        /// 根据类型地址以及需要写入的数据来生成指令头
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="value">数据值</param>
        /// <param name="plcNumber">PLC编号</param>
        /// <returns>带有成功标志的指令数据</returns>
        public static byte[] BuildWriteBoolCommand(string address, bool[] value, byte plcNumber)
        {
            Tuple<MelsecA1EDataType, int> operateResult = MelsecHelper.McA1EAnalysisAddress(address);
            if (operateResult == null)
            {
                return null;
            }
            else
            {
                byte[] array = MelsecHelper.TransBoolArrayToByteData(value);
                byte[] array2 = new byte[12 + array.Length];
                array2[0] = 2;
                array2[1] = plcNumber;
                array2[2] = 10;
                array2[3] = 0;
                array2[4] = BitConverter.GetBytes(operateResult.Item2)[0];
                array2[5] = BitConverter.GetBytes(operateResult.Item2)[1];
                array2[6] = BitConverter.GetBytes(operateResult.Item2)[2];
                array2[7] = BitConverter.GetBytes(operateResult.Item2)[3];
                array2[8] = BitConverter.GetBytes(operateResult.Item1.DataCode)[0];
                array2[9] = BitConverter.GetBytes(operateResult.Item1.DataCode)[1];
                array2[10] = BitConverter.GetBytes(value.Length)[0];
                array2[11] = BitConverter.GetBytes(value.Length)[1];
                Array.Copy(array, 0, array2, 12, array.Length);
                return array2;
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
