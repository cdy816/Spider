using Cdy.Spider;
using Cheetah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace Cdy.Api.SpiderTcp
{
    public class SpiderTcpApiRuntime : Cdy.Spider.ApiBase
    {

        #region ... Variables  ...
        private TcpApiData mData;

        private Dictionary<string, Dictionary<Tagbase, bool>> mChangedTags = new Dictionary<string, Dictionary<Tagbase, bool>>();

        private Dictionary<string, int> mCallBackTags = new Dictionary<string, int>();

        private bool mIsClosed = false;
        private bool mIsNeedInited = false;
        //private bool mIsLogin = false;
        private Thread mScanThread;

        private TcpClient mClient;

        DateTime mLastLoginTime;

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
        public override string TypeName => "SpiderTcpApi";

        #endregion ...Properties...

        #region ... Methods    ...

        public override void Load(XElement xe)
        {
            mData = new TcpApiData();
            mData.LoadFromXML(xe);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            base.Init();
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();

            foreach (var vv in manager.ListDevice())
            {
                foreach (var vvv in vv.ListTags().Where(e => e.DataTranseDirection != DataTransType.DeviceToDatabase))
                {
                    if (!mCallBackTags.ContainsKey(vvv.DatabaseName))
                    {
                        mCallBackTags.Add(vvv.DatabaseName, 0);
                    }
                }
                vv.RegistorCallBack((device, tag) => {
                    lock (mChangedTags)
                    {
                        if (mChangedTags.ContainsKey(device))
                        {
                            if (mChangedTags[device].ContainsKey(tag))
                            {
                                mChangedTags[device][tag] = true;
                            }
                            else
                            {
                                mChangedTags[device].Add(tag, true);
                            }
                        }
                        else
                        {
                            Dictionary<Tagbase, bool> re = new Dictionary<Tagbase, bool>();
                            re.Add(tag, true);
                            mChangedTags.Add(device, re);
                        }
                    }
                });
            }

            mClient = new TcpClient() { Port = mData.Port, ServerIp = mData.ServerIp,UserName=mData.UserName,Password=mData.Password };
                      
        }

        /// <summary>
        /// 
        /// </summary>
        private void ThreadPro()
        {
            
            while (!mIsClosed)
            {
                if (mData.PushDataOnly)
                {
                    UpdateChanged2();
                    Thread.Sleep(mData.Circle);
                }
                else
                {
                    if (!mClient.IsLogin)
                    {
                        if (mClient.IsConnected)
                        {
                            Login();
                        }
                        else
                        {
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
                        if (mIsNeedInited)
                        {
                            UpdateAllValue();
                        }
                        else
                        {
                            //如果是定时模式
                            UpdateChanged();
                        }
                        if ((DateTime.Now - mLastLoginTime).TotalSeconds > 4 * 60)
                        {
                            mClient.Hart();
                            mLastLoginTime = DateTime.Now;
                        }
                        Thread.Sleep(mData.Circle);

                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Login()
        {
            if(mClient.Login(mData.UserName,mData.Password))
            {
                mLastLoginTime = DateTime.Now;
                mIsNeedInited = true;
                var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
                StringBuilder sb = new StringBuilder();
                foreach (var vv in manager.ListDevice().Select(e => e.Name))
                {
                    sb.Append(vv + ",");
                }
                sb.Length = sb.Length > 0 ? sb.Length - 1 : sb.Length;
                mClient.Registor(sb.ToString());
            }
           
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateAllValue()
        {
            try
            {
                //UpdateDataFun fun = new UpdateDataFun();
                var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
                int vcount = 0;

                foreach (var vv in manager.ListDevice())
                {
                    vcount += vv.ListTags().Count;
                }
                var dcount = manager.ListDevice().Count;
                var vbuffer = mClient.AllowBufer(dcount, vcount, 128);
                vbuffer.Write(dcount);
                foreach (var vv in manager.ListDevice())
                {
                    vbuffer.Write(vv.Name);
                    vbuffer.Write(vv.ListTags().Count);
                    foreach (var vvv in vv.ListTags())
                    {
                        FillMemory(vbuffer, vvv);
                    }
                }
                mClient.UpdateValue(vbuffer);
                mIsNeedInited = false;
            }
            catch
            {

            }
        }


        //private void UpdateAllValue2()
        //{
        //    try
        //    {
        //        //UpdateDataFun fun = new UpdateDataFun();
        //        var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
        //        int vcount = 0;

        //        foreach (var vv in manager.ListDevice())
        //        {
        //            vcount += vv.ListTags().Count;
        //        }
        //        var dcount = manager.ListDevice().Count;
        //        var vbuffer = mClient.AllowBufer2(dcount, vcount, 128);
        //        vbuffer.Write(dcount);
        //        foreach (var vv in manager.ListDevice())
        //        {
        //            vbuffer.Write(vv.Name);
        //            vbuffer.Write(vv.ListTags().Count);
        //            foreach (var vvv in vv.ListTags())
        //            {
        //                FillMemory(vbuffer, vvv);
        //            }
        //        }
        //        mClient.UpdateValue2(vbuffer);
        //        mIsNeedInited = false;
        //    }
        //    catch
        //    {

        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vbuffer"></param>
        /// <param name="tag"></param>
        private void FillMemory(ByteBuffer vbuffer,Tagbase tag)
        {
            vbuffer.Write(tag.Name);
            vbuffer.WriteByte((byte)tag.Type);
            switch (tag.Type)
            {
                case TagType.Bool:
                    vbuffer.Write(Convert.ToByte(tag.Value));
                    break;
                case TagType.Byte:
                    vbuffer.Write(Convert.ToByte(tag.Value));
                    break;
                case TagType.Short:
                    vbuffer.Write(Convert.ToInt16(tag.Value));
                    break;
                case TagType.UShort:
                    vbuffer.Write(Convert.ToUInt16(tag.Value));
                    break;
                case TagType.Int:
                    vbuffer.Write(Convert.ToInt32(tag.Value));
                    break;
                case TagType.UInt:
                    vbuffer.Write(Convert.ToUInt32(tag.Value));
                    break;
                case TagType.Long:
                    vbuffer.Write(Convert.ToInt64(tag.Value));
                    break;
                case TagType.ULong:
                    vbuffer.Write(Convert.ToUInt64(tag.Value));
                    break;
                case TagType.Float:
                    vbuffer.Write(Convert.ToSingle(tag.Value));
                    break;
                case TagType.Double:
                    vbuffer.Write(Convert.ToDouble(tag.Value));
                    break;
                case TagType.String:
                    vbuffer.Write(Convert.ToString(tag.Value));
                    break;
                case TagType.DateTime:
                    vbuffer.Write(Convert.ToDateTime(tag.Value));
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private ByteBuffer AllowBuffer()
        {
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
            var device = manager.ListDevice();
            var dcount = device.Count;
            int vcount = 0;
            foreach (var vv in device)
            {
                vcount += vv.ListTags().Count;
            }
            return mClient.AllowBufer(dcount, vcount, 32);
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateChanged()
        {
            //UpdateDataFun fun = new UpdateDataFun();
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
            var dcount = manager.ListDevice().Count;
            int vcount = 0;

            Dictionary<string, List<Tagbase>> mSends = new Dictionary<string, List<Tagbase>>();
            lock (mChangedTags)
            {
                foreach (var vv in mChangedTags)
                {
                    foreach(var vvvv in vv.Value.Where(e=>e.Value))
                    {
                        if(mSends.ContainsKey(vv.Key))
                        {
                            mSends[vv.Key].Add(vvvv.Key);
                        }
                        else
                        {
                            mSends.Add(vv.Key, new List<Tagbase>() { vvvv.Key });
                        }
                        mChangedTags[vv.Key][vvvv.Key] = false;
                        vcount++;
                    }
                }
            }
           
            if (mSends.Count > 0)
            {
                var vbuffer = mClient.AllowBufer(dcount, vcount, 128);
                vbuffer.Write(mSends.Count);
                foreach (var vv in mSends)
                {
                    string dev = vv.Key;
                    vbuffer.Write(vv.Key);
                    vbuffer.Write(vv.Value.Count);

                    foreach (var vvv in vv.Value)
                    {
                        FillMemory(vbuffer, vvv);
                    }
                }
                mClient.UpdateValue(vbuffer);
            }
            
        }


        private void UpdateChanged2()
        {
            //UpdateDataFun fun = new UpdateDataFun();
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
            var dcount = manager.ListDevice().Count;
            int vcount = 0;

            Dictionary<string, List<Tagbase>> mSends = new Dictionary<string, List<Tagbase>>();
            lock (mChangedTags)
            {
                foreach (var vv in mChangedTags)
                {
                    foreach (var vvvv in vv.Value.Where(e => e.Value))
                    {
                        if (mSends.ContainsKey(vv.Key))
                        {
                            mSends[vv.Key].Add(vvvv.Key);
                        }
                        else
                        {
                            mSends.Add(vv.Key, new List<Tagbase>() { vvvv.Key });
                        }
                        mChangedTags[vv.Key][vvvv.Key] = false;
                        vcount++;
                    }
                }
            }

            if (mSends.Count > 0)
            {
                var vbuffer = mClient.AllowBufer2(dcount, vcount, 128);
                vbuffer.Write(mSends.Count);
                foreach (var vv in mSends)
                {
                    string dev = vv.Key;
                    vbuffer.Write(vv.Key);
                    vbuffer.Write(vv.Value.Count);

                    foreach (var vvv in vv.Value)
                    {
                        FillMemory(vbuffer, vvv);
                    }
                }
                mClient.UpdateValue2(vbuffer);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            mScanThread = new Thread(ThreadPro);
            mScanThread.IsBackground = true;
            mScanThread.Start();
            mClient.Open();
            base.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Stop()
        {
            mIsClosed = true;
            mClient.Close();
            base.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IApi NewApi()
        {
            return new SpiderTcpApiRuntime();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...





    }
}
