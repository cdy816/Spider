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
    public class MQTTServerChannel: ChannelBase2
    {

        #region ... Variables  ...

        private MQTTChannelData mData;

        private IManagedMqttClient mqttClient;

        private MqttFactory mqttFactory;

        private MqttClientOptions options;

        private ManualResetEvent eventreset = new ManualResetEvent(false);
        private MQTTServer mServer;

        //private List<string> deviceInfosCach;

        private byte[] mResDatas;

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

            //mServer = new MQTTServer();
        }


        /// <summary>
        /// Handles the publisher connected event.
        /// </summary>
        /// <param name="x">The MQTT client connected event args.</param>
        private void OnConnected(MqttClientConnectedEventArgs x)
        {
            ConnectedChanged(true);
            Task.Run(() => {
                this.mqttClient.SubscribeAsync(mData.LocalTopic);
                this.mqttClient.SubscribeAsync(mData.RemoteResponseTopic);
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
                if (x.ApplicationMessage.Topic.Equals(mData.RemoteResponseTopic, StringComparison.OrdinalIgnoreCase))
                {
                    mResDatas = x.ApplicationMessage.Payload;
                    eventreset.Set();
                }
                else
                {
                    //客户端主动推送过来数据
                    var res = this.OnReceiveCallBack("", x.ApplicationMessage.Payload);
                    if (!string.IsNullOrEmpty(x.ApplicationMessage.ResponseTopic) && res != null)
                    {
                        if (res is byte[])
                        {
                            SendToTopicDataWithoutResponse(x.ApplicationMessage.ResponseTopic, res as byte[]);
                        }
                        else
                        {
                            SendToTopicDataWithoutResponse(x.ApplicationMessage.ResponseTopic, Encoding.UTF8.GetBytes(res.ToString()));
                        }
                    }
                    else
                    {
                        if (res is byte[])
                        {
                            SendToTopicDataWithoutResponse(mData.LocalReponseTopic, res as byte[]);
                        }
                        else
                        {
                            SendToTopicDataWithoutResponse(mData.LocalReponseTopic, Encoding.UTF8.GetBytes(res.ToString()));
                        }
                    }
                    
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfos"></param>
        public override void Prepare(ChannelPrepareContext deviceInfos)
        {
            base.Prepare(deviceInfos);
            if (IsConnected)
            {
                this.mqttClient.SubscribeAsync(mData.LocalTopic);
                this.mqttClient.SubscribeAsync(mData.RemoteResponseTopic);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool InnerOpen()
        {
            mServer = MQTTServerManager.Manager.GetServer(mData.Port);

            if (!mServer.IsStarted)
            {
                mServer.UserName = mData.UserName;
                mServer.Password = mData.Password;
                mServer.Start();
            }

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
        public override ICommChannel2 NewApi()
        {
            return new MQTTServerChannel();
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
            byte[] bval = Encoding.UTF8.GetBytes(value.ToString());
            return ReadValueInner(new Span<byte>(bval),timeout,out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override object ReadValueInner(Span<byte> value, int timeout, out bool result)
        {
            eventreset.Reset();
            SendToTopicData(mData.RemoteTopic, mData.RemoteResponseTopic, value);
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
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override object WriteValueInner(string address, object value, int timeout,out bool result)
        {
            byte[] bval = Encoding.UTF8.GetBytes(value.ToString());
            return SendAndWaitInner(new Span<byte>(bval),timeout,out result);
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
            byte[] bval = Encoding.UTF8.GetBytes(value.ToString());
            return SendInner(bval);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override byte[] SendAndWaitInner(Span<byte> data, int timeout, out bool result)
        {
            eventreset.Reset();
            SendToTopicData(mData.RemoteTopic, mData.RemoteResponseTopic, data);
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
        protected override bool SendInner(Span<byte> data)
        {
            SendToTopicData(mData.RemoteTopic, mData.RemoteResponseTopic, data);
            return true;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
