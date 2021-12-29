//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/6 8:55:09.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider
{




    /// <summary>
    /// 
    /// </summary>
    public abstract class ChannelBase2 : ICommChannel2, ICommChannel2ForFactory
    {

        #region ... Variables  ...
        
        /// <summary>
        /// 
        /// </summary>
        protected bool mIsConnected = false;

        private AutoResetEvent mTakeEvent;

        protected bool mEnableSyncRead = false;

        private int mOpenCount = 0;

        private bool mIsOpened = false;

        private object mOpenLock = new object();

        private List<ICommChannel2.DataReceiveCallBackDelegate> mCallBack = new List<ICommChannel2.DataReceiveCallBackDelegate>();

        public event EventHandler CommChangedEvent;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public ChannelBase2()
        {
            mTakeEvent = new AutoResetEvent(true);
        }
        #endregion ...Constructor...

        #region ... Properties ...
        
        /// <summary>
        /// 
        /// </summary>
        public string Name { get { return Data.Name; } }

        /// <summary>
        /// 
        /// </summary>
        public  ChannelType Type { get { return Data.Type; } }

        /// <summary>
        /// 通信模式
        /// </summary>
        public virtual CommMode CommMode { get { return CommMode.Duplex; } }

        /// <summary>
        /// 
        /// </summary>
        public abstract string TypeName { get; }
        
        
        /// <summary>
        /// 
        /// </summary>
        public Action<bool> CommChangedCallBack { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ChannelData Data { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsConnected => mIsConnected;


        protected byte? mStartByte;

        protected byte? mEndByte;

        /// <summary>
        /// 
        /// </summary>
        public virtual string RemoteDescription
        {
            get
            {
                return string.Empty;
            }
        }


        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public virtual void Init()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        protected void ConnectedChanged(bool result)
        {
            if (mIsConnected == result) return;

            mIsConnected = result;
            Task.Run(() => {
                CommChangedEvent?.Invoke(this,null);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Close()
        {
            lock (mOpenLock)
            {
                mOpenCount--;
                if (mOpenCount <= 0)
                {
                    return InnerClose();
                }
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool InnerClose()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            LoggerService.Service.Info("Channel", "Start to Open channel " + this.Name);
            try
            {
                lock (mOpenLock)
                {
                    if (!mIsOpened)
                    {
                        mIsOpened = true;
                        return InnerOpen();
                    }
                    mOpenCount++;
                    return true;
                }
            }
            catch(Exception ex)
            {
                LoggerService.Service.Info("Channel", "Open channel " + this.Name+" failed."+ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool InnerOpen()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfos"></param>
        public virtual void Prepare(ChannelPrepareContext context)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected object OnReceiveCallBack(string key,object data)
        {
            bool res;
            foreach(var vv in mCallBack)
            {
                var rdata = vv.Invoke(key, data, out res);
                if (res) return rdata;
            }
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="startByte"></param>
        /// <param name="endByte"></param>
        public void RegistorPackageKeyByte(byte startByte, byte endByte)
        {
            mStartByte = startByte;
            mEndByte = endByte;
        }





        /// <summary>
        /// 
        /// </summary>
        public virtual void Flush()
        {
           
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool Take(int timeout)
        {
            lock (mTakeEvent)
            {
                if (timeout > 0)
                {
                    var re = mTakeEvent.WaitOne(timeout);
                    if (re)
                        mTakeEvent.Reset();
                    return re;
                }
                else
                {
                    mTakeEvent.WaitOne();
                    return true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Take()
        {
            return Take(Data.DataSendTimeout);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void Release()
        {
            //lock (mTakeEvent)
                mTakeEvent.Set();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public virtual void Load(XElement xe)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract ICommChannel2 NewApi();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="callBack"></param>
        public void RegistorReceiveCallBack(ICommChannel2.DataReceiveCallBackDelegate callBack)
        {
            mCallBack.Add(callBack);
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {

        }

        /// <summary>
        /// 读取值，面向高级通道
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object ReadValue(object value)
        {
            object redata = null;
            bool re = false;
            redata = ReadValueInner(value, Data.DataSendTimeout, out re);
            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    redata = ReadValueInner(value, Data.DataSendTimeout, out re);
                    count++;
                }
                if (!re && IsConnected)
                {
                    ConnectedChanged(false);
                }
            }
            else if (!IsConnected)
            {
                ConnectedChanged(true);
            }
            return redata;
        }

        /// <summary>
        /// 读取值，面向高级通道
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual object ReadValueInner(object value, int timeout, out bool result)
        {
            result = false;
            return null;
        }

        /// <summary>
        /// 读取值，面向高级通道
        /// </summary>
        /// <param name="valuebytes"></param>
        /// <returns></returns>
        public object ReadValue(byte[] valuebytes)
        {
            return ReadValue(valuebytes.AsSpan<byte>());
        }

        /// <summary>
        /// 读取值，面向高级通道
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual object ReadValueInner(Span<byte> value, int timeout, out bool result)
        {
            result = false;
            return null;
        }

        /// <summary>
        /// 读取值，面向高级通道
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object ReadValue(Span<byte> value)
        {
            object redata = null;
            bool re = false;
            redata = ReadValueInner(value, Data.DataSendTimeout, out re);
            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    redata = ReadValueInner(value, Data.DataSendTimeout, out re);
                    count++;
                }
                if (!re && IsConnected)
                {
                    ConnectedChanged(false);
                }
            }
            else if (!IsConnected)
            {
                ConnectedChanged(true);
            }
            return redata;
        }

        /// <summary>
        /// 写入值,面向高级通道
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public object WriteValue(string address, object value)
        {
            bool re = false;
            var val = WriteValueInner(address, value, Data.DataSendTimeout, out re);
            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    val = WriteValueInner(address, value, Data.DataSendTimeout, out re);
                    count++;
                }
                if (!re && IsConnected)
                {
                    ConnectedChanged(false);
                }
            }
            else if (!IsConnected)
            {
                ConnectedChanged(true);
            }
            return val;
        }

        /// <summary>
        /// 写入值,面向高级通道
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual object WriteValueInner(string address,object value, int timeout,out bool result)
        {
            result = false;
            return null;
        }

        /// <summary>
        /// 写入值不等返回值响应,面向高级通道
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public bool WriteValueNoWait(string address, object value)
        {
            bool re = false;
            re = WriteValueNoWaitInner(address, value, Data.DataSendTimeout);
            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                   re =  WriteValueNoWaitInner(address, value, Data.DataSendTimeout);
                    count++;
                }
                if (!re && IsConnected)
                {
                    ConnectedChanged(false);
                }
            }
            else if (!IsConnected)
            {
                ConnectedChanged(true);
            }
            return re;
        }

        /// <summary>
        /// 异步写入值,面向高级通道
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual bool WriteValueNoWaitInner(string address, object value, int timeout)
        {
            return false;
        }

        /// <summary>
        /// 直接从底层IO读取指定数量的数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public virtual byte[] Read(int count)
        {
            return null;
        }

        /// <summary>
        /// 直接从底层IO读取指定数量的数据
        /// </summary>
        /// <param name="count"></param>
        /// <param name="timecount">超时</param>
        /// <param name="receivecount"></param>
        /// <returns></returns>
        public virtual byte[] Read(int count, int timecount, out int receivecount)
        {
            receivecount = 0;
            return null;
        }


        /// <summary>
        /// 直接从底层IO读取指定数量的数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public virtual int Read(byte[] buffer, int offset, int len)
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public bool Send(Span<byte> data)
        {
            bool re = false;
            re = SendInner(data);

            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    re = SendInner(data);
                    count++;
                }
                if (!re && IsConnected)
                {
                    ConnectedChanged(false);
                }
            }
            else if (!IsConnected)
            {
                ConnectedChanged(true);
            }
            return re;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public bool Send(byte[] datas,int offset,int len)
        {
            return Send(new Span<byte>(datas, offset, len));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected virtual bool SendInner(Span<byte> data)
        {
            return false;
        }

        /// <summary>
        /// 直接从底层IO设备进行写入,发送数据;并等待数据返回
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public byte[] SendAndWait(Span<byte> datas)
        {
            byte[] redata = null;
            bool re = false;
            redata = SendAndWaitInner(datas, Data.DataSendTimeout, out re);
            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    redata = SendAndWaitInner(datas, Data.DataSendTimeout, out re);
                    count++;
                }
                if (!re && IsConnected)
                {
                    ConnectedChanged(false);
                }
            }
            else if (!IsConnected)
            {
                ConnectedChanged(true);
            }
            return redata;
        }

        /// <summary>
        /// 直接从底层IO设备进行写入,发送数据;并等待数据返回
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeout">超时</param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual byte[] SendAndWaitInner(Span<byte> data, int timeout, out bool result)
        {
            result = false;
            return null;
        }


        /// <summary>
        /// 直接从底层IO设备进行写入,发送数据;并等待数据指定数量的数据返回
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        /// <param name="waitResultCount"></param>
        /// <returns></returns>
        public byte[] SendAndWait(Span<byte> data, int waitResultCount)
        {
            byte[] redata = null;
            bool re = false;
            redata = SendAndWaitInner(data, Data.DataSendTimeout, waitResultCount, out re);
            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    redata = SendAndWaitInner(data, Data.DataSendTimeout, waitResultCount, out re);
                    count++;
                }
                if (!re && IsConnected)
                {
                    ConnectedChanged(false);
                }
            }
            else if (!IsConnected)
            {
                ConnectedChanged(true);
            }
            return redata;
        }

        /// <summary>
        /// 直接从底层IO设备进行写入,发送数据;并等待数据指定数量的数据返回
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeount"></param>
        /// <param name="waitResultCount"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual byte[] SendAndWaitInner(Span<byte> data, int timeount, int waitResultCount, out bool result)
        {
            result = false;
            return null;
        }

        /// <summary>
        ///  直接从底层IO设备进行写入,发送数据;并等待数据指定数据包头、尾的数据返回
        /// </summary>
        /// <param name="data"></param>
        /// <param name="waitPackageStartByte"></param>
        /// <param name="waitPackageEndByte"></param>
        /// <returns></returns>
        public byte[] SendAndWait(Span<byte> data, byte waitPackageStartByte, byte waitPackageEndByte)
        {
            byte[] redata = null;
            bool re = false;
            redata = SendAndWaitInner(data, Data.DataSendTimeout, waitPackageStartByte, waitPackageEndByte, out re);
            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    redata = SendAndWaitInner(data, Data.DataSendTimeout, waitPackageStartByte, waitPackageEndByte, out re);
                    count++;
                }
                if (!re && IsConnected)
                {
                    ConnectedChanged(false);
                }
            }
            else if (!IsConnected)
            {
                ConnectedChanged(true);
            }
            return redata;
        }

        /// <summary>
        ///  直接从底层IO设备进行写入,发送数据;并等待数据指定数据包头、尾的数据返回
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeount"></param>
        /// <param name="waitPackageStartByte"></param>
        /// <param name="waitPackageEndByte"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual byte[] SendAndWaitInner(Span<byte> data, int timeount, byte waitPackageStartByte, byte waitPackageEndByte, out bool result)
        {
            result = false;
            return null;
        }


        /// <summary>
        /// 直接从底层IO设备进行写入,发送数据;并等待数据返回
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public byte[] SendAndWait(byte[] buffer, int offset, int len)
        {
            return SendAndWait(new Span<byte>(buffer, offset, len));
        }

        /// <summary>
        /// 使能同步读写
        /// </summary>
        /// <param name="enable"></param>
        public void EnableSyncRead(bool enable)
        {
            mEnableSyncRead = enable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsRawComm()
        {
            return Type == ChannelType.TcpClient || Type == ChannelType.TcpServer || Type == ChannelType.UdpClient || Type == ChannelType.UdpServer || Type == ChannelType.PortClient || Type == ChannelType.PortServer || Type == ChannelType.WebSocketClient || Type == ChannelType.WebSocketServer || Type == ChannelType.MQTTClient || Type == ChannelType.MQTTServer;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void ClearBuffer()
        {
            
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...



    }
}
