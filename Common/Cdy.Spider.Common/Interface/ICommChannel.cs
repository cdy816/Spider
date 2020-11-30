//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/4 14:37:52.
//  Version 1.0
//  种道洋
//==============================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider
{
    /// <summary>
    /// 通信通道
    /// </summary>
    public interface ICommChannel
    {

        #region ... Variables  ...

        public delegate byte[] DataReceiveCallBackDelegate(string key, byte[] date,out bool handled);

        /// <summary>
        /// 通信状态改变事件
        /// </summary>
        public event EventHandler CommChangedEvent;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        ChannelType Type { get; }

        /// <summary>
        /// 
        /// </summary>
        string TypeName { get; }

        ///// <summary>
        ///// 通信失败
        ///// </summary>
        //Action<bool> CommChangedCallBack { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ChannelData Data { get; }

        /// <summary>
        /// 是否连接上
        /// </summary>
        bool IsConnected { get; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 打开通道
        /// </summary>
        /// <returns></returns>
        bool Open();

        /// <summary>
        /// 结束通道
        /// </summary>
        /// <returns></returns>
        bool Close();

        /// <summary>
        /// 初始化
        /// </summary>
        void Init();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callBack"></param>
        void  RegistorReceiveCallBack(DataReceiveCallBackDelegate callBack);

        /// <summary>
        /// 通信预处理
        /// </summary>
        /// <param name="deviceInfos"></param>
        void Prepare(List<string> deviceInfos);

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        byte[] SendAndWait(byte[] data, int start, int len, int timeout, params string[] paras);


        /// <summary>
        /// 同步发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        byte[] SendAndWait(byte[] data,int start,int len,params string[] paras);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        byte[] SendAndWait(Span<byte> data, params string[] paras);

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <param name="paras"></param>
        void SendAsync(byte[] data, int start, int len, params string[] paras);

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="paras"></param>

        void SendAsync(Span<byte> data, params string[] paras);

        /// <summary>
        /// 获取控制权限
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        bool Take(int timeout);

        /// <summary>
        /// 获取控制权限
        /// </summary>
        /// <returns></returns>
        bool Take();

        /// <summary>
        /// 释放控制权限
        /// </summary>
        /// <returns></returns>
        void Release();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        void Load(XElement xe);

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ICommChannelForFactory
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        string TypeName { get; }
        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ICommChannel NewApi();

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }



}
