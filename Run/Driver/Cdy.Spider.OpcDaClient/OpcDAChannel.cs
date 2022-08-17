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
using System.Threading.Tasks;
using System.Threading;
using OpcCom;
using Opc;
using Convert = System.Convert;

namespace Cdy.Spider.OpcDAClient
{
    /// <summary>
    /// 
    /// </summary>
    public class OpcUaChannel : ChannelBase2
    {

        #region ... Variables  ...
        private OpcDAChannelData mData;

        private Opc.Da.Server mClient;

        private Dictionary<string,string> mSubscription;

        private Opc.Da.Subscription mMonitoringSubscription = null;

        private static ServerEnumerator m_discovery = new ServerEnumerator();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "OpcDA";

        /// <summary>
        /// 
        /// </summary>
        public override ChannelData Data => mData;

        /// <summary>
        /// 
        /// </summary>
        public override string RemoteDescription => mData.ServerIp;

        /// <summary>
        /// 是否为订阅模式
        /// </summary>
        public bool IsSubscriptionMode { get; set; } = true;

        public int PackageCount { get; set; } = int.MaxValue;

        public int ScanCircle { get; set; } = 0;


        private static Opc.Da.SubscriptionState mMonitoringGroup = new Opc.Da.SubscriptionState();

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="host"></param>
        private Opc.Da.Server InitClient(Specification spec, string name,string host)
        {
            Opc.Server[] servers = m_discovery.GetAvailableServers(spec, host, null);

            if (servers != null)
            {
                foreach (Opc.Da.Server server in servers)
                {
                    if (server != null)
                    {
                        if (string.Compare(server.Name, name, true) == 0)
                        {
                            return server;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            mClient = InitClient(Specification.COM_DA_30, mData.ServerName, mData.ServerIp);
            if(mClient==null)
            {
                mClient = InitClient(Specification.COM_DA_20, mData.ServerName, mData.ServerIp);
                if(mClient==null)
                {
                    mClient = InitClient(Specification.COM_DA_10, mData.ServerName, mData.ServerIp);
                }
            }

            if (IsSubscriptionMode)
            {
                mMonitoringGroup.Name = "Monitoring";                          // Group Name
                mMonitoringGroup.ServerHandle = null;                          // The handle assigned by the server to the group.
                mMonitoringGroup.ClientHandle = Guid.NewGuid().ToString();     // The handle assigned by the client to the group.
                mMonitoringGroup.Active = true;                                // Activate the group.
                mMonitoringGroup.UpdateRate = ScanCircle;                             // The refresh rate is 1 second. -> 1000
                mMonitoringGroup.Deadband = 0;                                 // When the dead zone value is set to 0, the server will notify the group of any data changes in the group.
            }

            base.Init();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfos"></param>
        public override void Prepare(ChannelPrepareContext deviceInfos)
        {
            if(deviceInfos.ContainsKey("IsSubscriptionMode"))
            {
                IsSubscriptionMode = (bool)deviceInfos["IsSubscriptionMode"];
            }

            if(deviceInfos.ContainsKey("PackageCount"))
            {
                PackageCount = (int)(deviceInfos["PackageCount"]);
            }

            if(deviceInfos.ContainsKey("ScanCircle"))
            {
                ScanCircle = (int)(deviceInfos["ScanCircle"]);
            }

            mSubscription = new Dictionary<string, string>();
            
            if (IsSubscriptionMode)
            {
                foreach (var vv in deviceInfos["Tags"] as IEnumerable<string>)
                {
                    if (vv.IndexOf("||") > 0)
                    {
                        string skey = vv.Substring(0, vv.LastIndexOf("||"));
                        string stype = vv.Substring(vv.LastIndexOf("||") + 2);
                        mSubscription.Add(skey, stype);
                    }
                    else
                    {
                        mSubscription.Add(vv, "");
                    }
                }
            }
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
                if (mClient != null)
                {
                    if (!string.IsNullOrEmpty(mData.UserName))
                    {
                        mClient.Connect(new ConnectData(new System.Net.NetworkCredential() { UserName = mData.UserName, Password = mData.Password }));
                    }
                    else
                    {
                        mClient.Connect();
                    }
                    mClient.ServerShutdown += MClient_ServerShutdown;
                }
                //mClient.ConnectComplete += MClient_ConnectComplete;
                //mClient.ConnectServer(this.mData.ServerIp).Wait();
               
                if(mClient!=null && mClient.IsConnected)
                {
                    ProSubscriptionMonitor();
                    LoggerService.Service.Info("OpcDA", $"connect to {this.mData.ServerIp} sucessfull.");
                }
                return base.InnerOpen();
            }catch
            {
                LoggerService.Service.Warn("OpcDA", $"connect to {this.mData.ServerIp} failed.");
                Task.Run(TryConnect);
                return false;
            }
        }

        private void MClient_ServerShutdown(string reason)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void TryConnect()
        {
            while(mClient!=null && !mClient.IsConnected)
            {
                Thread.Sleep(3000);
                try
                {
                    if (!string.IsNullOrEmpty(mData.UserName))
                    {
                        mClient.Connect(new ConnectData(new System.Net.NetworkCredential() { UserName = mData.UserName, Password = mData.Password }));
                    }
                    else
                    {
                        mClient.Connect();
                    }
                    if (mClient.IsConnected)
                    {
                        ProSubscriptionMonitor();
                        LoggerService.Service.Info("OpcDA", $"connect to {this.mData.ServerIp} sucessfull.");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Thread.Sleep(2000);
            }
        }

        private void ProSubscriptionMonitor()
        {
            ConnectedChanged(mClient.IsConnected);
            if (IsSubscriptionMode)
            {
                if (mMonitoringGroup != null)
                {
                    mMonitoringGroup.Name = "Spider"+mData.Name;                          // Group Name
                    mMonitoringGroup.ServerHandle = null;                          // The handle assigned by the server to the group.
                    mMonitoringGroup.ClientHandle = Guid.NewGuid().ToString();     // The handle assigned by the client to the group.
                    mMonitoringGroup.Active = true;                                // Activate the group.
                    mMonitoringGroup.UpdateRate = ScanCircle;                             // The refresh rate is 1 second. -> 1000
                    mMonitoringGroup.Deadband = 0;                                 // When the dead zone value is set to 0, the server will notify the group of any data changes in the group.
                }

                mMonitoringSubscription = (Opc.Da.Subscription)mClient.CreateSubscription(mMonitoringGroup);

                List<Opc.Da.Item> items = new List<Opc.Da.Item>();

                int count = this.mSubscription.Keys.Count / PackageCount;
                count = this.mSubscription.Keys.Count % PackageCount > 0 ? count + 1 : count;

                for (int i = 0; i < count; i++)
                {
                    int icount = (i + 1) * PackageCount;
                    if (icount > this.mSubscription.Count)
                    {
                        icount = this.mSubscription.Count - i * PackageCount;
                    }
                    var vkeys = this.mSubscription.Keys.Skip(i * PackageCount).Take(icount);

                    foreach (var vkey in vkeys)
                    {
                        items.Add(new Opc.Da.Item() { ClientHandle = Guid.NewGuid().ToString(), ItemName = vkey });
                    }
                }

                mMonitoringSubscription.AddItems(items.ToArray());
                var itemValues = mMonitoringSubscription.Read(mMonitoringSubscription.Items);

                mMonitoringSubscription.DataChanged -= MMonitoringSubscription_DataChanged;
                mMonitoringSubscription.DataChanged += MMonitoringSubscription_DataChanged;
                MMonitoringSubscription_DataChanged(mMonitoringSubscription, null, itemValues);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriptionHandle"></param>
        /// <param name="requestHandle"></param>
        /// <param name="values"></param>
        private void MMonitoringSubscription_DataChanged(object subscriptionHandle, object requestHandle, Opc.Da.ItemValueResult[] values)
        {
            foreach (var vv in values)
            {
                OnReceiveCallBack(vv.ItemName, vv.Value);
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
                //mClient.RemoveSubscription("spider");
                mClient.Disconnect();
                mClient.ServerShutdown -= MClient_ServerShutdown;
                mClient = null;
            }
            return base.InnerClose();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuetype"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private object ConvertValue(string key,object value)
        {
            if(mSubscription.ContainsKey(key))
            {
                string sdtype = mSubscription[key].ToLower();
                switch (sdtype)
                {
                    case "boolean":
                        return Convert.ToBoolean(value);
                    case "byte":
                        return Convert.ToByte(value);
                    case "int16":
                        return Convert.ToInt16(value);
                    case "uint16":
                        return Convert.ToUInt16(value);
                    case "int32":
                        return Convert.ToInt32(value);
                    case "uint32":
                        return Convert.ToUInt32(value);
                    case "int64":
                        return Convert.ToInt64(value);
                    case "uint64":
                        return Convert.ToUInt64(value);
                    case "float":
                        return Convert.ToSingle(value);
                    case "double":
                        return Convert.ToDouble(value);
                    case "string":
                        return value.ToString();
                    case "datetime":
                        return Convert.ToDateTime(value);
                }

            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override object WriteValueInner(string address, object value, int timeout,out bool result)
        {
            try
            {
                if (mClient != null && mClient.IsConnected)
                {
                    string skey = address;
                    if (skey.IndexOf("||") > 0)
                    {
                        skey = skey.Substring(0, skey.LastIndexOf("||"));
                    }

                    mClient.Write(new Opc.Da.ItemValue[] { new Opc.Da.ItemValue() { ItemName = skey, Value = ConvertValue(skey, value) } });
                    result = true;
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
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        protected override bool WriteValueNoWaitInner(string address, object value, int timeout)
        {
            bool re = false;
            WriteValueInner(address, value, timeout,out re);
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override object ReadValueInner(object value, int timeout, out bool result)
        {
            if (mClient != null && mClient.IsConnected)
            {
                var tags = value as IEnumerable<string>;

                var vtags = new List<Opc.Da.Item>(tags.Count());
                foreach (var vv in tags)
                {
                    if (vv.LastIndexOf("||") > 0)
                    {
                        vtags.Add(new Opc.Da.Item() { ItemName = vv.Substring(0, vv.LastIndexOf("||")) });
                    }
                    else
                    {
                        vtags.Add(new Opc.Da.Item() { ItemName = vv });
                    }
                }
               
                var res = mClient.Read(vtags.ToArray());
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannel2 NewApi()
        {
            return new OpcUaChannel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new OpcDAChannelData();
            mData.LoadFromXML(xe);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
