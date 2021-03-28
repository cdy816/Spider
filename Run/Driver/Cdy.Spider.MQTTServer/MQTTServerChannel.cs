//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/6 15:03:55.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;

namespace Cdy.Spider.MQTTServer
{
    /// <summary>
    /// 
    /// </summary>
    public class MQTTServerChannel: ChannelBase
    {

        #region ... Variables  ...

        private MQTTChannelData mData;

        private IManagedMqttClient mqttClient;

        private MqttFactory mqttFactory;

        private MqttClientOptions options;

        private string mResTopic;

        private byte[] mResDatas;

        private ManualResetEvent eventreset = new ManualResetEvent(false);
        private MQTTServer mServer;

        private List<string> deviceInfosCach;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public MQTTServerChannel()
        {
            mqttFactory = new MqttFactory();
        }
        #endregion ...Constructor...

        #region ... Properties ...


        /// <summary>
        /// 
        /// </summary>
        public override ChannelData Data { get => mData;}

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "MQTTServer";

        

        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            base.Init();
            var tlsOptions = new MqttClientTlsOptions
            {
                UseTls = false,
                IgnoreCertificateChainErrors = true,
                IgnoreCertificateRevocationErrors = true,
                AllowUntrustedCertificates = true
            };

            options = new MqttClientOptions
            {
                ClientId = Guid.NewGuid().ToString(),
                ProtocolVersion = MqttProtocolVersion.V311,
                ChannelOptions = new MqttClientTcpOptions
                {
                    Server = "localhost",
                    Port = mData.Port,
                    TlsOptions = tlsOptions
                }
            };

            options.Credentials = new MqttClientCredentials
            {
                Username = mData.UserName,
                Password = Encoding.UTF8.GetBytes(mData.Password)
            };

            options.CleanSession = true;
            options.KeepAlivePeriod = TimeSpan.FromMilliseconds(mData.Timeout);

            mqttClient = mqttFactory.CreateManagedMqttClient();
            mqttClient.UseApplicationMessageReceivedHandler(HandleReceivedApplicationMessage);
            mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
            mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);

            mServer = new MQTTServer();
        }


        /// <summary>
        /// Handles the publisher connected event.
        /// </summary>
        /// <param name="x">The MQTT client connected event args.</param>
        private void OnConnected(MqttClientConnectedEventArgs x)
        {
            ConnectedChanged(true);
            Task.Run(() => {
                if (deviceInfosCach != null)
                {
                    foreach (var vv in deviceInfosCach)
                    {
                        //将本机认值服务器角色，下位设备认作客户端角色
                        //订购服务器端主题
                        this.mqttClient.SubscribeAsync(mData.TopicHeadString + vv + mData.ServerTopicAppendString);

                        //订购客户端回复主题
                        this.mqttClient.SubscribeAsync(mData.TopicHeadString + vv + mData.ClientTopicAppendString + mData.ResponseTopicAppendString);
                    }
                }
            });
            
        }

        /// <summary>
        /// Handles the publisher disconnected event.
        /// </summary>
        /// <param name="x">The MQTT client disconnected event args.</param>
        private void OnDisconnected(MqttClientDisconnectedEventArgs x)
        {
            ConnectedChanged(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        private void HandleReceivedApplicationMessage(MqttApplicationMessageReceivedEventArgs x)
        {
            if (x.ApplicationMessage.Payload != null && x.ApplicationMessage.Payload.Length > 0 && !string.IsNullOrEmpty(x.ApplicationMessage.Topic))
            {
                if (x.ApplicationMessage.Topic.Equals(mResTopic, StringComparison.OrdinalIgnoreCase))
                {
                    mResDatas = x.ApplicationMessage.Payload;
                    eventreset.Set();
                }
                else
                {
                    var vtop = x.ApplicationMessage.Topic;

                    if(vtop.EndsWith(mData.ResponseTopicAppendString))
                    {
                        return;
                    }

                    var vss = string.IsNullOrEmpty(mData.ServerTopicAppendString) ? vtop : vtop.Replace(mData.ServerTopicAppendString, "");

                    vss = string.IsNullOrEmpty(mData.TopicHeadString) ? vss : vss.Replace(mData.TopicHeadString, "");


                    var res = this.OnReceiveCallBack(vss, x.ApplicationMessage.Payload);
                    if (!string.IsNullOrEmpty(x.ApplicationMessage.ResponseTopic) && res != null)
                    {
                        SendToTopicDataWithoutResponse(x.ApplicationMessage.ResponseTopic, res);
                    }
                    else if (res != null)
                    {
                        SendToTopicDataWithoutResponse(vtop + mData.ResponseTopicAppendString, res);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfos"></param>
        public override void Prepare(List<string> deviceInfos)
        {
            base.Prepare(deviceInfos);
            if (IsConnected)
            {
                foreach (var vv in deviceInfos)
                {
                    //将本机认值服务器角色，下位设备认作客户端角色
                    //订购服务器端主题
                    this.mqttClient.SubscribeAsync(mData.TopicHeadString + vv + mData.ServerTopicAppendString);

                    //订购客户端回复主题
                    this.mqttClient.SubscribeAsync(mData.TopicHeadString + vv + mData.ClientTopicAppendString + mData.ResponseTopicAppendString);
                }
            }
            else
            {
                deviceInfosCach = deviceInfos;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool InnerOpen()
        {
            mServer.Port = mData.Port;
            mServer.UserName = mData.UserName;
            mServer.Password = mData.Password;
            mServer.Start();
            this.mqttClient.StartAsync(new ManagedMqttClientOptions
            {
                ClientOptions = options
            });
            return base.InnerOpen();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool InnerClose()
        {
            this.mqttClient.StopAsync();
            mServer.Stop();
            return base.InnerClose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="data"></param>
        private void SendToTopicData(string topic, string responeTopic, byte[] data,int start,int len)
        {
            var msg = new MqttApplicationMessageBuilder().WithTopic(topic).WithResponseTopic(responeTopic).WithPayload(new MemoryStream(data,start,len)).WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce).WithRetainFlag().Build();
            this.mqttClient.PublishAsync(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="responeTopic"></param>
        /// <param name="data"></param>
        private void SendToTopicData(string topic, string responeTopic, Span<byte> data)
        {
            var msg = new MqttApplicationMessageBuilder().WithTopic(topic).WithResponseTopic(responeTopic).WithPayload(data.ToArray()).WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce).WithRetainFlag().Build();
            this.mqttClient.PublishAsync(msg);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="data"></param>
        private void SendToTopicDataWithoutResponse(string topic, byte[] data)
        {
            var msg = new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(data).WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce).WithRetainFlag().Build();
            this.mqttClient.PublishAsync(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override byte[] SendInner(Span<byte> data, int timeout, out bool result)
        {
            string ss = this.Data.Name;
            string skey = mData.TopicHeadString + ss + mData.ClientTopicAppendString;
            string reskey = skey + mData.ResponseTopicAppendString;
            mResTopic = reskey;

            eventreset.Reset();
            SendToTopicData(skey, reskey, data);
            result = eventreset.WaitOne(timeout);

            if (result)
            {
                return mResDatas;
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
        /// <param name="data"></param>
        /// <returns></returns>
        protected override bool SendInnerAsync(Span<byte> data)
        {
            string ss = this.Data.Name;
            string skey = mData.TopicHeadString + ss + mData.ClientTopicAppendString;
            string reskey = ss + mData.ResponseTopicAppendString;
            mResTopic = reskey;
            SendToTopicData(skey, reskey, data);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override byte[] SendObjectInner(string key, Span<byte> value, int timeout, out bool result)
        {
            string ss = key;
            string skey = mData.TopicHeadString + ss + mData.ClientTopicAppendString;
            string reskey = skey + mData.ResponseTopicAppendString;
            mResTopic = reskey;

            eventreset.Reset();
            SendToTopicData(skey, reskey, value);
            result = eventreset.WaitOne(timeout);

            if (result)
            {
                return mResDatas;
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
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override object SendObjectInner(string key, object value, int timeout, out bool result)
        {
            string ss = key;
            string skey = mData.TopicHeadString + ss + mData.ClientTopicAppendString;
            string reskey = skey + mData.ResponseTopicAppendString;
            mResTopic = reskey;

            eventreset.Reset();

            var bval = Encoding.UTF8.GetBytes(value.ToString()).AsSpan<byte>();

            SendToTopicData(skey, reskey, bval);
            result = eventreset.WaitOne(timeout);

            if (result)
            {
                return Encoding.UTF8.GetString(mResDatas);
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
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override object SendObjectInner(object value, int timeout, out bool result)
        {
            return SendObjectInner(Data.Name, value, timeout, out result);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool SendObjectInnerAsync(string key, Span<byte> value)
        {
            string ss = key;
            string skey = mData.TopicHeadString + ss + mData.ClientTopicAppendString;
            string reskey = ss + mData.ResponseTopicAppendString;
            mResTopic = reskey;
            SendToTopicData(skey, reskey, value);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool SendObjectInnerAsync(string key, object value)
        {
            string ss = key;
            string skey = mData.TopicHeadString + ss + mData.ClientTopicAppendString;
            string reskey = ss + mData.ResponseTopicAppendString;
            mResTopic = reskey;
            var bval = Encoding.UTF8.GetBytes(value.ToString());
            SendToTopicData(skey, reskey, bval);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool SendObjectInnerAsync(object value)
        {
            return SendObjectInnerAsync(this.Data.Name, value);
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="data"></param>
        ///// <param name="result"></param>
        ///// <returns></returns>
        //protected override byte[] SendInner(byte[] data,int start,int len, int timeout, int waitResultCount, out bool result, params string[] paras)
        //{
        //    string ss = paras.Length > 0 ? this.Data.Name : paras[0];
        //    string skey = mData.TopicHeadString + ss + mData.ClientTopicAppendString;
        //    string reskey = paras.Length > 1 ? paras[1] : skey + mData.ResponseTopicAppendString;
        //    mResTopic = reskey;

        //    eventreset.Reset();
        //    SendToTopicData(skey, reskey, data);
        //    result = eventreset.WaitOne(timeout);

        //    if (result)
        //    {
        //        return mResDatas;
        //    }
        //    else
        //    {
        //        result = false;
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="datas"></param>
        ///// <param name="timeout"></param>
        ///// <param name="result"></param>
        ///// <param name="paras"></param>
        ///// <returns></returns>
        //protected override byte[] SendInner(Span<byte> datas, int timeout, int waitResultCount, out bool result, params string[] paras)
        //{
        //    string ss = paras.Length > 0 ? this.Data.Name : paras[0];
        //    string skey = mData.TopicHeadString + ss + mData.ClientTopicAppendString;
        //    string reskey = paras.Length > 1 ? paras[1] : skey + mData.ResponseTopicAppendString;
        //    mResTopic = reskey;

        //    eventreset.Reset();
        //    SendToTopicData(skey, reskey, datas);
        //    result = eventreset.WaitOne(timeout);

        //    if (result)
        //    {
        //        return mResDatas;
        //    }
        //    else
        //    {
        //        result = false;
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="data"></param>
        ///// <param name="result"></param>
        //protected override void SendInnerAsync(byte[] data,int start,int len, int waitResultCount, out bool result, params string[] paras)
        //{
        //    string ss = paras.Length > 0 ? this.Data.Name : paras[0];
        //    string skey = mData.TopicHeadString + ss + mData.ClientTopicAppendString;
        //    string reskey = paras.Length > 1 ? paras[1] : ss + mData.ResponseTopicAppendString;
        //    mResTopic = reskey;
        //    SendToTopicData(skey, reskey, data);
        //    result = true;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="datas"></param>
        ///// <param name="result"></param>
        ///// <param name="paras"></param>
        //protected override void SendInnerAsync(Span<byte> datas, int waitResultCount, out bool result, params string[] paras)
        //{
        //    string ss = paras.Length > 0 ? this.Data.Name : paras[0];
        //    string skey = mData.TopicHeadString + ss + mData.ClientTopicAppendString;
        //    string reskey = paras.Length > 1 ? paras[1] : ss + mData.ResponseTopicAppendString;
        //    mResTopic = reskey;
        //    SendToTopicData(skey, reskey, datas);
        //    result = true;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new MQTTChannelData();
            mData.LoadFromXML(xe);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannel NewApi()
        {
            return new MQTTServerChannel();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
