using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.SiemensDriver
{
    public class SiemensS7NetProxy : NetworkDeviceProxyBase
    {

        #region ... Variables  ...

        /// <summary>
        /// 
        /// </summary>
        private byte[] plcHead1 = new byte[]
        {
            3,0,0,
            22,
            17,
            224,
            0,
            0,
            0,
            1,
            0,
            192,
            1,
            10,
            193,
            2,
            1,
            2,
            194,
            2,
            1,
            0
        };

      
        private byte[] plcHead2 = new byte[]
        {
            3,
            0,
            0,
            25,
            2,
            240,
            128,
            50,
            1,
            0,
            0,
            4,
            0,
            0,
            8,
            0,
            0,
            240,
            0,
            0,
            1,
            0,
            1,
            1,
            224
        };

      
        private byte[] plcOrderNumber = new byte[]
        {
            3,
            0,
            0,
            33,
            2,
            240,
            128,
            50,
            7,
            0,
            0,
            0,
            1,
            0,
            8,
            0,
            8,
            0,
            1,
            18,
            4,
            17,
            68,
            1,
            0,
            byte.MaxValue,
            9,
            0,
            4,
            0,
            17,
            0,
            0
        };

        private SiemensPLCS CurrentPlc = SiemensPLCS.S1200;

        private byte[] plcHead1_200smart = new byte[]
        {
            3,
            0,
            0,
            22,
            17,
            224,
            0,
            0,
            0,
            1,
            0,
            193,
            2,
            16,
            0,
            194,
            2,
            3,
            0,
            192,
            1,
            10
        };

        private byte[] plcHead2_200smart = new byte[]
        {
            3,
            0,
            0,
            25,
            2,
            240,
            128,
            50,
            1,
            0,
            0,
            204,
            193,
            0,
            8,
            0,
            0,
            240,
            0,
            0,
            1,
            0,
            1,
            3,
            192
        };

        private byte[] plcHead1_200 = new byte[]
        {
            3,
            0,
            0,
            22,
            17,
            224,
            0,
            0,
            0,
            1,
            0,
            193,
            2,
            77,
            87,
            194,
            2,
            77,
            87,
            192,
            1,
            9
        };

        private byte[] plcHead2_200 = new byte[]
        {
            3,
            0,
            0,
            25,
            2,
            240,
            128,
            50,
            1,
            0,
            0,
            0,
            0,
            0,
            8,
            0,
            0,
            240,
            0,
            0,
            1,
            0,
            1,
            3,
            192
        };

        private byte[] S7_STOP = new byte[]
        {
            3,
            0,
            0,
            33,
            2,
            240,
            128,
            50,
            1,
            0,
            0,
            14,
            0,
            0,
            16,
            0,
            0,
            41,
            0,
            0,
            0,
            0,
            0,
            9,
            80,
            95,
            80,
            82,
            79,
            71,
            82,
            65,
            77
        };

        private byte[] S7_HOT_START = new byte[]
        {
            3,
            0,
            0,
            37,
            2,
            240,
            128,
            50,
            1,
            0,
            0,
            12,
            0,
            0,
            20,
            0,
            0,
            40,
            0,
            0,
            0,
            0,
            0,
            0,
            253,
            0,
            0,
            9,
            80,
            95,
            80,
            82,
            79,
            71,
            82,
            65,
            77
        };

        private byte[] S7_COLD_START = new byte[]
        {
            3,
            0,
            0,
            39,
            2,
            240,
            128,
            50,
            1,
            0,
            0,
            15,
            0,
            0,
            22,
            0,
            0,
            40,
            0,
            0,
            0,
            0,
            0,
            0,
            253,
            0,
            2,
            67,
            32,
            9,
            80,
            95,
            80,
            82,
            79,
            71,
            82,
            65,
            77
        };

        private byte plc_rack = 0;

        private byte plc_slot = 0;

        private int pdu_length = 0;

        private const byte pduStart = 40;

        private const byte pduStop = 41;

        private const byte pduAlreadyStarted = 2;

        private const byte pduAlreadyStopped = 7;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plc"></param>
        /// <param name="driver"></param>
        public SiemensS7NetProxy(SiemensPLCS plc,DriverRunnerBase driver):base(driver)
        {
            Initialization(plc);
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// PLC的槽号，针对S7-400的PLC设置的<br />
        /// The slot number of PLC is set for PLC of s7-400
        /// </summary>
        public byte Slot
        {
            get
            {
                return this.plc_slot;
            }
            set
            {
                this.plc_slot = value;
                bool flag = this.CurrentPlc != SiemensPLCS.S200 && this.CurrentPlc != SiemensPLCS.S200Smart;
                if (flag)
                {
                    this.plcHead1[21] = (byte)(this.plc_rack * 32 + this.plc_slot);
                }
            }
        }

        /// <summary>
        /// PLC的机架号，针对S7-400的PLC设置的<br />
        /// The frame number of the PLC is set for the PLC of s7-400
        /// </summary>
        public byte Rack
        {
            get
            {
                return this.plc_rack;
            }
            set
            {
                this.plc_rack = value;
                bool flag = this.CurrentPlc != SiemensPLCS.S200 && this.CurrentPlc != SiemensPLCS.S200Smart;
                if (flag)
                {
                    this.plcHead1[21] = (byte)(this.plc_rack * 32 + this.plc_slot);
                }
            }
        }

        /// <summary>
        /// 获取或设置当前PLC的连接方式，PG: 0x01，OP: 0x02，S7Basic: 0x03...0x10<br />
        /// Get or set the current PLC connection mode, PG: 0x01, OP: 0x02, S7Basic: 0x03...0x10
        /// </summary>
        public byte ConnectionType
        {
            get
            {
                return this.plcHead1[20];
            }
            set
            {
                bool flag = this.CurrentPlc == SiemensPLCS.S200 || this.CurrentPlc == SiemensPLCS.S200Smart;
                if (!flag)
                {
                    this.plcHead1[20] = value;
                }
            }
        }

        /// <summary>
        /// 西门子相关的远程TSAP参数信息<br />
        /// A parameter information related to Siemens
        /// </summary>
        public int DestTSAP
        {
            get
            {
                bool flag = this.CurrentPlc == SiemensPLCS.S200 || this.CurrentPlc == SiemensPLCS.S200Smart;
                int result;
                if (flag)
                {
                    result = (int)this.plcHead1[17] * 256 + (int)this.plcHead1[18];
                }
                else
                {
                    result = (int)this.plcHead1[20] * 256 + (int)this.plcHead1[21];
                }
                return result;
            }
            set
            {
                bool flag = this.CurrentPlc == SiemensPLCS.S200 || this.CurrentPlc == SiemensPLCS.S200Smart;
                if (flag)
                {
                    this.plcHead1[17] = BitConverter.GetBytes(value)[1];
                    this.plcHead1[18] = BitConverter.GetBytes(value)[0];
                }
                else
                {
                    this.plcHead1[20] = BitConverter.GetBytes(value)[1];
                    this.plcHead1[21] = BitConverter.GetBytes(value)[0];
                }
            }
        }


        /// <summary>
        /// 西门子相关的本地TSAP参数信息<br />
        /// A parameter information related to Siemens
        /// </summary>
        public int LocalTSAP
        {
            get
            {
                bool flag = this.CurrentPlc == SiemensPLCS.S200 || this.CurrentPlc == SiemensPLCS.S200Smart;
                int result;
                if (flag)
                {
                    result = (int)this.plcHead1[13] * 256 + (int)this.plcHead1[14];
                }
                else
                {
                    result = (int)this.plcHead1[16] * 256 + (int)this.plcHead1[17];
                }
                return result;
            }
            set
            {
                bool flag = this.CurrentPlc == SiemensPLCS.S200 || this.CurrentPlc == SiemensPLCS.S200Smart;
                if (flag)
                {
                    this.plcHead1[13] = BitConverter.GetBytes(value)[1];
                    this.plcHead1[14] = BitConverter.GetBytes(value)[0];
                }
                else
                {
                    this.plcHead1[16] = BitConverter.GetBytes(value)[1];
                    this.plcHead1[17] = BitConverter.GetBytes(value)[0];
                }
            }
        }

        /// <summary>
        /// 获取当前西门子的PDU的长度信息，不同型号PLC的值会不一样。<br />
        /// Get the length information of the current Siemens PDU, the value of different types of PLC will be different.
        /// </summary>
        public int PDULength
        {
            get
            {
                return this.pdu_length;
            }
        }
        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override INetMessage GetNewNetMessage()
        {
            return new S7Message();
        }

        /// <summary>
        /// 初始化方法<br />
        /// Initialize method
        /// </summary>
        /// <param name="siemens">指定西门子的型号 -&gt; Designation of Siemens</param>
        /// <param name="ipAddress">Ip地址 -&gt; IpAddress</param>
        private void Initialization(SiemensPLCS siemens)
        {
            base.WordLength = 2;
            this.CurrentPlc = siemens;
            base.ByteTransform = new ReverseBytesTransform();
            switch (siemens)
            {
                case SiemensPLCS.S1200:
                    this.plcHead1[21] = 0;
                    break;
                case SiemensPLCS.S300:
                    this.plcHead1[21] = 2;
                    break;
                case SiemensPLCS.S400:
                    this.plcHead1[21] = 3;
                    this.plcHead1[17] = 0;
                    break;
                case SiemensPLCS.S1500:
                    this.plcHead1[21] = 0;
                    break;
                case SiemensPLCS.S200Smart:
                    this.plcHead1 = this.plcHead1_200smart;
                    this.plcHead2 = this.plcHead2_200smart;
                    break;
                case SiemensPLCS.S200:
                    this.plcHead1 = this.plcHead1_200;
                    this.plcHead2 = this.plcHead2_200;
                    break;
                default:
                    this.plcHead1[18] = 0;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="send"></param>
        /// <param name="hasResponseData"></param>
        /// <param name="usePackAndUnpack"></param>
        /// <returns></returns>
        public override byte[] ReadFromCoreServer(byte[] send, bool hasResponseData = true, bool usePackAndUnpack = true)
        {
            byte[] operateResult=null;
            while (true)
            {
                operateResult = base.ReadFromCoreServer(send, hasResponseData, usePackAndUnpack);
                if (operateResult==null)
                {
                    return null;
                }
                bool flag2 = (int)operateResult[2] * 256 + (int)operateResult[3] != 7;
                if (flag2)
                {
                    return operateResult;
                }
            }
            return operateResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool InitializationOnConnect()
        {
            byte[] operateResult = this.ReadFromCoreServer(this.plcHead1, true, true);
            if (operateResult==null)
            {
                return false;
            }
            else
            {
                byte[] operateResult2 = this.ReadFromCoreServer( this.plcHead2, true, true);
                if (operateResult2==null)
                {
                    return false;
                }
                else
                {
                    this.pdu_length = (int)(base.ByteTransform.TransUInt16(operateResult2.SelectLast(2), 0) - 28);
                    if (this.pdu_length < 200)
                    {
                        this.pdu_length = 200;
                    }
                    return true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReadOrderNumber()
        {
            var res = this.ReadFromCoreServer(this.plcOrderNumber);
            if (res==null)
            {
                return String.Empty;
            }
            else
            {
                return Encoding.ASCII.GetString(res, 71, 20);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private bool CheckStartResult(byte[] content)
        {
            if (content.Length < 19)
            {
                return false;
            }
            else
            {
                if (content[19] != 40)
                {
                    return false;
                }
                else if(content[20] != 2)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private bool CheckStopResult(byte[] content)
        {
            if (content.Length < 19)
            {
                return false;
            }
            else
            {
                if (content[19] != 41)
                {
                    return false;
                }
                else if (content[20] != 7)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        /// <summary>
        /// 从PLC读取原始的字节数据，地址格式为I100，Q100，DB20.100，M100，长度参数以字节为单位<br />
        /// Read the original byte data from the PLC, the address format is I100, Q100, DB20.100, M100, length parameters in bytes
        /// </summary>
        /// <param name="address">起始地址，格式为I100，M100，Q100，DB20.100<br />
        /// Starting address, formatted as I100,M100,Q100,DB20.100</param>
        /// <param name="length">读取的数量，以字节为单位<br />
        /// The number of reads, in bytes</param>
        /// <returns>
        /// 是否读取成功的结果对象 <br />
        /// Whether to read the successful result object</returns>
        /// <remarks>
        /// </remarks>
        /// <example>
        /// 假设起始地址为M100，M100存储了温度，100.6℃值为1006，M102存储了压力，1.23Mpa值为123，M104，M105，M106，M107存储了产量计数，读取如下：
        /// 以下是读取不同类型数据的示例
        /// </example>
        /// 
        public override byte[] Read(string address, ushort length, out bool res)
        {
            S7AddressData operateResult = S7AddressData.ParseFrom(address, length);
            byte[] result;
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
                    ushort num2 = (ushort)Math.Min((int)(length - num), this.pdu_length);
                    operateResult.Length = num2;
                    byte[] operateResult2 = this.Read(new S7AddressData[]
                    {
                        operateResult
                    });
                    if (operateResult2==null)
                    {
                        res = false;
                        return null;
                    }
                    list.AddRange(operateResult2);
                    num += num2;
                    bool flag3 = operateResult.DataCode == 31 || operateResult.DataCode == 30;
                    if (flag3)
                    {
                        operateResult.AddressStart += (int)(num2 / 2);
                    }
                    else
                    {
                        operateResult.AddressStart += (int)(num2 * 8);
                    }
                }
                result = list.ToArray();
            }
            res = true;
            return result;
        }

        /// <summary>
        /// 一次性从PLC获取所有的数据，按照先后顺序返回一个统一的Buffer，需要按照顺序处理，两个数组长度必须一致，数组长度无限制<br />
        /// One-time from the PLC to obtain all the data, in order to return a unified buffer, need to be processed sequentially, two array length must be consistent
        /// </summary>
        /// <param name="address">起始地址，格式为I100，M100，Q100，DB20.100<br />
        /// Starting address, formatted as I100,M100,Q100,DB20.100</param>
        /// <param name="length">数据长度数组<br />
        /// Array of data Lengths</param>
        /// <returns>是否读取成功的结果对象 -&gt; Whether to read the successful result object</returns>
        /// <exception cref="T:System.NullReferenceException"></exception>
        /// <remarks>
        /// <note type="warning">原先的批量的长度为19，现在已经内部自动处理整合，目前的长度为任意和长度。</note>
        /// </remarks>
        /// <example>
        /// 以下是一个高级的读取示例
        /// </example>
        public byte[] Read(string[] address, ushort[] length)
        {
            S7AddressData[] array = new S7AddressData[address.Length];
            for (int i = 0; i < address.Length; i++)
            {
                S7AddressData operateResult = S7AddressData.ParseFrom(address[i], length[i]);
                if (operateResult==null)
                {
                    return null;
                }
                array[i] = operateResult;
            }
            return this.Read(array);
        }

        /// <summary>
        /// 读取指定地址的bool数据，地址格式为I100，M100，Q100，DB20.100<br />
        /// reads bool data for the specified address in the format I100，M100，Q100，DB20.100
        /// </summary>
        /// <param name="address">起始地址，格式为I100，M100，Q100，DB20.100 -&gt;
        /// Starting address, formatted as I100,M100,Q100,DB20.100</param>
        /// <returns>是否读取成功的结果对象 -&gt; Whether to read the successful result object</returns>
        /// <remarks>
        /// <note type="important">
        /// 对于200smartPLC的V区，就是DB1.X，例如，V100=DB1.100
        /// </note>
        /// </remarks>
        /// <example>
        /// 假设读取M100.0的位是否通断
        /// </example>
        public override bool ReadBool(string address,out bool res)
        {
            var re = this.ReadBitFromPLC(address);
            if (re != null)
            {
                res = true;
                return ByteTransformHelper.GetResultFromBytes<bool>(re, (byte[] m) => m[0] > 0,out res);
            }
            else
            {
                res = false;
                return false;
            }
        }

        /// <summary>
        /// 读取指定地址的bool数组，地址格式为I100，M100，Q100，DB20.100<br />
        /// reads bool array data for the specified address in the format I100，M100，Q100，DB20.100
        /// </summary>
        /// <param name="address">起始地址，格式为I100，M100，Q100，DB20.100 -&gt;
        /// Starting address, formatted as I100,M100,Q100,DB20.100</param>
        /// <param name="length">读取的长度信息</param>
        /// <returns>是否读取成功的结果对象 -&gt; Whether to read the successful result object</returns>
        /// <remarks>
        /// <note type="important">
        /// 对于200smartPLC的V区，就是DB1.X，例如，V100=DB1.100
        /// </note>
        /// </remarks>
        public override bool[] ReadBool(string address, ushort length,out bool res)
        {
            S7AddressData operateResult = S7AddressData.ParseFrom(address);
            if (operateResult==null)
            {
                res=false;
                return null;
            }
            else
            {
                int addressStart;
                ushort length2;
                int index;
                DataExtend.CalculateStartBitIndexAndLength(operateResult.AddressStart, length, out addressStart, out length2, out index);
                operateResult.AddressStart = addressStart;
                operateResult.Length = length2;
                byte[] operateResult2 = this.Read(new S7AddressData[]
                {
                    operateResult
                });
                if (operateResult2==null)
                {
                    res=false;
                    return null;
                }
                else
                {
                    res = true;
                    return operateResult2.ToBoolArray().SelectMiddle(index, (int)length);
                }
            }
        }


        /// <summary>
        /// 读取指定地址的byte数据，地址格式I100，M100，Q100，DB20.100<br />
        /// Reads the byte data of the specified address, the address format I100,Q100,DB20.100,M100
        /// </summary>
        /// <param name="address">起始地址，格式为I100，M100，Q100，DB20.100 -&gt;
        /// Starting address, formatted as I100,M100,Q100,DB20.100</param>
        /// <returns>是否读取成功的结果对象 -&gt; Whether to read the successful result object</returns>
        public byte ReadByte(string address,out bool res)
        {
            return ByteTransformHelper.GetResultFromArray<byte>(this.Read(address, 1,out res),out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public string ReadString(string address,out bool res)
        {
            return this.ReadString(address, Encoding.ASCII,out res);
        }

        /// <summary>
        /// 读取西门子的地址的字符串信息，这个信息是和西门子绑定在一起，长度随西门子的信息动态变化的<br />
        /// Read the Siemens address string information. This information is bound to Siemens and its length changes dynamically with the Siemens information
        /// </summary>
        /// <remarks>
        /// 如果指定编码，一般<see cref="P:System.Text.Encoding.ASCII" />即可，中文需要 Encoding.GetEncoding("gb2312")
        /// </remarks>
        /// <param name="address">数据地址，具体的格式需要参照类的说明文档</param>
        /// <param name="encoding">自定的编码信息，一般<see cref="P:System.Text.Encoding.ASCII" />即可，中文需要 Encoding.GetEncoding("gb2312")</param>
        /// <returns>带有是否成功的字符串结果类对象</returns>
        public string ReadString(string address, Encoding encoding,out bool res)
        {
            bool flag = this.CurrentPlc != SiemensPLCS.S200Smart;
            string result;
            if (this.CurrentPlc != SiemensPLCS.S200Smart)
            {
                byte[] operateResult = this.Read(address, 2,out res);
                if (!res)
                {
                    return String.Empty;
                }
                else
                {
                    if (operateResult[0] == 0 || operateResult[0] == byte.MaxValue)
                    {
                        res = false;
                        return String.Empty;
                    }
                    else
                    {
                        byte[] operateResult2 = this.Read(address, (ushort)(2 + operateResult[1]),out res);
                        if (!res)
                        {
                            return String.Empty;
                        }
                        else
                        {
                            result = encoding.GetString(operateResult2, 2, operateResult2.Length - 2);
                        }
                    }
                }
            }
            else
            {
                byte[] operateResult3 = this.Read(address, 1,out res);
                if (!res)
                {
                    return String.Empty;
                }
                else
                {
                    byte[] operateResult4 = this.Read(address, (ushort)(1 + operateResult3[0]),out res);
                    if (!res)
                    {
                        return String.Empty;
                    }
                    else
                    {
                        result = encoding.GetString(operateResult4, 1, operateResult4.Length - 1);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="encoding"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override string ReadString(string address, ushort length, Encoding encoding,out bool res)
        {
            string result;
            if (length == 0)
            {
                result = this.ReadString(address, encoding,out res);
            }
            else
            {
                result = base.ReadString(address, length, encoding,out res);
            }
            return result;
        }

        /// <summary>
        /// 读取西门子的地址的字符串信息，这个信息是和西门子绑定在一起，长度随西门子的信息动态变化的<br />
        /// Read the Siemens address string information. This information is bound to Siemens and its length changes dynamically with the Siemens information
        /// </summary>
        /// <param name="address">数据地址，具体的格式需要参照类的说明文档</param>
        /// <returns>带有是否成功的字符串结果类对象</returns>
        public string ReadWString(string address,out bool res)
        {
            string result;
            if (this.CurrentPlc != SiemensPLCS.S200Smart)
            {
                byte[] operateResult = this.Read(address, 4,out res);
                if (!res)
                {
                    return String.Empty;
                }
                else
                {
                    byte[] operateResult2 = this.Read(address, (ushort)(4 + ((int)operateResult[2] * 256 + (int)operateResult[3]) * 2),out res);
                    if (!res)
                    {
                        result = String.Empty;
                    }
                    else
                    {
                        result = Encoding.Unicode.GetString(DataExtend.BytesReverseByWord(operateResult2.RemoveBegin(4)));
                    }
                }
            }
            else
            {
                byte[] operateResult3 = this.Read(address, 1,out  res);
                if (!res)
                {
                    result = String.Empty;
                }
                else
                {
                    byte[] operateResult4 = this.Read(address, (ushort)(1 + operateResult3[0] * 2),out res);
                    if (!res)
                    {
                        result = String.Empty;
                    }
                    else
                    {
                        result = Encoding.Unicode.GetString(operateResult4, 1, operateResult4.Length - 1);
                    }
                }
            }
            return result;
        }

        ///// <summary>
        ///// 从PLC中读取时间格式的数据<br />
        ///// Read time format data from PLC
        ///// </summary>
        ///// <param name="address">地址</param>
        ///// <returns>时间对象</returns>
        //public DateTime ReadDateTime(string address, out bool res)
        //{
        //    return ByteTransformHelper.GetResultFromBytes<DateTime>(this.Read(address, 8, out res), new Func<byte[], DateTime>(SiemensDateTime.FromByteArray));
        //}

        /// <summary>
        /// 读取西门子的地址数据信息，支持任意个数的数据读取<br />
        /// Read Siemens address data information, support any number of data reading
        /// </summary>
        /// <param name="s7Addresses">
        /// 西门子的数据地址<br />
        /// Siemens data address</param>
        /// <returns>返回的结果对象信息 -&gt; Whether to read the successful result object</returns>
        // Token: 0x06000944 RID: 2372 RVA: 0x0003C084 File Offset: 0x0003A284
        public byte[] Read(S7AddressData[] s7Addresses)
        {
            byte[] result;
            if (s7Addresses.Length > 19)
            {
                List<byte> list = new List<byte>();
                List<S7AddressData[]> list2 = DataExtend.ArraySplitByLength<S7AddressData>(s7Addresses, 19);
                for (int i = 0; i < list2.Count; i++)
                {
                    byte[] operateResult = this.Read(list2[i]);
                    if (operateResult==null)
                    {
                        return operateResult;
                    }
                    list.AddRange(operateResult);
                }
                result = list.ToArray();
            }
            else
            {
                result = this.ReadS7AddressData(s7Addresses);
            }
            return result;
        }

        /// <summary>
        /// 单次的读取，只能读取最多19个数组的长度，所以不再对外公开该方法
        /// </summary>
        /// <param name="s7Addresses">西门子的地址对象</param>
        /// <returns>返回的结果对象信息</returns>
        // Token: 0x06000945 RID: 2373 RVA: 0x0003C118 File Offset: 0x0003A318
        private byte[] ReadS7AddressData(S7AddressData[] s7Addresses)
        {
            byte[] operateResult = BuildReadCommand(s7Addresses);
            byte[] result;
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                byte[] operateResult2 = this.ReadFromCoreServer(operateResult);
                if (operateResult2==null)
                {
                    return null;
                }
                else
                {
                    result = AnalysisReadByte(s7Addresses, operateResult2);
                }
            }
            return result;
        }

        /// <summary>
        /// A general method for generating a command header to read a Word data
        /// </summary>
        /// <param name="s7Addresses">siemens address</param>
        /// <returns>Message containing the result object</returns>
        public static byte[] BuildReadCommand(S7AddressData[] s7Addresses)
        {
            if (s7Addresses == null)
            {
                throw new NullReferenceException("s7Addresses");
            }
            if (s7Addresses.Length > 19)
            {
                throw new Exception("SiemensReadLengthCannotLargerThan19");
            }
            int num = s7Addresses.Length;
            byte[] array = new byte[19 + num * 12];
            array[0] = 3;
            array[1] = 0;
            array[2] = (byte)(array.Length / 256);
            array[3] = (byte)(array.Length % 256);
            array[4] = 2;
            array[5] = 240;
            array[6] = 128;
            array[7] = 50;
            array[8] = 1;
            array[9] = 0;
            array[10] = 0;
            array[11] = 0;
            array[12] = 1;
            array[13] = (byte)((array.Length - 17) / 256);
            array[14] = (byte)((array.Length - 17) % 256);
            array[15] = 0;
            array[16] = 0;
            array[17] = 4;
            array[18] = (byte)num;
            for (int i = 0; i < num; i++)
            {
                array[19 + i * 12] = 18;
                array[20 + i * 12] = 10;
                array[21 + i * 12] = 16;
                //bool flag3 = s7Addresses[i].DataCode == 30 || s7Addresses[i].DataCode == 31;
                if (s7Addresses[i].DataCode == 30 || s7Addresses[i].DataCode == 31)
                {
                    array[22 + i * 12] = s7Addresses[i].DataCode;
                    array[23 + i * 12] = (byte)(s7Addresses[i].Length / 2 / 256);
                    array[24 + i * 12] = (byte)(s7Addresses[i].Length / 2 % 256);
                }
                else
                {
                    //bool flag4 = s7Addresses[i].DataCode == 6 | s7Addresses[i].DataCode == 7;
                    if (s7Addresses[i].DataCode == 6 | s7Addresses[i].DataCode == 7)
                    {
                        array[22 + i * 12] = 4;
                        array[23 + i * 12] = (byte)(s7Addresses[i].Length / 2 / 256);
                        array[24 + i * 12] = (byte)(s7Addresses[i].Length / 2 % 256);
                    }
                    else
                    {
                        array[22 + i * 12] = 2;
                        array[23 + i * 12] = (byte)(s7Addresses[i].Length / 256);
                        array[24 + i * 12] = (byte)(s7Addresses[i].Length % 256);
                    }
                }
                array[25 + i * 12] = (byte)(s7Addresses[i].DbBlock / 256);
                array[26 + i * 12] = (byte)(s7Addresses[i].DbBlock % 256);
                array[27 + i * 12] = s7Addresses[i].DataCode;
                array[28 + i * 12] = (byte)(s7Addresses[i].AddressStart / 256 / 256 % 256);
                array[29 + i * 12] = (byte)(s7Addresses[i].AddressStart / 256 % 256);
                array[30 + i * 12] = (byte)(s7Addresses[i].AddressStart % 256);
            }
            return array;
        }

        /// <summary>
        /// 生成一个位读取数据指令头的通用方法 -&gt;
        /// A general method for generating a bit-read-Data instruction header
        /// </summary>
        /// <param name="address">起始地址，例如M100.0，I0.1，Q0.1，DB2.100.2 -&gt;
        /// Start address, such as M100.0,I0.1,Q0.1,DB2.100.2
        /// </param>
        /// <returns>包含结果对象的报文 -&gt; Message containing the result object</returns>
        // Token: 0x0600096F RID: 2415 RVA: 0x0003D3A8 File Offset: 0x0003B5A8
        public static byte[] BuildBitReadCommand(string address)
        {
            S7AddressData operateResult = S7AddressData.ParseFrom(address);
            byte[] result;
            if (operateResult==null)
            {
                result = null;
            }
            else
            {
                byte[] array = new byte[31];
                array[0] = 3;
                array[1] = 0;
                array[2] = (byte)(array.Length / 256);
                array[3] = (byte)(array.Length % 256);
                array[4] = 2;
                array[5] = 240;
                array[6] = 128;
                array[7] = 50;
                array[8] = 1;
                array[9] = 0;
                array[10] = 0;
                array[11] = 0;
                array[12] = 1;
                array[13] = (byte)((array.Length - 17) / 256);
                array[14] = (byte)((array.Length - 17) % 256);
                array[15] = 0;
                array[16] = 0;
                array[17] = 4;
                array[18] = 1;
                array[19] = 18;
                array[20] = 10;
                array[21] = 16;
                array[22] = 1;
                array[23] = 0;
                array[24] = 1;
                array[25] = (byte)(operateResult.DbBlock / 256);
                array[26] = (byte)(operateResult.DbBlock % 256);
                array[27] = operateResult.DataCode;
                array[28] = (byte)(operateResult.AddressStart / 256 / 256 % 256);
                array[29] = (byte)(operateResult.AddressStart / 256 % 256);
                array[30] = (byte)(operateResult.AddressStart % 256);
                result = array;
            }
            return result;
        }

        /// <summary>
        /// 生成一个写入字节数据的指令 -&gt; Generate an instruction to write byte data
        /// </summary>
        /// <param name="s7Address">起始地址，示例M100,I100,Q100,DB1.100 -&gt; Start Address, example M100,I100,Q100,DB1.100</param>
        /// <param name="data">原始的字节数据 -&gt; Raw byte data</param>
        /// <returns>包含结果对象的报文 -&gt; Message containing the result object</returns>
        public static byte[] BuildWriteByteCommand(S7AddressData s7Address, byte[] data)
        {
            byte[] array = new byte[35 + data.Length];
            array[0] = 3;
            array[1] = 0;
            array[2] = (byte)((35 + data.Length) / 256);
            array[3] = (byte)((35 + data.Length) % 256);
            array[4] = 2;
            array[5] = 240;
            array[6] = 128;
            array[7] = 50;
            array[8] = 1;
            array[9] = 0;
            array[10] = 0;
            array[11] = 0;
            array[12] = 1;
            array[13] = 0;
            array[14] = 14;
            array[15] = (byte)((4 + data.Length) / 256);
            array[16] = (byte)((4 + data.Length) % 256);
            array[17] = 5;
            array[18] = 1;
            array[19] = 18;
            array[20] = 10;
            array[21] = 16;
            //bool flag = s7Address.DataCode == 6 || s7Address.DataCode == 7;
            if (s7Address.DataCode == 6 || s7Address.DataCode == 7)
            {
                array[22] = 4;
                array[23] = (byte)(data.Length / 2 / 256);
                array[24] = (byte)(data.Length / 2 % 256);
            }
            else
            {
                array[22] = 2;
                array[23] = (byte)(data.Length / 256);
                array[24] = (byte)(data.Length % 256);
            }
            array[25] = (byte)(s7Address.DbBlock / 256);
            array[26] = (byte)(s7Address.DbBlock % 256);
            array[27] = s7Address.DataCode;
            array[28] = (byte)(s7Address.AddressStart / 256 / 256 % 256);
            array[29] = (byte)(s7Address.AddressStart / 256 % 256);
            array[30] = (byte)(s7Address.AddressStart % 256);
            array[31] = 0;
            array[32] = 4;
            array[33] = (byte)(data.Length * 8 / 256);
            array[34] = (byte)(data.Length * 8 % 256);
            data.CopyTo(array, 35);
            return array;
        }

        /// <summary>
        /// 生成一个写入位数据的指令 -&gt; Generate an instruction to write bit data
        /// </summary>
        /// <param name="address">起始地址，示例M100,I100,Q100,DB1.100 -&gt; Start Address, example M100,I100,Q100,DB1.100</param>
        /// <param name="data">是否通断 -&gt; Power on or off</param>
        /// <returns>包含结果对象的报文 -&gt; Message containing the result object</returns>
        public static byte[] BuildWriteBitCommand(string address, bool data)
        {
            S7AddressData operateResult = S7AddressData.ParseFrom(address);
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                byte[] array = new byte[]
                {
                    data ? (byte)1 : (byte)0
                };
                byte[] array2 = new byte[35 + array.Length];
                array2[0] = 3;
                array2[1] = 0;
                array2[2] = (byte)((35 + array.Length) / 256);
                array2[3] = (byte)((35 + array.Length) % 256);
                array2[4] = 2;
                array2[5] = 240;
                array2[6] = 128;
                array2[7] = 50;
                array2[8] = 1;
                array2[9] = 0;
                array2[10] = 0;
                array2[11] = 0;
                array2[12] = 1;
                array2[13] = 0;
                array2[14] = 14;
                array2[15] = (byte)((4 + array.Length) / 256);
                array2[16] = (byte)((4 + array.Length) % 256);
                array2[17] = 5;
                array2[18] = 1;
                array2[19] = 18;
                array2[20] = 10;
                array2[21] = 16;
                array2[22] = 1;
                array2[23] = (byte)(array.Length / 256);
                array2[24] = (byte)(array.Length % 256);
                array2[25] = (byte)(operateResult.DbBlock / 256);
                array2[26] = (byte)(operateResult.DbBlock % 256);
                array2[27] = operateResult.DataCode;
                array2[28] = (byte)(operateResult.AddressStart / 256 / 256);
                array2[29] = (byte)(operateResult.AddressStart / 256);
                array2[30] = (byte)(operateResult.AddressStart % 256);
                //bool flag2 = operateResult.DataCode == 28;
                if (operateResult.DataCode == 28)
                {
                    array2[31] = 0;
                    array2[32] = 9;
                }
                else
                {
                    array2[31] = 0;
                    array2[32] = 3;
                }
                array2[33] = (byte)(array.Length / 256);
                array2[34] = (byte)(array.Length % 256);
                array.CopyTo(array2, 35);
                return array2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s7Addresses"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private static byte[] AnalysisReadByte(S7AddressData[] s7Addresses, byte[] content)
        {
            int num = 0;
            for (int i = 0; i < s7Addresses.Length; i++)
            {
                bool flag = s7Addresses[i].DataCode == 31 || s7Addresses[i].DataCode == 30;
                if (flag)
                {
                    num += (int)(s7Addresses[i].Length * 2);
                }
                else
                {
                    num += (int)s7Addresses[i].Length;
                }
            }
            //bool flag2 = content.Length >= 21 && (int)content[20] == s7Addresses.Length;
            byte[] result;
            if (content.Length >= 21 && (int)content[20] == s7Addresses.Length)
            {
                byte[] array = new byte[num];
                int num2 = 0;
                int num3 = 0;
                for (int j = 21; j < content.Length; j++)
                {
                    bool flag3 = j + 1 < content.Length;
                    if (flag3)
                    {
                        bool flag4 = content[j] == byte.MaxValue && content[j + 1] == 4;
                        if (flag4)
                        {
                            Array.Copy(content, j + 4, array, num3, (int)s7Addresses[num2].Length);
                            j += (int)(s7Addresses[num2].Length + 3);
                            num3 += (int)s7Addresses[num2].Length;
                            num2++;
                        }
                        else
                        {
                            bool flag5 = content[j] == byte.MaxValue && content[j + 1] == 9;
                            if (flag5)
                            {
                                int num4 = (int)content[j + 2] * 256 + (int)content[j + 3];
                                bool flag6 = num4 % 3 == 0;
                                if (flag6)
                                {
                                    for (int k = 0; k < num4 / 3; k++)
                                    {
                                        Array.Copy(content, j + 5 + 3 * k, array, num3, 2);
                                        num3 += 2;
                                    }
                                }
                                else
                                {
                                    for (int l = 0; l < num4 / 5; l++)
                                    {
                                        Array.Copy(content, j + 7 + 5 * l, array, num3, 2);
                                        num3 += 2;
                                    }
                                }
                                j += num4 + 4;
                                num2++;
                            }
                            else
                            {
                                bool flag7 = content[j] == 5 && content[j + 1] == 0;
                                if (content[j] == 5 && content[j + 1] == 0)
                                {
                                    return null;
                                   // return new OperateResult<byte[]>((int)content[j], StringResources.Language.SiemensReadLengthOverPlcAssign);
                                }
                                //bool flag8 = content[j] == 6 && content[j + 1] == 0;
                                if (content[j] == 6 && content[j + 1] == 0)
                                {
                                    return null;
                                    //return new OperateResult<byte[]>((int)content[j], StringResources.Language.SiemensError0006);
                                }
                                //bool flag9 = content[j] == 10 && content[j + 1] == 0;
                                if (content[j] == 10 && content[j + 1] == 0)
                                {
                                    return null;
                                    //return new OperateResult<byte[]>((int)content[j], StringResources.Language.SiemensError000A);
                                }
                            }
                        }
                    }
                }
                result = array;
            }
            else
            {
                result = null;
               // result = new OperateResult<byte[]>(StringResources.Language.SiemensDataLengthCheckFailed + " Msg:" + SoftBasic.ByteToHexString(content, ' '));
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal static byte[] AnalysisReadBit(byte[] content)
        {
            if (content.Length >= 21 && content[20] == 1)
            {
                byte[] array = new byte[1];
                if (22 < content.Length)
                {
                    if (content[21] == byte.MaxValue && content[22] == 3)
                    {
                        array[0] = content[25];
                    }
                    else
                    {
                        return null;
                    }
                }
                return array;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static bool AnalysisWrite(byte[] content)
        {
            if (content.Length >= 22)
            {
                byte b = content[21];
                //bool flag2 = b != byte.MaxValue;
                if (content[21] != byte.MaxValue)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 从PLC读取数据，地址格式为I100，Q100，DB20.100，M100，以位为单位 -&gt;
        /// Read the data from the PLC, the address format is I100，Q100，DB20.100，M100, in bits units
        /// </summary>
        /// <param name="address">起始地址，格式为I100，M100，Q100，DB20.100 -&gt;
        /// Starting address, formatted as I100,M100,Q100,DB20.100</param>
        /// <returns>是否读取成功的结果对象 -&gt; Whether to read the successful result object</returns>
        private byte[] ReadBitFromPLC(string address)
        {
            byte[] operateResult = BuildBitReadCommand(address);
            byte[] result;
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                byte[] operateResult2 = this.ReadFromCoreServer(operateResult);
                if (operateResult2==null)
                {
                    return null;
                }
                else
                {
                    result = AnalysisReadBit(operateResult2);
                }
            }
            return result;
        }


        /// <summary>
        /// 将数据写入到PLC数据，地址格式为I100，Q100，DB20.100，M100，以字节为单位<br />
        /// Writes data to the PLC data, in the address format I100,Q100,DB20.100,M100, in bytes
        /// </summary>
        /// <param name="address">起始地址，格式为I100，M100，Q100，DB20.100 -&gt;
        /// Starting address, formatted as I100,M100,Q100,DB20.100</param>
        /// <param name="value">写入的原始数据 -&gt; Raw data written to</param>
        /// <returns>是否写入成功的结果对象 -&gt; Whether to write a successful result object</returns>
        /// <example>
        /// 假设起始地址为M100，M100,M101存储了温度，100.6℃值为1006，M102,M103存储了压力，1.23Mpa值为123，M104-M107存储了产量计数，写入如下：
        /// 以下是写入不同类型数据的示例
        /// </example>
        // Token: 0x06000947 RID: 2375 RVA: 0x0003C188 File Offset: 0x0003A388
        public override object Write(string address, byte[] value,out bool res)
        {
            S7AddressData operateResult = S7AddressData.ParseFrom(address);
            if (operateResult==null)
            {
                res = false;
                return null;
            }
            else
            {
                
                return this.Write(operateResult, value,out res);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        private object Write(S7AddressData address, byte[] value,out bool res)
        {
            int num = value.Length;
            ushort num2 = 0;
            object result = null;
            while ((int)num2 < num)
            {
                ushort num3 = (ushort)Math.Min(num - (int)num2, this.pdu_length);
                byte[] data = base.ByteTransform.TransByte(value, (int)num2, (int)num3);
                byte[] operateResult = BuildWriteByteCommand(address, data);
                if (operateResult==null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    var operateResult2 = this.WriteBase(operateResult,out res);
                    if (res)
                    {
                        num2 += num3;
                        address.AddressStart += (int)(num3 * 8);
                        continue;
                    }
                    result = operateResult2;
                }
                res = true;
                return result;
            }
            res = true;
            return result;
        }

        /// 基础的写入数据的操作支持<br />
        /// Operational support for the underlying write data
        /// </summary>
        /// <param name="entireValue">完整的字节数据 -&gt; Full byte data</param>
        /// <returns>是否写入成功的结果对象 -&gt; Whether to write a successful result object</returns>
        private object WriteBase(byte[] entireValue,out bool res)
        {
            var re = this.ReadFromCoreServer(entireValue);
            if(re!=null)
            {
                res = true;
                return AnalysisWrite(re);
            }
            else
            {
                res = false;
                return false;
            }
            //return ByteTransformHelper.GetResultFromOther<byte[]>(this.ReadFromCoreServer(entireValue), new Func<byte[], OperateResult>(AnalysisWrite));
        }

        /// <summary>
        /// 写入PLC的一个位，例如"M100.6"，"I100.7"，"Q100.0"，"DB20.100.0"，如果只写了"M100"默认为"M100.0"<br />
        /// Write a bit of PLC, for example  "M100.6",  "I100.7",  "Q100.0",  "DB20.100.0", if only write  "M100" defaults to  "M100.0"
        /// </summary>
        /// <param name="address">起始地址，格式为"M100.6",  "I100.7",  "Q100.0",  "DB20.100.0" -&gt;
        /// Start address, format  "M100.6",  "I100.7",  "Q100.0",  "DB20.100.0"</param>
        /// <param name="value">写入的数据，True或是False -&gt; Writes the data, either True or False</param>
        /// <returns>是否写入成功的结果对象 -&gt; Whether to write a successful result object</returns>
        /// <example>
        /// 假设写入M100.0的位是否通断
        /// </example>
        public override object Write(string address, bool value,out bool res)
        {
            object result = null;
            byte[] operateResult = BuildWriteBitCommand(address, value);
            if (operateResult==null)
            {
                res = false;
                result = operateResult;
            }
            else
            {
                result = this.WriteBase(operateResult,out res);
            }
            return result;
        }

        /// <summary>
        /// [警告] 向PLC中写入bool数组，比如你写入M100,那么data[0]对应M100.0，写入的长度应该小于1600位<br />
        /// [Warn] Write the bool array to the PLC, for example, if you write M100, then data[0] corresponds to M100.0, 
        /// The length of the write should be less than 1600 bits
        /// </summary>
        /// <param name="address">起始地址，格式为I100，M100，Q100，DB20.100 -&gt; Starting address, formatted as I100,mM100,Q100,DB20.100</param>
        /// <param name="values">要写入的bool数组，长度为8的倍数 -&gt; The bool array to write, a multiple of 8 in length</param>
        /// <returns>是否写入成功的结果对象 -&gt; Whether to write a successful result object</returns>
        /// <remarks>
        /// <note type="warning">
        /// 批量写入bool数组存在一定的风险，举例写入M100.5的值 [true,false,true,true,false,true]，会读取M100-M101的byte[]，然后修改中间的位，再写入回去，
        /// 如果读取之后写入之前，PLC修改了其他位，则会影响其他的位的数据，请谨慎使用。<br />
        /// There is a certain risk in batch writing bool arrays. For example, writing the value of M100.5 [true,false,true,true,false,true], 
        /// will read the byte[] of M100-M101, then modify the middle bit, and then Write back. 
        /// If the PLC modifies other bits after reading and before writing, it will affect the data of other bits. Please use it with caution.
        /// </note>
        /// </remarks>
        public override object Write(string address, bool[] values,out bool res)
        {
            S7AddressData operateResult = S7AddressData.ParseFrom(address);
            if (operateResult==null)
            {
                res=false;
                return null;
            }
            else
            {
                int addressStart;
                ushort length;
                int destinationIndex;
                DataExtend.CalculateStartBitIndexAndLength(operateResult.AddressStart, (ushort)values.Length, out addressStart, out length, out destinationIndex);
                operateResult.AddressStart = addressStart;
                operateResult.Length = length;
                byte[] operateResult2 = this.Read(new S7AddressData[]
                {
                    operateResult
                });
                if (operateResult2==null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    bool[] array = operateResult2.ToBoolArray();
                    Array.Copy(values, 0, array, destinationIndex, values.Length);
                    return this.Write(operateResult, DataExtend.BoolArrayToByte(array),out res);
                }
            }
        }


        /// <summary>
        /// 向PLC中写入byte数据，返回值说明<br />
        /// Write byte data to the PLC, return value description
        /// </summary>
        /// <param name="address">起始地址，格式为I100，M100，Q100，DB20.100 -&gt; Starting address, formatted as I100,mM100,Q100,DB20.100</param>
        /// <param name="value">byte数据 -&gt; Byte data</param>
        /// <returns>是否写入成功的结果对象 -&gt; Whether to write a successful result object</returns>
        public object Write(string address, byte value,out bool res)
        {
            return this.Write(address, new byte[]
            {
                value
            },out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, string value, Encoding encoding, out bool res)
        {
            if (value == null)
            {
                value = string.Empty;
            }
            byte[] array = encoding.GetBytes(value);
            bool flag2 = encoding == Encoding.Unicode;
            if (encoding == Encoding.Unicode)
            {
                array = DataExtend.BytesReverseByWord(array);
            }
            object result;
            if (this.CurrentPlc != SiemensPLCS.S200Smart)
            {
                byte[] operateResult = this.Read(address, 2, out res);
                if (!res)
                {
                    result = null;
                }
                else
                {
                    if (operateResult[0] == byte.MaxValue)
                    {
                        res = false;
                        result = null;
                    }
                    else
                    {
                        if (operateResult[0] == 0)
                        {
                            operateResult[0] = 254;
                        }
                        if (value.Length > (int)operateResult[0])
                        {
                            res = false;
                            return null;
                        }
                        else
                        {
                            result = this.Write(address, DataExtend.SpliceArray<byte>(new byte[][]
                            {
                        new byte[]
                        {
                            operateResult[0],
                            (byte)value.Length
                        },
                        array
                            }),out res);
                        }
                    }
                }
            }
            else
            {
                result = this.Write(address, DataExtend.SpliceArray<byte>(new byte[][]
                {
            new byte[]
            {
                (byte)value.Length
            },
            array
                }), out res);
            }
            return result;
        }

        /// <summary>
        /// 使用双字节编码的方式，将字符串以 Unicode 编码写入到PLC的地址里，可以使用中文。<br />
        /// Use the double-byte encoding method to write the character string to the address of the PLC in Unicode encoding. Chinese can be used.
        /// </summary>
        /// <param name="address">起始地址，格式为I100，M100，Q100，DB20.100 -&gt; Starting address, formatted as I100,mM100,Q100,DB20.100</param>
        /// <param name="value">字符串的值</param>
        /// <returns>是否写入成功的结果对象</returns>
        // Token: 0x0600095E RID: 2398 RVA: 0x0003CA04 File Offset: 0x0003AC04
        public object WriteWString(string address, string value,out bool res)
        {
            bool flag = this.CurrentPlc != SiemensPLCS.S200Smart;
            object result;
            if (flag)
            {
                bool flag2 = value == null;
                if (flag2)
                {
                    value = string.Empty;
                }
                byte[] array = Encoding.Unicode.GetBytes(value);
                array = DataExtend.BytesReverseByWord(array);
                byte[] operateResult = this.Read(address, 4,out res);
                if (!res)
                {
                    result = false;
                }
                else
                {
                    int num = (int)operateResult[0] * 256 + (int)operateResult[1];
                    if (value.Length > num)
                    {
                        res = false;
                        result = false;
                    }
                    else
                    {
                        byte[] array2 = new byte[array.Length + 4];
                        array2[0] = operateResult[0];
                        array2[1] = operateResult[1];
                        array2[2] = BitConverter.GetBytes(value.Length)[1];
                        array2[3] = BitConverter.GetBytes(value.Length)[0];
                        array.CopyTo(array2, 4);
                        result = this.Write(address, array2,out res);
                    }
                }
            }
            else
            {
                result = this.Write(address, value, Encoding.Unicode,out res);
            }
            return result;
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
