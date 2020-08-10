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

namespace Cdy.Spider.MQTTClient
{
    /// <summary>
    /// 
    /// </summary>
    public class MQTTDriver: DriverRunnerBase
    {

        #region ... Variables  ...
        private MQTTDriverData mData;
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

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public override void Prepare()
        {
            var vv = this.mCachTags.Keys.Select(e => this.Name + "/" + e).ToList();
            vv.Add(this.Name);
            mComm.Prepare(vv);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override byte[] OnReceiveData(string key, byte[] data)
        {
            if(key == this.Name)
            {
                //处理多个数据标签
                var vdata = Encoding.UTF8.GetString(data);
                string[] ss = vdata.Split(new char[] { ';' });
                foreach(var vv in ss)
                {
                    string[] sss = vv.Split(new char[] { ',' });
                    UpdateValue(sss[0], sss[1]);
                }
            }
            else
            {
                string devicename = key.Replace(this.Name + "/", "");
                
                UpdateValue(devicename, DecodeValue(data));
            }

            return base.OnReceiveData(key, data);
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
                    return (value as byte[])[0];
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
            string sname = this.Name;
            
            if (!string.IsNullOrEmpty(deviceInfo))
                sname += ("/" + deviceInfo);
            byte[] bvals = new byte[value.Length + 1];
            bvals[0] = type;
            value.CopyTo(bvals, 1);
            SendData(sname, bvals);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new MQTTDriverData();
            mData.LoadFromXML(xe);
            base.Load(xe);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
