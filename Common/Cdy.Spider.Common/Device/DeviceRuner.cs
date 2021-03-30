//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/6 11:33:53.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public class DeviceRunner : IDeviceRuntime, IDeviceForDriver
    {

        #region ... Variables  ...

        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, Tagbase> mDatabaseMapTags = new Dictionary<string, Tagbase>();

        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, List<Tagbase>> mDeviceMapTags = new Dictionary<string, List<Tagbase>>();

        /// <summary>
        /// 
        /// </summary>
        SortedDictionary<int, Tagbase> mIdMapTags = new SortedDictionary<int, Tagbase>();

        /// <summary>
        /// 
        /// </summary>
        private Action<string, Tagbase> mValueCallBack;

        /// <summary>
        /// 
        /// </summary>
        private Action<Tagbase, IEnumerable<HisValue>> mHisValuesCallback;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public DeviceData Device { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name => Device.Name;

        /// <summary>
        /// 
        /// </summary>
        public IDriverRuntime Driver { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICommChannel Channel { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Device?.Dispose();
            Device = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            mIdMapTags = Device.Tags;
            foreach (var vv in Device.Tags)
            {
                string dbname = vv.Value.DatabaseName;
                string dvname = vv.Value.DeviceInfo;
                if (!mDatabaseMapTags.ContainsKey(dbname))
                {
                    mDatabaseMapTags.Add(dbname, vv.Value);
                }

                if (mDeviceMapTags.ContainsKey(dvname))
                {
                    mDeviceMapTags[dvname].Add(vv.Value);
                }
                else
                {
                    List<Tagbase> ll = new List<Tagbase>() { vv.Value };
                    mDeviceMapTags.Add(dvname, ll);
                }
            }

            Channel = ServiceLocator.Locator.Resolve<ICommChannelRuntimeManager>().GetChannel(Device.ChannelName);
            Driver = ServiceLocator.Locator.Resolve<IDriverRuntimeManager>().GetDriver(Device.Name);
            if (Driver != null)
            {
                Driver.Device = this;
                Driver.Init();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object ReadValue(string name)
        {
            return mDatabaseMapTags.ContainsKey(name) ? mDatabaseMapTags[name].Value : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object ReadValue(int id)
        {
            return mIdMapTags.ContainsKey(id) ? mIdMapTags[id].Value : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseTag"></param>
        /// <returns></returns>
        public object ReadValueByDatabaseName(string databaseTag)
        {
            return mDatabaseMapTags.ContainsKey(databaseTag) ? mDatabaseMapTags[databaseTag].Value : null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Driver.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            Driver.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseTag"></param>
        /// <param name="value"></param>
        public void WriteValueByDatabaseName(string databaseTag, object value)
        {
            if (mDatabaseMapTags.ContainsKey(databaseTag))
            {
                var vtag = mDatabaseMapTags[databaseTag];
                if (vtag != null && !string.IsNullOrEmpty(vtag.DeviceInfo))
                    Driver.WriteValue(vtag.DeviceInfo, ConvertToBytes(vtag, value), (byte)(vtag.Type));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private byte[] ConvertToBytes(Tagbase tag, object value)
        {
            switch (tag.Type)
            {
                case TagType.Bool:
                    return BitConverter.GetBytes((bool)value);
                case TagType.Byte:
                    return BitConverter.GetBytes(Convert.ToByte(value));
                case TagType.DateTime:
                    return BitConverter.GetBytes(Convert.ToInt64(value));
                case TagType.Double:
                    return BitConverter.GetBytes(Convert.ToDouble(value));
                case TagType.Float:
                    return BitConverter.GetBytes(Convert.ToSingle(value));
                case TagType.Int:
                    return BitConverter.GetBytes(Convert.ToInt32(value));
                case TagType.Long:
                    return BitConverter.GetBytes(Convert.ToInt64(value));
                case TagType.Short:
                    return BitConverter.GetBytes(Convert.ToInt16(value));
                case TagType.String:
                    return Encoding.UTF8.GetBytes(value.ToString());
                case TagType.UInt:
                    return BitConverter.GetBytes(Convert.ToUInt32(value));
                case TagType.ULong:
                    return BitConverter.GetBytes(Convert.ToUInt64(value));
                case TagType.UShort:
                    return BitConverter.GetBytes(Convert.ToUInt16(value));
                case TagType.IntPoint:
                    IntPoint ival = (IntPoint)value;
                    byte[] val = new byte[8];
                    BitConverter.GetBytes(ival.X).CopyTo(val, 0);
                    BitConverter.GetBytes(ival.Y).CopyTo(val, 4);
                    return val;
                case TagType.IntPoint3:
                    IntPoint3 ival3 = (IntPoint3)value;
                    val = new byte[12];
                    BitConverter.GetBytes(ival3.X).CopyTo(val, 0);
                    BitConverter.GetBytes(ival3.Y).CopyTo(val, 4);
                    BitConverter.GetBytes(ival3.Z).CopyTo(val, 8);
                    return val;
                case TagType.UIntPoint3:
                    UIntPoint3 uival3 = (UIntPoint3)value;
                    val = new byte[12];
                    BitConverter.GetBytes(uival3.X).CopyTo(val, 0);
                    BitConverter.GetBytes(uival3.Y).CopyTo(val, 4);
                    BitConverter.GetBytes(uival3.Z).CopyTo(val, 8);
                    return val;
                case TagType.UIntPoint:
                    UIntPoint uival = (UIntPoint)value;
                    val = new byte[8];
                    BitConverter.GetBytes(uival.X).CopyTo(val, 0);
                    BitConverter.GetBytes(uival.Y).CopyTo(val, 4);
                    return val;
                case TagType.LongPoint:
                    LongPoint lval = (LongPoint)value;
                    val = new byte[16];
                    BitConverter.GetBytes(lval.X).CopyTo(val, 0);
                    BitConverter.GetBytes(lval.Y).CopyTo(val, 8);
                    return val;
                case TagType.LongPoint3:
                    LongPoint3 lval3 = (LongPoint3)value;
                    val = new byte[24];
                    BitConverter.GetBytes(lval3.X).CopyTo(val, 0);
                    BitConverter.GetBytes(lval3.Y).CopyTo(val, 8);
                    BitConverter.GetBytes(lval3.Z).CopyTo(val, 16);
                    return val;
                case TagType.ULongPoint:
                    ULongPoint ulval = (ULongPoint)value;
                    val = new byte[16];
                    BitConverter.GetBytes(ulval.X).CopyTo(val, 0);
                    BitConverter.GetBytes(ulval.Y).CopyTo(val, 8);
                    return val;
                case TagType.ULongPoint3:
                    ULongPoint3 ulval3 = (ULongPoint3)value;
                    val = new byte[24];
                    BitConverter.GetBytes(ulval3.X).CopyTo(val, 0);
                    BitConverter.GetBytes(ulval3.Y).CopyTo(val, 8);
                    BitConverter.GetBytes(ulval3.Z).CopyTo(val, 16);
                    return val;
            }
            return null;
        }

        #region IDeviceForDriver

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Tagbase> ListTags()
        {
            return mIdMapTags.Values.ToList();
        }



        /// <summary>
        /// 
        /// </summary>
        public void UpdateAllTagQualityToCommBad()
        {
            DateTime dtmp = DateTime.Now;
            foreach (var vv in mIdMapTags.Values)
            {
                vv.Quality = Tagbase.BadCommQuality;
                vv.Time = dtmp;
                mValueCallBack?.Invoke(this.Name, vv);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTag"></param>
        /// <param name="value"></param>
        public void UpdateDeviceValue(string deviceTag, object value)
        {
            if (mDeviceMapTags.ContainsKey(deviceTag))
            {
                DateTime dtmp = DateTime.Now;
                foreach (var vv in mDeviceMapTags[deviceTag])
                {
                    vv.Value = ConvertValue(vv, value);
                    vv.Time = dtmp;
                    vv.Quality = Tagbase.GoodQuality;

                    mValueCallBack?.Invoke(this.Name, vv);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private object ConvertValue(Tagbase tag, object value)
        {
            if (value == null) return null;
            switch (tag.Type)
            {
                case TagType.Bool:
                    if (value is string)
                    {
                        return bool.Parse(value.ToString());
                    }
                    else if (value is byte[])
                    {
                        return BitConverter.ToBoolean(value as byte[]);
                    }
                    else
                    {
                        return Convert.ToBoolean(value);
                    }
                case TagType.Byte:
                    if (value is string)
                    {
                        return byte.Parse(value.ToString());
                    }
                    else if (value is byte[])
                    {
                        return (value as byte[])[0];
                    }
                    else
                    {
                        return Convert.ToByte(value);
                    }
                case TagType.DateTime:
                    if (value is string)
                    {
                        return DateTime.Parse(value.ToString());
                    }
                    else if (value is byte[])
                    {
                        return Convert.ToDateTime(BitConverter.ToInt64(value as byte[]));
                    }
                    else
                    {
                        return Convert.ToDateTime(value);
                    }
                case TagType.Double:
                    if (value is string)
                    {
                        return double.Parse(value.ToString());
                    }
                    else if (value is byte[])
                    {
                        return BitConverter.ToDouble(value as byte[]);
                    }
                    else
                    {
                        return Convert.ToBoolean(value);
                    }
                case TagType.Float:
                    if (value is string)
                    {
                        return float.Parse(value.ToString());
                    }
                    else if (value is byte[])
                    {
                        return BitConverter.ToSingle(value as byte[]);
                    }
                    else
                    {
                        return Convert.ToSingle(value);
                    }
                case TagType.Int:
                    if (value is string)
                    {
                        return int.Parse(value.ToString());
                    }
                    else if (value is byte[])
                    {
                        return BitConverter.ToInt32(value as byte[]);
                    }
                    else
                    {
                        return Convert.ToInt32(value);
                    }
                case TagType.Long:
                    if (value is string)
                    {
                        return long.Parse(value.ToString());
                    }
                    else if (value is byte[])
                    {
                        return BitConverter.ToInt64(value as byte[]);
                    }
                    else
                    {
                        return Convert.ToInt64(value);
                    }
                case TagType.Short:
                    if (value is string)
                    {
                        return short.Parse(value.ToString());
                    }
                    else if (value is byte[])
                    {
                        return BitConverter.ToInt16(value as byte[]);
                    }
                    else
                    {
                        return Convert.ToInt16(value);
                    }
                case TagType.String:
                    if (value is byte[])
                    {
                        return Encoding.UTF8.GetString(value as byte[]);
                    }
                    else
                    {
                        return value.ToString();
                    }

                case TagType.UInt:
                    if (value is string)
                    {
                        return uint.Parse(value.ToString());
                    }
                    else if (value is byte[])
                    {
                        return BitConverter.ToUInt32(value as byte[]);
                    }
                    else
                    {
                        return Convert.ToUInt32(value);
                    }
                case TagType.ULong:
                    if (value is string)
                    {
                        return ulong.Parse(value.ToString());
                    }
                    else if (value is byte[])
                    {
                        return BitConverter.ToUInt64(value as byte[]);
                    }
                    else
                    {
                        return Convert.ToUInt64(value);
                    }
                case TagType.UShort:
                    if (value is string)
                    {
                        return ushort.Parse(value.ToString());
                    }
                    else if (value is byte[])
                    {
                        return BitConverter.ToUInt16(value as byte[]);
                    }
                    else
                    {
                        return Convert.ToUInt16(value);
                    }
                case TagType.IntPoint:
                    if (value is string)
                    {
                        string[] sval = value.ToString().Split(new char[] { ',' });
                        return new IntPoint() { X = int.Parse(sval[0]), Y = int.Parse(sval[1]) };
                    }
                    else if (value is byte[])
                    {
                        byte[] bval = value as byte[];
                        return new IntPoint() { X = BitConverter.ToInt32(bval.AsSpan(0, 4)), Y = BitConverter.ToInt32(bval.AsSpan(4, 4)) };
                    }
                    else
                    {
                        return (IntPoint)value;
                    }
                case TagType.IntPoint3:
                    if (value is string)
                    {
                        string[] sval = value.ToString().Split(new char[] { ',' });
                        return new IntPoint3() { X = int.Parse(sval[0]), Y = int.Parse(sval[1]), Z = int.Parse(sval[2]) };
                    }
                    else if (value is byte[])
                    {
                        byte[] bval = value as byte[];
                        return new IntPoint3() { X = BitConverter.ToInt32(bval.AsSpan(0, 4)), Y = BitConverter.ToInt32(bval.AsSpan(4, 4)), Z = BitConverter.ToInt32(bval.AsSpan(8, 4)) };
                    }
                    else
                    {
                        return (IntPoint3)value;
                    }
                case TagType.UIntPoint:
                    if (value is string)
                    {
                        string[] sval = value.ToString().Split(new char[] { ',' });
                        return new UIntPoint() { X = uint.Parse(sval[0]), Y = uint.Parse(sval[1]) };
                    }
                    else if (value is byte[])
                    {
                        byte[] bval = value as byte[];
                        return new UIntPoint() { X = BitConverter.ToUInt32(bval.AsSpan(0, 4)), Y = BitConverter.ToUInt32(bval.AsSpan(4, 4)) };
                    }
                    else
                    {
                        return (UIntPoint)value;
                    }
                case TagType.UIntPoint3:
                    if (value is string)
                    {
                        string[] sval = value.ToString().Split(new char[] { ',' });
                        return new UIntPoint3() { X = uint.Parse(sval[0]), Y = uint.Parse(sval[1]), Z = uint.Parse(sval[2]) };
                    }
                    else if (value is byte[])
                    {
                        byte[] bval = value as byte[];
                        return new UIntPoint3() { X = BitConverter.ToUInt32(bval.AsSpan(0, 4)), Y = BitConverter.ToUInt32(bval.AsSpan(4, 4)), Z = BitConverter.ToUInt32(bval.AsSpan(8, 4)) };
                    }
                    else
                    {
                        return (UIntPoint3)value;
                    }
                case TagType.LongPoint:
                    if (value is string)
                    {
                        string[] sval = value.ToString().Split(new char[] { ',' });
                        return new LongPoint() { X = long.Parse(sval[0]), Y = long.Parse(sval[1]) };
                    }
                    else if (value is byte[])
                    {
                        byte[] bval = value as byte[];
                        return new LongPoint() { X = BitConverter.ToInt64(bval.AsSpan(0, 8)), Y = BitConverter.ToInt64(bval.AsSpan(8, 8)) };
                    }
                    else
                    {
                        return (LongPoint)value;
                    }
                case TagType.LongPoint3:
                    if (value is string)
                    {
                        string[] sval = value.ToString().Split(new char[] { ',' });
                        return new LongPoint3() { X = long.Parse(sval[0]), Y = long.Parse(sval[1]), Z = long.Parse(sval[2]) };
                    }
                    else if (value is byte[])
                    {
                        byte[] bval = value as byte[];
                        return new LongPoint3() { X = BitConverter.ToInt64(bval.AsSpan(0, 8)), Y = BitConverter.ToInt64(bval.AsSpan(8, 8)), Z = BitConverter.ToInt64(bval.AsSpan(16, 8)) };
                    }
                    else
                    {
                        return (LongPoint3)value;
                    }
                case TagType.ULongPoint:
                    if (value is string)
                    {
                        string[] sval = value.ToString().Split(new char[] { ',' });
                        return new ULongPoint() { X = ulong.Parse(sval[0]), Y = ulong.Parse(sval[1]) };
                    }
                    else if (value is byte[])
                    {
                        byte[] bval = value as byte[];
                        return new ULongPoint() { X = BitConverter.ToUInt64(bval.AsSpan(0, 8)), Y = BitConverter.ToUInt64(bval.AsSpan(8, 8)) };
                    }
                    else
                    {
                        return (ULongPoint)value;
                    }
                case TagType.ULongPoint3:
                    if (value is string)
                    {
                        string[] sval = value.ToString().Split(new char[] { ',' });
                        return new ULongPoint3() { X = ulong.Parse(sval[0]), Y = ulong.Parse(sval[1]), Z = ulong.Parse(sval[2]) };
                    }
                    else if (value is byte[])
                    {
                        byte[] bval = value as byte[];
                        return new ULongPoint3() { X = BitConverter.ToUInt64(bval.AsSpan(0, 8)), Y = BitConverter.ToUInt64(bval.AsSpan(8, 8)), Z = BitConverter.ToUInt64(bval.AsSpan(16, 8)) };
                    }
                    else
                    {
                        return (ULongPoint3)value;
                    }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void UpdateDeviceValue(List<int> id, object value)
        {
            DateTime dtmp = DateTime.Now;
            foreach (var vv in id)
            {
                if (mIdMapTags.ContainsKey(vv))
                {
                    var vvv = mIdMapTags[vv];
                    vvv.Value = value;
                    vvv.Quality = Tagbase.GoodQuality;
                    vvv.Time = dtmp;

                    mValueCallBack?.Invoke(this.Name, vvv);
                }
            }
        }

        /// <summary>
        /// 更新某个变量的历史
        /// </summary>
        /// <param name="id"></param>
        /// <param name="values"></param>
        public void UpdateTagHisValue(int id, IEnumerable<HisValue> values)
        {
            if (mIdMapTags.ContainsKey(id))
            {
                mHisValuesCallback?.Invoke(mIdMapTags[id], values);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTag"></param>
        /// <param name="values"></param>
        public void UpdateTagHisValueByDeviceTag(string deviceTag, IEnumerable<HisValue> values)
        {
            if (mDeviceMapTags.ContainsKey(deviceTag))
            {
                DateTime dtmp = DateTime.Now;
                foreach (var vv in mDeviceMapTags[deviceTag])
                {
                    mHisValuesCallback?.Invoke(vv, values);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> ListDatabaseNames()
        {
            return mDatabaseMapTags.Keys.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callBack"></param>
        public void RegistorCallBack(Action<string, Tagbase> callBack)
        {
            mValueCallBack = callBack;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICommChannel GetCommChannel()
        {
            return Channel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hisValues"></param>
        public void RegistorHisValueCallBack(Action<Tagbase, IEnumerable<HisValue>> hisValues)
        {
            mHisValuesCallback = hisValues;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Tagbase GetTag(string name)
        {
            return mDatabaseMapTags.ContainsKey(name) ? mDatabaseMapTags[name] : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Tagbase> ListCacheHistoryTags()
        {
            return mIdMapTags.Where(e => e.Value.IsBufferEnabled).Select(e => e.Value).ToList();
        }

        public Tagbase GetTag(int id)
        {
            if (mIdMapTags.ContainsKey(id))
            {
                return mIdMapTags[id];
            }
            return null;
        }


        #endregion

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
