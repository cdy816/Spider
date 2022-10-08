using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.ModbusDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class ModbusRtuNetProxy: NetworkDeviceProxyBase, IModbus
    {

        #region ... Variables  ...

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
        public ModbusRtuNetProxy(DriverRunnerBase driver):base(driver)
        {
            base.ByteTransform = new ReverseWordTransform();
            base.WordLength = 1;
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

        /// <summary>
        /// 获取或设置是否启用CRC16校验码的检查功能，默认启用，如果需要忽略检查CRC16，则设置为 false 即可。<br />
        /// Gets or sets whether to enable the check function of CRC16 check code. It is enabled by default. If you need to ignore the check of CRC16, you can set it to false.
        /// </summary>
        public bool Crc16CheckEnable { get; set; } = true;
        #endregion ...Properties...

        #region ... Methods    ...

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
            return ModbusInfo.PackCommandToRtu(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="send"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public override byte[] UnpackResponseContent(byte[] send, byte[] response)
        {
            return ModbusHelper.ExtraRtuResponseContent(send, response, this.Crc16CheckEnable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public bool ReadCoil(string address,out bool res)
        {
            return this.ReadBool(address,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public bool[] ReadCoil(string address, ushort length,out bool res)
        {
            return this.ReadBool(address, length,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public bool ReadDiscrete(string address,out bool res)
        {
            return ByteTransformHelper.GetResultFromArray<bool>(this.ReadDiscrete(address, 1,out res),out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public bool[] ReadDiscrete(string address, ushort length,out bool res)
        {
            return ModbusHelper.ReadBoolHelper(this, address, length, 2,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override byte[] Read(string address, ushort length,out bool res)
        {
            return ModbusHelper.Read(this, address, length,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, byte[] value, out bool res)
        {
            return ModbusHelper.Write(this, address, value,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, short value,out bool res)
        {
            return ModbusHelper.Write(this, address, value,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, ushort value,out bool res)
        {
            return ModbusHelper.Write(this, address, value,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="andMask"></param>
        /// <param name="orMask"></param>
        /// <param name="res"></param>
        /// <returns></returns>
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
        public object WriteOneRegister(string address, ushort value,out bool res)
        {
            return this.Write(address, value,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override bool[] ReadBool(string address, ushort length,out bool res)
        {
            return ModbusHelper.ReadBoolHelper(this, address, length, 1,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, bool[] values,out bool res)
        {
            return ModbusHelper.Write(this, address, values,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, bool value,out bool res)
        {
            return ModbusHelper.Write(this, address, value,out res);
        }

        public override int[] ReadInt32(string address, ushort length, out bool res)
        {
            IByteTransform transform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return ByteTransformHelper.GetResultFromBytes<int[]>(this.Read(address, (ushort)(length * base.WordLength * 2), out res), (byte[] m) => transform.TransInt32(m, 0, (int)length), out res);
        }


        public override uint[] ReadUInt32(string address, ushort length, out bool res)
        {
            IByteTransform transform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return ByteTransformHelper.GetResultFromBytes<uint[]>(this.Read(address, (ushort)(length * base.WordLength * 2), out res), (byte[] m) => transform.TransUInt32(m, 0, (int)length), out res);
        }


        public override float[] ReadFloat(string address, ushort length, out bool res)
        {
            IByteTransform transform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return ByteTransformHelper.GetResultFromBytes<float[]>(this.Read(address, (ushort)(length * base.WordLength * 2), out res), (byte[] m) => transform.TransSingle(m, 0, (int)length), out res);
        }


        public override long[] ReadInt64(string address, ushort length, out bool res)
        {
            IByteTransform transform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return ByteTransformHelper.GetResultFromBytes<long[]>(this.Read(address, (ushort)(length * base.WordLength * 4), out res), (byte[] m) => transform.TransInt64(m, 0, (int)length), out res);
        }

 
        public override ulong[] ReadUInt64(string address, ushort length, out bool res)
        {
            IByteTransform transform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return ByteTransformHelper.GetResultFromBytes<ulong[]>(this.Read(address, (ushort)(length * base.WordLength * 4), out res), (byte[] m) => transform.TransUInt64(m, 0, (int)length), out res);
        }

      
        public override double[] ReadDouble(string address, ushort length, out bool res)
        {
            IByteTransform transform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return ByteTransformHelper.GetResultFromBytes<double[]>(this.Read(address, (ushort)(length * base.WordLength * 4), out res), (byte[] m) => transform.TransDouble(m, 0, (int)length), out res);
        }


        public override object Write(string address, int[] values, out bool res)
        {
            IByteTransform byteTransform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return this.Write(address, byteTransform.TransByte(values), out res);
        }


        public override object Write(string address, uint[] values, out bool res)
        {
            IByteTransform byteTransform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return this.Write(address, byteTransform.TransByte(values), out res);
        }


        public override object Write(string address, float[] values, out bool res)
        {
            IByteTransform byteTransform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return this.Write(address, byteTransform.TransByte(values), out res);
        }

    
        public override object Write(string address, long[] values, out bool res)
        {
            IByteTransform byteTransform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return this.Write(address, byteTransform.TransByte(values), out res);
        }

        
        public override object Write(string address, ulong[] values, out bool res)
        {
            IByteTransform byteTransform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return this.Write(address, byteTransform.TransByte(values), out res);
        }

        
        public override object Write(string address, double[] values, out bool res)
        {
            IByteTransform byteTransform = DataExtend.ExtractTransformParameter(ref address, base.ByteTransform);
            return this.Write(address, byteTransform.TransByte(values),out res);
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
