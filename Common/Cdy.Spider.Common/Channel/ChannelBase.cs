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
    public abstract class ChannelBase : ICommChannel
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
        public Func<string,byte[],byte[]> ReceiveCallBack { get; set; }
        
        
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
                CommChangedCallBack?.Invoke(result);
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
        public byte[] SendAndWait(string key, byte[] data, params string[] paras)
        {
            return SendAndWait(key,data, Data.DataSendTimeout,paras);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public  byte[] SendAndWait(string key, byte[] data, int timeout, params string[] paras)
        {
            byte[] redata = null;
            bool re = false;
            redata = SendInner(key,data, timeout, out re);
            if (!re)
            {
                int count = 0;
                while (!re && count < Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    redata = SendInner(key, data,timeout, out re,paras);
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
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual byte[] SendInner(string key,byte[] data, int timeout, out bool result, params string[] paras)
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
        protected virtual void SendInnerAsync(string key, byte[] data,out bool result, params string[] paras)
        {
            result = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void SendAsync(string key, byte[] data, params string[] paras)
        {
            bool re=false;
            SendInnerAsync(key,data,out re);

            if(!re)
            {
                int count = 0;
                while (!re && count<Data.ReTryCount)
                {
                    Thread.Sleep(Data.ReTryDuration);
                    SendInnerAsync(key,data, out re,paras);
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


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...



    }
}
