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
    public interface ICommChannel:IDisposable
    {

        #region ... Variables  ...

        public delegate byte[] DataReceiveCallBackDelegate(string key, byte[] date,out bool handled);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        public delegate object DataReceiveCallBackDelegate2(string key, object data, out bool handled);

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

        /// <summary>
        /// 目标信息描述
        /// </summary>
        string RemoteDescription { get; }

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
        /// 
        /// </summary>
        /// <param name="callBack"></param>
        void RegistorReceiveCallBack(DataReceiveCallBackDelegate2 callBack);

        /// <summary>
        /// 注册数据包起始、结束字符
        /// </summary>
        /// <param name="startByte"></param>
        /// <param name="endByte"></param>
        void RegistorPackageKeyByte(byte startByte, byte endByte);

        /// <summary>
        /// 通信预处理
        /// </summary>
        /// <param name="deviceInfos"></param>
        void Prepare(List<string> deviceInfos);

        /// <summary>
        /// 同步发送数据
        /// </summary>
        /// <param name="data">数据内容</param>
        /// <param name="timeout">发送超时</param>
        /// <param name="waitResultCount">期待接受数据数量</param>
        /// <returns></returns>
        byte[] Send(Span<byte> data, int waitResultCount);

        /// <summary>
        /// 同步发送数据
        /// </summary>
        /// <param name="data">数据内容</param>
        /// <param name="timeout">发送超时</param>
        /// <param name="waitPackageStartByte">期待接收的数据包头</param>
        /// <param name="waitPackageEndByte">期待接收的数据包头</param>
        /// <returns></returns>
        byte[] Send(Span<byte> data,  byte waitPackageStartByte, byte waitPackageEndByte);

        /// <summary>
        /// 同步发送并等待指定时间
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] Send(Span<byte> data);


        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="data">数据内容</param>
        /// <returns></returns>
        bool SendAsync(Span<byte> data);

        /// <summary>
        /// 发送关键字、对象
        /// </summary>
        /// <param name="key">关键Key</param>
        /// <param name="value">值</param>
        /// <param name="timeout">超时时间</param>
        /// <returns></returns>
        object SendObject(string key, object value);

        /// <summary>
        /// 发送关键字、字节数组
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        byte[] SendObject(string key, Span<byte> value);

        /// <summary>
        /// 同步发送对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        object SendObject(object value);



        /// <summary>
        /// 异步通过关键字发送数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool SendObjectAsync(string key, object value);

        /// <summary>
        /// 异步通过关键字发送数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool SendObjectAsync(string key, Span<byte> value);

        /// <summary>
        /// 异步发送
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        bool SendObjectAsync(object value);

        /// <summary>
        /// 接收指定数量的数据
        /// </summary>
        /// <param name="count">字节数量</param>
        /// <param name="timecount">超时</param>
        /// <param name="receivecount">实际接受的数量</param>
        /// <returns></returns>
        byte[] Receive(int count,int timecount,out int receivecount);

        /// <summary>
        /// 接收指定数量的数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        byte[] Receive(int count);

        /// <summary>
        /// 直接发送数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        bool Write(byte[] buffer, int offset, int len);

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        int Read(byte[] buffer, int offset, int len);

        /// <summary>
        /// 
        /// </summary>
        void Flush();

        /// <summary>
        /// 使能透明读写
        /// </summary>
        /// <param name="enable"></param>
        void EnableTransparentRead(bool enable);

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
