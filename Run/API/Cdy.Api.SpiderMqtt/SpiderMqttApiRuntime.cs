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
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Api.SpiderMqtt
{
    public class SpiderMqttApiRuntime : Cdy.Spider.ApiBase
    {

        #region ... Variables  ...
        private MqttApiData mData;
        private Dictionary<string, Dictionary<Tagbase, bool>> mChangedTags = new Dictionary<string, Dictionary<Tagbase, bool>>();


        private Dictionary<string, int> mCallBackTags = new Dictionary<string, int>();

        private IManagedMqttClient mqttClient;

        private MqttFactory mqttFactory;

        private MqttClientOptions options;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        public SpiderMqttApiRuntime()
        {
            mqttFactory = new MqttFactory();
        }
        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override ApiData Data => mData;

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "SpiderMqttApi";

        //private string mToken = "";
        //private bool mIsLogin = false;

        private bool mIsClosed = false;
        private bool mIsNeedInited = false;
        private Thread mScanThread;
        private bool mIsRegisted = false;

        #endregion ...Properties...

        #region ... Methods    ...

        

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
                            if(mChangedTags[device].ContainsKey(tag))
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

            InitMqttClient();

        }

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
                    Server = mData.ServerIp,
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
                //Login();
            });
        }

        /// <summary>
        /// Handles the publisher disconnected event.
        /// </summary>
        /// <param name="x">The MQTT client disconnected event args.</param>
        private void OnDisconnected(MqttClientDisconnectedEventArgs x)
        {
            //mIsLogin = false;
            mIsNeedInited = true;
            mIsRegisted = false;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //private void Login()
        //{
        //    SendToTopicData(mData.RemoteTopic, mData.RemoteResponseTopic, Newtonsoft.Json.JsonConvert.SerializeObject(new Login() { Fun = "login", UserName = mData.UserName, Password = mData.Password }));
        //}

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
                //mIsLogin = false;
                mIsNeedInited = true;
                mIsRegisted = false;
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
                //处理客户端主动下发数据

                var data = x.ApplicationMessage.Payload;
                if(data!=null && data.Length>0)
                {
                    var vdatas = JObject.Parse(Encoding.UTF8.GetString(data));
                    if(vdatas.ContainsKey("Fun"))
                    {
                        var sfun = vdatas["Fun"].ToString();
                        if(sfun== "write")
                        {
                            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
                            //执行数据下发
                            var wd = vdatas.ToObject<WriteDataFun>();
                            foreach(var vv in wd.Data)
                            {
                                string[] stmp = vv.Key.Split(new char[] { '|' });
                                var dd = manager.GetDevice(stmp[0]);
                                if(dd!=null)
                                {
                                    var tag = dd.GetTag(stmp[1]);
                                    if(tag!=null)
                                    {
                                        switch (tag.Type)
                                        {
                                            case TagType.Bool:
                                                try
                                                {
                                                    var vtmp = vv.Value.ToLower();
                                                    if (vtmp == "true" || vtmp == "false")
                                                    {
                                                        tag.Value = bool.Parse(vv.Value);
                                                    }
                                                    else
                                                    {
                                                        tag.Value = Convert.ToDouble(vv.Value) > 0;
                                                    }
                                                }
                                                catch
                                                {

                                                }
                                                break;
                                            default:
                                                tag.Value = stmp[1];
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        else if(sfun=="registorresponse")
                        {
                            var wd = vdatas.ToObject<RegistorResponseFun>();
                            if(wd.Result)
                            {
                                mIsRegisted = true;
                            }
                        }
                    }
                    
                }
            }
        }

        private void ThreadPro()
        {
            while (!mIsClosed)
            {
                if (mqttClient.IsConnected)
                {
                    if (mIsRegisted)
                    {
                        if (mIsNeedInited)
                        {
                            UpdateAllValue();
                            mIsNeedInited = false;
                        }
                        else if (mIsRegisted)
                        {
                            //如果是定时模式
                            UpdateChanged();
                        }
                    }
                    else
                    {
                        Registor();
                    }
                }
                Thread.Sleep(mData.Circle);
            }
        }

        private void Registor()
        {
            RegistorFun rf = new RegistorFun() { Topic = mData.LocalTopic };
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
            List<string> ltmp = new List<string>();
            rf.Devices = manager.ListDevice().Select(e => e.Name).ToArray();

            SendToTopicData(mData.RemoteTopic, mData.LocalTopic, Newtonsoft.Json.JsonConvert.SerializeObject(rf));
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateAllValue()
        {
            UpdateDataFun fun = new UpdateDataFun();
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
            foreach(var vv in manager.ListDevice())
            {
                DeviceItem ditem = new DeviceItem() { Device = vv.Name,Values = new Dictionary<string, object>() };
                foreach(var vvv in vv.ListTags())
                {
                    ditem.Values.Add(vvv.Name, vvv.Value == null ? "" : vvv.Value.ToString());
                }
                fun.Devices.Add(ditem);
            }

            SendToTopicData(mData.RemoteTopic, mData.LocalTopic, Newtonsoft.Json.JsonConvert.SerializeObject(fun));

        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateChanged()
        {
            UpdateDataFun fun = new UpdateDataFun();
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
            bool ishase = false;
            foreach (var vv in mChangedTags.ToArray())
            {
                DeviceItem ditem = new DeviceItem() { Device = vv.Key,Values=new Dictionary<string, object>() };
                foreach (var vvv in vv.Value.ToArray())
                {
                    if (vvv.Value)
                    {
                        ishase = true;
                        ditem.Values.Add(vvv.Key.Name, vvv.Key.Value == null ? "" : vvv.Key.Value.ToString());
                    }

                    lock(mChangedTags)
                    {
                        vv.Value[vvv.Key] = false;
                    }
                }
                fun.Devices.Add(ditem);
            }

            if(ishase)
            SendToTopicData(mData.RemoteTopic, mData.LocalTopic, Newtonsoft.Json.JsonConvert.SerializeObject(fun));
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

            mScanThread = new Thread(ThreadPro);
            mScanThread.IsBackground = true;
            mScanThread.Start();
            base.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Stop()
        {
            mIsClosed = true;
            this.mqttClient.StopAsync();
            base.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new MqttApiData();
            mData.LoadFromXML(xe);
            base.Load(xe);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IApi NewApi()
        {
            return new SpiderMqttApiRuntime();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...


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
        public Dictionary<string,string> Data { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UpdateDataFun : FunBase
    {
        public UpdateDataFun()
        {
            Fun = "update";
            Devices = new List<DeviceItem>();
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
    public class DeviceItem
    {
        public string Device { get; set; }

        public Dictionary<string,object> Values { get; set; }

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
