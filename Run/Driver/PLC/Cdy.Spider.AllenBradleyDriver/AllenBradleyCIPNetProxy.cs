using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cdy.Spider.AllenBradleyDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class AllenBradleyCIPNetProxy : NetworkDeviceProxyBase
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
        public AllenBradleyCIPNetProxy(DriverRunnerBase driver) : base(driver)
        {
            base.WordLength = 2;
            base.ByteTransform = new RegularByteTransform();
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// The current session handle, which is determined by the PLC when communicating with the PLC handshake
        /// </summary>
        public uint SessionHandle { get; protected set; }

        /// <summary>
        /// Gets or sets the slot number information for the current plc, which should be set before connections
        /// </summary>
        public byte Slot { get; set; } = 0;

        /// <summary>
        /// port and slot information
        /// </summary>
        public byte[] PortSlot { get; set; }

        /// <summary>
        /// 获取或设置整个交互指令的控制码，默认为0x6F，通常不需要修改<br />
        /// Gets or sets the control code of the entire interactive instruction. The default is 0x6F, and usually does not need to be modified.
        /// </summary>
        public ushort CipCommand { get; set; } = 111;

        /// <summary>
        /// 获取或设置当前的通信的消息路由信息，可以实现一些复杂情况的通信，数据包含背板号，路由参数，slot，例如：1.15.2.18.1.1<br />
        /// Get or set the message routing information of the current communication, which can realize some complicated communication. 
        /// The data includes the backplane number, routing parameters, and slot, for example: 1.15.2.18.1.1
        /// </summary>
        public MessageRouter MessageRouter { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override INetMessage GetNewNetMessage()
        {
            return new AllenBradleyMessage();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public override byte[] PackCommandWithHeader(byte[] command)
        {
            return AllenBradleyHelper.PackRequestHeader(this.CipCommand, this.SessionHandle, command, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool InitializationOnConnect()
        {
            byte[] operateResult = this.ReadFromCoreServer(AllenBradleyHelper.RegisterSessionHandle(null), true, false);
            if (operateResult==null)
            {
                return false;
            }
            else
            {
                var operateResult2 = AllenBradleyHelper.CheckResponse(operateResult);
                if (!operateResult2)
                {
                    return false;
                }
                else
                {
                    this.SessionHandle = base.ByteTransform.TransUInt32(operateResult, 4);
                    if (this.MessageRouter != null)
                    {
                        byte[] routerCIP = this.MessageRouter.GetRouterCIP();
                        byte[] operateResult3 = this.ReadFromCoreServer( AllenBradleyHelper.PackRequestHeader(111, this.SessionHandle, AllenBradleyHelper.PackCommandSpecificData(new byte[][]
                        {
                            new byte[4],
                            AllenBradleyHelper.PackCommandSingleService(routerCIP, 178, false)
                        }), null), true, false);
                        if (operateResult3==null)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool  ExtraOnDisconnect()
        {
            byte[] operateResult = this.ReadFromCoreServer(AllenBradleyHelper.UnRegisterSessionHandle(this.SessionHandle), true, false);
            if (operateResult==null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 创建一个读取标签的报文指定，标签地址可以手动动态指定slot编号，例如 slot=2;AAA<br />
        /// Build a read command bytes, The label address can manually specify the slot number dynamically, for example slot=2;AAA
        /// </summary>
        /// <param name="address">the address of the tag name</param>
        /// <param name="length">Array information, if not arrays, is 1 </param>
        /// <returns>Message information that contains the result object </returns>
        // Token: 0x060013AC RID: 5036 RVA: 0x0007E6B4 File Offset: 0x0007C8B4
        public virtual byte[] BuildReadCommand(string[] address, int[] length,out bool res)
        {
            byte[] result;
            if (address == null || length == null)
            {
                res = false;
                return null;
            }
            else
            {
                bool flag2 = address.Length != length.Length;
                if (flag2)
                {
                    res = false;
                    return null;
                }
                else
                {
                    try
                    {
                        byte b = this.Slot;
                        List<byte[]> list = new List<byte[]>();
                        for (int i = 0; i < address.Length; i++)
                        {
                            b = (byte)DataExtend.ExtractParameter(ref address[i], "slot", (int)this.Slot);
                            list.Add(AllenBradleyHelper.PackRequsetRead(address[i], length[i], false));
                        }
                        byte[][] array = new byte[2][];
                        array[0] = new byte[4];
                        int num = 1;
                        byte[] portSlot;
                        if ((portSlot = this.PortSlot) == null)
                        {
                            byte[] array2 = new byte[2];
                            array2[0] = 1;
                            portSlot = array2;
                            array2[1] = b;
                        }
                        array[num] = this.PackCommandService(portSlot, list.ToArray());
                        byte[] value = AllenBradleyHelper.PackCommandSpecificData(array);
                        res = true;
                        return value;
                    }
                    catch (Exception ex)
                    {
                        res = false;
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="portSlot"></param>
        /// <param name="cips"></param>
        /// <returns></returns>
        protected virtual byte[] PackCommandService(byte[] portSlot, params byte[][] cips)
        {
            if (this.MessageRouter != null)
            {
                portSlot = this.MessageRouter.GetRouter();
            }
            return AllenBradleyHelper.PackCommandService(portSlot, cips);
        }

        /// <summary>
        /// 创建一个读取多标签的报文<br />
        /// Build a read command bytes
        /// </summary>
        /// <param name="address">The address of the tag name </param>
        /// <returns>Message information that contains the result object </returns>
        public byte[] BuildReadCommand(string[] address,out bool res)
        {
            bool flag = address == null;
            byte[] result;
            if (address == null)
            {
                res = false;
                return null;
            }
            else
            {
                int[] array = new int[address.Length];
                for (int i = 0; i < address.Length; i++)
                {
                    array[i] = 1;
                }
                result = this.BuildReadCommand(address, array,out res);
            }
            return result;
        }


        /// <summary>
        /// Create a written message instruction
        /// </summary>
        /// <param name="address">The address of the tag name </param>
        /// <param name="typeCode">Data type</param>
        /// <param name="data">Source Data </param>
        /// <param name="length">In the case of arrays, the length of the array </param>
        /// <returns>Message information that contains the result object</returns>
        protected virtual byte[] BuildWriteCommand(string address, ushort typeCode, byte[] data, int length,out bool res)
        {
            byte[] result=null;
            try
            {
                byte b = (byte)DataExtend.ExtractParameter(ref address, "slot", (int)this.Slot);
                byte[] array = AllenBradleyHelper.PackRequestWrite(address, typeCode, data, length, false);
                byte[][] array2 = new byte[2][];
                array2[0] = new byte[4];
                int num = 1;
                byte[] portSlot;
                if ((portSlot = this.PortSlot) == null)
                {
                    byte[] array3 = new byte[2];
                    array3[0] = 1;
                    portSlot = array3;
                    array3[1] = b;
                }
                array2[num] = this.PackCommandService(portSlot, new byte[][]
                {
                    array
                });
                byte[] value = AllenBradleyHelper.PackCommandSpecificData(array2);
                result = value;
                res = true;
            }
            catch (Exception ex)
            {
               res= false;
               
            }
            return result;
        }

        /// <summary>
        /// Create a written message instruction
        /// </summary>
        /// <param name="address">The address of the tag name </param>
        /// <param name="data">Bool Data </param>
        /// <returns>Message information that contains the result object</returns>
        public byte[] BuildWriteCommand(string address, bool data,out bool res)
        {
            byte[] result=null;
            try
            {
                byte b = (byte)DataExtend.ExtractParameter(ref address, "slot", (int)this.Slot);
                byte[] array = AllenBradleyHelper.PackRequestWrite(address, data);
                byte[][] array2 = new byte[2][];
                array2[0] = new byte[4];
                int num = 1;
                byte[] portSlot;
                if ((portSlot = this.PortSlot) == null)
                {
                    byte[] array3 = new byte[2];
                    array3[0] = 1;
                    portSlot = array3;
                    array3[1] = b;
                }
                array2[num] = this.PackCommandService(portSlot, new byte[][]
                {
                    array
                });
                byte[] value = AllenBradleyHelper.PackCommandSpecificData(array2);
                result = value;
                res=true;
            }
            catch (Exception ex)
            {
                res = false;
            }
            return result;
        }


        /// <summary>
        /// Read data information, data length for read array length information
        /// </summary>
        /// <param name="address">Address format of the node</param>
        /// <param name="length">In the case of arrays, the length of the array </param>
        /// <returns>Result data with result object </returns>
        public override byte[] Read(string address, ushort length,out bool res)
        {
            DataExtend.ExtractParameter(ref address, "type", 0);
            byte[] result=null;
            if (length > 1)
            {
                result = this.ReadSegment(address, 0, (int)length,out res);
            }
            else
            {
                result = this.Read(new string[]
                {
                    address
                }, new int[]
                {
                    (int)length
                },out res);
            }
            return result;
        }

        /// <summary>
        /// <b>[商业授权]</b> 批量读取多地址的数据信息，例如我可以读取两个标签的数据 "A","B[0]"，每个地址的数据长度为1，表示一个数据，最终读取返回的是一整个的字节数组，需要自行解析<br />
        /// <b>[Authorization]</b> Batch read data information of multiple addresses, for example, I can read the data of two tags "A", "B[0]", the data length of each address is 1, 
        /// which means one data, and the final read returns a The entire byte array, which needs to be parsed by itself
        /// </summary>
        /// <param name="address">Name of the node </param>
        /// <returns>Result data with result object </returns>
        public byte[] Read(string[] address,out bool res)
        {
            byte[] result;
            if (address == null)
            {
                res=false;
                return null;
            }
            else
            {
                int[] array = new int[address.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = 1;
                }
                result = this.Read(address, array,out res);
            }
            return result;
        }

        /// <summary>
        /// 批量读取多地址的数据信息，例如我可以读取两个标签的数据 "A","B[0]"， 长度为 [1, 5]，返回的是一整个的字节数组，需要自行解析<br />
        /// Read the data information of multiple addresses in batches. For example, I can read the data "A", "B[0]" of two tags, 
        /// the length is [1, 5], and the return is an entire byte array, and I need to do it myself Parsing
        /// </summary>
        /// <param name="address">节点的名称 -&gt; Name of the node </param>
        /// <param name="length">如果是数组，就为数组长度 -&gt; In the case of arrays, the length of the array </param>
        /// <returns>带有结果对象的结果数据 -&gt; Result data with result object </returns>
        public byte[] Read(string[] address, int[] length,out bool res)
        {
            var operateResult = this.ReadWithType(address, length,out res);
            if (!res)
            {
                res = false;
                return null;
            }
            else
            {
                res = true;
                return operateResult.Item1;
            }
        }

        /// <summary>
        /// Read Segment Data Array form plc, use address tag name
        /// </summary>
        /// <param name="address">Tag name in plc</param>
        /// <param name="startIndex">array start index, uint byte index</param>
        /// <param name="length">array length, data item length</param>
        /// <returns>Results Bytes</returns>
        public byte[] ReadSegment(string address, int startIndex, int length,out bool res)
        {
            byte[] result;
            try
            {
                List<byte> list = new List<byte>();
                byte[] operateResult;
                Tuple<byte[], ushort, bool> operateResult2;
                for (; ; )
                {
                    operateResult = this.ReadCipFromServer(new byte[][]
                    {
                        AllenBradleyHelper.PackRequestReadSegment(address, startIndex, length)
                    },out res);
                    if (!res)
                    {
                        break;
                    }
                    operateResult2 = AllenBradleyHelper.ExtractActualData(operateResult, true);
                    if (operateResult2==null)
                    {
                        return null;
                    }
                    startIndex += operateResult2.Item1.Length;
                    list.AddRange(operateResult2.Item1);
                    bool flag3 = !operateResult2.Item3;
                    if (!operateResult2.Item3)
                    {
                        return list.ToArray();
                    }
                }
                return operateResult;
            }
            catch (Exception ex)
            {
                res = false;
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cips"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        private byte[] ReadByCips(byte[][] cips,out bool res)
        {
            byte[] operateResult = this.ReadCipFromServer(cips,out res);
            if (!res)
            {
                res = false;
                return null;
            }
            else
            {
                var operateResult2 = AllenBradleyHelper.ExtractActualData(operateResult, true);
                if (operateResult2==null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    res = true;
                    return operateResult2.Item1;
                }
            }
        }

        /// <summary>
        /// 使用CIP报文和服务器进行核心的数据交换
        /// </summary>
        /// <param name="cips">Cip commands</param>
        /// <returns>Results Bytes</returns>
        public byte[] ReadCipFromServer(byte[][] cips,out bool res)
        {
            byte[][] array = new byte[2][];
            array[0] = new byte[4];
            int num = 1;
            byte[] portSlot;
            if ((portSlot = this.PortSlot) == null)
            {
                byte[] array2 = new byte[2];
                array2[0] = 1;
                portSlot = array2;
                array2[1] = this.Slot;
            }
            array[num] = this.PackCommandService(portSlot, cips.ToArray<byte[]>());
            byte[] send = AllenBradleyHelper.PackCommandSpecificData(array);
            byte[] operateResult = this.ReadFromCoreServer(send);
            if (operateResult==null)
            {
                res = false;
                return null;
            }
            else
            {
                var operateResult2 = AllenBradleyHelper.CheckResponse(operateResult);
                if (!operateResult2)
                {
                    res=false;
                    return null;
                }
                else
                {
                    res = true;
                    return operateResult;
                }
            }
        }

        /// <summary>
        /// 使用EIP报文和服务器进行核心的数据交换
        /// </summary>
        /// <param name="eip">eip commands</param>
        /// <returns>Results Bytes</returns>
        public byte[] ReadEipFromServer(byte[][] eip, out bool res)
        {
            byte[] send = AllenBradleyHelper.PackCommandSpecificData(eip);
            byte[] operateResult = this.ReadFromCoreServer(send);
            byte[] result;
            if (operateResult==null)
            {
                res = false;
                return null;
            }
            else
            {
                var operateResult2 = AllenBradleyHelper.CheckResponse(operateResult);
                if (!operateResult2)
                {
                    res = false;
                    return null;
                }
                else
                {
                    res = true;
                    return operateResult;
                }
            }
        }

        /// <summary>
        /// 读取单个的bool数据信息，如果读取的是单bool变量，就直接写变量名，如果是由int组成的bool数组的一个值，一律带"i="开头访问，例如"i=A[0]" <br />
        /// Read a single bool data information, if it is a single bool variable, write the variable name directly, 
        /// if it is a value of a bool array composed of int, it is always accessed with "i=" at the beginning, for example, "i=A[0]"
        /// </summary>
        /// <param name="address">节点的名称 -&gt; Name of the node </param>
        /// <returns>带有结果对象的结果数据 -&gt; Result data with result info </returns>
        public override bool ReadBool(string address,out bool res)
        {
            bool result;
            if (address.StartsWith("i="))
            {
                address = address.Substring(2);
                int num;
                address = AllenBradleyHelper.AnalysisArrayIndex(address, out num);
                string str = (num / 32 == 0) ? "" : string.Format("[{0}]", num / 32);
                bool[] operateResult = this.ReadBoolArray(address + str,out res);
                if (!res)
                {
                   return false;
                }
                else
                {
                    return operateResult[num % 32];
                }
            }
            else
            {
                byte[] operateResult2 = this.Read(address, 1,out res);
                if (!res)
                {
                    return false;
                }
                else
                {
                    result = base.ByteTransform.TransBool(operateResult2, 0);
                }
            }
            return result;
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
            byte[] operateResult = this.Read(address, 1,out res);
            if (!res)
            {
                return null;
            }
            else
            {
               return DataExtend.ByteToBoolArray(operateResult, (int)length);
            }
        }

        /// <summary>
        /// 批量读取的bool数组信息，如果你有个Bool数组变量名为 A, 那么读第0个位，可以通过 ReadBool("A")，但是第二个位需要使用 
        /// ReadBoolArray("A[0]")   // 返回32个bool长度，0-31的索引，如果我想读取32-63的位索引，就需要 ReadBoolArray("A[1]") ，以此类推。<br />
        /// For batch read bool array information, if you have a Bool array variable named A, then you can read the 0th bit through ReadBool("A"), 
        /// but the second bit needs to use ReadBoolArray("A[0]" ) // Returns the length of 32 bools, the index is 0-31, 
        /// if I want to read the bit index of 32-63, I need ReadBoolArray("A[1]"), and so on.
        /// </summary>
        /// <param name="address">节点的名称 -&gt; Name of the node </param>
        /// <returns>带有结果对象的结果数据 -&gt; Result data with result info </returns>
        public bool[] ReadBoolArray(string address,out bool res)
        {
            byte[] operateResult = this.Read(address, 1,out res);
            bool[] result=null;
            if (!res)
            {
                return null;
            }
            else
            {
                result = operateResult.ToBoolArray();
            }
            return result;
        }

        /// <summary>
        /// 读取PLC的byte类型的数据<br />
        /// Read the byte type of PLC data
        /// </summary>
        /// <param name="address">节点的名称 -&gt; Name of the node </param>
        /// <returns>带有结果对象的结果数据 -&gt; Result data with result info </returns>
        public byte ReadByte(string address,out bool res)
        {
            return ByteTransformHelper.GetResultFromArray<byte>(this.Read(address, 1,out res),out res);
        }

        /// <summary>
        /// 从PLC里读取一个指定标签名的原始数据信息及其数据类型信息<br />
        /// Read the original data information of a specified tag name and its data type information from the PLC
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="address">PLC的标签地址信息</param>
        /// <param name="length">读取的数据长度</param>
        /// <returns>包含原始数据信息及数据类型的结果对象</returns>
        public Tuple<ushort, byte[]> ReadTag(string address, int length ,out bool res)
        {
            Tuple<byte[], ushort, bool> operateResult = this.ReadWithType(new string[]
            {
        address
            }, new int[]
            {
        length
            },out  res);
            if (!res)
            {
                return null;
            }
            else
            {
               return new Tuple<ushort, byte[]>(operateResult.Item2, operateResult.Item1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        private Tuple<byte[], ushort, bool> ReadWithType(string[] address, int[] length,out bool res)
        {
            byte[] operateResult = this.BuildReadCommand(address, length,out res);
            if (!res)
            {
                return null;
            }
            else
            {
                byte[] operateResult2 = this.ReadFromCoreServer(operateResult,true,true);
                if (operateResult2!=null)
                {
                    return null;
                }
                else
                {
                    var operateResult3 = AllenBradleyHelper.CheckResponse(operateResult2);
                    if (!operateResult3)
                    {
                        res = false;
                        return null;
                    }
                    else
                    {
                        res=true;
                        return AllenBradleyHelper.ExtractActualData(operateResult2, true);
                    }
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
        public override short[] ReadInt16(string address, ushort length,out bool res)
        {
            return ByteTransformHelper.GetResultFromBytes<short[]>(this.Read(address, length, out res), (byte[] m) => this.ByteTransform.TransInt16(m, 0, (int)length), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override ushort[] ReadUInt16(string address, ushort length, out bool res)
        {
            return ByteTransformHelper.GetResultFromBytes<ushort[]>(this.Read(address, length, out res), (byte[] m) => this.ByteTransform.TransUInt16(m, 0, (int)length), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override int[] ReadInt32(string address, ushort length, out bool res)
        {
            return ByteTransformHelper.GetResultFromBytes<int[]>(this.Read(address, length, out res), (byte[] m) => this.ByteTransform.TransInt32(m, 0, (int)length), out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override uint[] ReadUInt32(string address, ushort length, out bool res)
        {
            return ByteTransformHelper.GetResultFromBytes<uint[]>(this.Read(address, length, out res), (byte[] m) => this.ByteTransform.TransUInt32(m, 0, (int)length), out res);
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
            return ByteTransformHelper.GetResultFromBytes<float[]>(this.Read(address, length, out res), (byte[] m) => this.ByteTransform.TransSingle(m, 0, (int)length), out res);
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
            return ByteTransformHelper.GetResultFromBytes<long[]>(this.Read(address, length, out res), (byte[] m) => this.ByteTransform.TransInt64(m, 0, (int)length), out res);
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
            return ByteTransformHelper.GetResultFromBytes<ulong[]>(this.Read(address, length, out res), (byte[] m) => this.ByteTransform.TransUInt64(m, 0, (int)length), out res);
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
            return ByteTransformHelper.GetResultFromBytes<double[]>(this.Read(address, length,out res), (byte[] m) => this.ByteTransform.TransDouble(m, 0, (int)length),out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public string ReadString(string address, out bool res)
        {
            return this.ReadString(address, 1,out res);
        }

        /// <summary>
        /// 读取字符串数据，默认为<see cref="P:System.Text.Encoding.UTF8" />编码<br />
        /// Read string data, default is the <see cref="P:System.Text.Encoding.UTF8" /> encoding
        /// </summary>
        /// <param name="address">起始地址</param>
        /// <param name="length">数据长度</param>
        /// <returns>带有成功标识的string数据</returns>
        /// <example>
        /// 以下为三菱的连接对象示例，其他的设备读写情况参照下面的代码：
        /// </example>
        public override string ReadString(string address, ushort length, out bool res)
        {
            return this.ReadString(address, length, Encoding.UTF8,out res);
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
            byte[] operateResult = this.Read(address, length,out res);
            if (!res)
            {
                return String.Empty;
            }
            else
            {
                try
                {
                    bool flag2 = operateResult.Length >= 6;
                    if (operateResult.Length >= 6)
                    {
                        int count = base.ByteTransform.TransInt32(operateResult, 2);
                        return  encoding.GetString(operateResult, 6, count);
                    }
                    else
                    {
                        return encoding.GetString(operateResult);
                    }
                }
                catch (Exception ex)
                {
                    res = false;
                    return String.Empty;
                }
            }

            return String.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public TimeSpan ReadTime(string address,out bool res)
        {
            return AllenBradleyHelper.ReadTime(this, address,out res);
        }

        /// <summary>
        /// 当前写入字节数组使用数据类型 0xD1 写入，如果其他的字节类型需要调用  方法来实现。<br />
        /// The currently written byte array is written using the data type 0xD1. If other byte types need to be called  Method to achieve. <br />
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        /// <returns>写入结果值</returns>
        public override object Write(string address, byte[] value,out bool res)
        {
            return this.WriteTag(address, 209, value, DataExtend.IsAddressEndWithIndex(address) ? value.Length : 1,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, short[] values, out bool res)
        {
            return this.WriteTag(address, 195, base.ByteTransform.TransByte(values), values.Length,out res);
        }

        /// <summary>
        /// 使用指定的类型写入指定的节点数据，类型信息参考API文档，地址支持协议类型代号信息，例如 "type=0xD1;A"<br />
        /// Use the specified type to write the specified node data. For type information, refer to the API documentation. The address supports protocol type code information, such as "type=0xD1;A"
        /// </summary>
        /// <remarks>
        /// 关于参数 length 的含义，表示的是地址长度，一般的标量数据都是 1，如果PLC有个标签是 A，数据类型为 byte[10]，那我们写入 3 个byte就是 WriteTag( "A[5]", 0xD1, new byte[]{1,2,3}, 3 );<br />
        /// Regarding the meaning of the parameter length, it represents the address length. The general scalar data is 1. If the PLC has a tag of A and the data type is byte[10], then we write 3 bytes as WriteTag( "A[5 ]", 0xD1, new byte[]{1,2,3}, 3 );
        /// </remarks>
        /// <param name="address">节点的名称 -&gt; Name of the node </param>
        /// <param name="typeCode">类型代码，详细参见 上的常用字段</param>
        /// <param name="value">实际的数据值 -&gt; The actual data value </param>
        /// <param name="length">如果节点是数组，就是数组长度 -&gt; If the node is an array, it is the array length </param>
        /// <returns>是否写入成功 -&gt; Whether to write successfully</returns>
        public virtual object WriteTag(string address, ushort typeCode, byte[] value, int length,out bool res)
        {
            typeCode = (ushort)DataExtend.ExtractParameter(ref address, "type", (int)typeCode);
            byte[] operateResult = this.BuildWriteCommand(address, typeCode, value, length,out res);
            if (!res)
            {
                return null;
            }
            else
            {
                byte[] operateResult2 = this.ReadFromCoreServer(operateResult);
                if (operateResult2==null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    var operateResult3 = AllenBradleyHelper.CheckResponse(operateResult2);
                    if (!operateResult3)
                    {
                        res = false;
                        return null;
                    }
                    else
                    {
                        res = true;
                        return AllenBradleyHelper.ExtractActualData(operateResult2, false);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, ushort[] values, out bool res)
        {
            return this.WriteTag(address, 199, base.ByteTransform.TransByte(values), values.Length, out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public override object Write(string address, int[] values, out bool res)
        {
            return this.WriteTag(address, 196, base.ByteTransform.TransByte(values), values.Length, out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public override object Write(string address, uint[] values, out bool res)
        {
            return this.WriteTag(address, 200, base.ByteTransform.TransByte(values), values.Length, out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public override object Write(string address, float[] values, out bool res)
        {
            return this.WriteTag(address, 202, base.ByteTransform.TransByte(values), values.Length, out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public override object Write(string address, long[] values, out bool res)
        {
            return this.WriteTag(address, 197, base.ByteTransform.TransByte(values), values.Length, out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public override object Write(string address, ulong[] values, out bool res)
        {
            return this.WriteTag(address, 201, base.ByteTransform.TransByte(values), values.Length, out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public override object Write(string address, double[] values, out bool res)
        {
            return this.WriteTag(address, 203, base.ByteTransform.TransByte(values), values.Length, out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public override object Write(string address, string value, Encoding encoding, out bool res)
        {
            bool flag = string.IsNullOrEmpty(value);
            if (flag)
            {
                value = string.Empty;
            }
            byte[] bytes = encoding.GetBytes(value);
            var operateResult = base.Write(address + ".LEN", bytes.Length,out res);
            if (!res)
            {
                return null;
            }
            else
            {
                byte[] value2 = DataExtend.ArrayExpandToLengthEven<byte>(bytes);
                return this.WriteTag(address + ".DATA[0]", 194, value2, bytes.Length,out res);
            }
        }


        /// <summary>
        /// 写入单个Bool的数据信息。如果读取的是单bool变量，就直接写变量名，如果是bool数组的一个值，一律带下标访问，例如a[0]<br />
        /// Write the data information of a single Bool. If the read is a single bool variable, write the variable name directly, 
        /// if it is a value of the bool array, it will always be accessed with a subscript, such as a[0]
        /// </summary>
        /// <remarks>
        /// 如果写入的是类型代号 0xC1 的bool变量或是数组，直接使用标签名即可，比如：A,A[10]，如果写入的是类型代号0xD3的bool数组的值，则需要使用地址"i="开头，例如：i=A[10]<br />
        /// If you write a bool variable or array of type code 0xC1, you can use the tag name directly, such as: A,A[10], 
        /// if you write the value of a bool array of type code 0xD3, you need to use the address" i=" at the beginning, for example: i=A[10]
        /// </remarks>
        /// <param name="address">标签的地址数据</param>
        /// <param name="value">bool数据值</param>
        /// <returns>是否写入成功</returns>
        public override object Write(string address, bool value,out bool res)
        {
            bool flag = address.StartsWith("i=") && Regex.IsMatch(address, "\\[[0-9]+\\]$");
            if (flag)
            {
                byte[] operateResult = this.BuildWriteCommand(address.Substring(2), value,out res);
                if (!res)
                {
                    return null;
                }
                else
                {
                    byte[] operateResult2 = this.ReadFromCoreServer(operateResult);
                    if (operateResult2==null)
                    {
                        res = false;
                        return null;
                    }
                    else
                    {
                        var operateResult3 = AllenBradleyHelper.CheckResponse(operateResult2);
                        if (!operateResult3)
                        {
                            res = false;
                            return null;
                        }
                        else
                        {
                            res = true;
                            return AllenBradleyHelper.ExtractActualData(operateResult2, false);
                        }
                    }
                }
            }
            else
            {
                ushort typeCode = 193;
                byte[] value2;
                if (!value)
                {
                    value2 = new byte[2];
                }
                else
                {
                    byte[] array = new byte[2];
                    array[0] = byte.MaxValue;
                    value2 = array;
                    array[1] = byte.MaxValue;
                }
                return this.WriteTag(address, typeCode, value2, 1,out res);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, bool[] value,out bool res)
        {
            return this.WriteTag(address, 193, (from m in value select m ? (byte)1 :(byte)0).ToArray<byte>(), DataExtend.IsAddressEndWithIndex(address) ? value.Length : 1,out res);
        }

        /// <summary>
        /// 写入Byte数据，返回是否写入成功，默认使用类型 0xC2, 如果PLC的变量类型不一样，则需要指定实际的变量类型，例如PLC的变量 A 是0xD1类型，那么地址需要携带类型信息，type=0xD1;A <br />
        /// Write Byte data and return whether the writing is successful. The default type is 0xC2. If the variable types of the PLC are different, you need to specify the actual variable type. 
        /// For example, the variable A of the PLC is of type 0xD1, then the address needs to carry the type information, type= 0xD1;A
        /// </summary>
        /// <remarks>
        /// 如何确认PLC的变量的类型呢？可以在HslCommunicationDemo程序上测试知道，也可以直接调用 <see cref="M:HslCommunication.Profinet.AllenBradley.AllenBradleyNet.ReadWithType(System.String[],System.Int32[])" /> 来知道类型信息。
        /// </remarks>
        /// <param name="address">标签的地址数据</param>
        /// <param name="value">Byte数据</param>
        /// <returns>是否写入成功</returns>
        public virtual object Write(string address, byte value,out bool res)
        {
            ushort typeCode = 194;
            byte[] array = new byte[2];
            array[0] = value;
            return this.WriteTag(address, typeCode, array, 1,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="date"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public object WriteDate(string address, DateTime date,out bool res)
        {
            return AllenBradleyHelper.WriteDate(this, address, date,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="time"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public object WriteTime(string address, TimeSpan time,out bool res)
        {
            return AllenBradleyHelper.WriteTime(this, address, time,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="date"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public object WriteTimeAndDate(string address, DateTime date, out bool res)
        {
            return AllenBradleyHelper.WriteTimeAndDate(this, address, date,out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="timeOfDate"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public object WriteTimeOfDate(string address, TimeSpan timeOfDate,out bool res)
        {
            return AllenBradleyHelper.WriteTimeOfDate(this, address, timeOfDate,out res);
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
