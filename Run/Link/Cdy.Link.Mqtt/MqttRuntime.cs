using Cdy.Spider;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Link.Mqtt
{
    public class MqttRuntime : Spider.LinkRunner
    {

        #region ... Variables  ...

        private MqttLinkData mData;

        private MqttFactory mqttFactory;

        private MqttClientOptions options;

        private IManagedMqttClient mqttClient;

        private Dictionary<string, Action<Dictionary<string, object>>> mCallback = new Dictionary<string, Action<Dictionary<string, object>>>();

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, string> mDeviceMaps = new Dictionary<string, string>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        public MqttRuntime()
        {
            mqttFactory = new MqttFactory();
        }

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public override LinkData Data => mData;

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "MqttLink";

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            base.Init();
            InitMqttClient();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitMqttClient()
        {
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
                    Server = mData.ServerUrl,
                    Port = mData.ServerPort,
                    TlsOptions = tlsOptions
                }
            };

            options.Credentials = new MqttClientCredentials
            {
                Username = mData.ServerUser,
                Password = Encoding.UTF8.GetBytes(mData.ServerPassword)
            };

            options.CleanSession = true;
            options.KeepAlivePeriod = TimeSpan.FromMilliseconds(5000);

            mqttClient = mqttFactory.CreateManagedMqttClient();
            mqttClient.UseApplicationMessageReceivedHandler(HandleReceivedApplicationMessage);
            mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
            mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);
        }

        /// <summary>
        /// Handles the publisher connected event.
        /// </summary>
        /// <param name="x">The MQTT client connected event args.</param>
        private void OnConnected(MqttClientConnectedEventArgs x)
        {
            Task.Run(() => {
                this.mqttClient.SubscribeAsync(mData.LocalTopic);
                //this.mqttClient.SubscribeAsync(mData.RemoteResponseTopic);
            });
        }

        /// <summary>
        /// Handles the publisher disconnected event.
        /// </summary>
        /// <param name="x">The MQTT client disconnected event args.</param>
        private void OnDisconnected(MqttClientDisconnectedEventArgs x)
        {

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="responeTopic"></param>
        /// <param name="data"></param>
        private void SendToTopicData(string topic, string responeTopic, string data)
        {
            try
            {
                var msg = new MqttApplicationMessageBuilder().WithTopic(topic).WithResponseTopic(responeTopic).WithPayload(data).WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce).Build();
                this.mqttClient.PublishAsync(msg);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        private void HandleReceivedApplicationMessage(MqttApplicationMessageReceivedEventArgs x)
        {
            if (x.ApplicationMessage.Payload != null && x.ApplicationMessage.Payload.Length > 0 && !string.IsNullOrEmpty(x.ApplicationMessage.Topic))
            {
                //处理客户端主动上传数据

                var data = x.ApplicationMessage.Payload;
                if (data != null && data.Length > 0)
                {
                    var vdatas = JObject.Parse(Encoding.UTF8.GetString(data));
                    if (vdatas.ContainsKey("Fun"))
                    {
                        var sfun = vdatas["Fun"].ToString();
                        if (sfun == "update")
                        {
                            var manager = ServiceLocator.Locator.Resolve<IDeviceForDriver>();
                            //执行数据下发
                            var wd = vdatas.ToObject<UpdateDataFun>();
                            if (wd != null)
                            {
                                foreach (var vv in wd.Devices)
                                {
                                    if (mCallback.ContainsKey(vv.Device))
                                    {
                                        mCallback[vv.Device](vv.Values);
                                    }
                                }
                            }
                        }
                        else if(sfun== "registor")
                        {
                            var rg = vdatas.ToObject<RegistorFun>();
                            if(rg.Devices!=null)
                            {
                                foreach(var vv in rg.Devices)
                                {
                                    if(mDeviceMaps.ContainsKey(vv))
                                    {
                                        mDeviceMaps[vv] = rg.Topic;
                                    }
                                    else
                                    {
                                        mDeviceMaps.Add(vv, rg.Topic);
                                    }
                                }

                                //Task.Run(() => {
                                //    this.mqttClient.SubscribeAsync(rg.Topic);
                                //});

                                SendToTopicData(rg.Topic, mData.LocalTopic, Newtonsoft.Json.JsonConvert.SerializeObject(new RegistorResponseFun() { Result = true, Time = DateTime.Now }));

                            }
                           
                        }
                    }

                }
            }
        }

        public override void WriteValue(string device, string tagid, object value, byte valuetype)
        {
            Dictionary<string, string> dtmp = new Dictionary<string, string>();
            dtmp.Add(device+"|"+tagid, value.ToString());

            if(mDeviceMaps.ContainsKey(device))
            {
                var rk = mDeviceMaps[device];
                SendToTopicData(rk, mData.LocalTopic, Newtonsoft.Json.JsonConvert.SerializeObject(new WriteDataFun() { Fun = "write", Data = dtmp }));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new MqttLinkData();
            mData.LoadFromXML(xe);
            base.Load(xe);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            this.mqttClient.StartAsync(new ManagedMqttClientOptions
            {
                ClientOptions = options
            });
            base.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Stop()
        {
            mqttClient.StopAsync();
            base.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ILink NewLink()
        {
            return new MqttRuntime();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="valueUpdate"></param>
        public override void RegistorValueUpdateCallBack(string device, Action<Dictionary<string, object>> valueUpdate)
        {
            if(mCallback.ContainsKey(device))
            {
                mCallback[device] = valueUpdate;
            }
            else
            {
                mCallback.Add(device, valueUpdate);
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    public class RegistorResponseFun : FunBase
    {
        public RegistorResponseFun()
        {
            Fun = "registorresponse";
        }
        public DateTime Time { get; set; }
        public bool Result { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WriteDataFun
    {
        /// <summary>
        /// 
        /// </summary>
        public string Fun { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> Data { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UpdateDataFun : FunBase
    {
        public UpdateDataFun()
        {
            Fun = "update";
        }
        public List<DeviceItem> Devices { get; set; }
    }

    public class RegistorFun : FunBase
    {
        public RegistorFun()
        {
            Fun = "registor";
        }
        /// <summary>
        /// 主题
        /// </summary>
        public string Topic { get; set; }
        /// <summary>
        /// 设备列表
        /// </summary>
        public string[] Devices { get; set; }
    }



    /// <summary>
    /// 
    /// </summary>
    public class DeviceItem
    {
        public string Device { get; set; }

        public Dictionary<string, object> Values { get; set; }

    }




    public class FunBase
    {
        public string Fun { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Login : FunBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
    }
}
