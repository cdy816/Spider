﻿using Cdy.Spider;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using System.Linq;
using System.Runtime.InteropServices;
using System.Buffers;


namespace Cdy.Api.Mars
{
    /// <summary>
    /// 
    /// </summary>
    public class MarsApiRuntime : Cdy.Spider.ApiBase
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private ApiData mData;

        private SpiderDriver.ClientApi.DriverProxy mProxy;

        bool mIsClosed = false;

        private Dictionary<int, string> mIdNameMape = new Dictionary<int, string>();

        private Dictionary<string, int> mNameIdMape = new Dictionary<string, int>();

        //private Queue<Tagbase> mChangedTags = new Queue<Tagbase>();

        private Dictionary<Tagbase, bool> mChangedTags = new Dictionary<Tagbase, bool>();


        private Dictionary<string,int> mCallBackTags = new Dictionary<string, int>();

        private Dictionary<string,List<string>> mAllDatabaseTagNames = new Dictionary<string,List<string>>();

        private Thread mScanThread;

        SpiderDriver.ClientApi.RealDataBuffer rdb = new SpiderDriver.ClientApi.RealDataBuffer();

        SpiderDriver.ClientApi.RealDataBuffer rdbh = new SpiderDriver.ClientApi.RealDataBuffer();

        SpiderDriver.ClientApi.HisDataBuffer hdb = new SpiderDriver.ClientApi.HisDataBuffer();

        private bool mIsConnected = false;

        /// <summary>
        /// 是否需要重新初始化
        /// </summary>
        private bool mIsNeedReInit = false;

        private int mNoDataCount = 0;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override ApiData Data => mData;

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "MarsApi";

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new ApiData();
            mData.LoadFromXML(xe);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            base.Init();
            mCallBackTags.Clear();
            mAllDatabaseTagNames.Clear();
            mChangedTags.Clear();

            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();

            foreach(var vv in manager.ListDevice())
            {
                foreach(var vvv in vv.ListTags().Where(e=>e.DataTranseDirection!= DataTransType.DeviceToDatabase))
                {
                    if(!mCallBackTags.ContainsKey(vvv.DatabaseName))
                    {
                        mCallBackTags.Add(vvv.DatabaseName, 0);
                    }
                }
                
                foreach(var vvv in vv.ListDatabaseNames())
                {
                    if (!string.IsNullOrEmpty(vvv))
                    {
                        if (mAllDatabaseTagNames.ContainsKey(vvv))
                        {
                            mAllDatabaseTagNames[vvv].Add(vv.Name);
                        }
                        else
                        {
                            mAllDatabaseTagNames.Add(vvv, new List<string>() { vv.Name });
                        }
                    }
                }

                vv.RegistorCallBack((device, tag) => {
                    lock (mChangedTags)
                    {
                        if (mChangedTags.ContainsKey(tag))
                        {
                            mChangedTags[tag] = true;
                        }
                        else
                        {
                            mChangedTags.Add(tag, true);
                        }
                    }
                });

                vv.RegistorHisValueCallBack((tag, values) => {
                   
                    //如果已经登录，则直接转储
                    if(mProxy.IsLogin)
                    {
                        SendHisValue(tag.DatabaseName, (byte)tag.Type, values);
                    }
                });
            }
            InitProxy();
        }

        private void InitProxy()
        {

            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();

            mProxy = new SpiderDriver.ClientApi.DriverProxy();
            //接受到数据库消费端修改数据
            mProxy.ValueChanged = new SpiderDriver.ClientApi.DriverProxy.ProcessDataPushDelegate((values) => {

                foreach (var vv in values)
                {
                    if (mIdNameMape.ContainsKey(vv.Key))
                    {
                        string stag = mIdNameMape[vv.Key];
                        foreach (var vvd in mAllDatabaseTagNames[stag])
                        {
                            manager.GetDevice(vvd).WriteValueByDatabaseName(stag, vv.Value);
                        }
                    }
                }
            });

            mProxy.DatabaseChanged = new SpiderDriver.ClientApi.DriverProxy.DatabaseChangedDelegate((realchanged, hischanged) => {
                mIsNeedReInit = realchanged | hischanged;
            });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="type"></param>
        /// <param name="values"></param>
        private void SendHisValue(string tag,byte type,IEnumerable<HisValue> values)
        {
            if (mNameIdMape.ContainsKey(tag))
            {
                int id = mNameIdMape[tag];
                var tpu = (Cdy.Tag.TagType)(type);
                mProxy.SetTagHisValue(id, tpu, values.Select(e => new Tag.TagValue() { Value = e.Value, Quality = 0, Time = e.Time }).ToList());
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            mIsClosed = false;
            mProxy.Open(mData.ServerIp, mData.Port);
            
            mScanThread = new Thread(ThreadPro);
            mScanThread.IsBackground = true;
            mScanThread.Start();
           
            base.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ThreadPro()
        {
            int mcount = 0;
            while (!mIsClosed)
            {
                if (!mProxy.IsLogin)
                {
                    if (mProxy.IsConnected)
                    {
                        mIsConnected = true;
                        mProxy.Login(mData.UserName, mData.Password);
                        if (mProxy.IsLogin)
                        {
                            LoggerService.Service.Info("MarApi", "Login " + mData.ServerIp + " sucessfull！");
                            if(!UpdateTagId())
                            {
                                Thread.Sleep(1000);
                                mProxy.Logout();

                                mcount++;
                                //mProxy.Close();
                                //mProxy.Open(mData.ServerIp, mData.Port);
                                continue;
                            }
                            StopBuffer();
                            mProxy.AppendRegistorDataChangedCallBack(mCallBackTags.Values.ToList());
                            UpdateAllValue();
                        }
                        else
                        {
                            mcount++;
                            LoggerService.Service.Info("MarApi", "Login "+ mData.ServerIp + " failed！");
                        }
                    }
                    else
                    {
                        if(mIsConnected)
                        {
                            StartBuffer();
                            LoggerService.Service.Info("MarApi", "Login " + mData.ServerIp + " failed！");
                            mIsConnected = false;
                            mcount++;
                        }
                        else
                        {
                            mcount++;
                        }
                    }
                    Thread.Sleep(2000);

                    if(mcount>9)
                    {
                        LoggerService.Service.Info("MarApi", "ReInit to " + mData.ServerIp + "");
                        try
                        {
                            mcount = 0;
                            //如果长时间中断，则重新初始化
                            mProxy?.Close();
                            Thread.Sleep(1000);
                            InitProxy();
                            mProxy.Open(mData.ServerIp, mData.Port);
                        }
                        catch(Exception ex)
                        {
                            LoggerService.Service.Warn("MarApi", $"{ex.Message}  {ex.StackTrace}");
                        }
                    }
                }
                else
                {
                    mcount = 0;
                    if ((mIdNameMape.Count == 0 && mAllDatabaseTagNames.Count>0)|| mIsNeedReInit)
                    {
                        if (!UpdateTagId())
                        {
                            Thread.Sleep(1000);
                            mProxy.Logout();
                            //mProxy.Close();
                            //mProxy.Open(mData.ServerIp, mData.Port);
                            continue;
                        }
                        else
                        {
                            mIsNeedReInit = false;
                        }
                        mProxy.AppendRegistorDataChangedCallBack(mCallBackTags.Values.ToList());
                        UpdateAllValue();
                    }
                    else
                    {
                        //如果是定时模式
                        UpdateChangedTag();
                        Thread.Sleep(mData.Circle);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartBuffer()
        {
            LoggerService.Service.Info("MarApi", "开始数据缓冲");
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
            foreach (var vv in manager.ListDevice())
            {
                foreach (var vvv in vv.ListTags())
                {
                    vvv.IsBufferStarted = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void StopBuffer()
        {
            LoggerService.Service.Info("MarApi", "停止数据缓冲");
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
            foreach (var vv in manager.ListDevice())
            {
                foreach (var vvv in vv.ListTags())
                {
                    vvv.IsBufferStarted = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool UpdateTagId()
        {
            Thread.Sleep(1000);

            mIdNameMape.Clear();
            mNameIdMape.Clear();

            var vtags = mAllDatabaseTagNames.Keys.ToList();

            LoggerService.Service.Info("MarsApi", $"开始初始化，获取变量ID {vtags.Count}");
            var res = mProxy.QueryTagId(vtags);

            if(res==null || res.Count!=vtags.Count)
            {
                LoggerService.Service.Info("MarsApi", "网络通信错误，重试中...");
                return false;
            }
            LoggerService.Service.Info("MarsApi", $"开始同步变量ID信息 { res.Count } ");
            if (res != null && res.Count > 0 && res.Count == mAllDatabaseTagNames.Count)
            {
                for (int i = 0; i < res.Count; i++)
                {
                    int id = res[i];
                    string stag = vtags[i];

                    if (!mIdNameMape.ContainsKey(id))
                    {
                        mIdNameMape.Add(id, stag);
                    }

                    if (!mNameIdMape.ContainsKey(stag))
                    {
                        mNameIdMape.Add(stag, id);
                    }

                    if (mCallBackTags.ContainsKey(stag))
                    {
                        mCallBackTags[stag] = id;
                    }
                }
            }

            LoggerService.Service.Info("MarsApi", $"开始获取驱动记录类型变量列表");
            var gr = mProxy.GetDriverRecordTypeTagIds();

            int icount = gr != null ? gr.Count : 0;

            LoggerService.Service.Info("MarsApi", $"完成开始获取驱动记录类型变量列表 {icount}");

            var driverRecordtags = gr.Where(e=>mIdNameMape.ContainsKey(e)).Select(e => mIdNameMape[e]).ToList();

            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
            foreach (var vv in manager.ListDevice())
            {
                foreach(var vvv in vv.ListTags())
                {
                    if(driverRecordtags.Contains(vvv.DatabaseName) && !string.IsNullOrEmpty(vvv.DatabaseName))
                    {
                        vvv.EnableHisBuffer(true);
                    }
                    else
                    {
                        vvv.EnableHisBuffer(false);
                    }
                }
            }

            LoggerService.Service.Info("MarsApi", "变量ID同步完成...");
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateAllValue()
        {
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();

            //LoggerService.Service.Info("MarsApi", "首次数据同步...");

            foreach (var vv in manager.ListDevice())
            {
                //Dictionary<int, Tuple<Cdy.Tag.TagType, object,byte>> values = new Dictionary<int, Tuple<Cdy.Tag.TagType, object, byte>>();


                rdb.CheckAndResize(vv.ListTags().Count * 32);
                rdb.Clear();

                long size = 0;
                vv.ListCacheHistoryTags().ForEach(e => size += e.HisValueBuffer.Length);
                hdb.CheckAndResize(size);
                hdb.Clear();
                hdb.Position = 0;
                          
                foreach(var vvv in vv.ListCacheHistoryTags())
                {
                    if (!mNameIdMape.ContainsKey(vvv.DatabaseName))
                        continue;

                    int id = mNameIdMape[vvv.DatabaseName];
                    var tpu = (Tag.TagType)((int)vvv.Type);

                    switch (vvv.Type)
                    {

                        case TagType.Double:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id,val.Time, Convert.ToDouble(val.Value), 0);
                            }

                            break;
                        case TagType.Bool:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time, Convert.ToByte(val.Value), 0);
                            }

                            break;
                        case TagType.Byte:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time, Convert.ToByte(val.Value), 0);
                            }

                            break;
                        case TagType.DateTime:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time, Convert.ToDateTime(val.Value), 0);
                            }

                            break;
                        case TagType.Float:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time, Convert.ToSingle(val.Value), 0);
                            }

                            break;
                        case TagType.Int:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time, Convert.ToInt32(val.Value), 0);
                            }
                            break;
                        case TagType.Long:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time, Convert.ToInt64(val.Value), 0);
                            }
                            break;
                        case TagType.UInt:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time, Convert.ToUInt32(val.Value), 0);
                            }
                            break;
                        case TagType.ULong:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time, Convert.ToUInt64(val.Value), 0);
                            }
                            break;
                        case TagType.UShort:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time, Convert.ToUInt16(val.Value), 0);
                            }
                            break;
                        case TagType.Short:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time, Convert.ToInt16(val.Value), 0);
                            }
                            break;
                        case TagType.IntPoint:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time,(Tag.IntPointData)(val.Value), 0);
                            }
                            break;
                        case TagType.UIntPoint:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time, (Tag.UIntPointData)(val.Value), 0);
                            }
                            break;
                        case TagType.IntPoint3:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time, (Tag.IntPoint3Data)(val.Value), 0);
                            }
                            break;
                        case TagType.UIntPoint3:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time, (Tag.UIntPoint3Data)(val.Value), 0);
                            }
                            break;
                        case TagType.LongPoint:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time, (Tag.LongPointData)(val.Value), 0);
                            }
                            break;
                        case TagType.ULongPoint:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time, (Tag.ULongPointData)(val.Value), 0);
                            }
                            break;
                        case TagType.LongPoint3:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time, (Tag.LongPoint3Data)(val.Value), 0);
                            }
                            break;
                        case TagType.ULongPoint3:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time, (Tag.ULongPoint3Data)(val.Value), 0);
                            }
                            break;
                        case TagType.String:
                            foreach (var val in vvv.ReadHisValues())
                            {
                                hdb.AppendValue(id, val.Time, val.Value.ToString(), 0);
                            }

                            break;
                    }
                    if (hdb.ValueCount > 0)
                    {
                        LoggerService.Service.Info("MarsApi", $"同步 设备{vv.Name} 的历史缓存数据 { hdb.ValueCount}...");
                        mProxy.SetMutiTagHisValue(hdb, 10000);
                       
                    }
                    
                }

                foreach (var vvv in vv.ListTags())
                {
                    if (mNameIdMape.ContainsKey(vvv.DatabaseName) && vvv.Quality!= Tagbase.InitQuality)
                    {
                        int id = mNameIdMape[vvv.DatabaseName];
                        
                        if (id < 0) continue;

                        var tpu = (TagType)((int)vvv.Type);

                        switch (vvv.Type)
                        {

                            case TagType.Double:
                                rdb.AppendValue(id, Convert.ToDouble(vvv.Value),vvv.Quality);
                                break;
                            case TagType.Bool:
                                rdb.AppendValue(id, Convert.ToBoolean(vvv.Value), vvv.Quality);

                                break;
                            case TagType.Byte:
                                rdb.AppendValue(id, Convert.ToByte(vvv.Value), vvv.Quality);
                                break;
                            case TagType.DateTime:
                                rdb.AppendValue(id, Convert.ToDateTime(vvv.Value), vvv.Quality);

                                break;
                            case TagType.Float:
                                rdb.AppendValue(id, Convert.ToSingle(vvv.Value), vvv.Quality);

                                break;
                            case TagType.Int:
                                rdb.AppendValue(id, Convert.ToInt32(vvv.Value), vvv.Quality);
                                break;
                            case TagType.Long:
                                rdb.AppendValue(id,Convert.ToInt64(vvv.Value), vvv.Quality);
                                break;
                            case TagType.UInt:
                                rdb.AppendValue(id,Convert.ToUInt32(vvv.Value), vvv.Quality);
                                break;
                            case TagType.ULong:
                                rdb.AppendValue(id, Convert.ToUInt64(vvv.Value), vvv.Quality);
                                break;
                            case TagType.UShort:
                                rdb.AppendValue(id,Convert.ToUInt16(vvv.Value), vvv.Quality);
                                break;
                            case TagType.Short:
                                rdb.AppendValue(id, Convert.ToInt16(vvv.Value), vvv.Quality);
                                break;
                            case TagType.String:
                                rdb.AppendValue(id, vvv.Value.ToString(), vvv.Quality);
                                break;
                            case TagType.IntPoint:
                                var vpp = (Spider.IntPoint)vvv.Value;
                                rdb.AppendValue(id,new Tag.IntPointData(vpp.X,vpp.Y), vvv.Quality);
                                break;
                            case TagType.UIntPoint:
                                var uvpp = (Spider.UIntPoint)vvv.Value;
                                rdb.AppendValue(id, new Tag.UIntPointData(uvpp.X, uvpp.Y), vvv.Quality);
                                break;
                            case TagType.IntPoint3:
                                var vpp3 = (Spider.IntPoint3)vvv.Value;
                                rdb.AppendValue(id, new Tag.IntPoint3Data(vpp3.X,vpp3.Y,vpp3.Z), vvv.Quality);
                                break;
                            case TagType.UIntPoint3:
                                var uvpp3 = (Spider.IntPoint3)vvv.Value;
                                rdb.AppendValue(id, new Tag.IntPoint3Data(uvpp3.X, uvpp3.Y, uvpp3.Z), vvv.Quality);
                                break;
                            case TagType.LongPoint:
                                var lpp3 = (Spider.LongPoint)vvv.Value;
                                rdb.AppendValue(id, new Tag.LongPointData(lpp3.X,lpp3.Y),vvv.Quality);
                                break;
                            case TagType.ULongPoint:
                                var ulpp3 = (Spider.ULongPoint)vvv.Value;
                                rdb.AppendValue(id, new Tag.ULongPointData(ulpp3.X,ulpp3.Y), vvv.Quality);
                                break;
                            case TagType.LongPoint3:
                                var lp3 = (Spider.LongPoint3)vvv.Value;
                                rdb.AppendValue(id, new Tag.LongPoint3Data(lp3.X,lp3.Y,lp3.Z), vvv.Quality);
                                break;
                            case TagType.ULongPoint3:
                                var ulp3 = (Spider.ULongPoint3)vvv.Value;
                                rdb.AppendValue(id, new Tag.ULongPoint3Data(ulp3.X, ulp3.Y, ulp3.Z), vvv.Quality);
                                break;
                        }
                    }
                }

                if(rdb.ValueCount>0)
                mProxy.SetTagValueAndQuality(rdb);
            }

            //LoggerService.Service.Info("MarsApi", "首次数据同步完成...");
        }

        /// <summary>
        /// 更新质量戳为坏
        /// </summary>
        private void UpdateBadQuality()
        {
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();

            foreach (var vv in manager.ListDevice())
            {
                //Dictionary<int, Tuple<Cdy.Tag.TagType, object,byte>> values = new Dictionary<int, Tuple<Cdy.Tag.TagType, object, byte>>();
                rdb.CheckAndResize(vv.ListTags().Count * 32);
                rdb.Clear();

                long size = 0;
                //vv.ListCacheHistoryTags().ForEach(e => size += e.HisValueBuffer.Length);
                //hdb.CheckAndResize(size);
                //hdb.Clear();

                //离线质量戳
                byte badquality = Tagbase.BadCommQuality;

                foreach (var vvv in vv.ListTags())
                {
                    if (mNameIdMape.ContainsKey(vvv.DatabaseName) && vvv.Quality != Tagbase.InitQuality)
                    {
                        int id = mNameIdMape[vvv.DatabaseName];
                        var tpu = (TagType)((int)vvv.Type);

                        switch (vvv.Type)
                        {

                            case TagType.Double:
                                rdb.AppendValue(id, Convert.ToDouble(vvv.Value), badquality);
                                break;
                            case TagType.Bool:
                                rdb.AppendValue(id, Convert.ToBoolean(vvv.Value), badquality);

                                break;
                            case TagType.Byte:
                                rdb.AppendValue(id, Convert.ToByte(vvv.Value), badquality);
                                break;
                            case TagType.DateTime:
                                rdb.AppendValue(id, Convert.ToDateTime(vvv.Value), badquality);

                                break;
                            case TagType.Float:
                                rdb.AppendValue(id, Convert.ToSingle(vvv.Value), badquality);

                                break;
                            case TagType.Int:
                                rdb.AppendValue(id, Convert.ToInt32(vvv.Value), badquality);
                                break;
                            case TagType.Long:
                                rdb.AppendValue(id, Convert.ToInt64(vvv.Value), badquality);
                                break;
                            case TagType.UInt:
                                rdb.AppendValue(id, Convert.ToUInt32(vvv.Value), badquality);
                                break;
                            case TagType.ULong:
                                rdb.AppendValue(id, Convert.ToUInt64(vvv.Value), badquality);
                                break;
                            case TagType.UShort:
                                rdb.AppendValue(id, Convert.ToUInt16(vvv.Value), badquality);
                                break;
                            case TagType.Short:
                                rdb.AppendValue(id, Convert.ToInt16(vvv.Value), badquality);
                                break;
                            case TagType.String:
                                rdb.AppendValue(id, vvv.Value.ToString(), badquality);
                                break;
                            case TagType.IntPoint:
                                var vpp = (Spider.IntPoint)vvv.Value;
                                rdb.AppendValue(id, new Tag.IntPointData(vpp.X, vpp.Y), badquality);
                                break;
                            case TagType.UIntPoint:
                                var uvpp = (Spider.UIntPoint)vvv.Value;
                                rdb.AppendValue(id, new Tag.UIntPointData(uvpp.X, uvpp.Y), badquality);
                                break;
                            case TagType.IntPoint3:
                                var vpp3 = (Spider.IntPoint3)vvv.Value;
                                rdb.AppendValue(id, new Tag.IntPoint3Data(vpp3.X, vpp3.Y, vpp3.Z), badquality);
                                break;
                            case TagType.UIntPoint3:
                                var uvpp3 = (Spider.IntPoint3)vvv.Value;
                                rdb.AppendValue(id, new Tag.IntPoint3Data(uvpp3.X, uvpp3.Y, uvpp3.Z), badquality);
                                break;
                            case TagType.LongPoint:
                                var lpp3 = (Spider.LongPoint)vvv.Value;
                                rdb.AppendValue(id, new Tag.LongPointData(lpp3.X, lpp3.Y), badquality);
                                break;
                            case TagType.ULongPoint:
                                var ulpp3 = (Spider.ULongPoint)vvv.Value;
                                rdb.AppendValue(id, new Tag.ULongPointData(ulpp3.X, ulpp3.Y),badquality);
                                break;
                            case TagType.LongPoint3:
                                var lp3 = (Spider.LongPoint3)vvv.Value;
                                rdb.AppendValue(id, new Tag.LongPoint3Data(lp3.X, lp3.Y, lp3.Z), badquality);
                                break;
                            case TagType.ULongPoint3:
                                var ulp3 = (Spider.ULongPoint3)vvv.Value;
                                rdb.AppendValue(id, new Tag.ULongPoint3Data(ulp3.X, ulp3.Y, ulp3.Z), badquality);
                                break;
                        }
                    }
                }

                if (rdb.ValueCount > 0)
                    mProxy.SetTagValueAndQuality(rdb);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateChangedTag()
        {
            try
            {

                rdb.Clear();
                rdbh.Clear();

                lock (mChangedTags)
                {
                    rdb.CheckAndResize(mChangedTags.Count * 32);
                    rdbh.CheckAndResize(mChangedTags.Count * 32);

                    //while (mChangedTags.Count>0)
                    foreach (var vv in mChangedTags.Where(e => e.Value).ToList())
                    {
                        Tagbase stag = vv.Key;
                        mChangedTags[stag] = false;
                        //lock(mChangedTags)
                        //stag = mChangedTags.Dequeue();

                        if (!mNameIdMape.ContainsKey(stag.DatabaseName))
                            continue;

                        int id = mNameIdMape[stag.DatabaseName];

                        if (id < 0) continue;

                        var tpu = (TagType)((int)stag.Type);

                        if (stag.IsBufferEnabled)
                        {
                            switch (tpu)
                            {
                                case TagType.Double:
                                    rdbh.AppendValue(id, Convert.ToDouble(stag.Value), stag.Quality);
                                    break;
                                case TagType.Bool:
                                    rdbh.AppendValue(id, Convert.ToBoolean(stag.Value), stag.Quality);
                                    break;
                                case TagType.Byte:
                                    rdbh.AppendValue(id, Convert.ToByte(stag.Value), stag.Quality);
                                    break;
                                case TagType.DateTime:
                                    rdbh.AppendValue(id, Convert.ToDateTime(stag.Value), stag.Quality);
                                    break;
                                case TagType.Float:
                                    rdbh.AppendValue(id, Convert.ToSingle(stag.Value), stag.Quality);
                                    break;
                                case TagType.Int:
                                    rdbh.AppendValue(id, Convert.ToInt32(stag.Value), stag.Quality);
                                    break;
                                case TagType.Long:
                                    rdbh.AppendValue(id, Convert.ToInt64(stag.Value), stag.Quality);
                                    break;
                                case TagType.UInt:
                                    rdbh.AppendValue(id, Convert.ToUInt32(stag.Value), stag.Quality);
                                    break;
                                case TagType.ULong:
                                    rdbh.AppendValue(id, Convert.ToUInt64(stag.Value), stag.Quality);
                                    break;
                                case TagType.UShort:
                                    rdbh.AppendValue(id, Convert.ToUInt16(stag.Value), stag.Quality);
                                    break;
                                case TagType.Short:
                                    rdbh.AppendValue(id, Convert.ToInt16(stag.Value), stag.Quality);
                                    break;
                                case TagType.String:
                                    rdbh.AppendValue(id, stag.Value.ToString(), stag.Quality);
                                    break;
                                case TagType.IntPoint:
                                    var vpp = (Spider.IntPoint)stag.Value;
                                    rdbh.AppendValue(id, new Tag.IntPointData(vpp.X, vpp.Y), stag.Quality);
                                    break;
                                case TagType.UIntPoint:
                                    var uvpp = (Spider.UIntPoint)stag.Value;
                                    rdbh.AppendValue(id, new Tag.UIntPointData(uvpp.X, uvpp.Y), stag.Quality);
                                    break;
                                case TagType.IntPoint3:
                                    var vpp3 = (Spider.IntPoint3)stag.Value;
                                    rdbh.AppendValue(id, new Tag.IntPoint3Data(vpp3.X, vpp3.Y, vpp3.Z), stag.Quality);
                                    break;
                                case TagType.UIntPoint3:
                                    var uvpp3 = (Spider.IntPoint3)stag.Value;
                                    rdbh.AppendValue(id, new Tag.IntPoint3Data(uvpp3.X, uvpp3.Y, uvpp3.Z), stag.Quality);
                                    break;
                                case TagType.LongPoint:
                                    var lpp3 = (Spider.LongPoint)stag.Value;
                                    rdbh.AppendValue(id, new Tag.LongPointData(lpp3.X, lpp3.Y), stag.Quality);
                                    break;
                                case TagType.ULongPoint:
                                    var ulpp3 = (Spider.ULongPoint)stag.Value;
                                    rdbh.AppendValue(id, new Tag.ULongPointData(ulpp3.X, ulpp3.Y), stag.Quality);
                                    break;
                                case TagType.LongPoint3:
                                    var lp3 = (Spider.LongPoint3)stag.Value;
                                    rdbh.AppendValue(id, new Tag.LongPoint3Data(lp3.X, lp3.Y, lp3.Z), stag.Quality);
                                    break;
                                case TagType.ULongPoint3:
                                    var ulp3 = (Spider.ULongPoint3)stag.Value;
                                    rdbh.AppendValue(id, new Tag.ULongPoint3Data(ulp3.X, ulp3.Y, ulp3.Z), stag.Quality);
                                    break;
                            }
                        }
                        else
                        {
                            switch (tpu)
                            {

                                case TagType.Double:
                                    rdb.AppendValue(id, Convert.ToDouble(stag.Value), stag.Quality);
                                    break;
                                case TagType.Bool:
                                    rdb.AppendValue(id, Convert.ToBoolean(stag.Value), stag.Quality);

                                    break;
                                case TagType.Byte:
                                    rdb.AppendValue(id, Convert.ToByte(stag.Value), stag.Quality);

                                    break;
                                case TagType.DateTime:
                                    rdb.AppendValue(id, Convert.ToDateTime(stag.Value), stag.Quality);

                                    break;
                                case TagType.Float:
                                    rdb.AppendValue(id, Convert.ToSingle(stag.Value), stag.Quality);

                                    break;
                                case TagType.Int:
                                    rdb.AppendValue(id, Convert.ToInt32(stag.Value), stag.Quality);
                                    break;
                                case TagType.Long:
                                    rdb.AppendValue(id, Convert.ToInt64(stag.Value), stag.Quality);
                                    break;
                                case TagType.UInt:
                                    rdb.AppendValue(id, Convert.ToUInt32(stag.Value), stag.Quality);
                                    break;
                                case TagType.ULong:
                                    rdb.AppendValue(id, Convert.ToUInt64(stag.Value), stag.Quality);
                                    break;
                                case TagType.UShort:
                                    rdb.AppendValue(id, Convert.ToUInt16(stag.Value), stag.Quality);
                                    break;
                                case TagType.Short:
                                    rdb.AppendValue(id, Convert.ToInt16(stag.Value), stag.Quality);
                                    break;
                                case TagType.String:
                                    rdb.AppendValue(id, stag.Value.ToString(), stag.Quality);
                                    break;
                                case TagType.IntPoint:
                                    var vpp = (Spider.IntPoint)stag.Value;
                                    rdb.AppendValue(id, new Tag.IntPointData(vpp.X, vpp.Y), stag.Quality);
                                    break;
                                case TagType.UIntPoint:
                                    var uvpp = (Spider.UIntPoint)stag.Value;
                                    rdb.AppendValue(id, new Tag.UIntPointData(uvpp.X, uvpp.Y), stag.Quality);
                                    break;
                                case TagType.IntPoint3:
                                    var vpp3 = (Spider.IntPoint3)stag.Value;
                                    rdb.AppendValue(id, new Tag.IntPoint3Data(vpp3.X, vpp3.Y, vpp3.Z), stag.Quality);
                                    break;
                                case TagType.UIntPoint3:
                                    var uvpp3 = (Spider.IntPoint3)stag.Value;
                                    rdb.AppendValue(id, new Tag.IntPoint3Data(uvpp3.X, uvpp3.Y, uvpp3.Z), stag.Quality);
                                    break;
                                case TagType.LongPoint:
                                    var lpp3 = (Spider.LongPoint)stag.Value;
                                    rdb.AppendValue(id, new Tag.LongPointData(lpp3.X, lpp3.Y), stag.Quality);
                                    break;
                                case TagType.ULongPoint:
                                    var ulpp3 = (Spider.ULongPoint)stag.Value;
                                    rdb.AppendValue(id, new Tag.ULongPointData(ulpp3.X, ulpp3.Y), stag.Quality);
                                    break;
                                case TagType.LongPoint3:
                                    var lp3 = (Spider.LongPoint3)stag.Value;
                                    rdb.AppendValue(id, new Tag.LongPoint3Data(lp3.X, lp3.Y, lp3.Z), stag.Quality);
                                    break;
                                case TagType.ULongPoint3:
                                    var ulp3 = (Spider.ULongPoint3)stag.Value;
                                    rdb.AppendValue(id, new Tag.ULongPoint3Data(ulp3.X, ulp3.Y, ulp3.Z), stag.Quality);
                                    break;
                            }
                        }
                    }

                }

                if (rdb.ValueCount > 0)
                    mProxy.SetTagValueAndQuality(rdb);

                if (rdbh.ValueCount > 0)
                    mProxy.SetTagRealAndHisValue(rdbh);

                if (rdb.ValueCount == 0 && rdbh.ValueCount == 0)
                {
                    //长时间无数据，则发送心跳包
                    mNoDataCount++;
                    if (mNoDataCount > 10)
                    {
                        mNoDataCount = 0;
                        mProxy.Hart();
                    }
                }
                else
                    mNoDataCount = 0;
            }
            catch (Exception ex)
            {
                LoggerService.Service.Erro("MarsAPiRuntime", " UpdateChangedTag " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Stop()
        {
            mIsClosed = true;
            while (mScanThread.IsAlive) Thread.Sleep(1);

            if(mProxy.IsConnected)
            {
                UpdateBadQuality();
            }

            mProxy.Close();
            base.Stop();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IApi NewApi()
        {
            return new MarsApiRuntime();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
