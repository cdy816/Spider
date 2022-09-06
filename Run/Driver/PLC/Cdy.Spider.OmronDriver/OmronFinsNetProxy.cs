using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.OmronDriver
{
    public class OmronFinsNetProxy : NetworkDeviceProxyBase
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
        public OmronFinsNetProxy(DriverRunnerBase driver) : base(driver)
        {
            this.WordLength = 1;
            this.ByteTransform = new ReverseWordTransform();
            this.ByteTransform.DataFormat = DataFormat.CDAB;
            this.ByteTransform.IsStringReverseByteWord = true;
        }

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 信息控制字段，默认0x80<br />
        /// Information control field, default 0x80
        /// </summary>
        public byte ICF { get; set; } = 128;

        /// <summary>
        /// 系统使用的内部信息<br />
        /// Internal information used by the system
        /// </summary>
        public byte RSV { get; private set; } = 0;

        /// <summary>
        /// 网络层信息，默认0x02，如果有八层消息，就设置为0x07<br />
        /// Network layer information, default is 0x02, if there are eight layers of messages, set to 0x07
        /// </summary>
        public byte GCT { get; set; } = 2;

        /// <summary>
        /// PLC的网络号地址，默认0x00<br />
        /// PLC network number address, default 0x00
        /// </summary>
        /// <remarks>
        /// 00: Local network<br />
        /// 01-7F: Remote network address (decimal: 1 to 127)
        /// </remarks>
        public byte DNA { get; set; } = 0;

        /// <summary>
        /// PLC的节点地址，默认为0，在和PLC连接的过程中，自动从PLC获取到DA1的值。<br />
        /// The node address of the PLC is 0 by default. During the process of connecting with the PLC, the value of DA1 is automatically obtained from the PLC.
        /// </summary>
        public byte DA1 { get; set; } = 0;

        /// <summary>
        /// PLC的单元号地址，通常都为0<br />
        /// PLC unit number address, usually 0
        /// </summary>
        /// <remarks>
        /// 00: CPU Unit<br />
        /// FE: Controller Link Unit or Ethernet Unit connected to network<br />
        /// 10 TO 1F: CPU Bus Unit<br />
        /// E1: Inner Board
        /// </remarks>
        public byte DA2 { get; set; } = 0;

        /// <summary>
        /// 上位机的网络号地址<br />
        /// Network number and address of the computer
        /// </summary>
        /// <remarks>
        /// 00: Local network<br />
        /// 01-7F: Remote network (1 to 127 decimal)
        /// </remarks>
        public byte SNA { get; set; } = 0;

        /// <summary>
        /// 上位机的节点地址，默认是0x01，当连接PLC之后，将由PLC来设定当前的值。<br />
        /// The node address of the host computer is 0x01 by default. After connecting to the PLC, the PLC will set the current value.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public byte SA1 { get; set; } = 1;

        /// <summary>
        /// 上位机的单元号地址<br />
        /// Unit number and address of the computer
        /// </summary>
        /// <remarks>
        /// 00: CPU Unit<br />
        /// 10-1F: CPU Bus Unit
        /// </remarks>
        public byte SA2 { get; set; }

        /// <summary>
        /// 设备的标识号<br />
        /// Service ID. Used to identify the process generating the transmission. 
        /// Set the SID to any number between 00 and FF
        /// </summary>
        public byte SID { get; set; } = 0;

        /// <summary>
        /// 进行字读取的时候对于超长的情况按照本属性进行切割，默认500，如果不是CP1H及扩展模块的，可以设置为999，可以提高一倍的通信速度。<br />
        /// When reading words, it is cut according to this attribute for the case of overlength. The default is 500. 
        /// If it is not for CP1H and expansion modules, it can be set to 999, which can double the communication speed.
        /// </summary>
        public int ReadSplits { get; set; } = 500;

        private readonly byte[] handSingle = new byte[]
        {
            70,
            73,
            78,
            83,
            0,
            0,
            0,
            12,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0
        };

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 将普通的指令打包成完整的指令
        /// </summary>
        /// <param name="cmd">FINS的核心指令</param>
        /// <returns>完整的可用于发送PLC的命令</returns>
        private byte[] PackCommand(byte[] cmd)
        {
            byte[] array = new byte[26 + cmd.Length];
            Array.Copy(this.handSingle, 0, array, 0, 4);
            byte[] bytes = BitConverter.GetBytes(array.Length - 8);
            Array.Reverse(bytes);
            bytes.CopyTo(array, 4);
            array[11] = 2;
            array[16] = this.ICF;
            array[17] = this.RSV;
            array[18] = this.GCT;
            array[19] = this.DNA;
            array[20] = this.DA1;
            array[21] = this.DA2;
            array[22] = this.SNA;
            array[23] = this.SA1;
            array[24] = this.SA2;
            array[25] = this.SID;
            cmd.CopyTo(array, 26);
            return array;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool InitializationOnConnect()
        {
            byte[] operateResult = this.ReadFromCoreServer(this.handSingle, true, false);
            if (operateResult==null||operateResult.Length==0)
            {
                return false;
            }
            else
            {
                int num = BitConverter.ToInt32(new byte[]
                {
                    operateResult[15],
                    operateResult[14],
                    operateResult[13],
                    operateResult[12]
                }, 0);
                if (num != 0)
                {
                   return false;
                }
                else
                {
                    if (operateResult.Length >= 20)
                    {
                        this.SA1 = operateResult[19];
                    }
                    if (operateResult.Length >= 24)
                    {
                        this.DA1 = operateResult[23];
                    }
                    return true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public override byte[] PackCommandWithHeader(byte[] command)
        {
            return this.PackCommand(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="send"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public override byte[] UnpackResponseContent(byte[] send, byte[] response)
        {
            return OmronFinsNetHelper.ResponseValidAnalysis(response,out string err);
        }

   
        /// <example>
        /// 假设起始地址为D100，D100存储了温度，100.6℃值为1006，D101存储了压力，1.23Mpa值为123，D102,D103存储了产量计数，读取如下：
        /// 以下是读取不同类型数据的示例
        /// </example>
        public override byte[] Read(string address, ushort length, out bool res)
        {
            var re = OmronFinsNetHelper.Read(this, address, length, this.ReadSplits);
            res = re != null;
            return re;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public override bool[] ReadBool(string address, ushort length,out bool res)
        {
            var re = OmronFinsNetHelper.ReadBool(this, address, length, this.ReadSplits);
            res=re != null;
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override string ReadString(string address, ushort length, out bool res)
        {
            var re = base.ReadString(address, length, Encoding.UTF8,out res);
            return re;
        }

        /// <example>
        /// 假设起始地址为D100，D100存储了温度，100.6℃值为1006，D101存储了压力，1.23Mpa值为123，D102,D103存储了产量计数，读取如下：
        /// 以下是写入不同类型数据的示例
        /// </example>
        public override object Write(string address, byte[] value,out bool res)
        {
            var re = OmronFinsNetHelper.Write(this, address, value);
            res = re != null;
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, string value, out bool res)
        {
            return base.Write(address, value, Encoding.UTF8,out  res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="values"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public override object Write(string address, bool[] values, out bool res)
        {
            var re = OmronFinsNetHelper.Write(this, address, values);
            res = re != null;
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override INetMessage GetNewNetMessage()
        {
            return new FinsMessage();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
