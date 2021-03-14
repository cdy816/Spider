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
    public abstract class ChannelBase : ICommChannel, ICommChannelForFactory
    {

        #region ... Variables  ...
        
        /// <summary>
        /// 
        /// </summary>
        protected bool mIsConnected = false;

        private ManualResetEvent mTakeEvent;

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
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public object SendAndWait(string key, object data, params object[] paras)
        {
            return SendInner(key, data, Data.DataSendTimeout, paras);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        protected virtual object SendInner(string key,object data,int timeOut,object[] paras)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] SendAndWait(byte[] data,int start,int len, params string[] paras)
        {
            return SendAndWait(data,start,len, Data.DataSendTimeout,paras);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public  byte[] SendAndWait(byte[] data, int start, int len, int timeout, params string[] paras)
        {
            byte[] redata = null;
            bool re = false;
            redata = SendInner(data,start,len, timeout, out re,paras);
            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    redata = SendInner(data, start, len, timeout, out re,paras);
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
        /// <param name="datas"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        protected virtual byte[] SendInner(Span<byte> datas, int timeout, out bool result, params string[] paras)
        {
            result = false;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual byte[] SendInner(byte[] data, int start, int len, int timeout, out bool result, params string[] paras)
        {
            result = false;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="result"></param>
        protected virtual void SendInnerAsync(Span<byte> datas, out bool result, params string[] paras)
        {
            result = true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <param name="result"></param>
        /// <param name="paras"></param>
        protected virtual void SendInnerAsync(byte[] data, int start, int len, out bool result, params string[] paras)
        {
            result = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void SendAsync(byte[] data, int start, int len, params string[] paras)
        {
            bool re=false;
            SendInnerAsync(data,start,len,out re,paras);

            if(!re)
            {
                int count = 0;
                while (!re && count<Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    SendInnerAsync(data,start,len, out re,paras);
                    count++;
                }
                if(!re && IsConnected)
                {
                    ConnectedChanged(false);
                }
            }
            else if(!IsConnected)
            {
                ConnectedChanged(true);
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public byte[] SendAndWait(Span<byte> data, params string[] paras)
        {
            byte[] redata = null;
            bool re = false;
            redata = SendInner(data,Data.DataSendTimeout, out re,paras);
            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    redata = SendInner(data, Data.DataSendTimeout,  out re, paras);
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
        /// <param name="data"></param>
        /// <param name="paras"></param>
        public void SendAsync(Span<byte> data, params string[] paras)
        {
            bool re = false;
            SendInnerAsync(data, out re, paras);

            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    SendInnerAsync(data, out re, paras);
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



        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...



    }
}
