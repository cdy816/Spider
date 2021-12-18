using Cdy.Spider;
using Cheetah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cdy.Api.SpiderTcp
{
    /// <summary>
    /// 
    /// </summary>
    public class TcpClient : Cheetah.TcpSocket.TcpSocketClient
    {

        #region ... Variables  ...

        public event ConnectedChangedDelegate ConnectedChanged;

        public delegate void ConnectedChangedDelegate(object sender, bool isConnected);

        private long mLoginId = 0;

        /// <summary>
        /// 
        /// </summary>
        public const byte TagInfoRequestFun = 4;
        /// <summary>
        /// 
        /// </summary>
        public const byte RealValueFun = 1;

        /// <summary>
        /// 
        /// </summary>
        public const byte PushDataChangedFun = 3;

        public const byte LoginFun = 1;
        public const byte HartFun = 2;

        public const byte RegistorFun = 2;
        public const byte Update = 3;
        public const byte Write = 4;
        public const byte NoLogin = 9;

        public const byte Update2 = 103;


        private ManualResetEvent mLoginEvent = new ManualResetEvent(false);
        private ManualResetEvent mRegistorEvent = new ManualResetEvent(false);
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public bool IsLogin
        {
            get
            {
                return mLoginId > 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string  Password { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isConnected"></param>
        public override void OnConnected(bool isConnected)
        {
            ConnectedChanged?.Invoke(this,isConnected);
            if (!isConnected)
            {
                mLoginId = 0;
                LoggerService.Service.Warn("SpiderTcp", $"connect to {this.ServerIp} failed.");
            }
            else
            {
                LoggerService.Service.Info("SpiderTcp", $"connect to {this.ServerIp} sucessfull.");
            }
            base.OnConnected(isConnected);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public override void OnReceiveData(ByteBuffer data)
        {
            var cmd = data.ReadByte();
            switch (cmd)
            {
                case TagInfoRequestFun:
                    byte cd2 = data.ReadByte();
                    if (cd2 == LoginFun)
                    {
                        mLoginId = data.ReadLong();
                        Console.WriteLine("登录 Link 服务器成功");
                        mLoginEvent.Set();
                    }
                    break;
                case RealValueFun:
                    byte cd3 = data.ReadByte();
                    switch (cd3)
                    {
                        case NoLogin:
                            mLoginId = -1;
                            break;
                        case RegistorFun:
                            mRegistorEvent.Set();
                            break;
                        case Write:
                            ProcessWrite(data);
                            break;
                        case Update:
                            break;
                    }

                    break;
                case PushDataChangedFun:
                    break;
            }

            base.OnReceiveData(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void ProcessWrite(ByteBuffer data)
        {
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
            string device = data.ReadString();
            var vdd = manager.GetDevice(device);
            if (vdd == null) return;
            string tagName = data.ReadString();

            var tag = vdd.GetTag(tagName);
            if (tag == null) return;
            byte typ = data.ReadByte();
            switch (typ)
            {
                case (byte)TagType.Bool:
                    tag.Value = data.ReadByte();
                    break;
                case (byte)TagType.Byte:
                    tag.Value = data.ReadByte();
                    break;
                case (byte)TagType.Short:
                    tag.Value = data.ReadShort();
                    break;
                case (byte)TagType.UShort:
                    tag.Value = data.ReadUShort();
                    break;
                case (byte)TagType.Int:
                    tag.Value = data.ReadInt();
                    break;
                case (byte)TagType.UInt:
                    tag.Value = data.ReadUInt();
                    break;
                case (byte)TagType.Long:
                case (byte)TagType.ULong:
                    tag.Value = data.ReadLong();
                    break;
                case (byte)TagType.Float:
                    tag.Value = data.ReadFloat();
                    break;
                case (byte)TagType.Double:
                    tag.Value = data.ReadDouble();
                    break;
                case (byte)TagType.String:
                    tag.Value = data.ReadString();
                    break;
                case (byte)TagType.DateTime:
                    tag.Value = data.ReadDateTime();
                    break;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Login(string username,string password,int timeout=5000)
        {
            int len = username.Length*2 + 4 + password.Length * 2 + 4 + 4;
            var databuffer = this.MemoryPool.Alloc(len);
            databuffer.WriteByte(TagInfoRequestFun);
            databuffer.WriteByte(LoginFun);
            databuffer.Write(username);
            databuffer.Write(password);
            mLoginEvent.Reset();
            SendData(databuffer);
            if(mLoginEvent.WaitOne(timeout))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Hart()
        {
            int len = 10;
            var databuffer = this.MemoryPool.Alloc(len);
            databuffer.WriteByte(TagInfoRequestFun);
            databuffer.WriteByte(HartFun);
            databuffer.Write(mLoginId);
            SendData(databuffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagcount"></param>
        /// <param name="tagdatasize"></param>
        /// <returns></returns>
        public Cheetah.ByteBuffer AllowBufer(int deviceCount,int tagcount,int tagdatasize)
        {
            var re = this.MemoryPool.Alloc(2 + 8+ 4 + tagcount * tagdatasize+ (64 +4) * deviceCount);
            re.WriteIndex = 10;
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceCount"></param>
        /// <param name="tagcount"></param>
        /// <param name="tagdatasize"></param>
        /// <returns></returns>
        public Cheetah.ByteBuffer AllowBufer2(int deviceCount, int tagcount, int tagdatasize)
        {
            var re = this.MemoryPool.Alloc(2 +  4 + tagcount * tagdatasize + (64 + 4) * deviceCount + UserName.Length * 2 + 4 + Password.Length * 2 + 4);
            re.WriteIndex = 2 + UserName.Length * 2 + 4 + Password.Length * 2 + 4;
            return re;
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public bool UpdateValue(Cheetah.ByteBuffer datas)
        {
            datas.WriteByte(0,RealValueFun);
            datas.WriteByte(1, Update);
            datas.WriteLong(2, mLoginId);
            SendData(datas);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public bool UpdateValue2(Cheetah.ByteBuffer datas)
        {
            datas.WriteByte(0, RealValueFun);
            datas.WriteByte(1, Update2);
            datas.WriteString(2,UserName,Encoding.Unicode);
            datas.WriteString(2+UserName.Length*2+4,Password,Encoding.Unicode);
            SendData(datas);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool Registor(string device,int timeout=5000)
        {
            var databuffer = this.MemoryPool.Alloc(device.Length*2+4+2+8);
            databuffer.Write(RealValueFun);
            databuffer.Write(RegistorFun);
            databuffer.Write(mLoginId);
            databuffer.Write(device);
            mRegistorEvent.Reset();
            SendData(databuffer);
            if(mRegistorEvent.WaitOne(timeout))
            {
                return true;
            }
            return false;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
