using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Cdy.Spider.Common
{
    public class NetworkDeviceProxyBase : DeviceProxyBase
    {

        #region ... Variables  ...
        public Guid Token { get; set; }
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        public NetworkDeviceProxyBase(DriverRunnerBase driver) : base(driver)
        {
        }
        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual byte[] PackCommandWithHeader(byte[] command)
        {
            return command;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="send"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public virtual byte[] UnpackResponseContent(byte[] send, byte[] response)
        {
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool InitializationOnConnect()
        {
            return true;
        }

        /// <summary>
        /// 获取一个新的消息对象的方法，需要在继承类里面进行重写<br />
        /// The method to get a new message object needs to be overridden in the inheritance class
        /// </summary>
        /// <returns>消息类对象</returns>
        // Token: 0x060025B7 RID: 9655 RVA: 0x0004F310 File Offset: 0x0004D510
        protected virtual INetMessage GetNewNetMessage()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="send"></param>
        /// <returns></returns>
        public byte[] ReadFromCoreServer(IEnumerable<byte[]> send)
        {
            List<byte> list = new List<byte>();
            foreach (byte[] arg in send)
            {
                byte[] operateResult = ReadFromCoreServer(arg);
                if (operateResult==null)
                {
                    return null;
                }
                else
                {
                    list.AddRange(operateResult);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="send"></param>
        /// <returns></returns>
        public virtual byte[] ReadFromCoreServer(byte[] send)
        {
            return this.ReadFromCoreServer(send, true, true);
        }
        /// <summary>
        /// 将数据报文发送指定的网络通道上，根据当前指定的类型，返回一条完整的数据指令<br />
        /// Sends a data message to the specified network channel, and returns a complete data command according to the currently specified  type
        /// </summary>
        /// <param name="socket">指定的套接字</param>
        /// <param name="send">发送的完整的报文信息</param>
        /// <param name="hasResponseData">是否有等待的数据返回，默认为 true</param>
        /// <param name="usePackAndUnpack">是否需要对命令重新打包，在重写方法后才会有影响</param>
        /// <remarks>
        /// 无锁的基于套接字直接进行叠加协议的操作。
        /// </remarks>
        /// <example>
        /// 假设你有一个自己的socket连接了设备，本组件可以直接基于该socket实现modbus读取，三菱读取，西门子读取等等操作，前提是该服务器支持多协议，虽然这个需求听上去比较变态，但本组件支持这样的操作。
        /// </example>
        /// <returns>接收的完整的报文信息</returns>
        public virtual byte[] ReadFromCoreServer(byte[] send, bool hasResponseData = true, bool usePackAndUnpack = true)
        {
            byte[] array = usePackAndUnpack ? this.PackCommandWithHeader(send) : send;
            INetMessage newNetMessage = this.GetNewNetMessage();
            if(newNetMessage != null)
            {
                newNetMessage.SendBytes = array;
            }

            if (this.mDriver.Send(array))
            {
                if (hasResponseData && ReceiveTimeOut > 0)
                {
                    if (this.SleepTime > 0)
                    {
                        Thread.Sleep(this.SleepTime);
                    }
                    var operateResult2 = this.ReceiveByMessage(this.ReceiveTimeOut, newNetMessage); 
                    if(operateResult2 != null)
                    {
                        bool flag10 = newNetMessage != null && !newNetMessage.CheckHeadBytesLegal(Token.ToByteArray());
                        if(flag10)
                        {
                            return null;
                        }
                        else
                        {
                            return usePackAndUnpack ? this.UnpackResponseContent(array, operateResult2) : operateResult2;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return new byte[0];
                }
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 接收一条完整的 数据内容，需要指定超时时间，单位为毫秒。 <br />
        /// Receive a complete  data content, Need to specify a timeout period in milliseconds
        /// </summary>
        /// <param name="socket">网络的套接字</param>
        /// <param name="timeOut">超时时间，单位：毫秒</param>
        /// <param name="netMessage">消息的格式定义</param>
        /// <param name="reportProgress">接收消息的时候的进度报告</param>
        /// <returns>带有是否成功的byte数组对象</returns>
        protected virtual byte[] ReceiveByMessage(int timeOut, INetMessage netMessage)
        {
            byte[] result=null;
            if (netMessage == null)
            {
                result = this.Receive( -1, timeOut);
            }
            else
            {
                //bool flag2 = netMessage.ProtocolHeadBytesLength < 0;
                if (netMessage.ProtocolHeadBytesLength < 0)
                {
                    byte[] bytes = BitConverter.GetBytes(netMessage.ProtocolHeadBytesLength);
                    int num = (int)(bytes[3] & 15);
                    byte[] operateResult = null;
                    //bool flag3 = num == 1;
                    if (num == 1)
                    {
                        operateResult = this.ReceiveCommandLineFromSocket( bytes[1], timeOut);
                    }
                    else if(num == 2)
                    {
                        operateResult = this.ReceiveCommandLineFromSocket(bytes[1], bytes[0], timeOut);
                    }

                 
                    if (operateResult == null)
                    {
                        return null;
                    }
                    else
                    {
                        netMessage.HeadBytes = operateResult;
                        SpecifiedCharacterMessage specifiedCharacterMessage = netMessage as SpecifiedCharacterMessage;
                        if (specifiedCharacterMessage != null)
                        {
                            if (specifiedCharacterMessage.EndLength == 0)
                            {
                                result = operateResult;
                            }
                            else
                            {
                                byte[]  operateResult2 = this.Receive((int)specifiedCharacterMessage.EndLength, timeOut);
                                if (operateResult2==null)
                                {
                                    result = operateResult2;
                                }
                                else
                                {
                                    result = DataExtend.SpliceArray<byte>(new byte[][]
                                    {
                                            operateResult,
                                            operateResult2
                                    });
                                }
                            }
                        }
                        else
                        {
                            result = operateResult;
                        }
                    }
                }
                else
                {
                    byte[] operateResult3 = this.Receive( netMessage.ProtocolHeadBytesLength, timeOut);
                    if (operateResult3==null || operateResult3.Length==0)
                    {
                        result = operateResult3;
                    }
                    else
                    {
                        int num2 = netMessage.PependedUselesByteLength(operateResult3);
                        //bool flag11 = num2 > 0;
                        if (num2 > 0)
                        {
                            byte[] operateResult4 = this.Receive( num2, timeOut);
                            if (operateResult4==null)
                            {
                                return operateResult4;
                            }
                            operateResult3 = DataExtend.SpliceArray<byte>(new byte[][]
                            {
                                operateResult3.RemoveBegin(num2),
                                operateResult4
                            });
                        }
                        netMessage.HeadBytes = operateResult3;
                        int contentLengthByHeadBytes = netMessage.GetContentLengthByHeadBytes();
                        if (contentLengthByHeadBytes == 0)
                        {
                            result = operateResult3;
                        }
                        else
                        {
                            byte[] array = new byte[netMessage.ProtocolHeadBytesLength + contentLengthByHeadBytes];
                            operateResult3.CopyTo(array, 0);
                            var operateResult5 = this.Receive( array, netMessage.ProtocolHeadBytesLength, contentLengthByHeadBytes, timeOut);
                            if (operateResult5>0)
                            {
                                result = array;
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 接收一行命令数据，需要自己指定这个结束符，默认超时时间为60秒，也即是60000，单位是毫秒<br />
        /// To receive a line of command data, you need to specify the terminator yourself. The default timeout is 60 seconds, which is 60,000, in milliseconds.
        /// </summary>
        /// <param name="socket">网络套接字</param>
        /// <param name="endCode">结束符信息</param>
        /// <param name="timeout">超时时间，默认为60000，单位为毫秒，也就是60秒</param>
        /// <returns>带有结果对象的数据信息</returns>
        protected byte[] ReceiveCommandLineFromSocket(byte endCode, int timeout = 60000)
        {
            List<byte> list = new List<byte>(128);
            byte[] result=null;
            try
            {
                DateTime now = DateTime.Now;
                bool flag = false;
                while ((DateTime.Now - now).TotalMilliseconds < (double)timeout)
                {
                    byte[] operateResult = this.Receive(1, timeout);
                    if (operateResult==null)
                    {
                        //return null;
                        break;
                    }
                    list.AddRange(operateResult);
                    if (operateResult[0] == endCode)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    result = list.ToArray();
                }
                else
                {
                    Debug.Print("超时");
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        /// <summary>
        /// 接收一行命令数据，需要自己指定这个结束符，默认超时时间为60秒，也即是60000，单位是毫秒<br />
        /// To receive a line of command data, you need to specify the terminator yourself. The default timeout is 60 seconds, which is 60,000, in milliseconds.
        /// </summary>
        /// <param name="socket">网络套接字</param>
        /// <param name="endCode1">结束符1信息</param>
        /// <param name="endCode2">结束符2信息</param>
        /// /// <param name="timeout">超时时间，默认无穷大，单位毫秒</param>
        /// <returns>带有结果对象的数据信息</returns>
        protected byte[] ReceiveCommandLineFromSocket( byte endCode1, byte endCode2, int timeout = 60000)
        {
            List<byte> list = new List<byte>(128);
            byte[] result=null;
            try
            {
                DateTime now = DateTime.Now;
                bool flag = false;
                while ((DateTime.Now - now).TotalMilliseconds < (double)timeout)
                {
                    byte[] operateResult = this.Receive(1, timeout);
                    if (operateResult==null)
                    {
                        break;
                    }
                    list.AddRange(operateResult);
                    if (operateResult[0] == endCode2)
                    {
                        if (list.Count > 1 && list[list.Count - 2] == endCode1)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
               if(flag)
                {
                    result = list.ToArray();
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 接收固定长度的字节数组，允许指定超时时间，默认为60秒，当length大于0时，接收固定长度的数据内容，当length小于0时，接收不大于2048长度的随机数据信息<br />
        /// Receiving a fixed-length byte array, allowing a specified timeout time. The default is 60 seconds. When length is greater than 0, 
        /// fixed-length data content is received. When length is less than 0, random data information of a length not greater than 2048 is received.
        /// </summary>
        /// <param name="length">准备接收的数据长度，当length大于0时，接收固定长度的数据内容，当length小于0时，接收不大于1024长度的随机数据信息</param>
        /// <param name="timeOut">单位：毫秒，超时时间，默认为60秒，如果设置小于0，则不检查超时时间</param>
        /// <returns>包含了字节数据的结果类</returns>
        // Token: 0x06002448 RID: 9288 RVA: 0x000BDC58 File Offset: 0x000BBE58
        protected byte[] Receive(int length, int timeOut = 60000)
        {
            if (length == 0)
            {
                return new byte[0];
            }
            else
            {

                return mDriver.Read(length, timeOut);
            }
        }

        /// <summary>
        /// 接收固定长度的字节数组，允许指定超时时间，默认为60秒，当length大于0时，接收固定长度的数据内容，当length小于0时，buffer长度的缓存数据<br />
        /// Receiving a fixed-length byte array, allowing a specified timeout time. The default is 60 seconds. When length is greater than 0, 
        /// fixed-length data content is received. When length is less than 0, random data information of a length not greater than 2048 is received.
        /// </summary>
        /// <param name="socket">网络通讯的套接字<br />Network communication socket</param>
        /// <param name="buffer">等待接收的数据缓存信息</param>
        /// <param name="offset">开始接收数据的偏移地址</param>
        /// <param name="length">准备接收的数据长度，当length大于0时，接收固定长度的数据内容，当length小于0时，接收不大于1024长度的随机数据信息</param>
        /// <param name="timeOut">单位：毫秒，超时时间，默认为60秒，如果设置小于0，则不检查超时时间</param>
        /// <returns>包含了字节数据的结果类</returns>
        // Token: 0x06002447 RID: 9287 RVA: 0x000BDAF4 File Offset: 0x000BBCF4
        protected int Receive(byte[] buffer, int offset, int length, int timeOut = 60000)
        {
            if (length == 0)
            {
               return 0;
            }
            else
            {
                return mDriver.Read(buffer, offset, length, timeOut);

            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
