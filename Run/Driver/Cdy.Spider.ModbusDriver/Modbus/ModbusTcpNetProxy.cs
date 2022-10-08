using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.ModbusDriver
{
    public class ModbusTcpNetProxy:NetworkDeviceProxyBase,IModbus
    {

        #region ... Variables  ...
        private readonly SoftIncrementCount softIncrementCount;
        private byte station = 1;
        private bool isAddressStartWithZero = true;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        public ModbusTcpNetProxy(DriverRunnerBase driver):base(driver)
        {
            this.softIncrementCount = new SoftIncrementCount(65535L, 0L, 1);
            base.WordLength = 1;
            this.station = 1;
            base.ByteTransform = new ReverseWordTransform();
        }
        #endregion ...Constructor...

        #region ... Properties ...
        public bool AddressStartWithZero
        {
            get
            {
                return this.isAddressStartWithZero;
            }
            set
            {
                this.isAddressStartWithZero = value;
            }
        }

        public byte Station
        {
            get
            {
                return this.station;
            }
            set
            {
                this.station = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DataFormat DataFormat
        {
            get
            {
                return base.ByteTransform.DataFormat;
            }
            set
            {
                base.ByteTransform.DataFormat = value;
            }
        }

        public bool IsCheckMessageId { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public SoftIncrementCount MessageId
        {
            get
            {
                return this.softIncrementCount;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsStringReverse
        {
            get
            {
                return base.ByteTransform.IsStringReverseByteWord;
            }
            set
            {
                base.ByteTransform.IsStringReverseByteWord = value;
            }
        }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override int[] ReadInt32(string address, ushort length,out bool res)
        {
            IByteTransform transform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return ByteTransformHelper.GetResultFromBytes<int[]>(this.Read(address, this.GetWordLength(address, (int)length, 2), out res), (byte[] m) => transform.TransInt32(m, 0, (int)length), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override uint[] ReadUInt32(string address, ushort length, out bool res)
        {
            IByteTransform transform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return ByteTransformHelper.GetResultFromBytes<uint[]>(this.Read(address, this.GetWordLength(address, (int)length, 2), out res), (byte[] m) => transform.TransUInt32(m, 0, (int)length), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override float[] ReadFloat(string address, ushort length, out bool res)
        {
            IByteTransform transform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return ByteTransformHelper.GetResultFromBytes<float[]>(this.Read(address, this.GetWordLength(address, (int)length, 2), out res), (byte[] m) => transform.TransSingle(m, 0, (int)length), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override long[] ReadInt64(string address, ushort length, out bool res)
        {
            IByteTransform transform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return ByteTransformHelper.GetResultFromBytes<long[]>(this.Read(address, this.GetWordLength(address, (int)length, 4), out res), (byte[] m) => transform.TransInt64(m, 0, (int)length), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override ulong[] ReadUInt64(string address, ushort length, out bool res)
        {
            IByteTransform transform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return ByteTransformHelper.GetResultFromBytes<ulong[]>(this.Read(address, this.GetWordLength(address, (int)length, 4), out res), (byte[] m) => transform.TransUInt64(m, 0, (int)length), out res);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override double[] ReadDouble(string address, ushort length, out bool res)
        {
            IByteTransform transform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return ByteTransformHelper.GetResultFromBytes<double[]>(this.Read(address, this.GetWordLength(address, (int)length, 4), out res), (byte[] m) => transform.TransDouble(m, 0, (int)length), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, int[] values,out bool res)
        {
            IByteTransform byteTransform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return this.Write(address, byteTransform.TransByte(values),out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, uint[] values, out bool res)
        {
            IByteTransform byteTransform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return this.Write(address, byteTransform.TransByte(values), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, float[] values, out bool res)
        {
            IByteTransform byteTransform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return this.Write(address, byteTransform.TransByte(values), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, long[] values, out bool res)
        {
            IByteTransform byteTransform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return this.Write(address, byteTransform.TransByte(values), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, ulong[] values, out bool res)
        {
            IByteTransform byteTransform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return this.Write(address, byteTransform.TransByte(values), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, double[] values, out bool res)
        {
            IByteTransform byteTransform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return this.Write(address, byteTransform.TransByte(values), out res);
        }


        /// <summary>
        /// 读取线圈，需要指定起始地址，如果富文本地址不指定，默认使用的功能码是 0x01<br />
        /// To read the coil, you need to specify the start address. If the rich text address is not specified, the default function code is 0x01.
        /// </summary>
        /// <param name="address">起始地址，格式为"1234"</param>
        /// <returns>带有成功标志的bool对象</returns>
        public bool ReadCoil(string address,out bool res)
        {
            return this.ReadBool(address,out res);
        }

        /// <summary>
        /// 批量的读取线圈，需要指定起始地址，读取长度，如果富文本地址不指定，默认使用的功能码是 0x01<br />
        /// For batch reading coils, you need to specify the start address and read length. If the rich text address is not specified, the default function code is 0x01.
        /// </summary>
        /// <param name="address">起始地址，格式为"1234"</param>
        /// <param name="length">读取长度</param>
        /// <returns>带有成功标志的bool数组对象</returns>
        public bool[] ReadCoil(string address, ushort length,out bool res)
        {
            return this.ReadBool(address, length,out res);
        }

        /// <summary>
        /// 读取输入线圈，需要指定起始地址，如果富文本地址不指定，默认使用的功能码是 0x02<br />
        /// To read the input coil, you need to specify the start address. If the rich text address is not specified, the default function code is 0x02.
        /// </summary>
        /// <param name="address">起始地址，格式为"1234"</param>
        /// <returns>带有成功标志的bool对象</returns>
        public bool ReadDiscrete(string address,out bool res)
        {
            return ByteTransformHelper.GetResultFromArray<bool>(this.ReadDiscrete(address, 1,out res),out res);
        }

        /// <summary>
        /// 批量的读取输入点，需要指定起始地址，读取长度，如果富文本地址不指定，默认使用的功能码是 0x02<br />
        /// To read input points in batches, you need to specify the start address and read length. If the rich text address is not specified, the default function code is 0x02
        /// </summary>
        /// <param name="address">起始地址，格式为"1234"</param>
        /// <param name="length">读取长度</param>
        /// <returns>带有成功标志的bool数组对象</returns>
        public bool[] ReadDiscrete(string address, ushort length,out bool res)
        {
            return ModbusHelper.ReadBoolHelper(this, address, length, 2,out res);
        }

        /// <summary>
        /// 从Modbus服务器批量读取寄存器的信息，需要指定起始地址，读取长度，如果富文本地址不指定，默认使用的功能码是 0x03，如果需要使用04功能码，那么地址就写成 x=4;100<br />
        /// To read the register information from the Modbus server in batches, you need to specify the start address and read length. If the rich text address is not specified, 
        /// the default function code is 0x03. If you need to use the 04 function code, the address is written as x = 4; 100
        /// </summary>
        /// <param name="address">起始地址，比如"100"，"x=4;100"，"s=1;100","s=1;x=4;100"</param>
        /// <param name="length">读取的数量</param>
        /// <returns>带有成功标志的字节信息</returns>
        /// <remarks>
        /// 富地址格式，支持携带站号信息，功能码信息，具体参照类的示例代码
        /// </remarks>
        /// <example>
        /// 此处演示批量读取的示例
        /// </example>
        public override byte[] Read(string address, ushort length,out bool res)
        {
            return ModbusHelper.Read(this, address, length,out res);
        }

        /// <summary>
        /// 将数据写入到Modbus的寄存器上去，需要指定起始地址和数据内容，如果富文本地址不指定，默认使用的功能码是 0x10<br />
        /// To write data to Modbus registers, you need to specify the start address and data content. If the rich text address is not specified, the default function code is 0x10
        /// </summary>
        /// <param name="address">起始地址，比如"100"，"x=4;100"，"s=1;100","s=1;x=4;100"</param>
        /// <param name="value">写入的数据，长度根据data的长度来指示</param>
        /// <returns>返回写入结果</returns>
        /// <remarks>
        /// 富地址格式，支持携带站号信息，功能码信息，具体参照类的示例代码
        /// </remarks>
        /// <example>
        /// 此处演示批量写入的示例
        /// </example>
        public override object Write(string address, byte[] value,out bool res)
        {
            return ModbusHelper.Write(this, address, value,out res);
        }

        /// <summary>
        /// 将数据写入到Modbus的单个寄存器上去，需要指定起始地址和数据值，如果富文本地址不指定，默认使用的功能码是 0x06<br />
        /// To write data to a single register of Modbus, you need to specify the start address and data value. If the rich text address is not specified, the default function code is 0x06.
        /// </summary>
        /// <param name="address">起始地址，比如"100"，"x=4;100"，"s=1;100","s=1;x=4;100"</param>
        /// <param name="value">写入的short数据</param>
        /// <returns>是否写入成功</returns>
        public override object Write(string address, short value,out bool res)
        {
            return ModbusHelper.Write(this, address, value,out res);
        }

        /// <summary>
        /// 将数据写入到Modbus的单个寄存器上去，需要指定起始地址和数据值，如果富文本地址不指定，默认使用的功能码是 0x06<br />
        /// To write data to a single register of Modbus, you need to specify the start address and data value. If the rich text address is not specified, the default function code is 0x06.
        /// </summary>
        /// <param name="address">起始地址，比如"100"，"x=4;100"，"s=1;100","s=1;x=4;100"</param>
        /// <param name="value">写入的ushort数据</param>
        /// <returns>是否写入成功</returns>
        public override object Write(string address, ushort value,out bool res)
        {
            return ModbusHelper.Write(this, address, value,out res);
        }

        /// <summary>
        /// 向设备写入掩码数据，使用0x16功能码，需要确认对方是否支持相关的操作，掩码数据的操作主要针对寄存器。<br />
        /// To write mask data to the server, using the 0x16 function code, you need to confirm whether the other party supports related operations. 
        /// The operation of mask data is mainly directed to the register.
        /// </summary>
        /// <param name="address">起始地址，起始地址，比如"100"，"x=4;100"，"s=1;100","s=1;x=4;100"</param>
        /// <param name="andMask">等待与操作的掩码数据</param>
        /// <param name="orMask">等待或操作的掩码数据</param>
        /// <returns>是否写入成功</returns>
        public object WriteMask(string address, ushort andMask, ushort orMask,out bool res)
        {
            return ModbusHelper.WriteMask(this, address, andMask, orMask,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public object WriteOneRegister(string address, short value,out bool res)
        {
            return this.Write(address, value,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public object WriteOneRegister(string address, ushort value, out bool res)
        {
            return this.Write(address, value,out res);
        }

        /// <summary>
        /// 批量读取线圈或是离散的数据信息，需要指定地址和长度，具体的结果取决于实现，如果富文本地址不指定，默认使用的功能码是 0x01<br />
        /// To read coils or discrete data in batches, you need to specify the address and length. The specific result depends on the implementation. If the rich text address is not specified, the default function code is 0x01.
        /// </summary>
        /// <param name="address">数据地址，比如 "1234" </param>
        /// <param name="length">数据长度</param>
        /// <returns>带有成功标识的bool[]数组</returns>
        public override bool[] ReadBool(string address, ushort length,out bool res)
        {
            return ModbusHelper.ReadBoolHelper(this, address, length, 1,out res);
        }

        /// <summary>
        /// 向线圈中写入bool数组，返回是否写入成功，如果富文本地址不指定，默认使用的功能码是 0x0F<br />
        /// Write the bool array to the coil, and return whether the writing is successful. If the rich text address is not specified, the default function code is 0x0F.
        /// </summary>
        /// <param name="address">要写入的数据地址，比如"1234"</param>
        /// <param name="values">要写入的实际数组</param>
        /// <returns>返回写入结果</returns>
        public override object Write(string address, bool[] values,out bool res)
        {
            return ModbusHelper.Write(this, address, values,out res);
        }

        /// <summary>
        /// 向线圈中写入bool数值，返回是否写入成功，如果富文本地址不指定，默认使用的功能码是 0x05，
        /// 如果你的地址为字地址，例如100.2，那么将使用0x16的功能码，通过掩码的方式来修改寄存器的某一位，需要Modbus服务器支持，否则写入无效。<br />
        /// Write bool value to the coil and return whether the writing is successful. If the rich text address is not specified, the default function code is 0x05.
        /// If your address is a word address, such as 100.2, then you will use the function code of 0x16 to modify a bit of the register through a mask. 
        /// It needs Modbus server support, otherwise the writing is invalid.
        /// </summary>
        /// <param name="address">要写入的数据地址，比如"12345"</param>
        /// <param name="value">要写入的实际数据</param>
        /// <returns>返回写入结果</returns>
        public override object Write(string address, bool value,out bool res)
        {
            return ModbusHelper.Write(this, address, value,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override INetMessage GetNewNetMessage()
        {
            return new ModbusTcpMessage
            {
                IsCheckMessageId = this.IsCheckMessageId
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="modbusCode"></param>
        /// <returns></returns>
        public virtual string TranslateToModbusAddress(string address, byte modbusCode)
        {
            return address;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public override byte[] PackCommandWithHeader(byte[] command)
        {
            return ModbusInfo.PackCommandToTcp(command, (ushort)this.softIncrementCount.GetCurrentValue());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="send"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public override byte[] UnpackResponseContent(byte[] send, byte[] response)
        {
            return ModbusInfo.ExtractActualData(ModbusInfo.ExplodeTcpCommandToCore(response));
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
