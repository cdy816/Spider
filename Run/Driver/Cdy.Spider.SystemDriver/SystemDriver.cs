//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/6 17:13:48.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Buffers;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace Cdy.Spider
{
    public class FunBase
    {
        public string Fun { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Login:FunBase
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

    /// <summary>
    /// 
    /// </summary>
    public class ResponseBase
    {
        public bool Result { get; set; }

        public string ErroMessage { get; set; }
    }

    public class LoginResponse:ResponseBase
    {
        public string Token { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetValues : FunBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string[] Tags { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetWriteBackValues : FunBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetValuesResponse:ResponseBase
    {
        public Dictionary<string, string> Data { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataUpdate:FunBase
    {
        public string Token { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }


    //public class DataWrite : FunBase
    //{
    //    public Dictionary<string, string> Data { get; set; }
    //}

    /// <summary>
    /// System driver 通信规范
    /// login
    /// {
    ///     "fun":"login",
    ///     "username":"user1",
    ///     "password":"pass1"
    /// }
    /// login response
    /// {
    ///     "result":"true",
    ///     "token":"1wertrtrte",
    ///     "ErroMessage":""
    /// }
    /// 
    /// request data
    /// {
    ///     "token":"1wertrtrte"
    ///     "fun":"get",
    ///     "tags":["tag1","tag2"]
    /// }
    /// request data response
    /// {
    ///     "result":"true",
    ///     "data":{"tag1":"0","tag2":"10"}
    /// }
    /// 
    /// update data
    /// {
    ///     "token":"1wertrtrte",
    ///     "fun":"update",
    ///     "data":{"tag1":"0","tag2":"1","tag3":"3"}
    /// }
    /// 
    /// update data response
    /// {
    ///     "result":"true",
    ///     "resultmessage":""
    /// }
    /// 
    /// {
    ///     "Token":"1wertrtrte",
    ///     "Fun":"getwritebackvalue"
    ///     
    /// }
    ///  data write call back
    /// {
    ///     "fun":"write",
    ///     "data":{"tag1":"0","tag2":"1","tag3":"3"}
    /// }
    /// 
    /// </summary>
    public class SystemDriver: TimerDriverRunner
    {

        #region ... Variables  ...

        private SystemDriverData mData;

        private string mLoginToken = "";

        private Dictionary<string, object> mSendCach = new Dictionary<string, object>();

        private bool mIsLogin = false;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public override DriverData Data => mData;

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "SystemDriver";

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public override void Prepare()
        {
            //var vv = this.mCachTags.Keys.Select(e => this.Device.Name + "/" + e).ToList();
            //vv.Add(this.Device.Name);
            //mComm.Prepare(vv);

            using (ChannelPrepareContext ctx = new ChannelPrepareContext())
            {
                ctx.Add("Tags", this.mCachTags.Keys.Select(e => this.Device.Name + "/" + e).ToList());
                ctx.Add("DeviceName", this.Device.Name);
                mComm.Prepare(ctx);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        protected override object OnReceiveData(string key, object data,out bool handled)
        {
            //{'tag1':{1:1},'tag2':{ 0:'true'},'tag3':{ 12:[10,11]}}
            //{{变量:{数据类型:值}}，{变量:{数据类型:值}}}

            //处理多个数据标签
            string vdata;
            if (data is byte[])
            {
                vdata = Encoding.UTF8.GetString(data as byte[]);
            }
            else
            {
                vdata = data.ToString();
            }
            string re = "";
            var vdatas = JObject.Parse(vdata);

            if(vdatas.ContainsKey("Fun"))
            {
                string sfun = vdatas["Fun"].ToString().ToLower();
                if(sfun=="login")
                {
                    //登录
                    var ll = vdatas.ToObject<Login>();
                    string token = string.Empty;
                    if (OnLoginResponse(ll,out token))
                    {
                        re = Newtonsoft.Json.JsonConvert.SerializeObject( new LoginResponse() { Result = true, Token = token });
                    }
                    else
                    {
                        re = Newtonsoft.Json.JsonConvert.SerializeObject(new LoginResponse() { Result = false, ErroMessage="login failed" });
                    }
                }
                else if(sfun== "update")
                {
                    //更新数据
                    var ll = vdatas.ToObject<DataUpdate>();
                    if(!mIsLogin || ll.Token!=mLoginToken)
                    {
                        re = Newtonsoft.Json.JsonConvert.SerializeObject(new ResponseBase() { Result = false, ErroMessage = "login failed" });
                    }
                    else
                    {
                        foreach(var vv in ll.Data)
                        {
                            if (vv.Key.IndexOf(".") > 0)
                            {
                                string[] stmp = vv.Key.Split(new char[] { '.' });
                                if(stmp[0]==this.Data.Name)
                                {
                                    UpdateValue(stmp[1], vv.Value);
                                }
                            }
                            else
                            {
                                UpdateValue(vv.Key, vv.Value);
                            }
                        }
                        re = Newtonsoft.Json.JsonConvert.SerializeObject(new ResponseBase() { Result = true, ErroMessage = "" });
                    }
                }
                else if(sfun=="getwritebackvalue")
                {
                    //请求要回写的数值
                    var ll = vdatas.ToObject<GetWriteBackValues>();
                    if (!mIsLogin || ll.Token != mLoginToken)
                    {
                        re = Newtonsoft.Json.JsonConvert.SerializeObject(new GetValuesResponse() { Result = false, ErroMessage = "login failed" });
                    }
                    else
                    {
                        lock (mSendCach)
                        {
                            if (mSendCach.Count > 0)
                            {
                                var vtmp = mSendCach.ToDictionary(e=>e.Key,e=>e.Value.ToString());
                                mSendCach.Clear();
                                re = Newtonsoft.Json.JsonConvert.SerializeObject(new GetValuesResponse() { Result = true, ErroMessage = "", Data = vtmp });
                            }
                            else
                            {
                                re = Newtonsoft.Json.JsonConvert.SerializeObject(new GetValuesResponse() { Result = true, ErroMessage = "" });
                            }
                        }
                    }
                }
            }
            handled = true;
            return re;
        }

   

        public bool OnLoginResponse(Login lg,out string token)
        {
            if (lg.UserName == mData.UserName && lg.Password == mData.Password)
            {
                token = Guid.NewGuid().ToString().Replace("-", "");
                mLoginToken = token;
                mIsLogin = true;
                return true;
            }
            else
            {
                token = string.Empty;
            }
            return false;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="obj"></param>
        //private object DecodeSingleValue(JObject obj)
        //{
        //    object re=null;

        //    var pps = obj.Properties().ToList();
        //    int bval = 0;
        //    if(pps.Count>0)
        //    {
        //        bval = int.Parse(pps[0].Name);
        //        var val = pps[0].Value;

        //        switch ((TagType)bval)
        //        {
        //            case TagType.Bool:
        //                re = val.ToObject<bool>();
        //                break;
        //            case TagType.Byte:
        //                re = val.ToObject<byte>();
        //                break;
        //            case TagType.DateTime:
        //                re = val.ToObject<DateTime>();
        //                break;
        //            case TagType.Double:
        //                re = val.ToObject<double>();
        //                break;
        //            case TagType.Float:
        //                re = val.ToObject<float>();
        //                break;
        //            case TagType.Int:
        //                re = val.ToObject<int>();
        //                break;
        //            case TagType.Long:
        //                re = val.ToObject<long>();
        //                break;
        //            case TagType.Short:
        //                re = val.ToObject<short>();
        //                break;
        //            case TagType.String:
        //                re = val.ToObject<string>();
        //                break;
        //            case TagType.UInt:
        //                re = val.ToObject<uint>();
        //                break;
        //            case TagType.ULong:
        //                re = val.ToObject<ulong>();
        //                break;
        //            case TagType.UShort:
        //                re = val.ToObject<ushort>();
        //                break;
        //            case TagType.IntPoint:
        //                var vvs = val.ToObject<int[]>();
        //                re = new IntPoint() { X = vvs[0], Y = vvs[1] };
        //                break;
        //            case TagType.IntPoint3:
        //                var vvs1 = val.ToObject<int[]>();
        //                re = new IntPoint3() { X = vvs1[0], Y = vvs1[1], Z = vvs1[2] };
        //                break;
        //            case TagType.UIntPoint:
        //                var vvs2 = val.ToObject<uint[]>();
        //                re = new UIntPoint() { X = vvs2[0], Y = vvs2[1] };
        //                break;
        //            case TagType.UIntPoint3:
        //                var vvs3 = val.ToObject<uint[]>();
        //                re = new UIntPoint3() { X = vvs3[0], Y = vvs3[1], Z = vvs3[2] };
        //                break;
        //            case TagType.LongPoint:
        //                var vvs4 = val.ToObject<long[]>();
        //                re = new LongPoint() { X = vvs4[0], Y = vvs4[1] };
        //                break;
        //            case TagType.LongPoint3:
        //                var vvs5 = val.ToObject<long[]>();
        //                re = new LongPoint3() { X = vvs5[0], Y = vvs5[1], Z = vvs5[2] };
        //                break;
        //            case TagType.ULongPoint:
        //                var vvs6 = val.ToObject<ulong[]>();
        //                re = new ULongPoint() { X = vvs6[0], Y = vvs6[1] };
        //                break;
        //            case TagType.ULongPoint3:
        //                var vvs7 = val.ToObject<ulong[]>();
        //                re = new ULongPoint3() { X = vvs7[0], Y = vvs7[1], Z = vvs7[2] };
        //                break;
        //        }

        //    }
        //    return re;
        //}

        private bool ClientLogin()
        {
            Login lg = new Login() { Fun = "login", UserName = mData.UserName, Password = mData.Password };
            var reqstr = Newtonsoft.Json.JsonConvert.SerializeObject(lg);
            if(IsChannelRaw())
            {
                var res = SendAndWait(Encoding.UTF8.GetBytes(reqstr));
                if(res!=null && res.Length>0)
                {
                    try
                    {
                        var rr = JObject.Parse(Encoding.UTF8.GetString(res)).ToObject<LoginResponse>();
                        mLoginToken = rr.Token;
                        return rr.Result;
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                var vv = ReadValue(reqstr);
                try
                {
                    if (vv != null)
                    {
                        var rr = JObject.Parse(vv.ToString()).ToObject<LoginResponse>();
                        mLoginToken = rr.Token;
                        return rr.Result;
                    }
                }
                catch
                {

                }
            }
            
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessTimerElapsed()
        {
            int count = this.mCachTags.Count/100;
            count = this.mCachTags.Count % 100 > 0 ? count + 1 : count;

            if(!mIsLogin)
            {
                mIsLogin = ClientLogin();
            }
            if (mIsLogin)
            {
                for (int i = 0; i < count; i++)
                {
                    int icount = (i + 1) * 100;
                    if (icount > this.mCachTags.Count)
                    {
                        icount = this.mCachTags.Count - i * 100;
                    }
                    var vkeys = this.mCachTags.Keys.Skip(i * 100).Take(icount);
                    GetGroupTags(vkeys);
                    Thread.Sleep(10);
                }
            }
            base.ProcessTimerElapsed();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tags"></param>
        private void GetGroupTags(IEnumerable<string> tags)
        {
            //发送数据格式
            //['tag1','tag2'] 
            //['变量名称','变量名称']

            if (tags.Count() < 1 ) return;

            var req = new GetValues() { Fun = "get", Token = mLoginToken, Tags = tags.ToArray() };

            if (IsChannelRaw())
            {
                var datas = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(req));
                var vdata = SendAndWait(datas);
                if(vdata!=null && vdata.Length>0)
                {
                    try
                    {
                        var res = JObject.Parse(Encoding.UTF8.GetString(vdata)).ToObject<GetValuesResponse>();
                        foreach(var vv in res.Data)
                        {
                            UpdateValue(vv.Key, vv.Value);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                var vdata = ReadValue(Newtonsoft.Json.JsonConvert.SerializeObject(req));
                try
                {
                    if (vdata != null)
                    {
                        var res = JObject.Parse(vdata.ToString()).ToObject<GetValuesResponse>();
                        foreach (var vv in res.Data)
                        {
                            UpdateValue(vv.Key, vv.Value);
                        }
                    }
                }
                catch
                {

                }
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="startIndex"></param>
        ///// <returns></returns>
        //private object DecodeValue(byte[] value,ref int startIndex)
        //{
        //    TagType bval = (TagType)(value[startIndex]);
        //    object re = null;
        //    switch (bval)
        //    {
        //        case TagType.Bool:
        //            re = BitConverter.ToBoolean(value.AsSpan(startIndex+1));
        //            startIndex += 2;
        //            break;
        //        case TagType.Byte:
        //            re = (value as byte[])[startIndex + 1];
        //            startIndex += 2;
        //            break;
        //        case TagType.DateTime:
        //            re = Convert.ToDateTime(BitConverter.ToInt64(value.AsSpan(startIndex + 1)));
        //            startIndex += 9;
        //            break;
        //        case TagType.Double:
        //            re = BitConverter.ToDouble(value.AsSpan(startIndex + 1));
        //            startIndex += 9;
        //            break;
        //        case TagType.Float:
        //            re = BitConverter.ToSingle(value.AsSpan(startIndex + 1));
        //            startIndex += 5;
        //            break;
        //        case TagType.Int:
        //            re = BitConverter.ToInt32(value.AsSpan(startIndex + 1));
        //            startIndex += 5;
        //            break;
        //        case TagType.Long:
        //            re = BitConverter.ToInt64(value.AsSpan(startIndex + 1));
        //            startIndex += 9;
        //            break;
        //        case TagType.Short:
        //            re = BitConverter.ToInt16(value.AsSpan(startIndex + 1));
        //            startIndex += 3;
        //            break;
        //        case TagType.String:
        //            var vsize = BitConverter.ToInt16(value.AsSpan(startIndex + 1));
        //            re = Encoding.UTF8.GetString(value.AsSpan(startIndex + 3,vsize));
        //            startIndex += (3 + vsize);
        //            break;
        //        case TagType.UInt:
        //            re = BitConverter.ToUInt32(value.AsSpan(startIndex + 1));
        //            startIndex += 5;
        //            break;
        //        case TagType.ULong:
        //            re = BitConverter.ToUInt64(value.AsSpan(startIndex + 1));
        //            startIndex += 9;
        //            break;
        //        case TagType.UShort:
        //            re = BitConverter.ToUInt16(value.AsSpan(startIndex + 1));
        //            startIndex += 3;
        //            break;
        //        case TagType.IntPoint:
        //            re = new IntPoint() { X = BitConverter.ToInt32(value.AsSpan(startIndex + 1, 4)), Y = BitConverter.ToInt32(value.AsSpan(startIndex + 5, 4)) };
        //            startIndex += 9;
        //            break;
        //        case TagType.IntPoint3:
        //            re = new IntPoint3() { X = BitConverter.ToInt32(value.AsSpan(startIndex + 1, 4)), Y = BitConverter.ToInt32(value.AsSpan(startIndex + 5, 4)), Z = BitConverter.ToInt32(value.AsSpan(startIndex + 9, 4)) };
        //            startIndex += 13;
        //            break;
        //        case TagType.UIntPoint:
        //            re = new UIntPoint() { X = BitConverter.ToUInt32(value.AsSpan(startIndex + 1, 4)), Y = BitConverter.ToUInt32(value.AsSpan(startIndex + 5, 4)) };
        //            startIndex += 9;
        //            break;
        //        case TagType.UIntPoint3:
        //            re = new UIntPoint3() { X = BitConverter.ToUInt32(value.AsSpan(startIndex + 1, 4)), Y = BitConverter.ToUInt32(value.AsSpan(startIndex + 5, 4)), Z = BitConverter.ToUInt32(value.AsSpan(startIndex + 9, 4)) };
        //            startIndex += 13;
        //            break;
        //        case TagType.LongPoint:
        //            re = new LongPoint() { X = BitConverter.ToInt64(value.AsSpan(startIndex + 1, 8)), Y = BitConverter.ToInt64(value.AsSpan(startIndex + 9, 8)) };
        //            startIndex += 17;
        //            break;
        //        case TagType.LongPoint3:
        //            re = new LongPoint3() { X = BitConverter.ToInt64(value.AsSpan(startIndex + 1, 8)), Y = BitConverter.ToInt64(value.AsSpan(startIndex + 9, 8)), Z = BitConverter.ToInt64(value.AsSpan(startIndex + 17, 8)) };
        //            startIndex += 25;
        //            break;
        //        case TagType.ULongPoint:
        //            re = new ULongPoint() { X = BitConverter.ToUInt64(value.AsSpan(startIndex + 1, 8)), Y = BitConverter.ToUInt64(value.AsSpan(startIndex + 9, 8)) };
        //            startIndex += 17;
        //            break;
        //        case TagType.ULongPoint3:
        //            re = new ULongPoint3() { X = BitConverter.ToUInt64(value.AsSpan(startIndex + 1, 8)), Y = BitConverter.ToUInt64(value.AsSpan(startIndex + 9, 8)), Z = BitConverter.ToUInt64(value.AsSpan(startIndex + 17, 8)) };
        //            startIndex += 25;
        //            break;
        //    }
        //    return re;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //private object DecodeValue(byte[] value)
        //{
        //    TagType bval = (TagType)(value[0]);
        //    switch (bval)
        //    {
        //        case TagType.Bool:
        //            return BitConverter.ToBoolean(value.AsSpan(1));
        //        case TagType.Byte:
        //            return (value as byte[])[1];
        //        case TagType.DateTime:
        //            return Convert.ToDateTime(BitConverter.ToInt64(value.AsSpan(1)));
        //        case TagType.Double:
        //            return BitConverter.ToDouble(value.AsSpan(1));
        //        case TagType.Float:
        //            return BitConverter.ToSingle(value.AsSpan(1));
        //        case TagType.Int:
        //            return BitConverter.ToInt32(value.AsSpan(1));
        //        case TagType.Long:
        //            return BitConverter.ToInt64(value.AsSpan(1));
        //        case TagType.Short:
        //            return BitConverter.ToInt16(value.AsSpan(1));
        //        case TagType.String:
        //            return Encoding.UTF8.GetString(value.AsSpan(1));
        //        case TagType.UInt:
        //            return BitConverter.ToUInt32(value.AsSpan(1));
        //        case TagType.ULong:
        //            return BitConverter.ToUInt64(value.AsSpan(1));
        //        case TagType.UShort:
        //            return BitConverter.ToUInt16(value.AsSpan(1));
        //        case TagType.IntPoint:
        //            return new IntPoint() { X = BitConverter.ToInt32(value.AsSpan(1, 4)), Y = BitConverter.ToInt32(value.AsSpan(5, 4)) };
        //        case TagType.IntPoint3:
        //            return new IntPoint3() { X = BitConverter.ToInt32(value.AsSpan(1, 4)), Y = BitConverter.ToInt32(value.AsSpan(5, 4)), Z = BitConverter.ToInt32(value.AsSpan(9, 4)) };
        //        case TagType.UIntPoint:
        //            return new UIntPoint() { X = BitConverter.ToUInt32(value.AsSpan(1, 4)), Y = BitConverter.ToUInt32(value.AsSpan(5, 4)) };
        //        case TagType.UIntPoint3:
        //            return new UIntPoint3() { X = BitConverter.ToUInt32(value.AsSpan(1, 4)), Y = BitConverter.ToUInt32(value.AsSpan(5, 4)), Z = BitConverter.ToUInt32(value.AsSpan(9, 4)) };
        //        case TagType.LongPoint:
        //            return new LongPoint() { X = BitConverter.ToInt64(value.AsSpan(1, 8)), Y = BitConverter.ToInt64(value.AsSpan(9, 8)) };
        //        case TagType.LongPoint3:
        //            return new LongPoint3() { X = BitConverter.ToInt64(value.AsSpan(1, 8)), Y = BitConverter.ToInt64(value.AsSpan(9, 8)), Z = BitConverter.ToInt64(value.AsSpan(17, 8)) };
        //        case TagType.ULongPoint:
        //            return new ULongPoint() { X = BitConverter.ToUInt64(value.AsSpan(1, 8)), Y = BitConverter.ToUInt64(value.AsSpan(9, 8)) };
        //        case TagType.ULongPoint3:
        //            return new ULongPoint3() { X = BitConverter.ToUInt64(value.AsSpan(1, 8)), Y = BitConverter.ToUInt64(value.AsSpan(9, 8)), Z = BitConverter.ToUInt64(value.AsSpan(17, 8)) };
        //    }
        //    return null;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        public override void WriteValue(string deviceInfo, object value, byte valueType)
        {
            if(mData.Model == WorkMode.Active || mComm.CommMode == CommMode.Duplex)
            {
                Dictionary<string, string> dtmp = new Dictionary<string, string>();
                dtmp.Add(deviceInfo, value.ToString());
                if (IsChannelRaw())
                {
                    var datas = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(new DataUpdate() { Fun = "update", Token = mLoginToken, Data = dtmp } ));
                    Send(datas);
                }
                else
                {
                    WriteValueNoWait(deviceInfo, Newtonsoft.Json.JsonConvert.SerializeObject(new DataUpdate() { Fun = "update", Token = mLoginToken, Data = dtmp }));
                }
            }
            else
            {
                if (!mSendCach.ContainsKey(deviceInfo))
                {
                    mSendCach.Add(deviceInfo, value);
                }
                else
                {
                    mSendCach[deviceInfo] = value;
                }
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="deviceInfo"></param>
        ///// <param name="value"></param>
        //public override void WriteValue(string deviceInfo, byte[] value,byte type)
        //{
        //    string sname = this.Device.Name;
            
        //    if (!string.IsNullOrEmpty(deviceInfo))
        //        sname += ("/" + deviceInfo);

        //    var bvals = ArrayPool<byte>.Shared.Rent(value.Length + 1);
        //    bvals[0] = type;
        //    value.CopyTo(bvals, 1);
        //    SendData(sname, bvals, 0, bvals.Length);
        //    ArrayPool<byte>.Shared.Return(bvals);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new SystemDriverData();
            mData.LoadFromXML(xe);
            base.Load(xe);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverRuntime NewApi()
        {
            return new SystemDriver();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

}
