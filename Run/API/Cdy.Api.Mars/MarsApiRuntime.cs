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

        private Queue<Tagbae> mChangedTags = new Queue<Tagbae>();

        private Dictionary<string,int> mCallBackTags = new Dictionary<string, int>();

        private Dictionary<string,List<string>> mAllDatabaseTagNames = new Dictionary<string,List<string>>();

        private Thread mScanThread;

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
                        mChangedTags.Enqueue(tag);
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
                        UpdateTagId();
                        mProxy.AppendRegistorDataChangedCallBack(mCallBackTags.Values.ToList());
                        UpdateAllValue();
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
                for(int i=0;i<res.Count;i++)
                {
                    int id = res[i];
                    string stag = vtags[i];

                    if (!mIdNameMape.ContainsKey(id))
                    {
                        mIdNameMape.Add(id, stag);
                    }

                    if(!mNameIdMape.ContainsKey(stag))
                    {
                        mNameIdMape.Add(stag, id);
                    }

                    if(mCallBackTags.ContainsKey(stag))
                    {
                        mCallBackTags[stag] = id;
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
                Dictionary<int, Tuple<Cdy.Tag.TagType, object,byte>> values = new Dictionary<int, Tuple<Cdy.Tag.TagType, object, byte>>();
                foreach (var vvv in vv.ListTags())
                {
                    if (mNameIdMape.ContainsKey(vvv.DatabaseName))
                    {
                        int id = mNameIdMape[vvv.DatabaseName];
                        var tpu = (Cdy.Tag.TagType)((int)vvv.Type);

                        if (!values.ContainsKey(id))
                        {
                            values.Add(id, new Tuple<Tag.TagType, object, byte>(tpu, vvv.Value, vvv.Quality));
                        }
                    }
                }
                mProxy.SetTagValue(values);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateChangedTag()
        {
            Dictionary<int, Tuple<Cdy.Tag.TagType, object, byte>> values = new Dictionary<int, Tuple<Cdy.Tag.TagType, object, byte>>();
            while (mChangedTags.Count>0)
            {
                Tagbae stag;
                lock(mChangedTags)
                stag = mChangedTags.Dequeue();

                int id = mNameIdMape[stag.DatabaseName];
                var tpu = (Cdy.Tag.TagType)((int)stag.Type);

                if(!values.ContainsKey(id))
                {
                    values.Add(id, new Tuple<Tag.TagType, object, byte>(tpu, stag.Value,stag.Quality));
                }
            }
            if(values.Count>0)
            mProxy.SetTagValue(values);
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
