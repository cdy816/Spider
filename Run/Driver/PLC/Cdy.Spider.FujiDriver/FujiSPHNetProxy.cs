using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.FujiDriver
{
    /// <summary>
    /// 富士PLC的SPH通信协议，可以和富士PLC进行通信, 默认CPU0，需要根据实际进行调整。
    /// </summary>
    public class FujiSPHNetProxy:NetworkDeviceProxyBase
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
        public FujiSPHNetProxy(DriverRunnerBase driver) :base(driver)
        {
            base.ByteTransform = new RegularByteTransform();
            base.WordLength = 1;
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 对于 CPU0-CPU7来说是CPU的站号，分为对应 0xFE-0xF7，对于P/PE link, FL-net是模块站号，分别对应0xF6-0xEF
        /// </summary>
        public byte ConnectionID { get; set; } = 254;



        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override INetMessage GetNewNetMessage()
        {
            return new FujiSPHMessage();
        }

        /// <summary>
        /// 批量读取PLC的地址数据，长度单位为字。地址支持M1.1000，M3.1000，M10.1000，返回读取的原始字节数组。<br />
        /// Read PLC address data in batches, the length unit is words. The address supports M1.1000, M3.1000, M10.1000, 
        /// and returns the original byte array read.
        /// </summary>
        /// <param name="address">PLC的地址，支持M1.1000，M3.1000，M10.1000</param>
        /// <param name="length">读取的长度信息，按照字为单位</param>
        /// <returns>包含byte[]的原始字节数据内容</returns>
        public override byte[] Read(string address, ushort length,out bool res)
        {
            FujiSPHAddress operateResult = FujiSPHAddress.ParseFrom(address);
            if (operateResult==null)
            {
                res=false;
                return null;
            }
            else
            {
                res = true;
                return this.ReadFujiSPHAddress(operateResult, length);
            }
        }

        /// <summary>
        /// 批量写入字节数组到PLC的地址里，地址支持M1.1000，M3.1000，M10.1000，返回是否写入成功。<br />
        /// Batch write byte array to PLC address, the address supports M1.1000, M3.1000, M10.1000, 
        /// and return whether the writing is successful.
        /// </summary>
        /// <param name="address">PLC的地址，支持M1.1000，M3.1000，M10.1000</param>
        /// <param name="value">写入的原始字节数据</param>
        /// <returns>是否写入成功</returns>
        public override object Write(string address, byte[] value,out bool res)
        {
            byte[] operateResult = BuildWriteCommand(this.ConnectionID, address, value);
            if (operateResult==null)
            {
                res=false;
                return false;
            }
            else
            {
                byte[] operateResult2 = this.ReadFromCoreServer(operateResult);
                if (operateResult2==null)
                {
                    res = false;
                    return false;
                }
                else
                {
                    byte[] operateResult3 = ExtractActualData(operateResult2);
                    if (operateResult3==null)
                    {
                        res = false;
                        return false;
                    }
                    else
                    {
                        res = true;
                        return true;
                    }
                }
            }
        }

        /// <summary>
        /// 批量读取位数据的方法，需要传入位地址，读取的位长度，地址示例：M1.100.5，M3.1000.12，M10.1000.0<br />
        /// To read the bit data in batches, you need to pass in the bit address, the length of the read bit, address examples: M1.100.5, M3.1000.12, M10.1000.0
        /// </summary>
        /// <param name="address">PLC的地址，示例：M1.100.5，M3.1000.12，M10.1000.0</param>
        /// <param name="length">读取的bool长度信息</param>
        /// <returns>包含bool[]的结果对象</returns>
        public override bool[] ReadBool(string address, ushort length,out bool res)
        {
            FujiSPHAddress operateResult = FujiSPHAddress.ParseFrom(address);
            if (operateResult==null)
            {
               res = false;
                return null;
            }
            else
            {
                int num = operateResult.BitIndex + (int)length;
                int num2 = (num % 16 == 0) ? (num / 16) : (num / 16 + 1);
                byte[] operateResult2 = this.ReadFujiSPHAddress(operateResult, (ushort)num2);
                if (operateResult2==null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    res = true;
                    return operateResult2.ToBoolArray().SelectMiddle(operateResult.BitIndex, (int)length);
                }
            }
        }

        /// <summary>
        /// 批量写入位数据的方法，需要传入位地址，等待写入的boo[]数据，地址示例：M1.100.5，M3.1000.12，M10.1000.0<br />
        /// To write bit data in batches, you need to pass in the bit address and wait for the boo[] data to be written. Examples of addresses: M1.100.5, M3.1000.12, M10.1000.0
        /// </summary>
        /// <remarks>
        /// [警告] 由于协议没有提供位写入的命令，所有通过字写入间接实现，先读取字数据，修改中间的位，然后写入字数据，所以本质上不是安全的，确保相关的地址只有上位机可以写入。<br />
        /// [Warning] Since the protocol does not provide commands for bit writing, all are implemented indirectly through word writing. First read the word data, 
        /// modify the bits in the middle, and then write the word data, so it is inherently not safe. Make sure that the relevant address is only The host computer can write.
        /// </remarks>
        /// <param name="address">PLC的地址，示例：M1.100.5，M3.1000.12，M10.1000.0</param>
        /// <param name="value">等待写入的bool数组</param>
        /// <returns>是否写入成功的结果对象</returns>
        public override object Write(string address, bool[] value,out bool res)
        {
            FujiSPHAddress operateResult = FujiSPHAddress.ParseFrom(address);
            if (operateResult==null)
            {
                res=false;
                return false;
            }
            else
            {
                int num = operateResult.BitIndex + value.Length;
                int num2 = (num % 16 == 0) ? (num / 16) : (num / 16 + 1);
                byte[] operateResult2 = this.ReadFujiSPHAddress(operateResult, (ushort)num2);
                if (operateResult2==null)
                {
                    res = false;
                    return false;
                }
                else
                {
                    bool[] array = operateResult2.ToBoolArray();
                    value.CopyTo(array, operateResult.BitIndex);
                    byte[] operateResult3 = BuildWriteCommand(this.ConnectionID, address,DataExtend.BoolArrayToByte(array));
                    if (operateResult3==null)
                    {
                        res = false;
                        return false;
                    }
                    else
                    {
                        byte[] operateResult4 = this.ReadFromCoreServer(operateResult3);
                        if (operateResult4==null)
                        {
                            res = false;
                            return false;
                        }
                        else
                        {
                            byte[] operateResult5 = ExtractActualData(operateResult4);
                            if (operateResult5==null)
                            {
                                res = false;
                                return false;
                            }
                            else
                            {
                                res = true;
                                return true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private byte[] ReadFujiSPHAddress(FujiSPHAddress address, ushort length)
        {
            List<byte[]> operateResult = BuildReadCommand(this.ConnectionID, address, length);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                List<byte> list = new List<byte>();
                for (int i = 0; i < operateResult.Count; i++)
                {
                    byte[] operateResult2 = this.ReadFromCoreServer(operateResult[i]);
                    if (operateResult2==null)
                    {
                        return operateResult2;
                    }
                    byte[] operateResult3 = ExtractActualData(operateResult2);
                    if (operateResult3==null)
                    {
                        return operateResult3;
                    }
                    list.AddRange(operateResult3);
                }
               return list.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="command"></param>
        /// <param name="mode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static byte[] PackCommand(byte connectionId, byte command, byte mode, byte[] data)
        {
            bool flag = data == null;
            if (flag)
            {
                data = new byte[0];
            }
            byte[] array = new byte[20 + data.Length];
            array[0] = 251;
            array[1] = 128;
            array[2] = 128;
            array[3] = 0;
            array[4] = byte.MaxValue;
            array[5] = 123;
            array[6] = connectionId;
            array[7] = 0;
            array[8] = 17;
            array[9] = 0;
            array[10] = 0;
            array[11] = 0;
            array[12] = 0;
            array[13] = 0;
            array[14] = command;
            array[15] = mode;
            array[16] = 0;
            array[17] = 1;
            array[18] = BitConverter.GetBytes(data.Length)[0];
            array[19] = BitConverter.GetBytes(data.Length)[1];
            bool flag2 = data.Length != 0;
            if (flag2)
            {
                data.CopyTo(array, 20);
            }
            return array;
        }

        /// <summary>
        /// 构建读取数据的命令报文
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <param name="address">读取的PLC的地址</param>
        /// <param name="length">读取的长度信息，按照字为单位</param>
        /// <returns>构建成功的读取报文命令</returns>
        public static List<byte[]> BuildReadCommand(byte connectionId, string address, ushort length)
        {
            FujiSPHAddress operateResult = FujiSPHAddress.ParseFrom(address);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
               return BuildReadCommand(connectionId, operateResult, length);
            }
        }

        /// <summary>
        /// 构建读取数据的命令报文
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <param name="address">读取的PLC的地址</param>
        /// <param name="length">读取的长度信息，按照字为单位</param>
        /// <returns>构建成功的读取报文命令</returns>
        // Token: 0x0600118C RID: 4492 RVA: 0x00070F18 File Offset: 0x0006F118
        public static List<byte[]> BuildReadCommand(byte connectionId, FujiSPHAddress address, ushort length)
        {
            List<byte[]> list = new List<byte[]>();
            int[] array = DataExtend.SplitIntegerToArray((int)length, 230);
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(PackCommand(connectionId, 0, 0, new byte[]
                {
                    address.TypeCode,
                    BitConverter.GetBytes(address.AddressStart)[0],
                    BitConverter.GetBytes(address.AddressStart)[1],
                    BitConverter.GetBytes(address.AddressStart)[2],
                    BitConverter.GetBytes(array[i])[0],
                    BitConverter.GetBytes(array[i])[1]
                }));
                address.AddressStart += array[i];
            }
            return list;
        }

        /// <summary>
        /// 构建写入数据的命令报文
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <param name="address">写入的PLC的地址</param>
        /// <param name="data">原始数据内容</param>
        /// <returns>报文信息</returns>
        // Token: 0x0600118D RID: 4493 RVA: 0x00070FDC File Offset: 0x0006F1DC
        public static byte[] BuildWriteCommand(byte connectionId, string address, byte[] data)
        {
            FujiSPHAddress operateResult = FujiSPHAddress.ParseFrom(address);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                int value = data.Length / 2;
                byte[] array = new byte[6 + data.Length];
                array[0] = operateResult.TypeCode;
                array[1] = BitConverter.GetBytes(operateResult.AddressStart)[0];
                array[2] = BitConverter.GetBytes(operateResult.AddressStart)[1];
                array[3] = BitConverter.GetBytes(operateResult.AddressStart)[2];
                array[4] = BitConverter.GetBytes(value)[0];
                array[5] = BitConverter.GetBytes(value)[1];
                data.CopyTo(array, 6);
                return PackCommand(connectionId, 1, 0, array);
            }
        }

        /// <summary>
        /// 从PLC返回的报文里解析出实际的数据内容，如果发送了错误，则返回失败信息
        /// </summary>
        /// <param name="response">PLC返回的报文信息</param>
        /// <returns>是否成功的结果对象</returns>
        public static byte[] ExtractActualData(byte[] response)
        {
            try
            {
                if (response[4] > 0)
                {
                    return null;
                }
                else
                {
                    if (response.Length > 26)
                    {
                        return response.RemoveBegin(26);
                    }
                    else
                    {
                        return new byte[0];
                    }
                }
            }
            catch (Exception)
            {
            }
            return null;
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
