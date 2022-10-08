using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Cdy.Spider.ModbusDriver
{
    internal class ModbusHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="send"></param>
        /// <param name="response"></param>
        /// <param name="crcCheck"></param>
        /// <returns></returns>
        public static byte[] ExtraRtuResponseContent(byte[] send, byte[] response, bool crcCheck = true)
        {
            bool flag = response.Length < 5;
            if (response.Length < 5)
            {
                return null;
            }
            else
            {
                bool flag2 = crcCheck && !DataExtend.CheckCRC16(response);
                if (flag2)
                {
                    return null;
                }
                else if (send[0] != response[0])
                {
                    return null;
                }
                else if (send[1] + 128 == response[1])
                {
                    return null;
                }
                else if (send[1] != response[1])
                {
                    return null;
                }
                else
                {
                    return ModbusInfo.ExtractActualData(ModbusInfo.ExplodeRtuCommandToCore(response));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modbus"></param>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] Read(IModbus modbus, string address, ushort length,out bool res)
        {
            string operateResult = modbus.TranslateToModbusAddress(address, 3);
            if (operateResult==null)
            {
                res = false;
                return null;
            }
            else
            {
                byte[][] operateResult2 = ModbusInfo.BuildReadModbusCommand(operateResult, length, modbus.Station, modbus.AddressStartWithZero, 3);
                if (operateResult2==null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    res = true;
                    return modbus.ReadFromCoreServer(operateResult2);
                }
            }
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="modbus"></param>
       /// <param name="address"></param>
       /// <param name="value"></param>
       /// <param name="res"></param>
       /// <returns></returns>
        public static object Write(IModbus modbus, string address, byte[] value,out bool res)
        {
            string operateResult = modbus.TranslateToModbusAddress(address, 16);
            if (operateResult==null)
            {
                res=false;
                return null;
            }
            else
            {
                byte[] operateResult2 = ModbusInfo.BuildWriteWordModbusCommand(operateResult, value, modbus.Station, modbus.AddressStartWithZero, 16);
                if (operateResult == null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    res = modbus.ReadFromCoreServer(operateResult2)!=null;
                    return res;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modbus"></param>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object Write(IModbus modbus, string address, short value, out bool res)
        {
            var  operateResult = modbus.TranslateToModbusAddress(address, 6);
            if (operateResult == null)
            {
                res = false;
                return null;
            }
            else
            {
                var operateResult2 = ModbusInfo.BuildWriteWordModbusCommand(operateResult, value, modbus.Station, modbus.AddressStartWithZero, 6);
                if (operateResult == null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    res = modbus.ReadFromCoreServer(operateResult2)!=null;
                    return res;
                }
            }
        }

        
        public static object Write(IModbus modbus, string address, ushort value, out bool res)
        {
            var operateResult = modbus.TranslateToModbusAddress(address, 6);
            if (operateResult == null)
            {
                res = false;
                return null;
            }
            else
            {
                var operateResult2 = ModbusInfo.BuildWriteWordModbusCommand(operateResult, value, modbus.Station, modbus.AddressStartWithZero, 6);
                if (operateResult == null)
                {
                    res = false;
                    return null;
                }
                else
                {
                   res = modbus.ReadFromCoreServer(operateResult2)!=null;
                    return res;
                }
            }
        }

       
        public static object WriteMask(IModbus modbus, string address, ushort andMask, ushort orMask,out bool res)
        {
            var operateResult = modbus.TranslateToModbusAddress(address, 22);
            if (operateResult == null)
            {
                res = false;
                return null;
            }
            else
            {
               var operateResult2 = ModbusInfo.BuildWriteMaskModbusCommand(operateResult, andMask, orMask, modbus.Station, modbus.AddressStartWithZero, 22);
                if (operateResult == null)
                {
                    res = false;
                    return null;
                }
                else
                {
                   res= modbus.ReadFromCoreServer(operateResult2)!=null;
                    return res;
                }
            }
        }

       

        public static bool[] ReadBoolHelper(IModbus modbus, string address, ushort length, byte function,out bool res)
        {
            var operateResult = modbus.TranslateToModbusAddress(address, function);
            if (operateResult==null)
            {
                res = false;
                return null;
            }
            else
            {
                if (operateResult.IndexOf('.') > 0)
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
                    ushort length2 = (ushort)(((int)length + num + 15) / 16);
                    var operateResult2 = modbus.Read(array[0], length2,out  res);
                    if (operateResult2==null||!res)
                    {
                        res = false;
                        return null;
                    }
                    else
                    {
                        res = true;
                      return (DataExtend.BytesReverseByWord(operateResult2).ToBoolArray().SelectMiddle(num, (int)length));
                    }
                }
                else
                {
                    byte[][] operateResult3 = ModbusInfo.BuildReadModbusCommand(operateResult, length, modbus.Station, modbus.AddressStartWithZero, function);
                    if (operateResult3==null)
                    {
                        res = false;
                        return null;
                    }
                    else
                    {
                        List<bool> list = new List<bool>();
                        for (int i = 0; i < operateResult3.Length; i++)
                        {
                            var operateResult4 = modbus.ReadFromCoreServer(operateResult3[i]);
                            if (operateResult4==null)
                            {
                                res = false;
                                return null;
                            }
                            int length3 = (int)operateResult3[i][4] * 256 + (int)operateResult3[i][5];
                            list.AddRange(DataExtend.ByteToBoolArray(operateResult4, length3));
                        }
                        res = true;
                       return (list.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modbus"></param>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static object Write(IModbus modbus, string address, bool[] values,out bool res)
        {
            var operateResult = modbus.TranslateToModbusAddress(address, 15);
            if (operateResult==null)
            {
                res = false;
                return null;
            }
            else
            {
                var operateResult2 = ModbusInfo.BuildWriteBoolModbusCommand(operateResult, values, modbus.Station, modbus.AddressStartWithZero, 15);
                if (operateResult2==null)
                {
                    res = false;
                    return null;
                }
                else
                {
                   res= modbus.ReadFromCoreServer(operateResult2)!=null;
                    return res;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modbus"></param>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object Write(IModbus modbus, string address, bool value,out bool res)
        {
            var operateResult = modbus.TranslateToModbusAddress(address, 5);
            if (operateResult==null)
            {
                res = false;
                return null;
            }
            else
            {
                var operateResult2 = ModbusInfo.BuildWriteBoolModbusCommand(operateResult, value, modbus.Station, modbus.AddressStartWithZero, 5);
                if (operateResult2==null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    res= modbus.ReadFromCoreServer(operateResult2)!=null;
                    return res;
                }
            }
        }
        
    }
}
