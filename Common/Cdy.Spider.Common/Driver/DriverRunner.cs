//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/5 14:49:13.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider
{
    public class DriverRunnerBase : IDriverRuntime
    {

        #region ... Variables  ...

        protected ICommChannel mComm;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, List<int>> mCachTags = new Dictionary<string, List<int>>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public IDeviceForDriver Device { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DriverData Data { get; set; }


        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

        /// <summary>
        /// 
        /// </summary>
        public virtual void Init()
        {
            mComm = ServiceLocator.Locator.Resolve<ICommChannelRuntimeManager>().GetChannel(Data.ChannelName);

            mComm.CommChangedCallBack = CommChanged;
            mComm.ReceiveAsyncCallBack = ReceiveData;
            mComm.SendDataAsyncCallBack = SendDataAsyncCallBack;


            Device.RegistorSetValueCallBack(WriteValue);

            foreach(var vv in Device.ListTags())
            {
                if(!string.IsNullOrEmpty(vv.DeviceInfo))
                {
                    if(mCachTags.ContainsKey(vv.DeviceInfo))
                    {
                        mCachTags[vv.DeviceInfo].Add(vv.Id);
                    }
                    else
                    {
                        mCachTags.Add(vv.DeviceInfo, new List<int>() { vv.Id });
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="value"></param>
        protected virtual void UpdateValue(string deviceInfo,object value)
        {
            if(mCachTags.ContainsKey(deviceInfo))
            {
                UpdateValue(mCachTags[deviceInfo], value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        protected virtual void UpdateValue(List<int> id,object value)
        {
            Device?.UpdateDeviceValue(id, value);
        }

        /// <summary>
        /// 处理写硬件设备
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="value"></param>
        protected virtual void WriteValue(string deviceInfo,object value)
        {

        }

        /// <summary>
        /// 通信状态改变
        /// </summary>
        protected virtual void CommChanged(bool result)
        {

        }


        /// <summary>
        /// 接收到设备数据
        /// </summary>
        protected virtual void ReceiveData(byte[] data)
        {

        }

        /// <summary>
        /// 异步发送数据回调
        /// </summary>
        /// <param name="result"></param>
        protected virtual void SendDataAsyncCallBack(bool result)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        public virtual void Start()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Stop()
        {

        }
    }
}
