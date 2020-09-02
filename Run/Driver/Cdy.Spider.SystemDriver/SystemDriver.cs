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

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemDriver: TimerDriverRunner
    {

        #region ... Variables  ...

        private SystemDriverData mData;

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
            var vv = this.mCachTags.Keys.Select(e => this.Device.Name + "/" + e).ToList();
            vv.Add(this.Device.Name);
            mComm.Prepare(vv);
        }

        ///
        /// <returns></returns>
        protected override byte[] OnReceiveData(string key, byte[] data,out bool handled)
        {
            if(key == this.Device.Name)
            {
                //处理多个数据标签
                var vdata = Encoding.UTF8.GetString(data);
                string[] ss = vdata.Split(new char[] { ';' });
                foreach(var vv in ss)
                {
                    string[] sss = vv.Split(new char[] { ',' });
                    UpdateValue(sss[0], sss[1]);
                }
                handled = true;
                return null;
            }
            else if(key.StartsWith(this.Device.Name))
            {
                string devicename = key.Replace(this.Device.Name + "/", "");
                
                UpdateValue(devicename, DecodeValue(data));

                handled = true;
                return null;
            }
            return base.OnReceiveData(key, data, out handled);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessTimerElapsed()
        {
            int count = this.mCachTags.Count/100;
            count = this.mCachTags.Count % 100 > 0 ? count + 1 : count;

            for(int i=0;i<count;i++)
            {
                int icount = (i + 1) * 100;
                if(icount>this.mCachTags.Count)
                {
                    icount = this.mCachTags.Count - i * 100;
                }
                var vkeys = this.mCachTags.Keys.Skip(i * 100).Take(icount);
                SendGroupTags(vkeys);
                Thread.Sleep(10);
            }

            base.ProcessTimerElapsed();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tags"></param>
        private void SendGroupTags(IEnumerable<string> tags)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var vv in tags)
            {
                sb.Append(vv + ";");
            }
            sb.Length = sb.Length > 0 ? sb.Length - 1 : sb.Length;
            var res = SendData(this.Device.Name, Encoding.UTF8.GetBytes(sb.ToString()));
            if(res!=null && res.Length>0)
            {
                int idx = 0;
                foreach(var vv in tags)
                {
                    DecodeValue(res, ref idx);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private object DecodeValue(byte[] value,ref int startIndex)
        {
            TagType bval = (TagType)(value[startIndex]);
            object re = null;
            switch (bval)
            {
                case TagType.Bool:
                    re = BitConverter.ToBoolean(value.AsSpan(startIndex+1));
                    startIndex += 2;
                    break;
                case TagType.Byte:
                    re = (value as byte[])[startIndex + 1];
                    startIndex += 2;
                    break;
                case TagType.DateTime:
                    re = Convert.ToDateTime(BitConverter.ToInt64(value.AsSpan(startIndex + 1)));
                    startIndex += 9;
                    break;
                case TagType.Double:
                    re = BitConverter.ToDouble(value.AsSpan(startIndex + 1));
                    startIndex += 9;
                    break;
                case TagType.Float:
                    re = BitConverter.ToSingle(value.AsSpan(startIndex + 1));
                    startIndex += 5;
                    break;
                case TagType.Int:
                    re = BitConverter.ToInt32(value.AsSpan(startIndex + 1));
                    startIndex += 5;
                    break;
                case TagType.Long:
                    re = BitConverter.ToInt64(value.AsSpan(startIndex + 1));
                    startIndex += 9;
                    break;
                case TagType.Short:
                    re = BitConverter.ToInt16(value.AsSpan(startIndex + 1));
                    startIndex += 3;
                    break;
                case TagType.String:
                    var vsize = BitConverter.ToInt16(value.AsSpan(startIndex + 1));
                    re = Encoding.UTF8.GetString(value.AsSpan(startIndex + 3,vsize));
                    startIndex += (3 + vsize);
                    break;
                case TagType.UInt:
                    re = BitConverter.ToUInt32(value.AsSpan(startIndex + 1));
                    startIndex += 5;
                    break;
                case TagType.ULong:
                    re = BitConverter.ToUInt64(value.AsSpan(startIndex + 1));
                    startIndex += 9;
                    break;
                case TagType.UShort:
                    re = BitConverter.ToUInt16(value.AsSpan(startIndex + 1));
                    startIndex += 3;
                    break;
                case TagType.IntPoint:
                    re = new IntPoint() { X = BitConverter.ToInt32(value.AsSpan(startIndex + 1, 4)), Y = BitConverter.ToInt32(value.AsSpan(startIndex + 5, 4)) };
                    startIndex += 9;
                    break;
                case TagType.IntPoint3:
                    re = new IntPoint3() { X = BitConverter.ToInt32(value.AsSpan(startIndex + 1, 4)), Y = BitConverter.ToInt32(value.AsSpan(startIndex + 5, 4)), Z = BitConverter.ToInt32(value.AsSpan(startIndex + 9, 4)) };
                    startIndex += 13;
                    break;
                case TagType.UIntPoint:
                    re = new UIntPoint() { X = BitConverter.ToUInt32(value.AsSpan(startIndex + 1, 4)), Y = BitConverter.ToUInt32(value.AsSpan(startIndex + 5, 4)) };
                    startIndex += 9;
                    break;
                case TagType.UIntPoint3:
                    re = new UIntPoint3() { X = BitConverter.ToUInt32(value.AsSpan(startIndex + 1, 4)), Y = BitConverter.ToUInt32(value.AsSpan(startIndex + 5, 4)), Z = BitConverter.ToUInt32(value.AsSpan(startIndex + 9, 4)) };
                    startIndex += 13;
                    break;
                case TagType.LongPoint:
                    re = new LongPoint() { X = BitConverter.ToInt64(value.AsSpan(startIndex + 1, 8)), Y = BitConverter.ToInt64(value.AsSpan(startIndex + 9, 8)) };
                    startIndex += 17;
                    break;
                case TagType.LongPoint3:
                    re = new LongPoint3() { X = BitConverter.ToInt64(value.AsSpan(startIndex + 1, 8)), Y = BitConverter.ToInt64(value.AsSpan(startIndex + 9, 8)), Z = BitConverter.ToInt64(value.AsSpan(startIndex + 17, 8)) };
                    startIndex += 25;
                    break;
                case TagType.ULongPoint:
                    re = new ULongPoint() { X = BitConverter.ToUInt64(value.AsSpan(startIndex + 1, 8)), Y = BitConverter.ToUInt64(value.AsSpan(startIndex + 9, 8)) };
                    startIndex += 17;
                    break;
                case TagType.ULongPoint3:
                    re = new ULongPoint3() { X = BitConverter.ToUInt64(value.AsSpan(startIndex + 1, 8)), Y = BitConverter.ToUInt64(value.AsSpan(startIndex + 9, 8)), Z = BitConverter.ToUInt64(value.AsSpan(startIndex + 17, 8)) };
                    startIndex += 25;
                    break;
            }
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private object DecodeValue(byte[] value)
        {
            TagType bval = (TagType)(value[0]);
            switch (bval)
            {
                case TagType.Bool:
                    return BitConverter.ToBoolean(value.AsSpan(1));
                case TagType.Byte:
                    return (value as byte[])[1];
                case TagType.DateTime:
                    return Convert.ToDateTime(BitConverter.ToInt64(value.AsSpan(1)));
                case TagType.Double:
                    return BitConverter.ToDouble(value.AsSpan(1));
                case TagType.Float:
                    return BitConverter.ToSingle(value.AsSpan(1));
                case TagType.Int:
                    return BitConverter.ToInt32(value.AsSpan(1));
                case TagType.Long:
                    return BitConverter.ToInt64(value.AsSpan(1));
                case TagType.Short:
                    return BitConverter.ToInt16(value.AsSpan(1));
                case TagType.String:
                    return Encoding.UTF8.GetString(value.AsSpan(1));
                case TagType.UInt:
                    return BitConverter.ToUInt32(value.AsSpan(1));
                case TagType.ULong:
                    return BitConverter.ToUInt64(value.AsSpan(1));
                case TagType.UShort:
                    return BitConverter.ToUInt16(value.AsSpan(1));
                case TagType.IntPoint:
                    return new IntPoint() { X = BitConverter.ToInt32(value.AsSpan(1, 4)), Y = BitConverter.ToInt32(value.AsSpan(5, 4)) };
                case TagType.IntPoint3:
                    return new IntPoint3() { X = BitConverter.ToInt32(value.AsSpan(1, 4)), Y = BitConverter.ToInt32(value.AsSpan(5, 4)), Z = BitConverter.ToInt32(value.AsSpan(9, 4)) };
                case TagType.UIntPoint:
                    return new UIntPoint() { X = BitConverter.ToUInt32(value.AsSpan(1, 4)), Y = BitConverter.ToUInt32(value.AsSpan(5, 4)) };
                case TagType.UIntPoint3:
                    return new UIntPoint3() { X = BitConverter.ToUInt32(value.AsSpan(1, 4)), Y = BitConverter.ToUInt32(value.AsSpan(5, 4)), Z = BitConverter.ToUInt32(value.AsSpan(9, 4)) };
                case TagType.LongPoint:
                    return new LongPoint() { X = BitConverter.ToInt64(value.AsSpan(1, 8)), Y = BitConverter.ToInt64(value.AsSpan(9, 8)) };
                case TagType.LongPoint3:
                    return new LongPoint3() { X = BitConverter.ToInt64(value.AsSpan(1, 8)), Y = BitConverter.ToInt64(value.AsSpan(9, 8)), Z = BitConverter.ToInt64(value.AsSpan(17, 8)) };
                case TagType.ULongPoint:
                    return new ULongPoint() { X = BitConverter.ToUInt64(value.AsSpan(1, 8)), Y = BitConverter.ToUInt64(value.AsSpan(9, 8)) };
                case TagType.ULongPoint3:
                    return new ULongPoint3() { X = BitConverter.ToUInt64(value.AsSpan(1, 8)), Y = BitConverter.ToUInt64(value.AsSpan(9, 8)), Z = BitConverter.ToUInt64(value.AsSpan(17, 8)) };
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="value"></param>
        public override void WriteValue(string deviceInfo, byte[] value,byte type)
        {
            string sname = this.Device.Name;
            
            if (!string.IsNullOrEmpty(deviceInfo))
                sname += ("/" + deviceInfo);

            var bvals = ArrayPool<byte>.Shared.Rent(value.Length + 1);
            bvals[0] = type;
            value.CopyTo(bvals, 1);
            SendData(sname, bvals);
            ArrayPool<byte>.Shared.Return(bvals);
        }

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
