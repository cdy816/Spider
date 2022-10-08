using Cdy.Spider.Common;
using Cdy.Spider.MelsecDriver.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class MelsecMcRNetProxy : NetworkDeviceProxyBase, IReadWriteMc
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="runner"></param>
        public MelsecMcRNetProxy(DriverRunnerBase runner) : base(runner)
        {
            WordLength = 1;
            ByteTransform = new RegularByteTransform();
        }
        #endregion ...Constructor...

        #region ... Properties ...
        public McType McType
        {
            get
            {
                return McType.McRBinary;
            }
        }

        public byte NetworkNumber { get; set; } = 0;

        public byte NetworkStationNumber { get; set; } = 0;

        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override INetMessage GetNewNetMessage()
        {
            return new MelsecQnA3EBinaryMessage();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public virtual McAddressData McAnalysisAddress(string address, ushort length)
        {
            return McAddressData.ParseMelsecRFrom(address, length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public override byte[] PackCommandWithHeader(byte[] command)
        {
            return McBinaryHelper.PackMcCommand(command, NetworkNumber, NetworkStationNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="send"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public override byte[] UnpackResponseContent(byte[] send, byte[] response)
        {
            bool operateResult = McBinaryHelper.CheckResponseContentHelper(response);
            if (!operateResult)
            {
                return null;
            }
            else
            {
                return response.RemoveBegin(11);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="isBit"></param>
        /// <returns></returns>
        public byte[] ExtractActualData(byte[] response, bool isBit)
        {
            return McBinaryHelper.ExtractActualDataHelper(response, isBit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override byte[] Read(string address, ushort length, out bool res)
        {
            return McHelper.Read(this, address, length, out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override object Write(string address, byte[] value, out bool result)
        {
            result = McHelper.Write(this, address, value);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override bool[] ReadBool(string address, ushort length, out bool res)
        {
            return McHelper.ReadBool(this, address, length, out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, bool[] value, out bool res)
        {
            res = McHelper.Write(this, address, value);
            return res;
        }

        /// <summary>
        /// 随机读取PLC的数据信息，可以跨地址，跨类型组合，但是每个地址只能读取一个word，也就是2个字节的内容。收到结果后，需要自行解析数据
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public byte[] ReadRandom(string[] address, out bool res)
        {
            return McHelper.ReadRandom(this, address, out res);
        }

        /// <summary>
        /// 随机读取PLC的数据信息，可以跨地址，跨类型组合，每个地址是任意的长度。收到结果后，需要自行解析数据，目前只支持字地址，比如D区，W区，R区，不支持X，Y，M，B，L等等
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public byte[] ReadRandom(string[] address, ushort[] length, out bool res)
        {
            return McHelper.ReadRandom(this, address, length, out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public short[] ReadRandomInt16(string[] address, out bool res)
        {
            return McHelper.ReadRandomInt16(this, address, out res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public ushort[] ReadRandomUInt16(string[] address, out bool res)
        {
            return McHelper.ReadRandomUInt16(this, address, out res);
        }

        /// <summary>
        /// 从三菱地址，是否位读取进行创建读取的MC的核心报文
        /// </summary>
        /// <param name="address">地址数据</param>
        /// <param name="isBit">是否进行了位读取操作</param>
        /// <returns>带有成功标识的报文对象</returns>
        // Token: 0x06000D58 RID: 3416 RVA: 0x000544B4 File Offset: 0x000526B4
        public static byte[] BuildReadMcCoreCommand(McAddressData address, bool isBit)
        {
            return new byte[]
            {
                1,
                4,
                isBit ? (byte)1 : (byte)0,
                0,
                BitConverter.GetBytes(address.AddressStart)[0],
                BitConverter.GetBytes(address.AddressStart)[1],
                BitConverter.GetBytes(address.AddressStart)[2],
                BitConverter.GetBytes(address.AddressStart)[3],
                BitConverter.GetBytes(address.McDataType.DataCode)[0],
                BitConverter.GetBytes(address.McDataType.DataCode)[1],
                (byte)(address.Length % 256),
                (byte)(address.Length / 256)
            };
        }

        /// <summary>
        /// 以字为单位，创建数据写入的核心报文
        /// </summary>
        /// <param name="address">三菱的数据地址</param>
        /// <param name="value">实际的原始数据信息</param>
        /// <returns>带有成功标识的报文对象</returns>
        // Token: 0x06000D59 RID: 3417 RVA: 0x00054574 File Offset: 0x00052774
        public static byte[] BuildWriteWordCoreCommand(McAddressData address, byte[] value)
        {
            bool flag = value == null;
            if (flag)
            {
                value = new byte[0];
            }
            byte[] array = new byte[12 + value.Length];
            array[0] = 1;
            array[1] = 20;
            array[2] = 0;
            array[3] = 0;
            array[4] = BitConverter.GetBytes(address.AddressStart)[0];
            array[5] = BitConverter.GetBytes(address.AddressStart)[1];
            array[6] = BitConverter.GetBytes(address.AddressStart)[2];
            array[7] = BitConverter.GetBytes(address.AddressStart)[3];
            array[8] = BitConverter.GetBytes(address.McDataType.DataCode)[0];
            array[9] = BitConverter.GetBytes(address.McDataType.DataCode)[1];
            array[10] = (byte)(value.Length / 2 % 256);
            array[11] = (byte)(value.Length / 2 / 256);
            value.CopyTo(array, 12);
            return array;
        }

        /// <summary>
        /// 以位为单位，创建数据写入的核心报文
        /// </summary>
        /// <param name="address">三菱的地址信息</param>
        /// <param name="value">原始的bool数组数据</param>
        /// <returns>带有成功标识的报文对象</returns>
        // Token: 0x06000D5A RID: 3418 RVA: 0x0005464C File Offset: 0x0005284C
        public static byte[] BuildWriteBitCoreCommand(McAddressData address, bool[] value)
        {
            bool flag = value == null;
            if (flag)
            {
                value = new bool[0];
            }
            byte[] array = MelsecHelper.TransBoolArrayToByteData(value);
            byte[] array2 = new byte[12 + array.Length];
            array2[0] = 1;
            array2[1] = 20;
            array2[2] = 1;
            array2[3] = 0;
            array2[4] = BitConverter.GetBytes(address.AddressStart)[0];
            array2[5] = BitConverter.GetBytes(address.AddressStart)[1];
            array2[6] = BitConverter.GetBytes(address.AddressStart)[2];
            array2[7] = BitConverter.GetBytes(address.AddressStart)[3];
            array2[8] = BitConverter.GetBytes(address.McDataType.DataCode)[0];
            array2[9] = BitConverter.GetBytes(address.McDataType.DataCode)[1];
            array2[10] = (byte)(value.Length % 256);
            array2[11] = (byte)(value.Length / 256);
            array.CopyTo(array2, 12);
            return array2;
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
