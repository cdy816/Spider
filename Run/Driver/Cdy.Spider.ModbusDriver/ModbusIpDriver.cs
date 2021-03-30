using Modbus;
using System;
using System.Collections.Generic;
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

        private ModbusDriverData mData;

        Modbus.Device.ModbusIpMaster mMaster;

        private SortedDictionary<ushort, List<int>> mCoilStatusTags = new SortedDictionary<ushort, List<int>>();

        private List<ushort> mCoilStatusPackage = new List<ushort>();

        /// <summary>
        /// 
        /// </summary>
        private SortedDictionary<ushort, List<int>> mInputStatusTags = new SortedDictionary<ushort, List<int>>();

        private List<ushort> mInputStatusPackage = new List<ushort>();

        /// <summary>
        /// 
        /// </summary>
        private SortedDictionary<ushort, List<int>> mInputRegistorTags = new SortedDictionary<ushort, List<int>>();

        private List<ushort> mInputRegistorPackage = new List<ushort>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "ModbusIpDriver";

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

            foreach(var vv in mCachTags)
            {
                ushort addr = ushort.Parse(vv.Key.Substring(3));
                if (vv.Key.StartsWith("cs:"))
                {
                    //Coil status
                }
                else if (vv.Key.StartsWith("is:"))
                {
                    //Input status
                    mInputStatusTags.Add(addr, vv.Value);

                }
                else if(vv.Key.StartsWith("ir:"))
                {
                    //Input registor

                }
                else if(vv.Key.StartsWith("hr:"))
                {
                    //holding registor
                    mInputRegistorTags.Add(addr, vv.Value);
                }
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
                    mMaster.WriteSingleCoil(addr, Convert.ToBoolean(value));
                }
                else if (deviceInfo.StartsWith("hr:"))
                {
                    mMaster.WriteMultipleRegisters(addr, GetValue(value, valueType));
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

            foreach(var vv in mInputStatusPackage)
            {
                mMaster.ReadInputs((byte)mData.Id,vv, mData.PackageLen);
            }

            foreach(var vv in mInputRegistorPackage)
            {
                mMaster.ReadInputRegisters((byte)mData.Id, vv, mData.PackageLen);
            }


            mComm.Release();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new ModbusDriverData();
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
