using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Cdy.Spider.Common
{

    /// <summary>
    /// 
    /// </summary>
    public class SerialDeviceProxyBase : DeviceProxyBase
    {

        #region ... Variables  ...
        private string connectionId = string.Empty;
        private bool isClearCacheBeforeRead = false;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public SerialDeviceProxyBase(DriverRunnerBase driver):base(driver)
        {
            this.connectionId = DataExtend.GetUniqueStringByGuidAndRandom();
        }

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 是否在发送数据前清空缓冲数据，默认是false<br />
        /// Whether to empty the buffer before sending data, the default is false
        /// </summary>
        public bool IsClearCacheBeforeRead
        {
            get
            {
                return this.isClearCacheBeforeRead;
            }
            set
            {
                this.isClearCacheBeforeRead = value;
            }
        }
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
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool InitializationOnOpen()
        {
            return true;
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
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public virtual bool CheckDataReceiveCompletely(MemoryStream ms)
        {
            return false;
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
        /// </example>
        /// <returns>接收的完整的报文信息</returns>
        public virtual byte[] ReadFromCoreServer(byte[] send, bool hasResponseData = true, bool usePackAndUnpack = true)
        {
            byte[] array = usePackAndUnpack ? this.PackCommandWithHeader(send) : send;

            if (isClearCacheBeforeRead)
            {
                this.mDriver.ClearReveiceCach();
            }
            if (this.mDriver.Send(array))
            {
                if (hasResponseData && ReceiveTimeOut > 0)
                {
                    if (this.SleepTime > 0)
                    {
                        Thread.Sleep(this.SleepTime);
                    }

                    var revdata = this.mDriver.Read(this.ReceiveTimeOut, CheckDataReceiveCompletely);
                    if (revdata != null && revdata.Length > 0)
                    {
                        return usePackAndUnpack ? this.UnpackResponseContent(array, revdata) : revdata;
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

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
