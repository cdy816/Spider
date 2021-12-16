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
    public class MarsApiPushRuntime : Cdy.Spider.ApiBase
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private ApiData mData;

        private SpiderDriver.ClientApi.DriverProxy mProxy;

        bool mIsClosed = false;

        //private Dictionary<int, string> mIdNameMape = new Dictionary<int, string>();

        //private Dictionary<string, int> mNameIdMape = new Dictionary<string, int>();

        //private Queue<Tagbase> mChangedTags = new Queue<Tagbase>();

        private Dictionary<Tagbase, bool> mChangedTags = new Dictionary<Tagbase, bool>();


        private Dictionary<string,int> mCallBackTags = new Dictionary<string, int>();

        private Dictionary<string,List<string>> mAllDatabaseTagNames = new Dictionary<string,List<string>>();

        private Thread mScanThread;

        SpiderDriver.ClientApi.RealDataBuffer rdb = new SpiderDriver.ClientApi.RealDataBuffer();

        SpiderDriver.ClientApi.RealDataBuffer rdbh = new SpiderDriver.ClientApi.RealDataBuffer();

        //SpiderDriver.ClientApi.HisDataBuffer hdb = new SpiderDriver.ClientApi.HisDataBuffer();

        //private bool mIsConnected = false;

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
        public override string TypeName => "MarsApiPushOnly";

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

                //vv.RegistorHisValueCallBack((tag, values) => {
                   
                //    //如果已经登录，则直接转储
                //    if(mProxy.IsLogin)
                //    {
                //        SendHisValue(tag.DatabaseName, (byte)tag.Type, values);
                //    }
                //});
            }
          

            mProxy = new SpiderDriver.ClientApi.DriverProxy();
            ////接受到数据库消费端修改数据
            //mProxy.ValueChanged = new SpiderDriver.ClientApi.DriverProxy.ProcessDataPushDelegate((values) => { 

            //    foreach(var vv in values)
            //    {
            //        if(mIdNameMape.ContainsKey(vv.Key))
            //        {
            //            string stag = mIdNameMape[vv.Key];
            //            foreach(var vvd in mAllDatabaseTagNames[stag])
            //            {
            //                manager.GetDevice(vvd).WriteValueByDatabaseName(stag, vv.Value);
            //            }
            //        }
            //    }
            //});
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="tag"></param>
        ///// <param name="type"></param>
        ///// <param name="values"></param>
        //private void SendHisValue(string tag,byte type,IEnumerable<HisValue> values)
        //{
        //    if (mNameIdMape.ContainsKey(tag))
        //    {
        //        int id = mNameIdMape[tag];
        //        var tpu = (Cdy.Tag.TagType)(type);
        //        mProxy.SetTagHisValue(id, tpu, values.Select(e => new Tag.TagValue() { Value = e.Value, Quality = 0, Time = e.Time }).ToList());
        //    }
        //}
        

        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            mIsClosed = false;
            mProxy?.Open(mData.ServerIp, mData.Port);
            
            mScanThread = new Thread(ThreadPro);
            mScanThread.IsBackground = true;
            mScanThread.Start();
           
            base.Start();
        }

        private bool mIsConnected = false;

        /// <summary>
        /// 
        /// </summary>
        private void ThreadPro()
        {
            mProxy.UserName = mData.UserName;
            mProxy.Password = mData.Password;

            while (!mIsClosed)
            {
                if (mProxy.IsConnected)
                {
                    if(!mIsConnected)
                    {
                        mIsConnected = true;
                        NotifyTags();
                    }
                    //如果是定时模式
                    UpdateChangedTag();
                    Thread.Sleep(mData.Circle);
                }
                else
                {
                    mIsConnected = false;
                    LoggerService.Service.Info("MarApi", "Login " + mData.ServerIp + " failed！");
                    Thread.Sleep(2000);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void NotifyTags()
        {
            var vtags = mAllDatabaseTagNames.Keys.ToList();
            mProxy.NotifyTags(vtags);
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateChangedTag()
        {
            try
            {

                rdb.Clear();

                lock (mChangedTags)
                {
                    rdb.CheckAndResize(mChangedTags.Count * (32+64));
                    rdbh.CheckAndResize(mChangedTags.Count * (32 + 64));

                    //while (mChangedTags.Count>0)
                    foreach (var vv in mChangedTags.Where(e => e.Value).ToList())
                    {
                        Tagbase stag = vv.Key;
                        lock (mChangedTags)
                        {
                            mChangedTags[stag] = false;
                        }

                        string id = stag.DatabaseName;
                        if (string.IsNullOrEmpty(id)) continue;

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

                }

                if (rdb.ValueCount > 0)
                    mProxy.SetTagValueAndQuality2(rdb);

                if (rdbh.ValueCount > 0)
                    mProxy.SetTagRealAndHisValue2(rdbh);

            }
            catch(Exception ex)
            {
                LoggerService.Service.Erro("MarsAPiRuntime"," UpdateChangedTag "+ ex.Message);
            }
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

                //离线质量戳
                byte badquality = Tagbase.BadCommQuality;

                foreach (var vvv in vv.ListTags())
                {
                    Tagbase stag = vvv;
                    string id = stag.DatabaseName;
                    if (string.IsNullOrEmpty(id)) continue;

                    var tpu = (TagType)((int)stag.Type);

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
                            rdb.AppendValue(id, new Tag.ULongPointData(ulpp3.X, ulpp3.Y), badquality);
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

                if (rdb.ValueCount > 0)
                    mProxy.SetTagValueAndQuality(rdb);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Stop()
        {
            mIsClosed = true;
            while (mScanThread.IsAlive) Thread.Sleep(1);
            if (mProxy.IsConnected)
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
            return new MarsApiPushRuntime();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
