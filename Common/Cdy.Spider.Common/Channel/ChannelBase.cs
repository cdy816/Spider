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
    [Obsolete]
    public abstract class ChannelBase : ICommChannel ,ICommChannelForFactory
    {

        #region ... Variables  ...
        
        /// <summary>
        /// 
        /// </summary>
        protected bool mIsConnected = false;

        private ManualResetEvent mTakeEvent;

        protected bool mIsTransparentRead=false;

        private int mOpenCount = 0;

        private bool mIsOpened = false;

        private object mOpenLock = new object();

        private List<ICommChannel.DataReceiveCallBackDelegate> mCallBack = new List<ICommChannel.DataReceiveCallBackDelegate>();


        private List<ICommChannel.DataReceiveCallBackDelegate2> mCallBack2 = new List<ICommChannel.DataReceiveCallBackDelegate2>();

        public event EventHandler CommChangedEvent;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public ChannelBase()
        {
            mTakeEvent = new ManualResetEvent(true);
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
        /// 
        /// </summary>
        public abstract string TypeName { get; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public Func<string,byte[],byte[]> ReceiveCallBack { get; set; }
        
        
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
                if (mOpenCount == 0)
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
        /// <param name="enable"></param>
        public void EnableTransparentRead(bool enable)
        {
            mIsTransparentRead = enable;
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
        public virtual void Prepare(List<string> deviceInfos)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected byte[] OnReceiveCallBack(string key,byte[] data)
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
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected object OnReceiveCallBack2(string key, object data)
        {
            bool res;
            foreach (var vv in mCallBack2)
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
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        /// <param name="waitResultCount"></param>
        /// <returns></returns>
        public byte[] Send(Span<byte> data, int waitResultCount)
        {
            byte[] redata = null;
            bool re = false;
            redata = SendInner(data, Data.DataSendTimeout, waitResultCount, out re);
            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    redata = SendInner(data, Data.DataSendTimeout, waitResultCount, out re);
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
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeount"></param>
        /// <param name="waitResultCount"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual byte[] SendInner(Span<byte> data,int timeount,int waitResultCount,out bool result)
        {
            result = false;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="waitPackageStartByte"></param>
        /// <param name="waitPackageEndByte"></param>
        /// <returns></returns>
        public byte[] Send(Span<byte> data, byte waitPackageStartByte, byte waitPackageEndByte)
        {
            byte[] redata = null;
            bool re = false;
            redata = SendInner(data, Data.DataSendTimeout, waitPackageStartByte, waitPackageEndByte, out re);
            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    redata = SendInner(data, Data.DataSendTimeout, waitPackageStartByte, waitPackageEndByte, out re);
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
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeount"></param>
        /// <param name="waitPackageStartByte"></param>
        /// <param name="waitPackageEndByte"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual byte[] SendInner(Span<byte> data, int timeount, byte waitPackageStartByte, byte waitPackageEndByte, out bool result)
        {
            result = false;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Send(Span<byte> data)
        {
            byte[] redata = null;
            bool re = false;
            redata = SendInner(data, Data.DataSendTimeout,out re);
            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    redata = SendInner(data, Data.DataSendTimeout, out re);
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
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual byte[] SendInner(Span<byte> data,int timeout, out bool result)
        {
            result = false;
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool SendAsync(Span<byte> data)
        {
            bool re = false;
            re = SendInnerAsync(data);

            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    re = SendInnerAsync(data);
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
        /// <param name="data"></param>
        /// <returns></returns>
        protected  virtual bool SendInnerAsync(Span<byte> data)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public object SendObject(string key, object value)
        {
            object redata = null;
            bool re = false;
            redata = SendObjectInner(key,value, Data.DataSendTimeout, out re);
            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    redata = SendObjectInner(key, value, Data.DataSendTimeout, out re);
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
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected  virtual object SendObjectInner(string key, object value,int timeout,out bool result)
        {
            result = false;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public byte[] SendObject(string key, Span<byte> value)
        {
            byte[] redata = null;
            bool re = false;
            redata = SendObjectInner(key, value, Data.DataSendTimeout, out re);
            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    redata = SendObjectInner(key, value, Data.DataSendTimeout, out re);
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
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual byte[] SendObjectInner(string key, Span<byte> value, int timeout, out bool result)
        {
            result = false;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object SendObject(object value)
        {
            object redata = null;
            bool re = false;
            redata = SendObjectInner(value, Data.DataSendTimeout, out re);
            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    redata = SendObjectInner(value, Data.DataSendTimeout, out re);
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
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual object SendObjectInner(object value, int timeout, out bool result)
        {
            result = false;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SendObjectAsync(string key, object value)
        {
            bool re = false;
            re = SendObjectInnerAsync(key, value);

            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    re = SendObjectInnerAsync(key, value);
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
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual bool SendObjectInnerAsync(string key,object value)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SendObjectAsync(string key, Span<byte> value)
        {
            bool re = false;
            re = SendObjectInnerAsync(key, value);

            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    re = SendObjectInnerAsync(key, value);
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
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual bool SendObjectInnerAsync(string key, Span<byte> value)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SendObjectAsync(object value)
        {
            bool re = false;
            re = SendObjectInnerAsync(value);

            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    re = SendObjectInnerAsync(value);
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
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual bool SendObjectInnerAsync(object value)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public virtual bool Write(byte[] buffer, int offset, int len)
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public virtual byte[] Receive(int count,int timeout,out int receivecount)
        {
            receivecount = 0;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public virtual byte[] Receive(int count)
        {
            return null;
        }


        /// <summary>
        /// 
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
                var re = mTakeEvent.WaitOne(timeout);
                if (re)
                    mTakeEvent.Reset();
                return re;
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
            lock (mTakeEvent)
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
        public abstract ICommChannel NewApi();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="callBack"></param>
        public void RegistorReceiveCallBack(ICommChannel.DataReceiveCallBackDelegate callBack)
        {
            mCallBack.Add(callBack);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callBack"></param>
        public void RegistorReceiveCallBack(ICommChannel.DataReceiveCallBackDelegate2 callBack)
        {
            mCallBack2.Add(callBack);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {

        }




        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...



    }
}
