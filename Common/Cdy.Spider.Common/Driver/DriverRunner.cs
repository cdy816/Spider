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
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider
{
    /// <summary>
    /// 驱动基类
    /// </summary>
    public abstract class DriverRunnerBase : IDriverRuntime, IDriverForFactory
    {

        #region ... Variables  ...

        protected ICommChannel mComm;

        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<string, List<int>> mCachTags = new Dictionary<string, List<int>>();

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
        public virtual DriverData Data { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get { return Data.Name; }set { Data.Name = value; } }

        /// <summary>
        /// 
        /// </summary>
        public abstract string TypeName { get; }

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
            mComm = Device.GetCommChannel();
            if (mComm != null)
            {
                mComm.CommChangedEvent += MComm_CommChangedEvent;
                mComm.RegistorReceiveCallBack(OnReceiveData);

                foreach (var vv in Device.ListTags())
                {
                    if (!string.IsNullOrEmpty(vv.DeviceInfo))
                    {
                        if (mCachTags.ContainsKey(vv.DeviceInfo))
                        {
                            mCachTags[vv.DeviceInfo].Add(vv.Id);
                        }
                        else
                        {
                            mCachTags.Add(vv.DeviceInfo, new List<int>() { vv.Id });
                        }
                    }
                }
                mComm.Init();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MComm_CommChangedEvent(object sender, EventArgs e)
        {
            OnCommChanged(mComm.IsConnected);
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
        public virtual void WriteValue(string deviceInfo,byte[] value,byte valueType)
        {

        }

        /// <summary>
        /// 处理写硬件设备
        /// </summary>
        /// <param name="values"></param>
        public virtual void WriteValue(Dictionary<string, KeyValuePair<byte[], byte>> values)
        {
            foreach (var vv in values)
            {
                WriteValue(vv.Key, vv.Value.Key, vv.Value.Value);
            }
        }

        /// <summary>
        /// 通信状态改变
        /// </summary>
        protected virtual void OnCommChanged(bool result)
        {
            if(!result)
            {
                Device.UpdateAllTagQualityToCommBad();
            }
        }


        /// <summary>
        /// 接收到设备数据
        /// <paramref name="key"/>
        /// <paramref name="data"/>
        /// </summary>
        protected virtual byte[] OnReceiveData(string key,byte[] data,out bool handled)
        {
            handled = false;
            return null;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected byte[] SendData(string key,byte[] data)
        {
            byte[] re = null;
            if (!mComm.IsConnected) return null;
            var tre = mComm.Take();

            if (tre)
            {
                try
                {
                    re = mComm.SendAndWait(key,data);
                }
                finally
                {
                    mComm.Release();
                }
            }
            return re;
        }

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected bool SendDataAsync(string key,byte[] data)
        {
            if (!mComm.IsConnected) return false;

            var tre = mComm.Take();
            if (tre)
            {
                try
                {
                    mComm.SendAsync(key,data);
                }
                finally
                {
                    mComm.Release();
                }
            }
            return tre;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Prepare()
        {
            mComm.Prepare(mCachTags.Keys.ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Start()
        {
            mComm.Open();
            Prepare();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Stop()
        {
            mComm.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            Device = null;
            mCachTags.Clear();
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
        public abstract IDriverRuntime NewApi();
    }
}
