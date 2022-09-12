using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.OmronDriver.HostLink
{
    /// <summary>
    /// 
    /// </summary>
    public class OmronHostLinkHelper
    {


        /// <summary>
        /// 验证欧姆龙的Fins-TCP返回的数据是否正确的数据，如果正确的话，并返回所有的数据内容
        /// </summary>
        /// <param name="send">发送的报文信息</param>
        /// <param name="response">来自欧姆龙返回的数据内容</param>
        /// <returns>带有是否成功的结果对象</returns>
        public static byte[]? ResponseValidAnalysis(byte[] send, byte[] response, out string err)
        {
            byte[] result = null;
            if (response.Length >= 27)
            {
                if (Encoding.ASCII.GetString(send, 14, 4) != Encoding.ASCII.GetString(response, 15, 4))
                {
                    err = string.Concat(new string[]
                    {
                        "Send Command [",Encoding.ASCII.GetString(send, 14, 4),"] not the same as receive command [",Encoding.ASCII.GetString(response, 15, 4),"] "
                    });
                }
                else
                {
                    int num = Convert.ToInt32(Encoding.ASCII.GetString(response, 19, 4), 16);
                    byte[] array = new byte[0];
                    if (response.Length > 27)
                    {
                        array = DataExtend.HexStringToBytes(Encoding.ASCII.GetString(response, 23, response.Length - 27));
                    }
                    if (num > 0)
                    {
                        err = GetErrorText(num);
                        return null;
                    }
                    else
                    {
                        result = array;
                    }
                }
            }
            else
            {
                err = "OmronReceiveDataError Source Data: " + response.ToHexString(' ');
                return null;
            }
            err = "";
            return result;
        }

        /// <summary>
        /// 根据错误信息获取当前的文本描述信息
        /// </summary>
        /// <param name="error">错误代号</param>
        /// <returns>文本消息</returns>
        public static string GetErrorText(int error)
        {
            if (error <= 1284)
            {
                if (error <= 517)
                {
                    if (error == 1)
                    {
                        return "Service was canceled.";
                    }
                    switch (error)
                    {
                        case 257:
                            return "Local node is not participating in the network.";
                        case 258:
                            return "Token does not arrive.";
                        case 259:
                            return "Send was not possible during the specified number of retries.";
                        case 260:
                            return "Cannot send because maximum number of event frames exceeded.";
                        case 261:
                            return "Node address setting error occurred.";
                        case 262:
                            return "The same node address has been set twice in the same network.";
                        default:
                            switch (error)
                            {
                                case 513:
                                    return "The destination node is not in the network.";
                                case 514:
                                    return "There is no Unit with the specified unit address.";
                                case 515:
                                    return "The third node does not exist.";
                                case 516:
                                    return "The destination node is busy.";
                                case 517:
                                    return "The message was destroyed by noise";
                            }
                            break;
                    }
                }
                else if (error <= 1025)
                {
                    switch (error)
                    {
                        case 769:
                            return "An error occurred in the communications controller.";
                        case 770:
                            return "A CPU error occurred in the destination CPU Unit.";
                        case 771:
                            return "A response was not returned because an error occurred in the Board.";
                        case 772:
                            return "The unit number was set incorrectly";
                        default:
                            if (error == 1025)
                            {
                                return "The Unit/Board does not support the specified command code.";
                            }
                            break;
                    }
                }
                else
                {
                    if (error == 1026)
                    {
                        return "The command cannot be executed because the model or version is incorrect";
                    }
                    switch (error)
                    {
                        case 1281:
                            return "The destination network or node address is not set in the routing tables.";
                        case 1282:
                            return "Relaying is not possible because there are no routing tables";
                        case 1283:
                            return "There is an error in the routing tables.";
                        case 1284:
                            return "An attempt was made to send to a network that was over 3 networks away";
                    }
                }
            }
            else if (error <= 8199)
            {
                switch (error)
                {
                    case 4097:
                        return "The command is longer than the maximum permissible length.";
                    case 4098:
                        return "The command is shorter than the minimum permissible length.";
                    case 4099:
                        return "The designated number of elements differs from the number of write data items.";
                    case 4100:
                        return "An incorrect format was used.";
                    case 4101:
                        return "Either the relay table in the local node or the local network table in the relay node is incorrect.";
                    default:
                        switch (error)
                        {
                            case 4353:
                                return "The specified word does not exist in the memory area or there is no EM Area.";
                            case 4354:
                                return "The access size specification is incorrect or an odd word address is specified.";
                            case 4355:
                                return "The start address in command process is beyond the accessible area";
                            case 4356:
                                return "The end address in command process is beyond the accessible area.";
                            case 4357:
                            case 4359:
                            case 4360:
                            case 4362:
                                break;
                            case 4358:
                                return "FFFF hex was not specified.";
                            case 4361:
                                return "A large–small relationship in the elements in the command data is incorrect.";
                            case 4363:
                                return "The response format is longer than the maximum permissible length.";
                            case 4364:
                                return "There is an error in one of the parameter settings.";
                            default:
                                switch (error)
                                {
                                    case 8194:
                                        return "The program area is protected.";
                                    case 8195:
                                        return "A table has not been registered.";
                                    case 8196:
                                        return "The search data does not exist.";
                                    case 8197:
                                        return "A non-existing program number has been specified.";
                                    case 8198:
                                        return "The file does not exist at the specified file device.";
                                    case 8199:
                                        return "A data being compared is not the same.";
                                }
                                break;
                        }
                        break;
                }
            }
            else if (error <= 8712)
            {
                switch (error)
                {
                    case 8449:
                        return "The specified area is read-only.";
                    case 8450:
                        return "The program area is protected.";
                    case 8451:
                        return "The file cannot be created because the limit has been exceeded.";
                    case 8452:
                        break;
                    case 8453:
                        return "A non-existing program number has been specified.";
                    case 8454:
                        return "The file does not exist at the specified file device.";
                    case 8455:
                        return "A file with the same name already exists in the specified file device.";
                    case 8456:
                        return "The change cannot be made because doing so would create a problem.";
                    default:
                        switch (error)
                        {
                            case 8705:
                            case 8706:
                            case 8712:
                                return "The mode is incorrect.";
                            case 8707:
                                return "The PLC is in PROGRAM mode.";
                            case 8708:
                                return "The PLC is in DEBUG mode.";
                            case 8709:
                                return "The PLC is in MONITOR mode.";
                            case 8710:
                                return "The PLC is in RUN mode.";
                            case 8711:
                                return "The specified node is not the polling node.";
                        }
                        break;
                }
            }
            else
            {
                switch (error)
                {
                    case 8961:
                        return "The specified memory does not exist as a file device.";
                    case 8962:
                        return "There is no file memory.";
                    case 8963:
                        return "There is no clock.";
                    default:
                        if (error == 9217)
                        {
                            return "The data link tables have not been registered or they contain an error.";
                        }
                        break;
                }
            }
            return "UnknownError";
        }

        /// <summary>
        /// 将 fins 命令的报文打包成 HostLink 格式的报文信息，打包之后的结果可以直接发送给PLC<br />
        /// Pack the message of the fins command into the message information in the HostLink format, and the packaged result can be sent directly to the PLC
        /// </summary>
        /// <param name="hostLink">HostLink协议的plc通信对象</param>
        /// <param name="station">站号信息</param>
        /// <param name="cmd">fins命令</param>
        /// <returns>可发送PLC的完整的报文信息</returns>
        public static byte[] PackCommand(byte station, byte[] cmd, IHostLink proxy)
        {
            cmd = DataExtend.BytesToAsciiBytes(cmd);
            byte[] array = new byte[18 + cmd.Length];
            array[0] = 64;
            array[1] = DataExtend.BuildAsciiBytesFrom(station)[0];
            array[2] = DataExtend.BuildAsciiBytesFrom(station)[1];
            array[3] = 70;
            array[4] = 65;
            array[5] = proxy.ResponseWaitTime;
            array[6] = DataExtend.BuildAsciiBytesFrom(proxy.ICF)[0];
            array[7] = DataExtend.BuildAsciiBytesFrom(proxy.ICF)[1];
            array[8] = DataExtend.BuildAsciiBytesFrom(proxy.DA2)[0];
            array[9] = DataExtend.BuildAsciiBytesFrom(proxy.DA2)[1];
            array[10] = DataExtend.BuildAsciiBytesFrom(proxy.SA2)[0];
            array[11] = DataExtend.BuildAsciiBytesFrom(proxy.SA2)[1];
            array[12] = DataExtend.BuildAsciiBytesFrom(proxy.SID)[0];
            array[13] = DataExtend.BuildAsciiBytesFrom(proxy.SID)[1];
            array[array.Length - 2] = 42;
            array[array.Length - 1] = 13;
            cmd.CopyTo(array, 14);
            int num = array[0];
            for (int i = 1; i < array.Length - 4; i++)
            {
                num ^= array[i];
            }
            array[array.Length - 4] = DataExtend.BuildAsciiBytesFrom((byte)num)[0];
            array[array.Length - 3] = DataExtend.BuildAsciiBytesFrom((byte)num)[1];
            return array;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostLink"></param>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] Read(IHostLink hostLink, string address, ushort length, out bool res)
        {
            byte station = (byte)DataExtend.ExtractParameter(ref address, "s", hostLink.UnitNumber);
            List<byte[]> operateResult = OmronFinsNetHelper.BuildReadCommand(address, length, false, hostLink.ReadSplits);
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
                    byte[] operateResult2 = hostLink.ReadFromCoreServer(PackCommand(station, operateResult[i], hostLink));
                    if (operateResult2 == null)
                    {
                        res = false;
                        return null;
                    }
                    list.AddRange(operateResult2);
                }
                res = true;
                return list.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostLink"></param>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public static object Write(IHostLink hostLink, string address, byte[] value, out bool res)
        {
            byte station = (byte)DataExtend.ExtractParameter(ref address, "s", hostLink.UnitNumber);
            byte[] operateResult = OmronFinsNetHelper.BuildWriteWordCommand(address, value, false);
            if (operateResult == null)
            {
                res = false;
                return false;
            }
            else
            {
                byte[] operateResult2 = hostLink.ReadFromCoreServer(PackCommand(station, operateResult, hostLink));
                res = operateResult2 != null;
                return operateResult2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostLink"></param>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool[] ReadBool(IHostLink hostLink, string address, ushort length, out bool res)
        {
            byte station = (byte)DataExtend.ExtractParameter(ref address, "s", hostLink.UnitNumber);
            List<byte[]> operateResult = OmronFinsNetHelper.BuildReadCommand(address, length, true, 500);
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
                    byte[] operateResult2 = hostLink.ReadFromCoreServer(PackCommand(station, operateResult[i], hostLink));
                    if (operateResult2 == null || operateResult2.Length == 0)
                    {
                        res = false;
                        return null;
                    }
                    else
                    {
                        list.AddRange(from m in operateResult2
                                      select m > 0);
                    }
                }
                res = true;
                return list.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostLink"></param>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static object Write(IHostLink hostLink, string address, bool[] values, out bool res)
        {
            byte station = (byte)DataExtend.ExtractParameter(ref address, "s", hostLink.UnitNumber);
            byte[] operateResult = OmronFinsNetHelper.BuildWriteWordCommand(address, (from m in values select m ? (byte)1 : (byte)0).ToArray(), true);
            if (operateResult == null)
            {
                res = false;
                return null;
            }
            else
            {
                byte[] operateResult2 = hostLink.ReadFromCoreServer(PackCommand(station, operateResult, hostLink));
                res = operateResult2 != null;
                return operateResult2;
            }
        }
    }
}
