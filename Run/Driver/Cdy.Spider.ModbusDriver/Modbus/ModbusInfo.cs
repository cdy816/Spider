using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.ModbusDriver
{
    /// <summary>
    ///  Modbus协议相关的一些信息，包括功能码定义，报文的生成的定义等等信息
    /// </summary>
    public class ModbusInfo
    {
        /// <summary>
        /// 读取线圈
        /// </summary>
        public const byte ReadCoil = 1;

        /// <summary>
        /// 读取离散量
        /// </summary>
        public const byte ReadDiscrete = 2;

        /// <summary>
        /// 读取寄存器
        /// </summary>
        public const byte ReadRegister = 3;

        /// <summary>
        /// 读取输入寄存器
        /// </summary>
        public const byte ReadInputRegister = 4;

        /// <summary>
        /// 写单个线圈
        /// </summary>
        public const byte WriteOneCoil = 5;

        /// <summary>
        /// 写单个寄存器
        /// </summary>
        public const byte WriteOneRegister = 6;

        /// <summary>
        /// 写多个线圈
        /// </summary>
        public const byte WriteCoil = 15;

        /// <summary>
        /// 写多个寄存器
        /// </summary>
        public const byte WriteRegister = 16;

        /// <summary>
        /// 使用掩码的方式写入寄存器
        /// </summary>
        public const byte WriteMaskRegister = 22;

        /// <summary>
        /// 不支持该功能码
        /// </summary>
        public const byte FunctionCodeNotSupport = 1;

        /// <summary>
        /// 该地址越界
        /// </summary>
        public const byte FunctionCodeOverBound = 2;

        /// <summary>
        /// 读取长度超过最大值
        /// </summary>
        public const byte FunctionCodeQuantityOver = 3;

        /// <summary>
        /// 读写异常
        /// </summary>
        public const byte FunctionCodeReadWriteException = 4;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mAddress"></param>
        /// <param name="isStartWithZero"></param>
        /// <exception cref="Exception"></exception>
        private static void CheckModbusAddressStart(ModbusAddress mAddress, bool isStartWithZero)
        {
            bool flag = !isStartWithZero;
            if (!isStartWithZero)
            {
                if (mAddress.AddressStart < 1)
                {
                    throw new Exception("ModbusAddressMustMoreThanOne");
                }
                mAddress.AddressStart -= 1;
            }
        }

        /// <summary>
        /// 构建Modbus读取数据的核心报文，需要指定地址，长度，站号，是否起始地址0，默认的功能码应该根据bool或是字来区分<br />
        /// To construct the core message of Modbus reading data, you need to specify the address, length, station number, 
        /// whether the starting address is 0, and the default function code should be distinguished according to bool or word
        /// </summary>
        /// <param name="address">Modbus的富文本地址</param>
        /// <param name="length">读取的数据长度</param>
        /// <param name="station">默认的站号信息</param>
        /// <param name="isStartWithZero">起始地址是否从0开始</param>
        /// <param name="defaultFunction">默认的功能码</param>
        /// <returns>包含最终命令的结果对象</returns>
        public static byte[][] BuildReadModbusCommand(string address, ushort length, byte station, bool isStartWithZero, byte defaultFunction)
        {
            byte[][] result;
            try
            {
                ModbusAddress mAddress = new ModbusAddress(address, station, defaultFunction);
                ModbusInfo.CheckModbusAddressStart(mAddress, isStartWithZero);
                result = ModbusInfo.BuildReadModbusCommand(mAddress, length);
            }
            catch (Exception)
            {
               return null;
            }
            return result;
        }

        /// <summary>
        /// 构建Modbus读取数据的核心报文，需要指定地址，长度，站号，是否起始地址0，默认的功能码应该根据bool或是字来区分<br />
        /// To construct the core message of Modbus reading data, you need to specify the address, length, station number, 
        /// whether the starting address is 0, and the default function code should be distinguished according to bool or word
        /// </summary>
        /// <param name="mAddress">Modbus的富文本地址</param>
        /// <param name="length">读取的数据长度</param>
        /// <returns>包含最终命令的结果对象</returns>
        public static byte[][] BuildReadModbusCommand(ModbusAddress mAddress, ushort length)
        {
            List<byte[]> list = new List<byte[]>();
            bool flag = mAddress.Function == 1 || mAddress.Function == 2 || mAddress.Function == 3 || mAddress.Function == 4;
            if (flag)
            {
                Tuple<int[], int[]> operateResult = DataExtend.SplitReadLength((int)mAddress.AddressStart, length, (mAddress.Function == 1 || mAddress.Function == 2) ? (ushort)2000 : (ushort)120);
                for (int i = 0; i < operateResult.Item1.Length; i++)
                {
                    list.Add(new byte[]
                    {
                        (byte)mAddress.Station,
                        (byte)mAddress.Function,
                        BitConverter.GetBytes(operateResult.Item1[i])[1],
                        BitConverter.GetBytes(operateResult.Item1[i])[0],
                        BitConverter.GetBytes(operateResult.Item2[i])[1],
                        BitConverter.GetBytes(operateResult.Item2[i])[0]
                    });
                }
            }
            else
            {
                list.Add(new byte[]
                {
                    (byte)mAddress.Station,
                    (byte)mAddress.Function,
                    BitConverter.GetBytes(mAddress.AddressStart)[1],
                    BitConverter.GetBytes(mAddress.AddressStart)[0],
                    BitConverter.GetBytes(length)[1],
                    BitConverter.GetBytes(length)[0]
                });
            }
            return list.ToArray();
        }

        /// <summary>
        /// 构建Modbus写入bool数据的核心报文，需要指定地址，长度，站号，是否起始地址0，默认的功能码<br />
        /// To construct the core message that Modbus writes to bool data, you need to specify the address, length,
        /// station number, whether the starting address is 0, and the default function code
        /// </summary>
        /// <param name="address">Modbus的富文本地址</param>
        /// <param name="values">bool数组的信息</param>
        /// <param name="station">默认的站号信息</param>
        /// <param name="isStartWithZero">起始地址是否从0开始</param>
        /// <param name="defaultFunction">默认的功能码</param>
        /// <returns>包含最终命令的结果对象</returns>
        public static byte[] BuildWriteBoolModbusCommand(string address, bool[] values, byte station, bool isStartWithZero, byte defaultFunction)
        {
            byte[] result;
            try
            {
                ModbusAddress mAddress = new ModbusAddress(address, station, defaultFunction);
                ModbusInfo.CheckModbusAddressStart(mAddress, isStartWithZero);
                result = ModbusInfo.BuildWriteBoolModbusCommand(mAddress, values);
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        /// <summary>
        /// 构建Modbus写入bool数据的核心报文，需要指定地址，长度，站号，是否起始地址0，默认的功能码<br />
        /// To construct the core message that Modbus writes to bool data, you need to specify the address, length, station number, whether the starting address is 0, and the default function code
        /// </summary>
        /// <param name="address">Modbus的富文本地址</param>
        /// <param name="value">bool的信息</param>
        /// <param name="station">默认的站号信息</param>
        /// <param name="isStartWithZero">起始地址是否从0开始</param>
        /// <param name="defaultFunction">默认的功能码</param>
        /// <returns>包含最终命令的结果对象</returns>
        public static byte[] BuildWriteBoolModbusCommand(string address, bool value, byte station, bool isStartWithZero, byte defaultFunction)
        {
            byte[] result;
            try
            {
                bool flag = address.IndexOf('.') <= 0;
                if (flag)
                {
                    ModbusAddress mAddress = new ModbusAddress(address, station, defaultFunction);
                    ModbusInfo.CheckModbusAddressStart(mAddress, isStartWithZero);
                    result = ModbusInfo.BuildWriteBoolModbusCommand(mAddress, value);
                }
                else
                {
                    int num = Convert.ToInt32(address.Substring(address.IndexOf('.') + 1));
                    //bool flag3 = num < 0 || num > 15;
                    if (num < 0 || num > 15)
                    {
                        return null;
                    }
                    else
                    {
                        int num2 = 1 << num;
                        int num3 = ~num2;
                        //bool flag4 = !value;
                        if (!value)
                        {
                            num2 = 0;
                        }
                        return  ModbusInfo.BuildWriteMaskModbusCommand(address.Substring(0, address.IndexOf('.')), (ushort)num3, (ushort)num2, station, isStartWithZero, 22);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }

        /// <summary>
        /// 构建Modbus写入bool数组的核心报文，需要指定地址，长度，站号，是否起始地址0，默认的功能码<br />
        /// To construct the core message that Modbus writes to the bool array, you need to specify the address, length, 
        /// station number, whether the starting address is 0, and the default function code
        /// </summary>
        /// <param name="mAddress">Modbus的富文本地址</param>
        /// <param name="values">bool数组的信息</param>
        /// <returns>包含最终命令的结果对象</returns>
        public static byte[] BuildWriteBoolModbusCommand(ModbusAddress mAddress, bool[] values)
        {
            try
            {
                byte[] array = DataExtend.BoolArrayToByte(values);
                byte[] array2 = new byte[7 + array.Length];
                array2[0] = (byte)mAddress.Station;
                array2[1] = (byte)mAddress.Function;
                array2[2] = BitConverter.GetBytes(mAddress.AddressStart)[1];
                array2[3] = BitConverter.GetBytes(mAddress.AddressStart)[0];
                array2[4] = (byte)(values.Length / 256);
                array2[5] = (byte)(values.Length % 256);
                array2[6] = (byte)array.Length;
                array.CopyTo(array2, 7);
               return (array2);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 构建Modbus写入bool数据的核心报文，需要指定地址，长度，站号，是否起始地址0，默认的功能码<br />
        /// To construct the core message that Modbus writes to bool data, you need to specify the address, length, station number, whether the starting address is 0, and the default function code
        /// </summary>
        /// <param name="mAddress">Modbus的富文本地址</param>
        /// <param name="value">bool数据的信息</param>
        /// <returns>包含最终命令的结果对象</returns>
        public static byte[] BuildWriteBoolModbusCommand(ModbusAddress mAddress, bool value)
        {
            byte[] array = new byte[6];
            array[0] = (byte)mAddress.Station;
            array[1] = (byte)mAddress.Function;
            array[2] = BitConverter.GetBytes(mAddress.AddressStart)[1];
            array[3] = BitConverter.GetBytes(mAddress.AddressStart)[0];
            if (value)
            {
                array[4] = byte.MaxValue;
                array[5] = 0;
            }
            else
            {
                array[4] = 0;
                array[5] = 0;
            }
            return array;
        }

        /// <summary>
        /// 构建Modbus写入字数据的核心报文，需要指定地址，长度，站号，是否起始地址0，默认的功能码<br />
        /// To construct the core message of Modbus writing word data, you need to specify the address, length, 
        /// station number, whether the starting address is 0, and the default function code
        /// </summary>
        /// <param name="address">Modbus的富文本地址</param>
        /// <param name="values">bool数组的信息</param>
        /// <param name="station">默认的站号信息</param>
        /// <param name="isStartWithZero">起始地址是否从0开始</param>
        /// <param name="defaultFunction">默认的功能码</param>
        /// <returns>包含最终命令的结果对象</returns>
        public static byte[] BuildWriteWordModbusCommand(string address, byte[] values, byte station, bool isStartWithZero, byte defaultFunction)
        {
            try
            {
                ModbusAddress modbusAddress = new ModbusAddress(address, station, defaultFunction);
                bool flag = modbusAddress.Function == 3;
                if (flag)
                {
                    modbusAddress.Function = (int)defaultFunction;
                }
                ModbusInfo.CheckModbusAddressStart(modbusAddress, isStartWithZero);
                return  ModbusInfo.BuildWriteWordModbusCommand(modbusAddress, values);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 构建Modbus写入字数据的核心报文，需要指定地址，长度，站号，是否起始地址0，默认的功能码<br />
        /// To construct the core message of Modbus writing word data, you need to specify the address, length, 
        /// station number, whether the starting address is 0, and the default function code
        /// </summary>
        /// <param name="address">Modbus的富文本地址</param>
        /// <param name="value">short数据信息</param>
        /// <param name="station">默认的站号信息</param>
        /// <param name="isStartWithZero">起始地址是否从0开始</param>
        /// <param name="defaultFunction">默认的功能码</param>
        /// <returns>包含最终命令的结果对象</returns>
        public static byte[] BuildWriteWordModbusCommand(string address, short value, byte station, bool isStartWithZero, byte defaultFunction)
        {
            try
            {
                ModbusAddress modbusAddress = new ModbusAddress(address, station, defaultFunction);
                bool flag = modbusAddress.Function == 3;
                if (flag)
                {
                    modbusAddress.Function = (int)defaultFunction;
                }
                ModbusInfo.CheckModbusAddressStart(modbusAddress, isStartWithZero);
                return ModbusInfo.BuildWriteOneRegisterModbusCommand(modbusAddress, value);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 构建Modbus写入字数据的核心报文，需要指定地址，长度，站号，是否起始地址0，默认的功能码<br />
        /// To construct the core message of Modbus writing word data, you need to specify the address, length, 
        /// station number, whether the starting address is 0, and the default function code
        /// </summary>
        /// <param name="address">Modbus的富文本地址</param>
        /// <param name="value">bool数组的信息</param>
        /// <param name="station">默认的站号信息</param>
        /// <param name="isStartWithZero">起始地址是否从0开始</param>
        /// <param name="defaultFunction">默认的功能码</param>
        /// <returns>包含最终命令的结果对象</returns>
        public static byte[] BuildWriteWordModbusCommand(string address, ushort value, byte station, bool isStartWithZero, byte defaultFunction)
        {
            try
            {
                ModbusAddress modbusAddress = new ModbusAddress(address, station, defaultFunction);
                bool flag = modbusAddress.Function == 3;
                if (flag)
                {
                    modbusAddress.Function = (int)defaultFunction;
                }
                ModbusInfo.CheckModbusAddressStart(modbusAddress, isStartWithZero);
                return ModbusInfo.BuildWriteOneRegisterModbusCommand(modbusAddress, value);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 构建Modbus写入掩码的核心报文，需要指定地址，长度，站号，是否起始地址0，默认的功能码<br />
        /// To construct the Modbus write mask core message, you need to specify the address, length, 
        /// station number, whether the starting address is 0, and the default function code
        /// </summary>
        /// <param name="address">Modbus的富文本地址</param>
        /// <param name="andMask">进行与操作的掩码信息</param>
        /// <param name="orMask">进行或操作的掩码信息</param>
        /// <param name="station">默认的站号信息</param>
        /// <param name="isStartWithZero">起始地址是否从0开始</param>
        /// <param name="defaultFunction">默认的功能码</param>
        /// <returns>包含最终命令的结果对象</returns>
        public static byte[] BuildWriteMaskModbusCommand(string address, ushort andMask, ushort orMask, byte station, bool isStartWithZero, byte defaultFunction)
        {
            try
            {
                ModbusAddress modbusAddress = new ModbusAddress(address, station, defaultFunction);
                bool flag = modbusAddress.Function == 3;
                if (flag)
                {
                    modbusAddress.Function = (int)defaultFunction;
                }
                ModbusInfo.CheckModbusAddressStart(modbusAddress, isStartWithZero);
                return ModbusInfo.BuildWriteMaskModbusCommand(modbusAddress, andMask, orMask);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 构建Modbus写入字数据的核心报文，需要指定地址，长度，站号，是否起始地址0，默认的功能码<br />
        /// To construct the core message of Modbus writing word data, you need to specify the address, length, 
        /// station number, whether the starting address is 0, and the default function code
        /// </summary>
        /// <param name="mAddress">Modbus的富文本地址</param>
        /// <param name="values">bool数组的信息</param>
        /// <returns>包含最终命令的结果对象</returns>
        public static byte[] BuildWriteWordModbusCommand(ModbusAddress mAddress, byte[] values)
        {
            byte[] array = new byte[7 + values.Length];
            array[0] = (byte)mAddress.Station;
            array[1] = (byte)mAddress.Function;
            array[2] = BitConverter.GetBytes(mAddress.AddressStart)[1];
            array[3] = BitConverter.GetBytes(mAddress.AddressStart)[0];
            array[4] = (byte)(values.Length / 2 / 256);
            array[5] = (byte)(values.Length / 2 % 256);
            array[6] = (byte)values.Length;
            values.CopyTo(array, 7);
            return array;
        }

        /// <summary>
        /// 构建Modbus写入掩码数据的核心报文，需要指定地址，长度，站号，是否起始地址0，默认的功能码<br />
        /// To construct the core message of Modbus writing mask data, you need to specify the address, length, 
        /// station number, whether the starting address is 0, and the default function code
        /// </summary>
        /// <param name="mAddress">Modbus的富文本地址</param>
        /// <param name="andMask">等待进行与操作的掩码</param>
        /// <param name="orMask">等待进行或操作的掩码</param>
        /// <returns>包含最终命令的结果对象</returns>
        public static byte[] BuildWriteMaskModbusCommand(ModbusAddress mAddress, ushort andMask, ushort orMask)
        {
            return (new byte[]
            {
                (byte)mAddress.Station,
                (byte)mAddress.Function,
                BitConverter.GetBytes(mAddress.AddressStart)[1],
                BitConverter.GetBytes(mAddress.AddressStart)[0],
                BitConverter.GetBytes(andMask)[1],
                BitConverter.GetBytes(andMask)[0],
                BitConverter.GetBytes(orMask)[1],
                BitConverter.GetBytes(orMask)[0]
            });
        }

        /// <summary>
        /// 构建Modbus写入字数据的核心报文，需要指定地址，长度，站号，是否起始地址0，默认的功能码<br />
        /// To construct the core message of Modbus writing word data, you need to specify the address, length, 
        /// station number, whether the starting address is 0, and the default function code
        /// </summary>
        /// <param name="mAddress">Modbus的富文本地址</param>
        /// <param name="value">short的值</param>
        /// <returns>包含最终命令的结果对象</returns>
        public static byte[] BuildWriteOneRegisterModbusCommand(ModbusAddress mAddress, short value)
        {
            return (new byte[]
            {
                (byte)mAddress.Station,
                (byte)mAddress.Function,
                BitConverter.GetBytes(mAddress.AddressStart)[1],
                BitConverter.GetBytes(mAddress.AddressStart)[0],
                BitConverter.GetBytes(value)[1],
                BitConverter.GetBytes(value)[0]
            });
        }

        /// <summary>
        /// 构建Modbus写入字数据的核心报文，需要指定地址，长度，站号，是否起始地址0，默认的功能码<br />
        /// To construct the core message of Modbus writing word data, you need to specify the address, length, 
        /// station number, whether the starting address is 0, and the default function code
        /// </summary>
        /// <param name="mAddress">Modbus的富文本地址</param>
        /// <param name="value">ushort的值</param>
        /// <returns>包含最终命令的结果对象</returns>
        public static byte[] BuildWriteOneRegisterModbusCommand(ModbusAddress mAddress, ushort value)
        {
            return (new byte[]
            {
                (byte)mAddress.Station,
                (byte)mAddress.Function,
                BitConverter.GetBytes(mAddress.AddressStart)[1],
                BitConverter.GetBytes(mAddress.AddressStart)[0],
                BitConverter.GetBytes(value)[1],
                BitConverter.GetBytes(value)[0]
            });
        }

        /// <summary>
        /// 从返回的modbus的书内容中，提取出真实的数据，适用于写入和读取操作<br />
        /// Extract real data from the content of the returned modbus book, suitable for writing and reading operations
        /// </summary>
        /// <param name="response">返回的核心modbus报文信息</param>
        /// <returns>结果数据内容</returns>
        public static byte[] ExtractActualData(byte[] response)
        {
            try
            {
                if (response[1] >= 128)
                {
                    return null;
                }
                else
                {
                    bool flag2 = response.Length > 3;
                    if (flag2)
                    {
                       return (DataExtend.ArrayRemoveBegin<byte>(response, 3));
                    }
                    else
                    {
                       return (new byte[0]);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 将modbus指令打包成Modbus-Tcp指令，需要指定ID信息来添加6个字节的报文头<br />
        /// Pack the Modbus command into Modbus-Tcp command, you need to specify the ID information to add a 6-byte message header
        /// </summary>
        /// <param name="modbus">Modbus核心指令</param>
        /// <param name="id">消息的序号</param>
        /// <returns>Modbus-Tcp指令</returns>
        public static byte[] PackCommandToTcp(byte[] modbus, ushort id)
        {
            byte[] array = new byte[modbus.Length + 6];
            array[0] = BitConverter.GetBytes(id)[1];
            array[1] = BitConverter.GetBytes(id)[0];
            array[4] = BitConverter.GetBytes(modbus.Length)[1];
            array[5] = BitConverter.GetBytes(modbus.Length)[0];
            modbus.CopyTo(array, 6);
            return array;
        }

        /// <summary>
        /// 将modbus-tcp的报文数据重新还原成modbus指令，移除6个字节的报文头数据<br />
        /// Re-modify the message data of modbus-tcp into the modbus command, remove the 6-byte message header data
        /// </summary>
        /// <param name="modbusTcp">modbus-tcp的报文</param>
        /// <returns>modbus数据报文</returns>
        public static byte[] ExplodeTcpCommandToCore(byte[] modbusTcp)
        {
            return modbusTcp.RemoveBegin(6);
        }

        /// <summary>
        /// 将modbus-rtu的数据重新还原成modbus数据，移除CRC校验的内容<br />
        /// Restore the data of modbus-rtu to modbus data again, remove the content of CRC check
        /// </summary>
        /// <param name="modbusRtu">modbus-rtu的报文</param>
        /// <returns>modbus数据报文</returns>
        public static byte[] ExplodeRtuCommandToCore(byte[] modbusRtu)
        {
            return modbusRtu.RemoveLast(2);
        }

        /// <summary>
        /// 将modbus指令打包成Modbus-Rtu指令，在报文的末尾添加CRC16的校验码<br />
        /// Pack the modbus instruction into Modbus-Rtu instruction, add CRC16 check code at the end of the message
        /// </summary>
        /// <param name="modbus">Modbus指令</param>
        /// <returns>Modbus-Rtu指令</returns>
        public static byte[] PackCommandToRtu(byte[] modbus)
        {
            return DataExtend.CRC16(modbus);
        }

        /// <summary>
        /// 将一个modbus核心的数据报文，转换成modbus-ascii的数据报文，增加LRC校验，增加首尾标记数据<br />
        /// Convert a Modbus core data message into a Modbus-ascii data message, add LRC check, and add head and tail tag data
        /// </summary>
        /// <param name="modbus">modbus-rtu的完整报文，携带相关的校验码</param>
        /// <returns>可以用于直接发送的modbus-ascii的报文</returns>
        public static byte[] TransModbusCoreToAsciiPackCommand(byte[] modbus)
        {
            byte[] inBytes = DataExtend.LRC(modbus);
            byte[] array = DataExtend.BytesToAsciiBytes(inBytes);
            return DataExtend.SpliceArray<byte>(new byte[][]
            {
                new byte[]
                {
                    58
                },
                array,
                new byte[]
                {
                    13,
                    10
                }
            });
        }

        /// <summary>
        /// 将一个modbus-ascii的数据报文，转换成的modbus核心数据报文，移除首尾标记，移除LRC校验<br />
        /// Convert a Modbus-ascii data message into a Modbus core data message, remove the first and last tags, and remove the LRC check
        /// </summary>
        /// <param name="modbusAscii">modbus-ascii的完整报文，携带相关的校验码</param>
        /// <returns>可以用于直接发送的modbus的报文</returns>
        public static byte[] TransAsciiPackCommandToCore(byte[] modbusAscii)
        {
            try
            {
                if (modbusAscii[0] != 58 || modbusAscii[modbusAscii.Length - 2] != 13 || modbusAscii[modbusAscii.Length - 1] != 10)
                {
                    return null;
                }
                else
                {
                    byte[] array = DataExtend.AsciiBytesToBytes(modbusAscii.RemoveDouble(1, 2));
                    if (!DataExtend.CheckLRC(array))
                    {
                        return null;
                    }
                    else
                    {
                        return (array.RemoveLast(1));
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 分析Modbus协议的地址信息，该地址适应于tcp及rtu模式<br />
        /// Analysis of the address information of Modbus protocol, the address is adapted to tcp and rtu mode
        /// </summary>
        /// <param name="address">带格式的地址，比如"100"，"x=4;100"，"s=1;100","s=1;x=4;100"</param>
        /// <param name="defaultStation">默认的站号信息</param>
        /// <param name="isStartWithZero">起始地址是否从0开始</param>
        /// <param name="defaultFunction">默认的功能码信息</param>
        /// <returns>转换后的地址信息</returns>
        public static ModbusAddress AnalysisAddress(string address, byte defaultStation, bool isStartWithZero, byte defaultFunction)
        {
            try
            {
                ModbusAddress modbusAddress = new ModbusAddress(address, defaultStation, defaultFunction);
                if (!isStartWithZero)
                {
                    bool flag2 = modbusAddress.AddressStart < 1;
                    if (modbusAddress.AddressStart < 1)
                    {
                        return null;
                    }
                    modbusAddress.AddressStart -= 1;
                }
               return (modbusAddress);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static bool CheckRtuReceiveDataComplete(byte[] response)
        {
            if (response.Length > 2)
            {
                if (response[1] == 6 || response[1] == 16 || response[1] == 15 || response[1] == 5)
                {
                    return response.Length >= 8;
                }
                else if (response[1] == 1 || response[1] == 2 || response[1] == 3 || response[1] == 4)
                {
                    return response.Length >= (int)(response[2] + 3 + 2);
                }
                else if (response[1] == 22)
                {
                    return response.Length >= 10;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receive"></param>
        /// <returns></returns>
        public static bool CheckServerRtuReceiveDataComplete(byte[] receive)
        {
            if (receive.Length > 2)
            {
                if (receive[1] == 16 || receive[1] == 15)
                {
                    return receive.Length > 8 && receive.Length >= (int)(receive[6] + 7 + 2);
                }
                else if (receive[1] == 1 || receive[1] == 2 || receive[1] == 3 || receive[1] == 4 || receive[1] == 6 || receive[1] == 5)
                {
                    return receive.Length >= 8;
                }
                else if (receive[1] == 22)
                {
                    return receive.Length >= 10;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modbusAscii"></param>
        /// <returns></returns>
        public static bool CheckAsciiReceiveDataComplete(byte[] modbusAscii)
        {
            return ModbusInfo.CheckAsciiReceiveDataComplete(modbusAscii, modbusAscii.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modbusAscii"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool CheckAsciiReceiveDataComplete(byte[] modbusAscii, int length)
        {
            return length > 5 && (modbusAscii[0] == 58 && modbusAscii[length - 2] == 13) && modbusAscii[length - 1] == 10;
        }


    }
}
