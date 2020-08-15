using Cdy.Spider;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using System.Linq;

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

        private Queue<string> mChangedTags = new Queue<string>();

        private Dictionary<string,int> mCallBackTags = new Dictionary<string, int>();

        private List<string> mAllDatabaseTagNames = new List<string>();

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

                mAllDatabaseTagNames.AddRange(vv.ListDatabaseNames());
            }
          

            mProxy = new SpiderDriver.ClientApi.DriverProxy();
            mProxy.ValueChanged = new SpiderDriver.ClientApi.DriverProxy.ProcessDataPushDelegate((values) => { 

                foreach(var vv in values)
                {
                    if(mIdNameMape.ContainsKey(vv.Key))
                    {
                        string stag = mIdNameMape[vv.Key];

                    }
                }

                //to do update value to device
            });
        }

        

        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            mIsClosed = false;
            mProxy.Connect(mData.ServerIp, mData.Port);
            
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
                }
                else
                {
                    //如果是定时模式
                    UpdateChangedTag();
                    Thread.Sleep(mData.Circle);
                }
            }
        }

        private void UpdateTagId()
        {
            var res = mProxy.QueryTagId(mAllDatabaseTagNames);
            if (res != null && res.Count > 0 && res.Count == mAllDatabaseTagNames.Count)
            {
                for(int i=0;i<res.Count;i++)
                {
                    int id = res[i];
                    string stag = mAllDatabaseTagNames[id];
                    if (!mIdNameMape.ContainsKey(id))
                    {
                        mIdNameMape.Add(id, stag);
                    }
                    if(mCallBackTags.ContainsKey(stag))
                    {
                        mCallBackTags[stag] = id;
                    }
                }
            }
        }

        private void UpdateAllValue()
        {

        }

        private void UpdateChangedTag()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tags"></param>
        private void UpdateTagValue(List<string> tags)
        {

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

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
