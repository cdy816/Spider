//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/4 14:23:28.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDeviceRuntime:IDisposable, IDeviceForApi
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
        string Name { get; }


        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        void Init();

        /// <summary>
        /// 
        /// </summary>
        void Start();

        /// <summary>
        /// 
        /// </summary>
        void Stop();



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        object ReadValue(int id);



        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 
    /// </summary>
    public struct HisValue { 
        public DateTime Time { get; set; }
     
        public object Value { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public interface IDeviceForApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<string> ListDatabaseNames();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<Tagbase> ListTags();


        /// <summary>
        /// 列出所有缓冲历史数据的变量
        /// </summary>
        /// <returns></returns>
        List<Tagbase> ListCacheHistoryTags();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Tagbase GetTag(string name);

        /// <summary>
        /// 注册值改变
        /// </summary>
        /// <param name="callBack"></param>
        void RegistorCallBack(Action<string,Tagbase> callBack);

        /// <summary>
        /// 历史记录更新
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="hisValues"></param>
        void RegistorHisValueCallBack(Action<Tagbase , IEnumerable<HisValue>> hisValues);

        /// <summary>
        /// 接收数据库下发值，写入到设备中
        /// </summary>
        /// <param name="databaTag"></param>
        /// <param name="value"></param>
        void WriteValueByDatabaseName(string databaseTag, object value);


        /// <summary>
        /// 根据数据变量的名称读取设备的值
        /// </summary>
        /// <param name="databaseTag"></param>
        /// <returns></returns>
        object ReadValueByDatabaseName(string databaseTag);



    }


    /// <summary>
    /// 
    /// </summary>
    public interface IDeviceForDriver
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
        string Name { get; }
        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 驱动解析完数据，更新到数据库中
        /// </summary>
        /// <param name="deviceTag"></param>
        /// <param name="value"></param>
        void UpdateDeviceValue(string deviceTag, object value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        void UpdateDeviceValue(List<int> id, object value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        void UpdateDeviceValue(int id, object value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="quality"></param>
        void UpdateDeviceValue(int id, object value, byte quality);


        void UpdateDeviceValue(string deviceTag, object value, byte quality);


        /// <summary>
        /// 更新所有变量的质量戳为通信故障
        /// </summary>
        void UpdateAllTagQualityToCommBad();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<Tagbase> ListTags();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Tagbase GetTag(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        int GetTagId(string deviceInfo);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ICommChannel2 GetCommChannel();

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

}
