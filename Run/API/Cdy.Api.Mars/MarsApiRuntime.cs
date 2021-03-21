using Cdy.Spider;
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
                    if (mAllDatabaseTagNames.ContainsKey(vvv))
                    {
                        mAllDatabaseTagNames[vvv].Add(vv.Name);
                    }
                    else
                    {
                        mAllDatabaseTagNames.Add(vvv, new List<string>() { vv.Name });
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
          

            mProxy = new SpiderDriver.ClientApi.DriverProxy();
            //接受到数据库消费端修改数据
            mProxy.ValueChanged = new SpiderDriver.ClientApi.DriverProxy.ProcessDataPushDelegate((values) => { 

                foreach(var vv in values)
                {
                    if(mIdNameMape.ContainsKey(vv.Key))
                    {
                        string stag = mIdNameMape[vv.Key];
                        foreach(var vvd in mAllDatabaseTagNames[stag])
                        {
                            manager.GetDevice(vvd).WriteValueByDatabaseName(stag, vv.Value);
                        }
                    }
                }
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
            mProxy.Connect(mData.ServerIp, mData.Port);
            
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
            while (!mIsClosed)
            {
                if (!mProxy.IsLogin)
                {
                    if (mProxy.IsConnected)
                    {
                        mProxy.Login(mData.UserName, mData.Password);
                        if (mProxy.IsLogin)
                        {
                            UpdateTagId();
                            mProxy.AppendRegistorDataChangedCallBack(mCallBackTags.Values.ToList());
                            UpdateAllValue();
                        }
                    }
                    else
                    {
                        if (mProxy.NeedReConnected)
                            mProxy.Connect(mData.ServerIp, mData.Port);
                        lock (mChangedTags)
                        {
                            if (mCallBackTags.Count > 100) 
                                mCallBackTags.Clear();
                        }
                    }
                    Thread.Sleep(2000);
                }
                else
                {
                   
                    //如果是定时模式
                    UpdateChangedTag();
                    Thread.Sleep(mData.Circle);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateTagId()
        {
            mIdNameMape.Clear();
            mNameIdMape.Clear();

            var vtags = mAllDatabaseTagNames.Keys.ToList();
            var res = mProxy.QueryTagId(vtags);
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

            var driverRecordtags = mProxy.GetDriverRecordTypeTagIds().Select(e => mIdNameMape.ContainsKey(e) ? mIdNameMape[e] : string.Empty);

            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
            foreach (var vv in manager.ListDevice())
            {
                foreach(var vvv in vv.ListTags())
                {
                    if(driverRecordtags.Contains(vvv.DatabaseName))
                    {
                        vvv.EnableHisBuffer(true);
                    }
                    else
                    {
                        vvv.EnableHisBuffer(false);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateAllValue()
        {
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();

            foreach (var vv in manager.ListDevice())
            {
                //Dictionary<int, Tuple<Cdy.Tag.TagType, object,byte>> values = new Dictionary<int, Tuple<Cdy.Tag.TagType, object, byte>>();
                rdb.CheckAndResize(vv.ListTags().Count * 32);
                rdb.Clear();

                long size = 0;
                vv.ListCacheHistoryTags().ForEach(e => size += e.HisValueBuffer.Length);
                hdb.CheckAndResize(size);
                hdb.Clear();
                
                
                foreach(var vvv in vv.ListCacheHistoryTags())
                {
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
                    }

                    mProxy.SetMutiTagHisValue(hdb,10000);
                    
                }

                foreach (var vvv in vv.ListTags())
                {
                    if (mNameIdMape.ContainsKey(vvv.DatabaseName))
                    {
                        int id = mNameIdMape[vvv.DatabaseName];
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
                                rdb.AppendValue(id, new Tag.ULongPointData(ulpp3.X,ulpp3.Y));
                                break;
                            case TagType.LongPoint3:
                                var lp3 = (Spider.LongPoint3)vvv.Value;
                                rdb.AppendValue(id, new Tag.LongPoint3Data(lp3.X,lp3.Y,lp3.Z));
                                break;
                            case TagType.ULongPoint3:
                                var ulp3 = (Spider.ULongPoint3)vvv.Value;
                                rdb.AppendValue(id, new Tag.ULongPoint3Data(ulp3.X, ulp3.Y, ulp3.Z));
                                break;
                        }
                    }
                }

                if(rdb.ValueCount>0)
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
                //List<Tag.RealTagValue> values = new List<Tag.RealTagValue>();

                //foreach(var vv in mChangedTags.Where(e=>e.Value).ToList())
                //{
                //    int id = mNameIdMape[vv.Key.DatabaseName];
                //    values.Add(new Tag.RealTagValue() { ValueType = (byte)vv.Key.Type, Value = vv.Key.Value, Id = id, Quality = vv.Key.Quality });
                //}
                //mProxy.SetTagRealAndHisValue(values);


                rdb.Clear();
                rdbh.Clear();

                rdb.CheckAndResize(mChangedTags.Count * 32);
                rdbh.CheckAndResize(mChangedTags.Count * 32);

                //while (mChangedTags.Count>0)
                foreach (var vv in mChangedTags.Where(e => e.Value).ToList())
                {
                    Tagbase stag = vv.Key;
                    lock (mChangedTags)
                    {
                        mChangedTags[stag] = false;
                    }
                    //lock(mChangedTags)
                    //stag = mChangedTags.Dequeue();

                    int id = mNameIdMape[stag.DatabaseName];
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
                                rdbh.AppendValue(id, new Tag.ULongPointData(ulpp3.X, ulpp3.Y));
                                break;
                            case TagType.LongPoint3:
                                var lp3 = (Spider.LongPoint3)stag.Value;
                                rdbh.AppendValue(id, new Tag.LongPoint3Data(lp3.X, lp3.Y, lp3.Z));
                                break;
                            case TagType.ULongPoint3:
                                var ulp3 = (Spider.ULongPoint3)stag.Value;
                                rdbh.AppendValue(id, new Tag.ULongPoint3Data(ulp3.X, ulp3.Y, ulp3.Z));
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
                                rdb.AppendValue(id, new Tag.ULongPointData(ulpp3.X, ulpp3.Y));
                                break;
                            case TagType.LongPoint3:
                                var lp3 = (Spider.LongPoint3)stag.Value;
                                rdb.AppendValue(id, new Tag.LongPoint3Data(lp3.X, lp3.Y, lp3.Z));
                                break;
                            case TagType.ULongPoint3:
                                var ulp3 = (Spider.ULongPoint3)stag.Value;
                                rdb.AppendValue(id, new Tag.ULongPoint3Data(ulp3.X, ulp3.Y, ulp3.Z));
                                break;
                        }
                    }
                }
                if (rdb.ValueCount > 0)
                    mProxy.SetTagValueAndQuality(rdb);



                if (rdbh.ValueCount > 0)
                    mProxy.SetTagRealAndHisValue(rdbh);
            }
            catch(Exception ex)
            {
                LoggerService.Service.Erro("MarsAPiRuntime"," UpdateChangedTag "+ ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Stop()
        {
            mIsClosed = true;
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
