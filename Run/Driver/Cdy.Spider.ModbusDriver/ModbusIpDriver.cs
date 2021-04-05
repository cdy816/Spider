﻿using Modbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public class ModbusIpDriver:TimerDriverRunner
    {

        #region ... Variables  ...

        private ModbusIpDriverData mData;

        Modbus.Device.ModbusIpMaster mMaster;

        private SortedDictionary<ushort, List<int>> mCoilStatusTags = new SortedDictionary<ushort, List<int>>();

        private Dictionary<ushort,ushort> mCoilStatusPackage = new Dictionary<ushort, ushort>();

        /// <summary>
        /// 
        /// </summary>
        private SortedDictionary<ushort, List<int>> mInputStatusTags = new SortedDictionary<ushort, List<int>>();

        private Dictionary<ushort, ushort> mInputStatusPackage = new Dictionary<ushort, ushort>();

        /// <summary>
        /// 
        /// </summary>
        private SortedDictionary<ushort,Tuple<ushort,List<int>>> mInputRegistorTags = new SortedDictionary<ushort, Tuple<ushort, List<int>>>();

        private Dictionary<ushort, ushort> mInputRegistorPackage = new Dictionary<ushort, ushort>();


        /// <summary>
        /// 
        /// </summary>
        private SortedDictionary<ushort, Tuple<ushort, List<int>>> mHoldRegistorTags = new SortedDictionary<ushort, Tuple<ushort, List<int>>>();

        private Dictionary<ushort, ushort> mHoldtRegistorPackage = new Dictionary<ushort, ushort>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "ModbusTcpMasterDriver";

        /// <summary>
        /// 
        /// </summary>
        public override DriverData Data => mData;

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            base.Init();

            int clen=-1,cstart=-1, ilen=-1,istart=-1, irlen=-1,irstart=-1, hrlen=-1,hrstart=-1;

            foreach(var vv in mCachTags)
            {
                var dtmp = vv.Key.ToLower().Split(new char[] { ':' });
                ushort addr = ushort.Parse(dtmp[1]);

                if (dtmp[0] ==("cs"))
                {
                    //Coil status
                    if(cstart == -1)
                    {
                        cstart = addr;
                        clen = 1;
                    }
                    else
                    {
                        if((addr - cstart + 1)>mData.PackageLen)
                        {
                            mCoilStatusPackage.Add((ushort)cstart, (ushort)clen);
                            cstart = -1;
                        }
                        else
                        {
                            clen = addr - cstart + 1;
                        }
                    }
                    if (!mCoilStatusTags.ContainsKey(addr))
                    {
                        mCoilStatusTags.Add(addr,vv.Value);
                    }
                }
                else if (dtmp[0] == ("is"))
                {
                    //Input status
                    if (istart == -1)
                    {
                        istart = addr;
                        ilen = 1;
                    }
                    else
                    {
                        if ((addr - istart + 1) > mData.PackageLen)
                        {
                            mInputStatusPackage.Add((ushort)istart, (ushort)ilen);
                            istart = -1;
                        }
                        else
                        {
                            ilen = addr - istart + 1;
                        }
                    }
                    if (!mInputStatusTags.ContainsKey(addr))
                        mInputStatusTags.Add(addr, vv.Value);

                }
                else if(dtmp[0] == ("ir"))
                {
                    ushort len = ushort.Parse(dtmp[2]);
                    //Input registor

                    if (irstart == -1)
                    {
                        irstart = addr;
                        irlen = len;
                    }
                    else
                    {
                        if ((addr - irstart + len) > mData.PackageLen)
                        {
                            mInputRegistorPackage.Add((ushort)irstart, (ushort)irlen);
                            irstart = -1;
                        }
                        else
                        {
                            irlen = addr - irstart + len;
                        }
                    }
                    mInputRegistorTags.Add(addr,new Tuple<ushort, List<int>>(len,vv.Value));

                }
                else if(dtmp[0] == ("hr"))
                {
                    //holding registor
                    ushort len = ushort.Parse(dtmp[2]);

                    if (hrstart == -1)
                    {
                        hrstart = addr;
                        hrlen = len;
                    }
                    else
                    {
                        if ((addr - hrstart + len) > mData.PackageLen)
                        {
                            mHoldtRegistorPackage.Add((ushort)hrstart, (ushort)hrlen);
                            hrstart = -1;
                        }
                        else
                        {
                            hrlen = addr - hrstart + len;
                        }
                    }
                    mHoldRegistorTags.Add(addr, new Tuple<ushort, List<int>>(len, vv.Value));
                }
            }

            if(cstart!=-1)
            {
                mCoilStatusPackage.Add((ushort)cstart, (ushort)clen);
            }

            if (istart != -1)
            {
                mInputStatusPackage.Add((ushort)istart, (ushort)ilen);
            }

            if (irstart != -1)
            {
                mInputRegistorPackage.Add((ushort)irstart, (ushort)irlen);
            }

            if (hrstart != -1)
            {
                mHoldtRegistorPackage.Add((ushort)hrstart, (ushort)hrlen);
            }

            mComm.EnableTransparentRead(true);
            mMaster = Modbus.Device.ModbusIpMaster.CreateIp(mComm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        public override void WriteValue(string deviceInfo, object value, byte valueType)
        {
            if (mComm.IsConnected)
            {
                ushort addr = ushort.Parse(deviceInfo.Substring(3));
                if (deviceInfo.StartsWith("cs:"))
                {
                    mMaster.WriteSingleCoil((byte)mData.Id,addr, Convert.ToBoolean(value));
                }
                else if (deviceInfo.StartsWith("hr:"))
                {
                    mMaster.WriteMultipleRegisters((byte)mData.Id, addr, GetValue(value, valueType));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        private ushort[] GetValue(object value,byte valueType)
        {
            TagType tp = (TagType) valueType;
            switch (tp)
            {
                case TagType.Bool:
                case TagType.Byte:
                    return new ushort[] { Convert.ToByte(value) };
                case TagType.Short:
                case TagType.UShort:
                    return new ushort[] { Convert.ToUInt16(value) };
                case TagType.Int:
                case TagType.UInt:
                    return IntToByte(Convert.ToInt32(value));
                case TagType.Long:
                case TagType.ULong:
                    return LongToByte(Convert.ToInt64(value));
                case TagType.DateTime:
                    return LongToByte(Convert.ToDateTime(value).ToBinary());
                case TagType.Double:
                    return DoubleToByte(Convert.ToDouble(value));
                case TagType.Float:
                    return FloatToByte(Convert.ToSingle(value));
                case TagType.String:
                    if (mData.StringEncoding == StringEncoding.Ascii)
                    {
                        return BytesToHostUInt16(Encoding.ASCII.GetBytes(value.ToString()));
                    }
                    else if(mData.StringEncoding == StringEncoding.Utf8)
                    {
                        return BytesToHostUInt16(Encoding.UTF8.GetBytes(value.ToString()));
                    }
                    else
                    {
                        return BytesToHostUInt16(Encoding.Unicode.GetBytes(value.ToString()));
                    }
            }

            return null;
        }

        private ushort[] IntToByte(int value)
        {
            var btmp = BitConverter.GetBytes(value);
            return ConvertByteToUnshort(btmp, mData.IntFormate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private ushort[] FloatToByte(float value)
        {
            var btmp = BitConverter.GetBytes(value);
            return ConvertByteToUnshort(btmp, mData.FloatFormate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private ushort[] DoubleToByte(double value)
        {
            var btmp = BitConverter.GetBytes(value);
            return ConvertByteToUnshort(btmp, mData.DoubleFormate);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="btmp"></param>
        /// <param name="formate"></param>
        /// <returns></returns>
        private ushort[] ConvertByteToUnshort(byte[] btmp, FourValueFormate formate)
        {
            if (formate == FourValueFormate.D12)
            {
                return BytesToHostUInt16(btmp);
            }
            else
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(4))
                {
                    ms.Write(btmp, 2, 2);
                    ms.Write(btmp, 0, 2);

                    return BytesToHostUInt16(ms.ToArray());
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="btmp"></param>
        /// <param name="formate"></param>
        /// <returns></returns>
        private ushort[] ConvertByteToUnshort(byte[] btmp, EightValueFormate formate)
        {
            switch (mData.LongFormate)
            {
                case EightValueFormate.D1234:
                    return BytesToHostUInt16(btmp);
                case EightValueFormate.D4321:
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(8))
                    {
                        ms.Write(btmp, 6, 2);
                        ms.Write(btmp, 4, 2);
                        ms.Write(btmp, 2, 2);
                        ms.Write(btmp, 0, 2);

                        return BytesToHostUInt16(ms.ToArray());
                    }
                case EightValueFormate.D2143:
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(8))
                    {
                        ms.Write(btmp, 2, 2);
                        ms.Write(btmp, 0, 2);
                        ms.Write(btmp, 6, 2);
                        ms.Write(btmp, 4, 2);
                        return BytesToHostUInt16(ms.ToArray());
                    }
                case EightValueFormate.D3412:
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(8))
                    {
                        ms.Write(btmp, 4, 2);
                        ms.Write(btmp, 6, 2);
                        ms.Write(btmp, 0, 2);
                        ms.Write(btmp, 2, 2);
                        return BytesToHostUInt16(ms.ToArray());
                    }
            }
            return BytesToHostUInt16(btmp);
        }

        private ushort[] LongToByte(long value)
        {
            var btmp = BitConverter.GetBytes(value);
            return ConvertByteToUnshort(btmp, mData.LongFormate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="networkBytes"></param>
        /// <returns></returns>
        public static ushort[] BytesToHostUInt16(byte[] networkBytes)
        {
            if (networkBytes == null)
            {
                throw new ArgumentNullException(nameof(networkBytes));
            }

            if (networkBytes.Length % 2 != 0)
            {
                throw new FormatException(Resources.NetworkBytesNotEven);
            }

            ushort[] result = new ushort[networkBytes.Length / 2];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (ushort)BitConverter.ToInt16(networkBytes, i * 2);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        protected override void OnCommChanged(bool result)
        {
            base.OnCommChanged(result);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessTimerElapsed()
        {
            if (mComm.IsConnected)
            {
                ProcessRead();
            }
            base.ProcessTimerElapsed();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessRead()
        {
            mComm.Take();
            try
            {
                foreach (var vv in mInputStatusPackage)
                {
                    var result = mMaster.ReadInputs((byte)mData.Id, vv.Key, vv.Value);
                    if (result != null && result.Length == vv.Value)
                    {
                        for (ushort i = 0; i < vv.Value; i++)
                        {
                            ushort addr = (ushort)(vv.Key + i);
                            if (this.mInputStatusTags.ContainsKey(addr))
                            {
                                UpdateValue(mInputStatusTags[addr], result[i]);
                            }
                        }
                    }
                }

                foreach (var vv in mCoilStatusPackage)
                {
                    var result = mMaster.ReadCoils((byte)mData.Id, vv.Key, vv.Value);

                    if (result != null && result.Length == vv.Value)
                    {
                        for (ushort i = 0; i < vv.Value; i++)
                        {
                            ushort addr = (ushort)(vv.Key + i);
                            if (this.mCoilStatusTags.ContainsKey(addr))
                            {
                                UpdateValue(mCoilStatusTags[addr], result[i]);
                            }
                        }
                    }
                }

                foreach (var vv in mInputRegistorPackage)
                {
                    var result = mMaster.ReadInputRegisters((byte)mData.Id, vv.Key, vv.Value);
                    if (result != null && result.Length == vv.Value)
                    {
                        for (ushort i = 0; i < vv.Value; i++)
                        {
                            ushort addr = (ushort)(vv.Key + i);
                            UpdateRegistor(addr, result, mInputRegistorTags);
                        }
                    }
                }

                foreach (var vv in mHoldtRegistorPackage)
                {
                    var result = mMaster.ReadHoldingRegisters((byte)mData.Id, vv.Key, vv.Value);
                    if (result != null && result.Length == vv.Value)
                    {
                        for (ushort i = 0; i < vv.Value; i++)
                        {
                            ushort addr = (ushort)(vv.Key + i);
                            UpdateRegistor(addr, result, mHoldRegistorTags);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                LoggerService.Service.Warn("Modbus ip Driver", ex.Message);
            }
            mComm.Release();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="result"></param>
        /// <param name="mtagcach"></param>
        private void UpdateRegistor(ushort addr,ushort[] result,SortedDictionary<ushort,Tuple<ushort,List<int>>> mtagcach)
        {
            if (mtagcach.ContainsKey(addr))
            {
                var vdd = mtagcach[addr];
                var res = result.AsSpan<ushort>(addr, vdd.Item1);

                var tp = Device.GetTag(vdd.Item2[0]).Type;

                switch (tp)
                {
                    case TagType.Bool:
                    case TagType.Byte:
                        UpdateValue(vdd.Item2, res[0]);
                        break;
                    case TagType.Short:
                    case TagType.UShort:
                        UpdateValue(vdd.Item2, res[0]);
                        break;
                    case TagType.Int:
                    case TagType.UInt:
                        UpdateValue(vdd.Item2, ToUInt(res));
                        break;
                    case TagType.Long:
                    case TagType.ULong:
                        UpdateValue(vdd.Item2, ToLong(res));
                        break;
                    case TagType.Double:
                        UpdateValue(vdd.Item2, ToDouble(res));
                        break;
                    case TagType.Float:
                        UpdateValue(vdd.Item2, ToFloat(res));
                        break;
                    case TagType.String:
                        UpdateValue(vdd.Item2, ToStringValue(res));
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public string ToStringValue(Span<ushort> datas)
        {
            IEnumerable<byte> bvals=null;
            foreach(var vv in datas)
            {
                if (bvals == null)
                {
                    bvals = BitConverter.GetBytes(vv);
                }
                else
                {
                    bvals = bvals.Concat(BitConverter.GetBytes(vv));
                }
            }

            if(bvals!=null)
            {
                switch (mData.StringEncoding)
                {
                    case StringEncoding.Ascii:
                        return Encoding.ASCII.GetString(bvals.ToArray());
                    case StringEncoding.Utf8:
                        return Encoding.UTF8.GetString(bvals.ToArray());
                    case StringEncoding.Unicode:
                        return Encoding.Unicode.GetString(bvals.ToArray());
                }
                return Encoding.Default.GetString(bvals.ToArray());
            }
            else
            {
                return string.Empty;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public uint ToUInt(Span<ushort> datas)
        {
            switch (mData.IntFormate)
            {
                case FourValueFormate.D12:
                    return Modbus.Utility.ModbusUtility.GetUInt32(datas[0], datas[1]);
                case FourValueFormate.D21:
                    return Modbus.Utility.ModbusUtility.GetUInt32(datas[1], datas[0]);
            }
            return Modbus.Utility.ModbusUtility.GetUInt32(datas[0], datas[1]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public float ToFloat(Span<ushort> datas)
        {
            switch (mData.FloatFormate)
            {
                case FourValueFormate.D12:
                    return Modbus.Utility.ModbusUtility.GetSingle(datas[0], datas[1]);
                case FourValueFormate.D21:
                    return Modbus.Utility.ModbusUtility.GetSingle(datas[1], datas[0]);
            }
            return Modbus.Utility.ModbusUtility.GetSingle(datas[0], datas[1]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public double ToDouble(Span<ushort> datas)
        {
            switch (mData.DoubleFormate)
            {
                case EightValueFormate.D1234:
                    return Modbus.Utility.ModbusUtility.GetDouble(datas[0], datas[1], datas[2], datas[3]);
                case EightValueFormate.D4321:
                    return Modbus.Utility.ModbusUtility.GetDouble(datas[3], datas[2], datas[1], datas[0]);
                case EightValueFormate.D2143:
                    return Modbus.Utility.ModbusUtility.GetDouble(datas[1], datas[0], datas[3], datas[2]);
                case EightValueFormate.D3412:
                    return Modbus.Utility.ModbusUtility.GetDouble(datas[2], datas[3], datas[0], datas[1]);
            }
            return Modbus.Utility.ModbusUtility.GetDouble(datas[0], datas[1], datas[2], datas[3]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public long ToLong(Span<ushort> datas)
        {
            switch (mData.LongFormate)
            {
                case EightValueFormate.D1234:
                    return Modbus.Utility.ModbusUtility.GetLong(datas[0], datas[1], datas[2], datas[3]);
                case EightValueFormate.D4321:
                    return Modbus.Utility.ModbusUtility.GetLong(datas[3], datas[2], datas[1], datas[0]);
                case EightValueFormate.D2143:
                    return Modbus.Utility.ModbusUtility.GetLong(datas[1], datas[0], datas[3], datas[2]);
                case EightValueFormate.D3412:
                    return Modbus.Utility.ModbusUtility.GetLong(datas[2], datas[3], datas[0], datas[1]);
            }
            return Modbus.Utility.ModbusUtility.GetLong(datas[0], datas[1], datas[2], datas[3]);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new ModbusIpDriverData();
            mData.LoadFromXML(xe);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverRuntime NewApi()
        {
            return new ModbusIpDriver();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
