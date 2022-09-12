using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Cdy.Spider.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class DeviceProxyBase
    {

        #region ... Variables  ...
        protected DriverRunnerBase mDriver;

        private IByteTransform byteTransform;

        private AutoResetEvent autoResetEvent;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        public DeviceProxyBase(DriverRunnerBase driver)
        {
            mDriver = driver;
        }

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 一个字单位的数据表示的地址长度，西门子为2，三菱，欧姆龙，modbusTcp就为1，AB PLC无效<br />
        /// The address length represented by one word of data, Siemens is 2, Mitsubishi, Omron, modbusTcp is 1, AB PLC is invalid
        /// </summary>
        /// <remarks>
        /// 对设备来说，一个地址的数据对应的字节数，或是1个字节或是2个字节，4个字节，通常是这四个选择，当设置为0时，则表示4字节的地址长度信息<br />
        /// For the device, the number of bytes corresponding to the data of an address, either 1 byte or 2 bytes, 4 bytes, usually these four choices, when set to 0, it means 4 words Section address length information
        /// </remarks>
        protected ushort WordLength { get; set; } = 1;

        /// <summary>
        /// 当前的数据变换机制，当你需要从字节数据转换类型数据的时候需要。<br />
        /// The current data transformation mechanism is required when you need to convert type data from byte data.
        /// </summary>
        /// <remarks>
        /// 在HSL里提供了三种数据变换机制，分别是 <see cref="T:HslCommunication.Core.RegularByteTransform" />, <see cref="T:HslCommunication.Core.ReverseBytesTransform" />,
        /// <see cref="T:HslCommunication.Core.ReverseWordTransform" />，各自的<see cref="T:HslCommunication.Core.DataFormat" />属性也可以自定调整，基本满足所有的情况使用。<br />
        /// Three data transformation mechanisms are provided in HSL, namely <see cref="T:HslCommunication.Core.RegularByteTransform" />, <see cref="T:HslCommunication.Core.ReverseBytesTransform" />, 
        /// <see cref="T:HslCommunication.Core.ReverseWordTransform" />, and their respective <see cref="T:HslCommunication.Core.DataFormat" /> property can also be adjusted by itself, basically satisfying all situations.
        /// </remarks>
        /// <example>
        /// 主要是用来转换数据类型的，下面仅仅演示了2个方法，其他的类型转换，类似处理。
        /// </example>
        public IByteTransform ByteTransform
        {
            get
            {
                return this.byteTransform;
            }
            set
            {
                this.byteTransform = value;
            }
        }

        /// <summary>
        /// 获取或设置接收服务器反馈的时间，如果为负数，则不接收反馈 <br />
        /// Gets or sets the time to receive server feedback, and if it is a negative number, does not receive feedback
        /// </summary>
        /// <example>
        /// 设置1秒的接收超时的示例
        /// </example>
        /// <remarks>
        /// 超时的通常原因是服务器端没有配置好，导致访问失败，为了不卡死软件，所以有了这个超时的属性。
        /// </remarks>
        public int ReceiveTimeOut
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置在正式接收对方返回数据前的时候，需要休息的时间，当设置为0的时候，不需要休息。<br />
        /// Get or set the time required to rest before officially receiving the data from the other party. When it is set to 0, no rest is required.
        /// </summary>
        public int SleepTime
        {
            get;
            set;
        }

        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public virtual byte[] Read(string address, ushort length,out bool res)
        {
            res = false;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public virtual object Write(string address, byte[] value,out bool result)
        {
            result = false;
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual bool[] ReadBool(string address, ushort length,out bool res)
        {
            res = false;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual bool ReadBool(string address,out bool res)
        {
            var re = this.ReadBool(address, 1, out bool r);
            if(r)
            {
                return ByteTransformHelper.GetResultFromArray<bool>(re, out res);
            }
            else
            {
                res = false;
                return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual object Write(string address, bool[] value,out bool res)
        {
            res=false;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual object Write(string address, bool value,out bool res)
        {
            return this.Write(address, new bool[]
            {
                value
            },out res);
        }


        //public virtual T Read<T>(out bool res) where T : class, new()
        //{
        //    var tp = typeof(T).Name.ToLower();
        //    switch (tp)
        //    {
        //        case "byte":
                
        //            break;
        //    }

        //    return Read<T>(mDriver);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="data"></param>
        ///// <param name="res"></param>
        ///// <returns></returns>
        //public virtual byte[] Write<T>(T data, out bool res) where T : class, new()
        //{
        //    return Write<T>(data, mDriver, out res);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public short ReadInt16(string address,out bool res)
        {
            var re = this.ReadInt16(address, 1, out res);
            if (re != null && re.Length > 0)
            {
                return re[0];
            }
            else
            {
                return default(short);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual short[] ReadInt16(string address, ushort length,out bool res)
        {
            var re = this.Read(address, this.GetWordLength(address, (int)length, 1),out res);
            if (res && re.Length>0)
                return this.ByteTransform.TransInt16(re, 0, (int)length);
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public ushort ReadUInt16(string address,out bool res)
        {
           var re = this.ReadUInt16(address, 1, out res);
            if(re!=null&&re.Length>0)
            {
                return re[0];
            }
            else
            {
                return default(ushort);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual ushort[] ReadUInt16(string address, ushort length,out bool res)
        {
            var re = this.Read(address, this.GetWordLength(address, (int)length, 1), out res);
            if (res && re.Length > 0)
                return this.ByteTransform.TransUInt16(re, 0, (int)length);
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public int ReadInt32(string address,out bool res)
        {
            var re = this.ReadInt32(address, 1, out res);
            if (re != null && re.Length > 0)
            {
                return re[0];
            }
            else
            {
                return default(int);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual int[] ReadInt32(string address, ushort length,out bool res)
        {
            var re = this.Read(address, this.GetWordLength(address, (int)length, 2), out res);
            if (res && re.Length > 0)
                return this.ByteTransform.TransInt32(re, 0, (int)length);
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public uint ReadUInt32(string address,out bool res)
        {
            var re = this.ReadUInt32(address, 1, out res);
            if (re != null && re.Length > 0)
            {
                return re[0];
            }
            else
            {
                return default(uint);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual uint[] ReadUInt32(string address, ushort length,out bool res)
        {
            var re = this.Read(address, this.GetWordLength(address, (int)length, 2), out res);
            if (res && re.Length > 0)
                return this.ByteTransform.TransUInt32(re, 0, (int)length);
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public float ReadFloat(string address,out bool res)
        {
            var re = this.ReadFloat(address, 1, out res);
            if (re != null && re.Length > 0)
            {
                return re[0];
            }
            else
            {
                return default(float);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual float[] ReadFloat(string address, ushort length,out bool res)
        {
            var re = this.Read(address, this.GetWordLength(address, (int)length, 2), out res);
            if (res && re.Length>0)
                return this.ByteTransform.TransSingle(re, 0, (int)length);
            else
            {
                return null;
            }
        }

        
        public long ReadInt64(string address, out bool res)
        {
            var re = this.ReadInt64(address, 1, out res);
            if (re != null && re.Length > 0)
            {
                return re[0];
            }
            else
            {
                return default(long);
            }
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="address"></param>
       /// <param name="length"></param>
       /// <param name="res"></param>
       /// <returns></returns>
        public virtual long[]  ReadInt64(string address, ushort length, out bool res)
        {
            var re = this.Read(address, this.GetWordLength(address, (int)length, 4), out res);
            if (res && re.Length > 0)
                return this.ByteTransform.TransInt64(re, 0, (int)length);
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public ulong ReadUInt64(string address,out bool res)
        {
            var re = this.ReadUInt64(address, 1, out res);
            if (re != null && re.Length > 0)
            {
                return re[0];
            }
            else
            {
                return default(ulong);
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual ulong[] ReadUInt64(string address, ushort length, out bool res)
        {
            var re = this.Read(address, this.GetWordLength(address, (int)length, 4), out res);
            if(res && re.Length > 0)
            return this.ByteTransform.TransUInt64(re, 0, (int)length);
             else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public double ReadDouble(string address,out bool res)
        {
            var re = this.ReadDouble(address, 1, out res);
            if (re != null && re.Length > 0)
            {
                return re[0];
            }
            else
            {
                return default(double);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual double[] ReadDouble(string address, ushort length, out bool res)
        {
            var re = this.Read(address, this.GetWordLength(address, (int)length, 4), out res);
            if (res && re.Length > 0)
                return this.ByteTransform.TransDouble(re, 0, (int)length);
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual string ReadString(string address, ushort length,out bool res)
        {
            return this.ReadString(address, length, Encoding.ASCII,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="encoding"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual string ReadString(string address, ushort length, Encoding encoding,out bool res)
        {
            var re = this.Read(address, length,out  res);
            if(res && re.Length > 0)
            {
                return this.ByteTransform.TransString(re, 0, re.Length, encoding);
            }
            else
            {
                
                return String.Empty;
            }
        }


        public virtual object Write(string address, short[] values,out bool res)
        {
            return this.Write(address, ByteTransform.TransByte(values),out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual object Write(string address, short value,out bool res)
        {
            return this.Write(address, new short[]
            {
                value
            },out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual object Write(string address, ushort[] values, out bool res)
        {
            return this.Write(address, ByteTransform.TransByte(values), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual object Write(string address, ushort value, out bool res)
        {
            return this.Write(address, new ushort[]
            {
                value
            }, out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual object Write(string address, int[] values, out bool res)
        {
            return this.Write(address, ByteTransform.TransByte(values), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual object Write(string address, int value, out bool res)
        {
            return this.Write(address, new int[]
            {
                value
            }, out res);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual object Write(string address, uint[] values, out bool res)
        {
            return this.Write(address, ByteTransform.TransByte(values), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual object Write(string address, uint value, out bool res)
        {
            return this.Write(address, new uint[]
            {
                value
            }, out res);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual object Write(string address, long[] values, out bool res)
        {
            return this.Write(address, ByteTransform.TransByte(values), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual object Write(string address, long value, out bool res)
        {
            return this.Write(address, new long[]
            {
                value
            }, out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual object Write(string address, ulong[] values, out bool res)
        {
            return this.Write(address, ByteTransform.TransByte(values), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual object Write(string address, ulong value, out bool res)
        {
            return this.Write(address, new ulong[]
            {
                value
            }, out res);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual object Write(string address, float[] values, out bool res)
        {
            return this.Write(address, ByteTransform.TransByte(values), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual object Write(string address, float value, out bool res)
        {
            return this.Write(address, new float[]
            {
                value
            }, out res);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual object Write(string address, double[] values, out bool res)
        {
            return this.Write(address, ByteTransform.TransByte(values), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual object Write(string address, double value, out bool res)
        {
            return this.Write(address, new double[]
            {
                value
            }, out res);
        }

        public virtual object Write(string address, string value,out bool res)
        {
            return this.Write(address, value, Encoding.ASCII,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual object Write(string address, string value, int length,out bool res)
        {
            return this.Write(address, value, length, Encoding.ASCII,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual object Write(string address, string value, Encoding encoding,out bool res)
        {
            byte[] array = ByteTransform.TransByte(value, encoding);
            if (this.WordLength == 1)
            {
                array = DataExtend.ArrayExpandToLengthEven<byte>(array);
            }
            return this.Write(address, array,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="encoding"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual object Write(string address, string value, int length, Encoding encoding,out bool res)
        {
            byte[] array = ByteTransform.TransByte(value, encoding);
            bool flag = this.WordLength == 1;
            if (flag)
            {
                array = DataExtend.ArrayExpandToLengthEven<byte>(array);
            }
            array = DataExtend.ArrayExpandToLength<byte>(array, length);
            return this.Write(address, array,out res);
        }

        /// <summary>
        /// 一个字单位的数据表示的地址长度，西门子为2，三菱，欧姆龙，modbusTcp就为1，AB PLC无效<br />
        /// The address length represented by one word of data, Siemens is 2, Mitsubishi, Omron, modbusTcp is 1, AB PLC is invalid
        /// </summary>
        /// <remarks>
        /// 对设备来说，一个地址的数据对应的字节数，或是1个字节或是2个字节，通常是这两个选择。<br />
        /// 当前也可以重写来根据不同的地址动态控制不同的地址长度，比如有的地址是一个地址一个字节的，有的地址是一个地址两个字节的
        /// </remarks>
        /// <param name="address">读取的设备的地址信息</param>
        /// <param name="length">读取的数据长度信息</param>
        /// <param name="dataTypeLength">数据类型的字节长度信息，比如short, 就是2，int,float就是4</param>
        protected virtual ushort GetWordLength(string address, int length, int dataTypeLength)
        {
            ushort result;
            if (this.WordLength == 0)
            {
                int num = length * dataTypeLength * 2 / 4;
                result = (ushort)((num == 0) ? 1 : ((ushort)num));
            }
            else
            {
                result = (ushort)((int)this.WordLength * length * dataTypeLength);
            }
            return result;
        }

        

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
