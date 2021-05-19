//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/7 17:08:18.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using Opc.Ua;
using System.Threading.Tasks;
using System.Threading;

namespace Cdy.Spider.OpcClient
{
    /// <summary>
    /// 
    /// </summary>
    public class OpcUaChannel : ChannelBase
    {

        #region ... Variables  ...
        private OpcUAChannelData mData;

        private OpcUaClient mClient;

        private List<string> mSubscription;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "OpcUa";

        /// <summary>
        /// 
        /// </summary>
        public override ChannelData Data => mData;

        /// <summary>
        /// 
        /// </summary>
        public override string RemoteDescription => mData.ServerIp+":"+mData.Port;

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            mClient = new OpcUaClient();
            if(!string.IsNullOrEmpty(mData.UserName))
            {
                mClient.UseSecurity = true;
                mClient.UserIdentity = new Opc.Ua.UserIdentity(mData.UserName, mData.Password);
            }
            base.Init();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfos"></param>
        public override void Prepare(List<string> deviceInfos)
        {
            mSubscription = deviceInfos;
            base.Prepare(deviceInfos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool InnerOpen()
        {
            try
            {
                mClient.ConnectComplete += MClient_ConnectComplete;
                mClient.ConnectServer(this.mData.ServerIp).Wait();
               
                return base.InnerOpen();
            }catch
            {
                Task.Run(TryConnect);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void TryConnect()
        {
            while(!mClient.Connected)
            {
                Thread.Sleep(3000);
                try
                {
                    mClient.ConnectServer(this.mData.ServerIp).Wait();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MClient_ConnectComplete(object sender, EventArgs e)
        {
            ConnectedChanged(mClient.Connected);
            if (mSubscription != null && mClient.Connected)
            {
                Task.Run(() =>
                {
                    mClient.AddSubscription("spider", this.mSubscription.ToArray(), new Action<string, Opc.Ua.Client.MonitoredItem, Opc.Ua.Client.MonitoredItemNotificationEventArgs>((tag, item, arg) => {

                        MonitoredItemNotification notification = arg.NotificationValue as MonitoredItemNotification;
                        OnReceiveCallBack2(item.DisplayName, notification.Value.Value);
                    }));

                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool InnerClose()
        {
            if(mClient!=null)
            {
                mClient.RemoveSubscription("spider");
                mClient.Disconnect();
                mClient.ConnectComplete -= MClient_ConnectComplete;
                mClient = null;
            }
            return base.InnerClose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override object SendObjectInner(string key, object value, int timeout, out bool result)
        {
            try
            {
                if (mClient != null && mClient.Connected)
                {
                    result = mClient.WriteNode(key, value);
                    return true;
                }
                else
                {
                    result = false;
                    return false;
                }
            }
            catch
            {
                result = false;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override object SendObjectInner(object value, int timeout, out bool result)
        {
            if (mClient != null && mClient.Connected)
            {
                var tags = value as IEnumerable<string>;
                var res = mClient.ReadNodes(tags.Select(e => new NodeId(e)).ToArray());
                if (res != null)
                {
                    result = true;
                    return res.Select(e => e.Value).ToList();
                }
                else
                {
                    result = false;
                    return null;
                }
            }
            else
            {
                result = false;
                return null;
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="data"></param>
        ///// <param name="timeOut"></param>
        ///// <param name="paras"></param>
        ///// <returns></returns>
        //protected override object SendInner(string key, object data, int timeOut, object[] paras)
        //{
        //    try
        //    {
        //        if (mClient != null && mClient.Connected)
        //        {
        //            if (paras.Length > 0)
        //            {
        //                mClient.WriteNode(key, data);
        //                return true;
        //            }
        //            else
        //            {
        //                var tags = data as IEnumerable<string>;
        //                var res = mClient.ReadNodes(tags.Select(e => new NodeId(e)).ToArray());
        //                if (res != null)
        //                    return res.Select(e => e.Value).ToList();
        //                else
        //                {
        //                    return null;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannel NewApi()
        {
            return new OpcUaChannel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new OpcUAChannelData();
            mData.LoadFromXML(xe);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
