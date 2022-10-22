using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.BeckhoffDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class BeckoffNetProxy : NetworkDeviceProxyBase
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private byte[] targetAMSNetId = new byte[8];

        /// <summary>
        /// 
        /// </summary>
        private byte[] sourceAMSNetId = new byte[8];

        /// <summary>
        /// 
        /// </summary>
        private string senderAMSNetId = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        private string _targetAmsNetID = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        private bool useAutoAmsNetID = false;

        /// <summary>
        /// 
        /// </summary>
        private bool useTagCache = false;

        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<string, uint> tagCaches = new Dictionary<string, uint>();

        /// <summary>
        /// 
        /// </summary>
        private readonly object tagLock = new object();


        private readonly SoftIncrementCount incrementCount = new SoftIncrementCount(2147483647L, 1L, 1);
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        public BeckoffNetProxy(DriverRunnerBase driver) : base(driver)
        {
            this.targetAMSNetId[4] = 1;
            this.targetAMSNetId[5] = 1;
            this.targetAMSNetId[6] = 83;
            this.targetAMSNetId[7] = 3;
            this.sourceAMSNetId[4] = 1;
            this.sourceAMSNetId[5] = 1;
            base.ByteTransform = new RegularByteTransform();
            WordLength = 2;
        }

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 是否使用标签的名称缓存功能，默认为False
        /// </summary>
        public bool UseTagCache
        {
            get
            {
                return this.useTagCache;
            }
            set
            {
                this.useTagCache = value;
            }
        }

        /// <summary>
        /// 是否使用服务器自动的NETID信息，默认手动设置
        /// </summary>
        public bool UseAutoAmsNetID
        {
            get
            {
                return this.useAutoAmsNetID;
            }
            set
            {
                this.useAutoAmsNetID = value;
            }
        }

        /// <summary>
        /// 获取或设置Ams的端口号信息，TwinCAT2，端口号801,811,821,831；TwinCAT3，端口号为851,852,853
        /// </summary>
        public int AmsPort
        {
            get
            {
                return (int)BitConverter.ToUInt16(this.targetAMSNetId, 6);
            }
            set
            {
                this.targetAMSNetId[6] = BitConverter.GetBytes(value)[0];
                this.targetAMSNetId[7] = BitConverter.GetBytes(value)[1];
            }
        }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 目标的地址，举例 192.168.0.1.1.1；也可以是带端口号 192.168.0.1.1.1:801<br />
        /// The address of the destination, for example 192.168.0.1.1.1; it can also be the port number 192.168.0.1.1.1: 801
        /// </summary>
        /// <remarks>
        /// Port：1: AMS Router; 2: AMS Debugger; 800: Ring 0 TC2 PLC; 801: TC2 PLC Runtime System 1; 811: TC2 PLC Runtime System 2; <br />
        /// 821: TC2 PLC Runtime System 3; 831: TC2 PLC Runtime System 4; 850: Ring 0 TC3 PLC; 851: TC3 PLC Runtime System 1<br />
        /// 852: TC3 PLC Runtime System 2; 853: TC3 PLC Runtime System 3; 854: TC3 PLC Runtime System 4; ...
        /// </remarks>
        /// <param name="amsNetId">AMSNet Id地址</param>
        // Token: 0x060012AE RID: 4782 RVA: 0x000774B4 File Offset: 0x000756B4
        public void SetTargetAMSNetId(string amsNetId)
        {
            if (string.IsNullOrEmpty(amsNetId))
            {
                AdsHelper.StrToAMSNetId(amsNetId).CopyTo(this.targetAMSNetId, 0);
                this._targetAmsNetID = amsNetId;
            }
        }

        /// <summary>
        /// 设置原目标地址 举例 192.168.0.100.1.1；也可以是带端口号 192.168.0.100.1.1:34567<br />
        /// Set the original destination address Example: 192.168.0.100.1.1; it can also be the port number 192.168.0.100.1.1: 34567
        /// </summary>
        /// <param name="amsNetId">原地址</param>
        // Token: 0x060012AF RID: 4783 RVA: 0x000774EC File Offset: 0x000756EC
        public void SetSenderAMSNetId(string amsNetId)
        {
            if (!string.IsNullOrEmpty(amsNetId))
            {
                AdsHelper.StrToAMSNetId(amsNetId).CopyTo(this.sourceAMSNetId, 0);
                this.senderAMSNetId = amsNetId;
            }
        }

        /// <summary>
        /// 获取当前目标的AMS网络的ID信息
        /// </summary>
        /// <returns>AMS目标信息</returns>
        public string GetTargetAMSNetId()
        {
            return AdsHelper.GetAmsNetIdString(this.targetAMSNetId, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public override byte[] PackCommandWithHeader(byte[] command)
        {
            uint value = (uint)this.incrementCount.GetCurrentValue();
            this.targetAMSNetId.CopyTo(command, 6);
            this.sourceAMSNetId.CopyTo(command, 14);
            command[34] = BitConverter.GetBytes(value)[0];
            command[35] = BitConverter.GetBytes(value)[1];
            command[36] = BitConverter.GetBytes(value)[2];
            command[37] = BitConverter.GetBytes(value)[3];
            return base.PackCommandWithHeader(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="send"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public override byte[] UnpackResponseContent(byte[] send, byte[] response)
        {
            if (response.Length >= 38)
            {
                ushort num = base.ByteTransform.TransUInt16(response, 22);
                bool operateResult = AdsHelper.CheckResponse(response);
                if (!operateResult)
                {
                    return null;
                }
                else if (num == 1)
                {
                    return response.RemoveBegin(42);
                }
                else if (num == 2)
                {
                    return response.RemoveBegin(46);
                }
                else if (num == 3)
                {
                    return new byte[0];
                }
                else if (num == 4)
                {
                    return response.RemoveBegin(42);
                }
                else if (num == 5)
                {
                    return response.RemoveBegin(42);
                }
                else if (num == 6)
                {
                    return response.RemoveBegin(42);
                }
                else if (num == 7)
                {
                    return new byte[0];
                }
                else if (num == 9)
                {
                    return response.RemoveBegin(46);
                }
            }
            return base.UnpackResponseContent(send, response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="read"></param>
        protected override void ExtraAfterReadFromCoreServer(byte[] read)
        {
            if(read == null && this.useTagCache)
            {
                lock(this.tagLock)
                {
                    this.tagCaches.Clear();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override INetMessage GetNewNetMessage()
        {
            return new AdsNetMessage();
        }

        public override bool InitializationOnConnect()
        {
            if (string.IsNullOrEmpty(this.senderAMSNetId) && string.IsNullOrEmpty(this._targetAmsNetID))
            {
                this.useAutoAmsNetID = true;
            }
            if (this.useAutoAmsNetID)
            {
                byte[] localNetId = this.GetLocalNetId();
                if (localNetId==null)
                {
                    return false;
                }
                else
                {
                    if (localNetId.Length >= 12)
                    {
                        Array.Copy(localNetId, 6, this.targetAMSNetId, 0, 6);
                    }
                    var operateResult = this.mDriver.Send( AdsHelper.PackAmsTcpHelper(AmsTcpHeaderFlags.PortConnect, new byte[2]));
                    if (operateResult==null)
                    {
                        return false;
                    }
                    else
                    {
                        byte[] operateResult2 = this.ReceiveByMessage( base.ReceiveTimeOut, this.GetNewNetMessage());
                        if (operateResult2==null)
                        {
                            return false;
                        }
                        else
                        {
                            if (operateResult2.Length >= 14)
                            {
                                Array.Copy(operateResult2, 6, this.sourceAMSNetId, 0, 8);
                            }
                            return true;
                        }
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(this.senderAMSNetId))
                {
                    EndPoint ep = ((dynamic)(mDriver.RawChannel)).LocalEndPoint;
                    IPEndPoint ipendPoint = (IPEndPoint)ep;
                    this.sourceAMSNetId[6] = BitConverter.GetBytes(ipendPoint.Port)[0];
                    this.sourceAMSNetId[7] = BitConverter.GetBytes(ipendPoint.Port)[1];
                    ipendPoint.Address.GetAddressBytes().CopyTo(this.sourceAMSNetId, 0);
                }
                bool flag9 = this.useTagCache;
                if (flag9)
                {
                    object obj = this.tagLock;
                    lock (obj)
                    {
                        this.tagCaches.Clear();
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private byte[] GetLocalNetId()
        {
            var operateResult2 = mDriver.Send(AdsHelper.PackAmsTcpHelper(AmsTcpHeaderFlags.GetLocalNetId, new byte[4]));
            if (operateResult2==null)
            {
                return null;
            }
            else
            {
                byte[] operateResult3 = this.ReceiveByMessage(base.ReceiveTimeOut, this.GetNewNetMessage());
                if (operateResult3==null)
                {
                    return null;
                }
                else
                {
                    return operateResult3;
                }
            }
        }

        /// <summary>
        /// 根据当前标签的地址获取到内存偏移地址<br />
        /// Get the memory offset address based on the address of the current label
        /// </summary>
        /// <param name="address">带标签的地址信息，例如s=A,那么标签就是A</param>
        /// <returns>内存偏移地址</returns>
        public uint? ReadValueHandle(string address)
        {
            if (!address.StartsWith("s="))
            {
                return null;
            }
            else
            {
                byte[] operateResult = AdsHelper.BuildReadWriteCommand(address, 4, false, AdsHelper.StrToAdsBytes(address.Substring(2)));
                if (operateResult==null)
                {
                    return null;
                }
                else
                {
                    var operateResult2 = this.ReadFromCoreServer(operateResult);
                    if (operateResult2==null)
                    {
                        return null;
                    }
                    else
                    {
                        return BitConverter.ToUInt32(operateResult2, 0);
                    }
                }
            }
        }

        /// <summary>
        /// 将字符串的地址转换为内存的地址，其他地址则不操作<br />
        /// Converts the address of a string to the address of a memory, other addresses do not operate
        /// </summary>
        /// <param name="address">地址信息，s=A的地址转换为i=100000的形式</param>
        /// <returns>地址</returns>
        public string TransValueHandle(string address)
        {
            if (address.StartsWith("s=") || address.StartsWith("S="))
            {
                if (this.useTagCache)
                {
                    lock (this.tagLock)
                    {
                        if (this.tagCaches.ContainsKey(address))
                        {
                            return string.Format("i={0}", this.tagCaches[address]);
                        }
                    }
                }
                var operateResult = this.ReadValueHandle(address);
                if (operateResult==null)
                {
                    return null;
                }
                else
                {
                    if (this.useTagCache)
                    {
                        lock (this.tagLock)
                        {
                            if (!this.tagCaches.ContainsKey(address))
                            {
                                this.tagCaches.Add(address, operateResult.Value);
                            }
                        }
                    }
                    return string.Format("i={0}", operateResult);
                }
            }
            else
            {
                return address;
            }
        }


        /// <summary>
        /// 读取Ads设备的设备信息。主要是版本号，设备名称<br />
        /// Read the device information of the Ads device. Mainly version number, device name
        /// </summary>
        /// <returns>设备信息</returns>
        public AdsDeviceInfo ReadAdsDeviceInfo()
        {
            var operateResult = AdsHelper.BuildReadDeviceInfoCommand();
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                var operateResult2 = this.ReadFromCoreServer(operateResult);
                if (operateResult2==null)
                {
                    return null;
                }
                else
                {
                    return new AdsDeviceInfo(operateResult2);
                }
            }
        }

        /// <summary>
        /// 读取Ads设备的状态信息，其中是Ads State，是Device State<br />
        /// Read the status information of the Ads device, where  is the Ads State, and is the Device State
        /// </summary>
        /// <returns>设备状态信息</returns>
        public Tuple<ushort, ushort> ReadAdsState()
        {
            var operateResult = AdsHelper.BuildReadStateCommand();
            if (operateResult==null)
            {
                return null;
            }
            else
            {
                var operateResult2 = this.ReadFromCoreServer(operateResult);
                if (operateResult2==null)
                {
                    return null;
                }
                else
                {
                   return new Tuple<ushort,ushort>(BitConverter.ToUInt16(operateResult2, 0), BitConverter.ToUInt16(operateResult2, 2));
                }
            }
        }

        /// <summary>
        /// 写入Ads的状态，可以携带数据信息，数据可以为空<br />
        /// Write the status of Ads, can carry data information, and the data can be empty
        /// </summary>
        /// <param name="state">ads state</param>
        /// <param name="deviceState">device state</param>
        /// <param name="data">数据信息</param>
        /// <returns>是否写入成功</returns>
        public object WriteAdsState(short state, short deviceState, byte[] data,out bool res)
        {
            var operateResult = AdsHelper.BuildWriteControlCommand(state, deviceState, data);
            if (operateResult==null)
            {
                res=false;
                return null;
            }
            else
            {
                res = true;
               return this.ReadFromCoreServer(operateResult);
            }
        }

        /// <summary>
        /// 释放当前的系统句柄，该句柄是通过 获取的
        /// </summary>
        /// <param name="handle">句柄</param>
        /// <returns>是否释放成功</returns>
        public object ReleaseSystemHandle(uint handle,out bool res)
        {
            var operateResult = AdsHelper.BuildReleaseSystemHandle(handle);
            if (operateResult==null)
            {
                res = false;
                return null;
            }
            else
            {
                res = true;
                return this.ReadFromCoreServer(operateResult);
            }
        }

        /// <summary>
        /// 读取PLC的数据，地址共有三种格式，一：I,Q,M数据信息，举例M0,M100；二：内存地址，i=100000；三：标签地址，s=A<br />
        /// Read PLC data, there are three formats of address, one: I, Q, M data information, such as M0, M100; two: memory address, i = 100000; three: tag address, s = A
        /// </summary>
        /// <param name="address">地址信息，地址共有三种格式，一：I,Q,M数据信息，举例M0,M100；二：内存地址，i=100000；三：标签地址，s=A</param>
        /// <param name="length">长度</param>
        /// <returns>包含是否成功的结果对象</returns>
        public override byte[] Read(string address, ushort length,out bool res)
        {
            string operateResult = this.TransValueHandle(address);
            if (operateResult==null)
            {
                res = false;
                return null;
            }
            else
            {
                address = operateResult;
                var operateResult2 = AdsHelper.BuildReadCommand(address, (int)length, false);
                if (operateResult2==null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    res = true;
                    return this.ReadFromCoreServer(operateResult2);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <param name="offset"></param>
        /// <param name="index"></param>
        /// <param name="len"></param>
        private void AddLengthAndOffset(List<ushort> length, List<int> offset, ref int index, int len)
        {
            length.Add((ushort)len);
            offset.Add(index);
            index += len;
        }


        /// <summary>
        /// 写入PLC的数据，地址共有三种格式，一：I,Q,M数据信息，举例M0,M100；二：内存地址，i=100000；三：标签地址，s=A<br />
        /// There are three formats for the data written into the PLC. One: I, Q, M data information, such as M0, M100; two: memory address, i = 100000; three: tag address, s = A
        /// </summary>
        /// <param name="address">地址信息，地址共有三种格式，一：I,Q,M数据信息，举例M0,M100；二：内存地址，i=100000；三：标签地址，s=A</param>
        /// <param name="value">数据值</param>
        /// <returns>是否写入成功</returns>
        public override object Write(string address, byte[] value,out bool res)
        {
            string operateResult = this.TransValueHandle(address);
            if (operateResult==null)
            {
                res=false;
                return false;
            }
            else
            {
                address = operateResult;
               var operateResult2 = AdsHelper.BuildWriteCommand(address, value, false);
                if (operateResult2==null)
                {
                    res = false;
                    return false;
                }
                else
                {
                    var val = this.ReadFromCoreServer(operateResult2);
                    res = val != null;
                    return val;
                }
            }
        }

        /// <summary>
        /// 读取PLC的数据，地址共有三种格式，一：I,Q,M数据信息，举例M0,M100；二：内存地址，i=100000；三：标签地址，s=A<br />
        /// Read PLC data, there are three formats of address, one: I, Q, M data information, such as M0, M100; two: memory address, i = 100000; three: tag address, s = A
        /// </summary>
        /// <param name="address">PLC的地址信息，例如 M10</param>
        /// <param name="length">数据长度</param>
        /// <returns>包含是否成功的结果对象</returns>
        public override bool[] ReadBool(string address, ushort length,out bool res)
        {
            var operateResult = this.TransValueHandle(address);
            if (operateResult==null)
            {
                res = false;
                return null;
            }
            else
            {
                address = operateResult;
                var operateResult2 = AdsHelper.BuildReadCommand(address, (int)length, true);
                if (operateResult2==null)
                {
                    res = false;
                    return null;
                }
                else
                {
                   var operateResult3 = this.ReadFromCoreServer(operateResult2);
                    if (operateResult3==null)
                    {
                        res = false;
                        return null;
                    }
                    else
                    {
                        res = true;
                        return (from m in operateResult3 select m > 0).ToArray<bool>();
                    }
                }
            }
        }

        /// <summary>
        /// 写入PLC的数据，地址共有三种格式，一：I,Q,M数据信息，举例M0,M100；二：内存地址，i=100000；三：标签地址，s=A<br />
        /// There are three formats for the data written into the PLC. One: I, Q, M data information, such as M0, M100; two: memory address, i = 100000; three: tag address, s = A
        /// </summary>
        /// <param name="address">地址信息</param>
        /// <param name="value">数据值</param>
        /// <returns>是否写入成功</returns>
        public override object Write(string address, bool[] value, out bool res)
        {
            var operateResult = this.TransValueHandle(address);
            if (operateResult == null)
            {
                res = false;
                return null;
            }
            else
            {
                address = operateResult;
                byte[] operateResult2 = AdsHelper.BuildWriteCommand(address, value, true);
                if (operateResult2 == null)
                {
                    res = false;
                    return null;
                }
                else
                {
                    var val = this.ReadFromCoreServer(operateResult2);
                    res = val != null;
                    return val;
                }
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
