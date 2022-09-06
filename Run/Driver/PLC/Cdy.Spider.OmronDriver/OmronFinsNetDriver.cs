using Cdy.Spider.Common;
using Cdy.Spider.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.OmronDriver
{
    /// <summary>
    /// Omron Fins 驱动
    /// </summary>
    public class OmronFinsNetDriver : TimerDriverRunner
    {

        #region ... Variables  ...
        private NetworkDeviceProxyBase mProxy;

        private AddressManager mAddressManager;

        private OmronFinsDriverData mData;

        private bool mIsReady = false;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "OmronFins";

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

            if(this.mComm.TypeName == "TcpClient")
            {
                var mnp = new OmronFinsNetProxy(this);
                //mnp.SA2 = (byte)this.mData.SA2;
                //mnp.DA2 = (byte)this.mData.DA2;
                //mnp.SID = (byte)this.mData.SID;
                mnp.SA1 = mnp.DA1 = 1;
                mnp.ByteTransform.DataFormat = this.mData.DataFormate;

                mnp.ReceiveTimeOut = 5000;
                

                mProxy = mnp;
            }
            else
            {
                var mnp = new OmronFinsUDPProxy(this);
                mnp.SA2 = (byte)this.mData.SA2;
                mnp.DA2 = (byte)this.mData.DA2;
                mnp.SID = (byte)this.mData.SID;
                mnp.ByteTransform.DataFormat = this.mData.DataFormate;
                mProxy = mnp;
            }
            mAddressManager = new AddressManager();

            foreach (var vv in this.Device.ListTags())
            {
                if (vv.Type == TagType.String)
                {
                    var vss = vv.DeviceInfo.Split(":");
                    var operateResult = OmronFinsAddress.ParseFrom(vss[0], 1, out bool result);
                    if(result)
                    {
                        mAddressManager.Add(vv.Type, operateResult.AddressStart, vss[0], true, int.Parse(vss[1]));
                    }
                }
                else
                {
                    var operateResult = OmronFinsAddress.ParseFrom(vv.DeviceInfo, 1, out bool result);
                    if (result)
                        mAddressManager.Add(vv.Type, operateResult.AddressStart, vv.DeviceInfo, true, 0);
                }
            }
            this.mComm.EnableSyncRead(true);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        public override void WriteValue(string deviceInfo, object value, byte valueType)
        {
            TagType tp = (TagType) valueType;
            bool result;
            switch(tp)
            {
                case TagType.Bool:
                    mProxy.Write(deviceInfo, Convert.ToBoolean(value), out result);
                    break;
                case TagType.Byte:
                    mProxy.Write(deviceInfo, Convert.ToByte(value), out result);
                    break;
                case TagType.Short:
                    mProxy.Write(deviceInfo, Convert.ToInt16(value), out result);
                    break;
                case TagType.UShort:
                    mProxy.Write(deviceInfo, Convert.ToUInt16(value), out result);
                    break;
                case TagType.Int:
                    mProxy.Write(deviceInfo, Convert.ToInt32(value), out result);
                    break;
                case TagType.UInt:
                    mProxy.Write(deviceInfo, Convert.ToUInt32(value), out result);
                    break;
                case TagType.Long:
                    mProxy.Write(deviceInfo, Convert.ToInt64(value), out result);
                    break;
                case TagType.ULong:
                    mProxy.Write(deviceInfo, Convert.ToUInt64(value), out result);
                    break;
                case TagType.Double:
                    mProxy.Write(deviceInfo, Convert.ToDouble(value), out result);
                    break;
                case TagType.Float:
                    mProxy.Write(deviceInfo, Convert.ToSingle(value), out result);
                    break;
                case TagType.DateTime:
                    mProxy.Write(deviceInfo, Convert.ToDateTime(value).Ticks, out result);
                    break;
                case TagType.IntPoint:
                    mProxy.Write(deviceInfo, new int[] { ((IntPoint)(value)).X, ((IntPoint)(value)).Y }, out result);
                    break;
                case TagType.UIntPoint:
                    mProxy.Write(deviceInfo, new uint[] { ((UIntPoint)(value)).X, ((UIntPoint)(value)).Y }, out result);
                    break;
                case TagType.IntPoint3:
                    mProxy.Write(deviceInfo, new int[] { ((IntPoint3)(value)).X, ((IntPoint3)(value)).Y, ((IntPoint3)(value)).Z }, out result);
                    break;
                case TagType.UIntPoint3:
                    mProxy.Write(deviceInfo, new uint[] { ((UIntPoint3)(value)).X, ((UIntPoint3)(value)).Y, ((UIntPoint3)(value)).Z }, out result);
                    break;
                case TagType.LongPoint:
                    mProxy.Write(deviceInfo, new long[] { ((LongPoint)(value)).X, ((LongPoint)(value)).Y }, out result);
                    break;
                case TagType.ULongPoint:
                    mProxy.Write(deviceInfo, new ulong[] { ((ULongPoint)(value)).X, ((ULongPoint)(value)).Y }, out result);
                    break;
                case TagType.LongPoint3:
                    mProxy.Write(deviceInfo, new long[] { ((LongPoint3)(value)).X, ((LongPoint3)(value)).Y, ((LongPoint3)(value)).Z }, out result);
                    break;
                case TagType.ULongPoint3:
                    mProxy.Write(deviceInfo, new ulong[] { ((ULongPoint3)(value)).X, ((ULongPoint3)(value)).Y, ((ULongPoint3)(value)).Z }, out result);
                    break;
                case TagType.String:
                    mProxy.Write(deviceInfo.Split(":")[0], value.ToString(), out result);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        protected override void OnCommChanged(bool result)
        {
            if(result && mProxy is OmronFinsNetProxy)
            {
                var val = (mProxy as OmronFinsNetProxy)?.InitializationOnConnect();
                if(val != null)
                {
                    mIsReady = val.Value;
                }
            }
            else if(!result && mProxy is OmronFinsNetProxy)
            {
                mIsReady = false;
            }
            base.OnCommChanged(result);
        }

        /// <summary>
        /// 定时执行
        /// </summary>
        protected override void ProcessTimerElapsed()
        {
            if (!mComm.IsConnected||!mIsReady)
                return;

            bool result = false;
            foreach (var vv in mAddressManager.ListAddressBlock())
            {
                switch (vv.TagType)
                {
                    case TagType.Bool:
                        var bvals = mProxy.ReadBool(vv.Address, (ushort)vv.SubAddress.Count, out result);
                        if (bvals != null && bvals.Length == vv.SubAddress.Count && result)
                        {
                            for(int i = 0; i < bvals.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], bvals[i]);
                            }
                        }
                        break;
                    case TagType.Byte:
                        var btvals = mProxy.Read(vv.Address, (ushort)vv.SubAddress.Count, out result);
                        if (btvals != null && btvals.Length == vv.SubAddress.Count && result)
                        {
                            for (int i = 0; i < btvals.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], btvals[i]);
                            }
                        }
                        break;
                    case TagType.Short:
                        var svals = mProxy.ReadInt16(vv.Address, (ushort)vv.SubAddress.Count, out result);
                        if (svals != null && svals.Length == vv.SubAddress.Count && result)
                        {
                            for (int i = 0; i < svals.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], svals[i]);
                            }
                        }
                        break;
                    case TagType.UShort:
                        var usvals = mProxy.ReadUInt16(vv.Address, (ushort)vv.SubAddress.Count, out result);
                        if (usvals != null && usvals.Length == vv.SubAddress.Count && result)
                        {
                            for (int i = 0; i < usvals.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], usvals[i]);
                            }
                        }
                        break;
                    case TagType.Int:
                        var ivals = mProxy.ReadInt32(vv.Address, (ushort)vv.SubAddress.Count, out result);
                        if (ivals != null && ivals.Length == vv.SubAddress.Count && result)
                        {
                            for (int i = 0; i < ivals.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], ivals[i]);
                            }
                        }
                        break;
                    case TagType.UInt:
                        var uivals = mProxy.ReadUInt32(vv.Address, (ushort)vv.SubAddress.Count, out result);
                        if (uivals != null && uivals.Length == vv.SubAddress.Count && result)
                        {
                            for (int i = 0; i < uivals.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], uivals[i]);
                            }
                        }
                        break;
                    case TagType.Long:
                        var lvals = mProxy.ReadInt64(vv.Address, (ushort)vv.SubAddress.Count, out result);
                        if (lvals != null && lvals.Length == vv.SubAddress.Count && result)
                        {
                            for (int i = 0; i < lvals.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], lvals[i]);
                            }
                        }
                        break;
                    case TagType.ULong:
                        var ulvals = mProxy.ReadUInt64(vv.Address, (ushort)vv.SubAddress.Count, out result);
                        if (ulvals != null && ulvals.Length == vv.SubAddress.Count && result)
                        {
                            for (int i = 0; i < ulvals.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], ulvals[i]);
                            }
                        }
                        break;
                    case TagType.DateTime:
                        var dtvals = mProxy.ReadInt64(vv.Address, (ushort)vv.SubAddress.Count, out result);
                        if (dtvals != null && dtvals.Length == vv.SubAddress.Count && result)
                        {
                            for (int i = 0; i < dtvals.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], DateTime.FromBinary(dtvals[i]));
                            }
                        }
                        break;
                    case TagType.Double:
                        var dvals = mProxy.ReadDouble(vv.Address, (ushort)vv.SubAddress.Count, out result);
                        if (dvals != null && dvals.Length == vv.SubAddress.Count && result)
                        {
                            for (int i = 0; i < dvals.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], dvals[i]);
                            }
                        }
                        break;
                    case TagType.Float:
                        var fvals = mProxy.ReadFloat(vv.Address, (ushort)vv.SubAddress.Count, out result);
                        if (fvals != null && fvals.Length == vv.SubAddress.Count&& result)
                        {
                            for (int i = 0; i < fvals.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], fvals[i]);
                            }
                        }
                        break;
                    case TagType.IntPoint:
                        var ipvals = mProxy.ReadInt32(vv.Address, (ushort)(vv.SubAddress.Count*2), out result);
                        if (ipvals != null && ipvals.Length == vv.SubAddress.Count*2 && result)
                        {
                            for (int i = 0; i < ipvals.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], new IntPoint() { X = ipvals[i], Y = ipvals[i + 1] });
                                i++;
                            }
                        }
                        break;
                    case TagType.UIntPoint:
                        var uipvals = mProxy.ReadUInt32(vv.Address, (ushort)(vv.SubAddress.Count * 2), out result);
                        if (uipvals != null && uipvals.Length == vv.SubAddress.Count * 2 && result)
                        {
                            for (int i = 0; i < uipvals.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], new UIntPoint() { X = uipvals[i], Y = uipvals[i + 1] });
                                i++;
                            }
                        }
                        break;
                    case TagType.IntPoint3:
                        var ipvals3 = mProxy.ReadInt32(vv.Address, (ushort)(vv.SubAddress.Count * 3), out result);
                        if (ipvals3 != null && ipvals3.Length == vv.SubAddress.Count * 2 && result)
                        {
                            for (int i = 0; i < ipvals3.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], new IntPoint3() { X = ipvals3[i], Y = ipvals3[i + 1], Z = ipvals3[i + 1] });
                                i+=2;
                            }
                        }
                        break;
                    case TagType.UIntPoint3:
                        var uipvals3 = mProxy.ReadUInt32(vv.Address, (ushort)(vv.SubAddress.Count * 3), out result);
                        if (uipvals3 != null && uipvals3.Length == vv.SubAddress.Count * 2 && result)
                        {
                            for (int i = 0; i < uipvals3.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], new UIntPoint3() { X = uipvals3[i], Y = uipvals3[i + 1], Z = uipvals3[i + 1] });
                                i += 2;
                            }
                        }
                        break;
                    case TagType.LongPoint:
                        var lpvals = mProxy.ReadInt64(vv.Address, (ushort)(vv.SubAddress.Count * 2), out result);
                        if (lpvals != null && lpvals.Length == vv.SubAddress.Count * 2 && result)
                        {
                            for (int i = 0; i < lpvals.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], new LongPoint() { X = lpvals[i], Y = lpvals[i + 1] });
                                i++;
                            }
                        }
                        break;
                    case TagType.ULongPoint:
                        var ulpvals = mProxy.ReadUInt64(vv.Address, (ushort)(vv.SubAddress.Count * 2), out result);
                        if (ulpvals != null && ulpvals.Length == vv.SubAddress.Count * 2 && result)
                        {
                            for (int i = 0; i < ulpvals.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], new ULongPoint() { X = ulpvals[i], Y = ulpvals[i + 1] });
                                i++;
                            }
                        }
                        break;
                    case TagType.LongPoint3:
                        var lpvals3 = mProxy.ReadInt64(vv.Address, (ushort)(vv.SubAddress.Count * 3), out result);
                        if (lpvals3 != null && lpvals3.Length == vv.SubAddress.Count * 2 && result)
                        {
                            for (int i = 0; i < lpvals3.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], new LongPoint3() { X = lpvals3[i], Y = lpvals3[i + 1], Z = lpvals3[i + 1] });
                                i += 2;
                            }
                        }
                        break;
                    case TagType.ULongPoint3:
                        var ulpvals3 = mProxy.ReadUInt64(vv.Address, (ushort)(vv.SubAddress.Count * 3), out result);
                        if (ulpvals3 != null && ulpvals3.Length == vv.SubAddress.Count * 2 && result)
                        {
                            for (int i = 0; i < ulpvals3.Length; i++)
                            {
                                UpdateValue(vv.SubAddress[i], new ULongPoint3() { X = ulpvals3[i], Y = ulpvals3[i + 1], Z = ulpvals3[i + 1] });
                                i += 2;
                            }
                        }
                        break;
                    case TagType.String:

                        foreach(var vvs in vv.SubAddress)
                        {
                            string sval = mProxy.ReadString(vvs, (ushort)vv.PerAddressLength, out result);
                            if(result)
                            {
                                UpdateValue(vvs, sval);
                            }
                        }
                        break;
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverRuntime NewApi()
        {
            return new OmronFinsNetDriver();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new OmronFinsDriverData();
            mData.LoadFromXML(xe);
            //base.Load(xe);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...



    }
}
