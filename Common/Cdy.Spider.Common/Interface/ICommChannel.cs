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

namespace Cdy.Spider
{
    /// <summary>
    /// 通信通道
    /// </summary>
    public interface ICommChannel
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
        Action<byte[]> ReceiveAsyncCallBack { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Action<bool> SendDataAsyncCallBack { get; set; }

        /// <summary>
        /// 通信失败
        /// </summary>
        Action<bool> CommChangedCallBack { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ChannelData Data { get; set; }

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
        /// 通信预处理
        /// </summary>
        /// <param name="deviceInfos"></param>
        void Prepare(List<string> deviceInfos);

        /// <summary>
        /// 异步接收数据
        /// </summary>
        void StartReceiveAsync();

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        byte[] Receive(int timeout,out bool result);

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        bool Send(byte[] data,int timeout);

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="data"></param>
        void SendAsync(byte[] data);

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }



}
