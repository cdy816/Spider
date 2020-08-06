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
        ChannelType Type { get; }

        /// <summary>
        /// 
        /// </summary>
        Func<string, byte[], byte[]> ReceiveCallBack { get; set; }

        /// <summary>
        /// 通信失败
        /// </summary>
        Action<bool> CommChangedCallBack { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ChannelData Data { get; set; }

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
        /// 通信预处理
        /// </summary>
        /// <param name="deviceInfos"></param>
        void Prepare(List<string> deviceInfos);

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="key"/>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        byte[] SendAndWait(string key,byte[] data,int timeout);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"/>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] SendAndWait(string key,byte[] data);

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="key"/>
        /// <param name="data"></param>
        void SendAsync(string key,byte[] data);

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

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }



}
