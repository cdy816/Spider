using Cdy.Spider.Common;
using Cdy.Spider.MelsecDriver.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver
{
    public class MelsecA1EAsciiNetProxy : NetworkDeviceProxyBase
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
        public MelsecA1EAsciiNetProxy(DriverRunnerBase driver) : base(driver)
        {
            WordLength = 1;
            //this.LogMsgFormatBinary = false;
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
            byte[] operateResult = BuildReadCommand(address, length, false, PLCNumber);
            if (operateResult == null)
            {
                res = false;
                return null;
            }
            else
            {
                byte[] operateResult2 = ReadFromCoreServer(operateResult);
                if (operateResult2 == null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    var operateResult3 = CheckResponseLegal(operateResult2);
                    if (!operateResult3)
                    {
                        res = false;
                        return null;
                    }
                    else
                    {
                        res = true;
                        return ExtractActualData(operateResult2, false);
                    }
                }
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
                return false;
            }
            else
            {
                byte[] operateResult2 = ReadFromCoreServer(operateResult);
                if (operateResult2 == null || !CheckResponseLegal(operateResult2))
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override bool[] ReadBool(string address, ushort length, out bool res)
        {
            var operateResult = BuildReadCommand(address, length, true, PLCNumber);
            if (operateResult == null)
            {
                res = false;
                return null;
            }
            else
            {
                var operateResult2 = ReadFromCoreServer(operateResult);
                if (operateResult2 == null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    var operateResult3 = CheckResponseLegal(operateResult2);
                    if (!operateResult3)
                    {
                        res = false;
                        return null;
                    }
                    else
                    {
                        var operateResult4 = ExtractActualData(operateResult2, true);
                        if (operateResult4 == null)
                        {
                            res = false;
                            return null;
                        }
                        else
                        {
                            res = true;
                            return (from m in operateResult4 select m == 1).Take(length).ToArray();
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, bool[] value, out bool res)
        {
            var operateResult = BuildWriteBoolCommand(address, value, PLCNumber);
            if (operateResult == null)
            {
                res = false;
                return false;
            }
            else
            {
                var operateResult2 = ReadFromCoreServer(operateResult);
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
            return new MelsecA1EAsciiMessage();
        }

        /// <summary>
        /// 根据类型地址长度确认需要读取的指令头
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="length">长度</param>
        /// <param name="isBit">指示是否按照位成批的读出</param>
        /// <param name="plcNumber">PLC编号</param>
        /// <returns>带有成功标志的指令数据</returns>
        // Token: 0x06000C20 RID: 3104 RVA: 0x0004DCFC File Offset: 0x0004BEFC
        public static byte[] BuildReadCommand(string address, ushort length, bool isBit, byte plcNumber)
        {
            Tuple<MelsecA1EDataType, int> operateResult = MelsecHelper.McA1EAnalysisAddress(address);
            if (operateResult == null)
            {
                return null;
            }
            else
            {
                byte value = isBit ? (byte)0 : (byte)1;
                return new byte[]
                 {
                    DataExtend.BuildAsciiBytesFrom(value)[0],
                    DataExtend.BuildAsciiBytesFrom(value)[1],
                    DataExtend.BuildAsciiBytesFrom(plcNumber)[0],
                    DataExtend.BuildAsciiBytesFrom(plcNumber)[1],
                    48,
                    48,
                    48,
                    65,
                    DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item1.DataCode)[1])[0],
                    DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item1.DataCode)[1])[1],
                    DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item1.DataCode)[0])[0],
                    DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item1.DataCode)[0])[1],
                    DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[3])[0],
                    DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[3])[1],
                    DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[2])[0],
                    DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[2])[1],
                    DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[1])[0],
                    DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[1])[1],
                    DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[0])[0],
                    DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[0])[1],
                    DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(length % 256)[0])[0],
                    DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(length % 256)[0])[1],
                    48,
                    48
                 };
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
                value = MelsecHelper.TransByteArrayToAsciiByteArray(value);
                byte[] array = new byte[24 + value.Length];
                array[0] = 48;
                array[1] = 51;
                array[2] = DataExtend.BuildAsciiBytesFrom(plcNumber)[0];
                array[3] = DataExtend.BuildAsciiBytesFrom(plcNumber)[1];
                array[4] = 48;
                array[5] = 48;
                array[6] = 48;
                array[7] = 65;
                array[8] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item1.DataCode)[1])[0];
                array[9] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item1.DataCode)[1])[1];
                array[10] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item1.DataCode)[0])[0];
                array[11] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item1.DataCode)[0])[1];
                array[12] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[3])[0];
                array[13] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[3])[1];
                array[14] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[2])[0];
                array[15] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[2])[1];
                array[16] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[1])[0];
                array[17] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[1])[1];
                array[18] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[0])[0];
                array[19] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[0])[1];
                array[20] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(value.Length / 4)[0])[0];
                array[21] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(value.Length / 4)[0])[1];
                array[22] = 48;
                array[23] = 48;
                value.CopyTo(array, 24);
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
                byte[] array = (from m in value select m ? (byte)49 : (byte)48).ToArray();
                bool flag2 = array.Length % 2 == 1;
                if (flag2)
                {
                    array = DataExtend.SpliceArray(new byte[][]
                    {
                        array,
                        new byte[]
                        {
                            48
                        }
                    });
                }
                byte[] array2 = new byte[24 + array.Length];
                array2[0] = 48;
                array2[1] = 50;
                array2[2] = DataExtend.BuildAsciiBytesFrom(plcNumber)[0];
                array2[3] = DataExtend.BuildAsciiBytesFrom(plcNumber)[1];
                array2[4] = 48;
                array2[5] = 48;
                array2[6] = 48;
                array2[7] = 65;
                array2[8] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item1.DataCode)[1])[0];
                array2[9] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item1.DataCode)[1])[1];
                array2[10] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item1.DataCode)[0])[0];
                array2[11] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item1.DataCode)[0])[1];
                array2[12] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[3])[0];
                array2[13] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[3])[1];
                array2[14] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[2])[0];
                array2[15] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[2])[1];
                array2[16] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[1])[0];
                array2[17] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[1])[1];
                array2[18] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[0])[0];
                array2[19] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(operateResult.Item2)[0])[1];
                array2[20] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(value.Length)[0])[0];
                array2[21] = DataExtend.BuildAsciiBytesFrom(BitConverter.GetBytes(value.Length)[0])[1];
                array2[22] = 48;
                array2[23] = 48;
                array.CopyTo(array2, 24);
                return array2;
            }
        }

        /// <summary>
        /// 检测反馈的消息是否合法
        /// </summary>
        /// <param name="response">接收的报文</param>
        /// <returns>是否成功</returns>
        // Token: 0x06000C23 RID: 3107 RVA: 0x0004E338 File Offset: 0x0004C538
        public static bool CheckResponseLegal(byte[] response)
        {
            if (response.Length < 4)
            {
                return false;
            }
            else
            {
                if (response[2] == 48 && response[3] == 48)
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
        /// 从PLC反馈的数据中提取出实际的数据内容，需要传入反馈数据，是否位读取
        /// </summary>
        /// <param name="response">反馈的数据内容</param>
        /// <param name="isBit">是否位读取</param>
        /// <returns>解析后的结果对象</returns>
        public static byte[] ExtractActualData(byte[] response, bool isBit)
        {
            if (isBit)
            {
                return (from m in response.RemoveBegin(4) select m == 48 ? (byte)0 : (byte)1).ToArray();
            }
            else
            {
                return MelsecHelper.TransAsciiByteArrayToByteArray(response.RemoveBegin(4));
            }
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
