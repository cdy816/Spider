using Cdy.Spider.Common;
using Cdy.Spider.MelsecDriver.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver
{
    public class MelsecMcNetProxy : NetworkDeviceProxyBase, IReadWriteMc
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
        public MelsecMcNetProxy(DriverRunnerBase runner) : base(runner)
        {
            WordLength = 1;
            ByteTransform = new RegularByteTransform();
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public McType McType
        {
            get
            {
                return McType.McBinary;
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
            return McAddressData.ParseMelsecFrom(address, length);
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
            var operateResult = McBinaryHelper.CheckResponseContentHelper(response);
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
        /// <returns></returns>
        public override object Write(string address, byte[] value, out bool res)
        {
            res = McHelper.Write(this, address, value);
            return res;
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
        /// 读取PLC的标签信息，需要传入标签的名称，读取的字长度，标签举例：A; label[1]; bbb[10,10,10]
        /// </summary>
        /// <param name="tag">数据标签</param>
        /// <param name="length">读取的数据长度</param>
        /// <returns></returns>
        public byte[] ReadTags(string tag, ushort length)
        {
            return ReadTags(new string[]
            {
                tag
            }, new ushort[]
            {
                length
            });
        }

        /// <summary>
        /// 批量读取PLC的标签信息，需要传入标签的名称，读取的字长度，标签举例：A; label[1]; bbb[10,10,10]
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public byte[] ReadTags(string[] tags, ushort[] length)
        {
            return McBinaryHelper.ReadTags(this, tags, length);
        }

        /// <summary>
        /// 读取扩展的数据信息，需要在原有的地址，长度信息之外，输入扩展值信息
        /// </summary>
        /// <param name="extend"></param>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public byte[] ReadExtend(ushort extend, string address, ushort length, out bool res)
        {
            return McHelper.ReadExtend(this, extend, address, length, out res);
        }

        /// <summary>
        /// 读取缓冲寄存器的数据信息，地址直接为偏移地址
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public byte[] ReadMemory(string address, ushort length, out bool res)
        {
            return McHelper.ReadMemory(this, address, length, out res);
        }

        /// <summary>
        /// 读取智能模块的数据信息，需要指定模块地址，偏移地址，读取的字节长度
        /// </summary>
        /// <param name="module"></param>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public byte[] ReadSmartModule(ushort module, string address, ushort length, out bool res)
        {
            return McHelper.ReadSmartModule(this, module, address, length, out res);
        }

        /// <summary>
        /// 读取PLC的型号信息，例如 Q02H CPU
        /// </summary>
        /// <returns></returns>
        public string ReadPlcType(out bool res)
        {
            return McHelper.ReadPlcType(this, out res);
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
