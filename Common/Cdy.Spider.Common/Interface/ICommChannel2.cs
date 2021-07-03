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

    public class ChannelPrepareContext : Dictionary<string, object>, IDisposable
    {
        public void Dispose()
        {
            this.Clear();
        }
    }

    /// <summary>
    /// 通信通道
    /// </summary>
    public interface ICommChannel2:IDisposable
    {

        #region ... Variables  ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        public delegate object DataReceiveCallBackDelegate(string key, object data, out bool handled);

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

        /// <summary>
        /// 
        /// </summary>
        CommMode CommMode { get; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsRawComm();

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
        /// 注册数据包起始、结束字符
        /// </summary>
        /// <param name="startByte"></param>
        /// <param name="endByte"></param>
        void RegistorPackageKeyByte(byte startByte, byte endByte);

        /// <summary>
        /// 通信预处理
        /// </summary>
        /// <param name="deviceInfos">上下文</param>
        void Prepare(ChannelPrepareContext deviceInfos);

        /// <summary>
        /// 读取值操作,面向非底层通道使用
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object ReadValue(object value);

        /// <summary>
        /// 读取值操作,面向非底层通道使用
        /// </summary>
        /// <param name="valuebytes"></param>
        /// <returns></returns>
        object ReadValue(byte[] valuebytes);

        /// <summary>
        /// 读取值,面向非底层通道使用
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object ReadValue(Span<byte> value);

        /// <summary>
        /// 写设备操作,面向非底层通道使用
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        object WriteValue(string address, object value);

        /// <summary>
        /// 异步写设备值操作,面向非底层通道使用
        /// </summary>
        /// <param name="adderss"></param>
        /// <param name="value"></param>
        bool WriteValueNoWait(string adderss, object value);



        /// <summary>
        /// 直接从底层IO设备进行读取,接收指定数量的数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        byte[] Read(int count);

        /// <summary>
        /// 直接从底层IO设备进行读取,接收指定数量的数据
        /// </summary>
        /// <param name="count">字节数量</param>
        /// <param name="timecount">超时</param>
        /// <param name="receivecount">实际接受的数量</param>
        /// <returns></returns>
        byte[] Read(int count, int timecount, out int receivecount);

        /// <summary>
        /// 直接从底层IO设备进行读取,接收指定数量的数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        int Read(byte[] buffer, int offset, int len);


        /// <summary>
        /// 直接从底层IO设备进行写入,发送数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        bool Send(byte[] buffer, int offset, int len);

        /// <summary>
        /// 直接从底层IO设备进行写入
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        bool Send(Span<byte> buffer);

        /// <summary>
        /// 直接从底层IO设备进行写入,发送数据;并等待数据返回
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        byte[] SendAndWait(Span<byte> datas);

        /// <summary>
        /// 直接从底层IO设备进行写入,发送数据;并等待数据指定数量的数据返回
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="waitResultCount"></param>
        /// <returns></returns>
        byte[] SendAndWait(Span<byte> datas, int waitResultCount);


        /// <summary>
        /// 直接从底层IO设备进行写入,发送数据;并等待数据指定数据包头、尾的数据返回
        /// </summary>
        /// <param name="data"></param>
        /// <param name="waitPackageStartByte"></param>
        /// <param name="waitPackageEndByte"></param>
        /// <returns></returns>
        byte[] SendAndWait(Span<byte> datas, byte waitPackageStartByte, byte waitPackageEndByte);

        /// <summary>
        /// 直接从底层IO设备进行写入,发送数据;并等待数据返回
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        byte[] SendAndWait(byte[] buffer, int offset, int len);


        /// <summary>
        /// 
        /// </summary>
        void Flush();

        /// <summary>
        /// 清空缓冲
        /// </summary>
        void ClearBuffer();

        /// <summary>
        /// 使能同步读写
        /// </summary>
        /// <param name="enable"></param>
        void EnableSyncRead(bool enable);

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
    public interface ICommChannel2ForFactory
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
        ICommChannel2 NewApi();

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }



}
