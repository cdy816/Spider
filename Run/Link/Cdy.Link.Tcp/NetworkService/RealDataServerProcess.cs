using Cdy.Spider;
using Cheetah;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cdy.Link.Tcp
{
    /// <summary>
    /// 
    /// </summary>
    public class RealDataServerProcess : ServerProcessBase
    {

        #region ... Variables  ...

        private Dictionary<string, string[]> mMaps = new Dictionary<string, string[]>();

        private string mName;

        public const byte Registor = 2;
        public const byte Update = 3;
        public const byte Write = 4;
        public const byte NoLogin = 9;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        public RealDataServerProcess()
        {
           
        }

        #endregion ...Constructor...

        #region ... Properties ...

        public override byte FunId => APIConst.RealValueFun;

        public TcpRuntime Service { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            mName = "TcpLink" + Guid.NewGuid().ToString();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="data"></param>
        protected override void ProcessSingleData(string client, ByteBuffer data)
        {
            if (data.RefCount == 0)
            {
                Debug.Print("invailed data buffer in RealDataServerProcess");
                return;
            }

            byte cmd = data.ReadByte();
            long id = data.ReadLong();

            if(SecurityService.Service.CheckId(id))
            {
                switch (cmd)
                {
                    case Registor:
                        string sdevice = data.ReadString();
                        if(!string.IsNullOrEmpty(sdevice))
                        {
                            string[] devices = sdevice.Split(new char[] { ',' });
                            lock (mMaps)
                            {
                                if (!mMaps.ContainsKey(client))
                                {
                                    mMaps.Add(client, devices);
                                }
                                else
                                {
                                    mMaps[client] = devices;
                                }
                            }
                        }
                        Parent.AsyncCallback(client,ToByteBuffer(FunId, Registor, 1));
                        break;
                    case Update:
                        UpdateData(client, data);
                        break;
                }

            }
            else
            {
                Parent.AsyncCallback(client, ToByteBuffer(FunId, NoLogin, 0));
                Console.WriteLine("无效登录:"+client+" "+id);
            }
            base.ProcessSingleData(client, data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="block"></param>
        private void UpdateData(string clientId,ByteBuffer block)
        {
            //读取设备个数
            int devicecount = block.ReadInt();
            for(int i=0;i<devicecount;i++)
            {
                //设备名称
                string devicename = block.ReadString();

                int tagcount = block.ReadInt();

                Dictionary<string, object> mValues = new Dictionary<string, object>();

                for(int j=0;j<tagcount;j++)
                {
                    //tag name
                    string id = block.ReadString();
                    //tag type
                    int typ = block.ReadByte();

                    if(string.IsNullOrEmpty(id))
                    {
                        Console.WriteLine("收到无效数据!!!!!");
                        return;
                    }

                    switch (typ)
                    {
                        case (byte)TagType.Bool:
                            mValues.Add(id, block.ReadByte());

                            break;
                        case (byte)TagType.Byte:
                            mValues.Add(id, block.ReadByte());
                            break;
                        case (byte)TagType.Short:
                            mValues.Add(id, block.ReadShort());
                           
                            break;
                        case (byte)TagType.UShort:
                            mValues.Add(id, block.ReadUShort());
                            
                            break;
                        case (byte)TagType.Int:
                            mValues.Add(id, block.ReadInt());
                            break;
                        case (byte)TagType.UInt:
                            mValues.Add(id, block.ReadUInt());
                            break;
                        case (byte)TagType.Long:
                            mValues.Add(id, block.ReadLong());
                            break;
                        case (byte)TagType.ULong:
                            mValues.Add(id, block.ReadULong());
                            break;
                        case (byte)TagType.Float:
                            mValues.Add(id, block.ReadFloat());
                            break;
                        case (byte)TagType.Double:
                            mValues.Add(id, block.ReadDouble());
                            break;
                        case (byte)TagType.String:
                            mValues.Add(id, block.ReadString());
                            break;
                        case (byte)TagType.DateTime:
                            mValues.Add(id, block.ReadDateTime());
                            break;
                        
                    }
                }

                Service.UpdateValue(devicename, mValues);
            }
            Parent.AsyncCallback(clientId, ToByteBuffer(FunId, Update, 1));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="tag"></param>
        /// <param name="tagtype"></param>
        /// <param name="value"></param>
        public void WriteDeviceValue(string device, string tag, byte tagtype, object value)
        {
            string clid = string.Empty;
            foreach (var vv in mMaps)
            {
                if (vv.Value.Contains(device))
                {
                    clid = vv.Key;
                    break;
                }
            }

            if(!string.IsNullOrEmpty(clid))
            {
                var buffer = Parent.Allocate(256*2 + 5 + 2+8);
                buffer.WriteByte(FunId);
                buffer.WriteByte(Write);
                buffer.Write(device);
                ProcessTagPush(buffer, tag, tagtype, value);
                Parent.SendDataToClient(clid, buffer);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="id"></param>
        private void ProcessTagPush(ByteBuffer re, string tag, byte type, object value)
        {
            re.Write(tag);
            re.Write(type);
            switch (type)
            {
                case (byte)TagType.Bool:
                    re.Write(Convert.ToByte(value));
                    break;
                case (byte)TagType.Byte:
                    re.Write(Convert.ToByte(value));
                    break;
                case (byte)TagType.Short:
                    re.Write(Convert.ToInt16(value));
                    break;
                case (byte)TagType.UShort:
                    re.Write(Convert.ToUInt16(value));
                    break;
                case (byte)TagType.Int:
                    re.Write(Convert.ToInt32(value));
                    break;
                case (byte)TagType.UInt:
                    re.Write(Convert.ToUInt32(value));
                    break;
                case (byte)TagType.Long:
                case (byte)TagType.ULong:
                    re.Write(Convert.ToInt64(value));
                    break;
                case (byte)TagType.Float:
                    re.Write(Convert.ToSingle(value));
                    break;
                case (byte)TagType.Double:
                    re.Write(Convert.ToDouble(value));
                    break;
                case (byte)TagType.String:
                    string sval = value.ToString();
                    re.Write(sval, Encoding.Unicode);
                    break;
                case (byte)TagType.DateTime:
                    re.Write(((DateTime)value));
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public override void OnClientConnected(string id)
        {
            base.OnClientConnected(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public override void OnClientDisconnected(string id)
        {
            lock (mMaps)
            {
                if (mMaps.ContainsKey(id))
                {
                    mMaps.Remove(id);
                }
            }
            base.OnClientDisconnected(id);
        }



        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
