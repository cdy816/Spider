using Cdy.Spider;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Cdy.Link.Tcp
{
    public class TcpRuntime : Spider.LinkRunner
    {

        #region ... Variables  ...
        private TcpLinkData mData;
        private Dictionary<string, Action<Dictionary<string, object>>> mCallback = new Dictionary<string, Action<Dictionary<string, object>>>();

        private DataServerBase mTcpServer;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override LinkData Data => mData;

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "TcpLink";


        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            base.Init();
            mTcpServer = new DataServerBase() { Owner=this };
            SecurityService.Service.UserName = mData.UserName;
            SecurityService.Service.Password = mData.Password;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            base.Start();
            mTcpServer.Start(mData.Port);
            SecurityService.Service.Start();
        }


        /// <summary>
        /// 
        /// </summary>
        public override void Stop()
        {
            base.Stop();
            mTcpServer.Stop();
            SecurityService.Service.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new TcpLinkData();
            mData.LoadFromXML(xe);
            base.Load(xe);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ILink NewLink()
        {
            return new TcpRuntime();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="valueUpdate"></param>
        public override void RegistorValueUpdateCallBack(string device, Action<Dictionary<string, object>> valueUpdate)
        {
            if (mCallback.ContainsKey(device))
            {
                mCallback[device] = valueUpdate;
            }
            else
            {
                mCallback.Add(device, valueUpdate);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="tagid"></param>
        /// <param name="value"></param>
        /// <param name="valuetype"></param>
        public override void WriteValue(string device, string tagid, object value, byte valuetype)
        {
            mTcpServer.WriteDeviceValue(device,tagid,valuetype,value);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="values"></param>
        public void UpdateValue(string device,Dictionary<string,object> values)
        {
            if (mCallback.ContainsKey(device))
            {
                mCallback[device](values);
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...



    }
}
