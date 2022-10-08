using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver
{
    /// <summary>
    /// MelsecA3CNet1协议通信的辅助类
    /// </summary>
    public class MelsecA3CNetHelper
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
        /// 将命令进行打包传送，可选站号及是否和校验机制
        /// </summary>
        /// <param name="plc">PLC设备通信对象</param>
        /// <param name="mcCommand">mc协议的命令</param>
        /// <param name="station">PLC的站号</param>
        /// <returns>最终的原始报文信息</returns>
        public static byte[] PackCommand(IReadWriteA3C plc, byte[] mcCommand, byte station = 0)
        {
            MemoryStream memoryStream = new MemoryStream();
            bool flag = plc.Format != 3;
            if (flag)
            {
                memoryStream.WriteByte(5);
            }
            else
            {
                memoryStream.WriteByte(2);
            }
            bool flag2 = plc.Format == 2;
            if (flag2)
            {
                memoryStream.WriteByte(48);
                memoryStream.WriteByte(48);
            }
            memoryStream.WriteByte(70);
            memoryStream.WriteByte(57);
            memoryStream.WriteByte(DataExtend.BuildAsciiBytesFrom(station)[0]);
            memoryStream.WriteByte(DataExtend.BuildAsciiBytesFrom(station)[1]);
            memoryStream.WriteByte(48);
            memoryStream.WriteByte(48);
            memoryStream.WriteByte(70);
            memoryStream.WriteByte(70);
            memoryStream.WriteByte(48);
            memoryStream.WriteByte(48);
            memoryStream.Write(mcCommand, 0, mcCommand.Length);
            bool flag3 = plc.Format == 3;
            if (flag3)
            {
                memoryStream.WriteByte(3);
            }
            bool sumCheck = plc.SumCheck;
            if (sumCheck)
            {
                byte[] array = memoryStream.ToArray();
                int num = 0;
                for (int i = 1; i < array.Length; i++)
                {
                    num += array[i];
                }
                memoryStream.WriteByte(DataExtend.BuildAsciiBytesFrom((byte)num)[0]);
                memoryStream.WriteByte(DataExtend.BuildAsciiBytesFrom((byte)num)[1]);
            }
            bool flag4 = plc.Format == 4;
            if (flag4)
            {
                memoryStream.WriteByte(13);
                memoryStream.WriteByte(10);
            }
            byte[] result = memoryStream.ToArray();
            memoryStream.Dispose();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plc"></param>
        /// <returns></returns>
        private static int GetErrorCodeOrDataStartIndex(IReadWriteA3C plc)
        {
            int result = 11;
            switch (plc.Format)
            {
                case 1:
                    result = 11;
                    break;
                case 2:
                    result = 13;
                    break;
                case 3:
                    result = 15;
                    break;
                case 4:
                    result = 11;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 根据PLC返回的数据信息，获取到实际的数据内容
        /// </summary>
        /// <param name="plc">PLC设备通信对象</param>
        /// <param name="response">PLC返回的数据信息</param>
        /// <returns>带有是否成功的读取结果对象内容</returns>
        public static byte[] ExtraReadActualResponse(IReadWriteA3C plc, byte[] response)
        {
            byte[] result;
            try
            {
                int errorCodeOrDataStartIndex = GetErrorCodeOrDataStartIndex(plc);
                if (plc.Format == 1 || plc.Format == 2 || plc.Format == 4)
                {
                    bool flag2 = response[0] == 21;
                    if (response[0] != 2 || response[0] == 21)
                    {
                        return null;
                        //int num = Convert.ToInt32(Encoding.ASCII.GetString(response, errorCodeOrDataStartIndex, 4), 16);
                        //return new OperateResult<byte[]>(num, MelsecHelper.GetErrorDescription(num));
                    }
                    //if (response[0] != 2)
                    //{
                    //    return new OperateResult<byte[]>((int)response[0], "Read Faild:" + SoftBasic.GetAsciiStringRender(response));
                    //}
                }
                else
                {
                    bool flag4 = plc.Format == 3;
                    if (flag4)
                    {
                        string @string = Encoding.ASCII.GetString(response, 11, 4);
                        if (@string == "QNAK" || @string != "QACK")
                        {
                            return null;
                        }
                        //if (@string != "QACK")
                        //{
                        //    return new OperateResult<byte[]>((int)response[0], "Read Faild:" + SoftBasic.GetAsciiStringRender(response));
                        //}
                    }
                }
                int num3 = -1;
                for (int i = errorCodeOrDataStartIndex; i < response.Length; i++)
                {
                    bool flag7 = response[i] == 3;
                    if (flag7)
                    {
                        num3 = i;
                        break;
                    }
                }
                if (num3 == -1)
                {
                    num3 = response.Length;
                }
                return response.SelectMiddle(errorCodeOrDataStartIndex, num3 - errorCodeOrDataStartIndex);
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plc"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public static bool CheckWriteResponse(IReadWriteA3C plc, byte[] response)
        {
            int errorCodeOrDataStartIndex = GetErrorCodeOrDataStartIndex(plc);
            bool flag = plc.Format == 1 || plc.Format == 2;
            if (flag)
            {
                //bool flag2 = response[0] == 21;
                //if (flag2)
                //{
                //    int num = Convert.ToInt32(Encoding.ASCII.GetString(response, errorCodeOrDataStartIndex, 4), 16);
                //    return new OperateResult<byte[]>(num, MelsecHelper.GetErrorDescription(num));
                //}
                bool flag3 = response[0] != 6;
                if (response[0] != 6)
                {
                    return false;
                }
            }
            else
            {
                //bool flag4 = plc.Format == 3;
                if (plc.Format == 3)
                {
                    bool flag5 = response[0] != 2;
                    if (response[0] != 2)
                    {
                        return false;
                        // return new OperateResult<byte[]>((int)response[0], "Write Faild:" + SoftBasic.GetAsciiStringRender(response));
                    }
                    string @string = Encoding.ASCII.GetString(response, 11, 4);
                    //bool flag6 = @string == "QNAK";
                    if (@string == "QNAK" || @string != "QACK")
                    {
                        return false;
                        //int num2 = Convert.ToInt32(Encoding.ASCII.GetString(response, errorCodeOrDataStartIndex, 4), 16);
                        //return new OperateResult<byte[]>(num2, MelsecHelper.GetErrorDescription(num2));
                    }
                    //bool flag7 = @string != "QACK";
                    //if (flag7)
                    //{
                    //    return new OperateResult<byte[]>((int)response[0], "Write Faild:" + SoftBasic.GetAsciiStringRender(response));
                    //}
                }
                else
                {
                    //bool flag8 = plc.Format == 4;
                    if (plc.Format == 4)
                    {
                        //bool flag9 = response[0] == 21;
                        if (response[0] != 6)
                        {
                            return false;
                            //int num3 = Convert.ToInt32(Encoding.ASCII.GetString(response, errorCodeOrDataStartIndex, 4), 16);
                            //return new OperateResult<byte[]>(num3, MelsecHelper.GetErrorDescription(num3));
                        }
                        //bool flag10 = response[0] != 6;
                        //if (flag10)
                        //{
                        //    return new OperateResult<byte[]>((int)response[0], "Write Faild:" + SoftBasic.GetAsciiStringRender(response));
                        //}
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// 批量读取PLC的数据，以字为单位，支持读取X,Y,M,S,D,T,C，具体的地址范围需要根据PLC型号来确认
        /// </summary>
        /// <param name="plc">PLC设备通信对象</param>
        /// <param name="station"></param>
        /// <param name="address">地址信息</param>
        /// <param name="length">数据长度</param>
        /// <param name="res"></param>
        /// <returns>读取结果信息</returns>
        public static byte[] Read(IReadWriteA3C plc, string address, ushort length, out bool res)
        {
            byte station = (byte)DataExtend.ExtractParameter(ref address, "s", plc.Station);
            McAddressData operateResult = McAddressData.ParseMelsecFrom(address, length);
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
                    ushort num2 = (ushort)Math.Min(length - num, McHelper.GetReadWordLength(McType.MCAscii));
                    operateResult.Length = num2;
                    byte[] mcCommand = McAsciiHelper.BuildAsciiReadMcCoreCommand(operateResult, false);
                    byte[] operateResult2 = plc.ReadFromCoreServer(PackCommand(plc, mcCommand, station));
                    if (operateResult2 == null)
                    {
                        res = false;
                        return null;
                    }
                    byte[] operateResult3 = ExtraReadActualResponse(plc, operateResult2);
                    if (operateResult3 == null)
                    {
                        res = false;
                        return null;
                    }
                    list.AddRange(MelsecHelper.TransAsciiByteArrayToByteArray(operateResult3));
                    num += num2;
                    if (operateResult.McDataType.DataType == 0)
                    {
                        operateResult.AddressStart += num2;
                    }
                    else
                    {
                        operateResult.AddressStart += num2 * 16;
                    }
                }
                res = true;
                return list.ToArray();
            }
        }


        /// <summary>
        /// 批量写入PLC的数据，以字为单位，也就是说最少2个字节信息，支持X,Y,M,S,D,T,C，具体的地址范围需要根据PLC型号来确认
        /// </summary>
        /// <param name="plc">PLC设备通信对象</param>
        /// <param name="address">地址信息</param>
        /// <param name="value">数据值</param>
        /// <returns>是否写入成功</returns>
        public static object Write(IReadWriteA3C plc, string address, byte[] value, out bool res)
        {
            byte station = (byte)DataExtend.ExtractParameter(ref address, "s", plc.Station);
            McAddressData operateResult = McAddressData.ParseMelsecFrom(address, 0);
            if (operateResult == null)
            {
                res = false;
                return null;
            }
            else
            {
                byte[] mcCommand = McAsciiHelper.BuildAsciiWriteWordCoreCommand(operateResult, value);
                byte[] operateResult2 = plc.ReadFromCoreServer(PackCommand(plc, mcCommand, station));
                if (operateResult2 == null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    res = CheckWriteResponse(plc, operateResult2);
                    return res;
                }
            }
        }

        /// <summary>
        /// 批量读取bool类型数据，支持的类型为X,Y,S,T,C，具体的地址范围取决于PLC的类型
        /// </summary>
        /// <param name="plc">PLC设备通信对象</param>
        /// <param name="address">地址信息，比如X10,Y17，注意X，Y的地址是8进制的</param>
        /// <param name="length">读取的长度</param>
        /// <returns>读取结果信息</returns>
        public static bool[] ReadBool(IReadWriteA3C plc, string address, ushort length, out bool res)
        {
            byte station = (byte)DataExtend.ExtractParameter(ref address, "s", plc.Station);
            McAddressData operateResult = McAddressData.ParseMelsecFrom(address, length);
            if (operateResult == null)
            {
                res = false;
                return null;
            }
            else
            {
                List<bool> list = new List<bool>();
                ushort num = 0;
                while (num < length)
                {
                    ushort num2 = (ushort)Math.Min(length - num, McHelper.GetReadBoolLength(McType.MCAscii));
                    operateResult.Length = num2;
                    byte[] mcCommand = McAsciiHelper.BuildAsciiReadMcCoreCommand(operateResult, true);
                    byte[] operateResult2 = plc.ReadFromCoreServer(PackCommand(plc, mcCommand, station));
                    if (operateResult2 == null)
                    {
                        res = false;
                        return null;
                    }
                    byte[] operateResult3 = ExtraReadActualResponse(plc, operateResult2);
                    if (operateResult3 == null)
                    {
                        res = false;
                        return null;
                    }
                    list.AddRange((from m in operateResult3
                                   select m == 49).ToArray());
                    num += num2;
                    operateResult.AddressStart += num2;
                }
                res = true;
                return list.ToArray();
            }
        }

        /// <summary>
        /// 读取PLC的型号信息
        /// </summary>
        /// <param name="plc">PLC设备通信对象</param>
        /// <returns>返回型号的结果对象</returns>
        public static string ReadPlcType(IReadWriteA3C plc, out bool res)
        {
            byte[] operateResult = plc.ReadFromCoreServer(PackCommand(plc, Encoding.ASCII.GetBytes("01010000"), plc.Station));
            if (operateResult == null)
            {
                res = false;
                return string.Empty;
            }
            else
            {
                byte[] operateResult2 = ExtraReadActualResponse(plc, operateResult);
                if (operateResult2 == null)
                {
                    res = false;
                    return string.Empty;
                }
                else
                {
                    res = true;
                    return Encoding.ASCII.GetString(operateResult2, 0, 16).TrimEnd(new char[0]);
                }
            }
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
