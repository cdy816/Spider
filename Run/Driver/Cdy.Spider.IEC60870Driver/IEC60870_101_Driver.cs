using Cdy.Spider.Common;
using Cdy.Spider.Common.Helper;
using Cdy.Spider.IEC60870Driver;
using lib60870.CS101;
using lib60870.CS104;
using lib60870.linklayer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.IEC60870Driver
{
    /// <summary>
    /// IEC60870 101 驱动
    /// </summary>
    public class IEC60870_101_Driver : TimerDriverRunner
    {

        #region ... Variables  ...

        private CS101Master mProxy;

        private IEC60870_101_DriverData mData;

        private bool mIsReady = false;

        private bool mIsStarted = false;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "IEC60870_101";

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
            foreach (var vv in Device.ListTags())
            {
                if (!string.IsNullOrEmpty(vv.DeviceInfo))
                {
                    string saddr = vv.DeviceInfo;
                    if(saddr.Contains("|"))
                    {
                        saddr = saddr.Substring(0, saddr.IndexOf("|"));
                    }
                    if (mCachTags.ContainsKey(saddr))
                    {
                        mCachTags[saddr].Add(vv.Id);
                    }
                    else
                    {
                        mCachTags.Add(saddr, new List<int>() { vv.Id });
                    }
                }
            }

            mComm = Device.GetCommChannel();
            if (mComm != null)
            {
                mComm.CommChangedEvent += MComm_CommChangedEvent;
                RegistorReceiveCallBack(mComm);


                mComm.Init();
            }
            LinkLayerParameters llParameters = new LinkLayerParameters();
            llParameters.AddressLength = 1;
            if(mData.Balanced)
            {
                llParameters.UseSingleCharACK = false;
            }

            var mnp = new CS101Master(this.mComm, mData.Balanced ? lib60870.linklayer.LinkLayerMode.BALANCED : lib60870.linklayer.LinkLayerMode.UNBALANCED, llParameters);
            mnp.SlaveAddress = mData.StationId;
            if(mData.Balanced)
            {
                mnp.OwnAddress = mData.OwnAddress;
            }
            else
            {
                mnp.AddSlave(mData.StationId);
            }
            
            mProxy = mnp;
            mnp.SetASDUReceivedHandler(OnC101DataRecevice, null);
            this.mComm.EnableSyncRead(true);
           
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            base.Start();
            mIsStarted = true;
            StartScan();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Stop()
        {
            base.Stop();
            mIsStarted = false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessTimerElapsed()
        {
            if (mIsReady)
            {
                if (!mData.Balanced)
                {
                    mProxy.PollSingleSlave(mData.StationId);
                }

                //
                if (mProxy.GetLinkLayerState() == lib60870.linklayer.LinkLayerState.AVAILABLE)
                {

                    mProxy.SendInterrogationCommand(CauseOfTransmission.ACTIVATION, mData.StationId, 20);
                }
            }
            base.ProcessTimerElapsed();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="asdu"></param>
        /// <returns></returns>
        private bool OnC101DataRecevice(object parameter, int address, ASDU asdu)
        {
            if (asdu.TypeId == TypeID.M_SP_NA_1)
            {
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var val = (SinglePointInformation)asdu.GetElement(i);
                    Console.WriteLine("  IOA: " + val.ObjectAddress + " value: " + val.Value);
                    UpdateValue(val.ObjectAddress.ToString(), val.Value);
                }
            }
            else if (asdu.TypeId == TypeID.M_ME_TE_1)
            {

                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var msv = (MeasuredValueScaledWithCP56Time2a)asdu.GetElement(i);
                    Console.WriteLine("  IOA: " + msv.ObjectAddress + " value: " + msv.ScaledValue.Value);
                    UpdateValue(msv.ObjectAddress.ToString(), msv.ScaledValue.Value,msv.Quality.EncodedValue);
                }

            }
            else if (asdu.TypeId == TypeID.M_ME_TF_1)
            {
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var mfv = (MeasuredValueShortWithCP56Time2a)asdu.GetElement(i);
                    Console.WriteLine("  IOA: " + mfv.ObjectAddress + " value: " + mfv.Value);
                    UpdateValue(mfv.ObjectAddress.ToString(), mfv.Value, mfv.Quality.EncodedValue);
                }
            }
            else if (asdu.TypeId == TypeID.M_SP_TB_1)
            {
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var val = (SinglePointWithCP56Time2a)asdu.GetElement(i);
                    Console.WriteLine("  IOA: " + val.ObjectAddress + " value: " + val.Value);
                    UpdateValue(val.ObjectAddress.ToString(), val.Value, val.Quality.EncodedValue);
                }
            }
            else if (asdu.TypeId == TypeID.M_ME_NC_1)
            {
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var mfv = (MeasuredValueShort)asdu.GetElement(i);
                    Console.WriteLine("  IOA: " + mfv.ObjectAddress + " value: " + mfv.Value);
                    UpdateValue(mfv.ObjectAddress.ToString(), mfv.Value, mfv.Quality.EncodedValue);
                }
            }
            else if (asdu.TypeId == TypeID.M_ME_NB_1)
            {
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var msv = (MeasuredValueScaled)asdu.GetElement(i);
                    Console.WriteLine("  IOA: " + msv.ObjectAddress + " value: " + msv.ScaledValue.Value);
                    UpdateValue(msv.ObjectAddress.ToString(), msv.ScaledValue.Value, msv.Quality.EncodedValue);
                }

            }
            else if (asdu.TypeId == TypeID.M_ME_ND_1)
            {
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var msv = (MeasuredValueNormalizedWithoutQuality)asdu.GetElement(i);
                    Console.WriteLine("  IOA: " + msv.ObjectAddress + " value: " + msv.NormalizedValue);
                    UpdateValue(msv.ObjectAddress.ToString(), msv.NormalizedValue);
                }

            }
            else if (asdu.TypeId == TypeID.C_IC_NA_1)
            {
                //if (asdu.Cot == CauseOfTransmission.ACTIVATION_CON)
                //    Console.WriteLine((asdu.IsNegative ? "Negative" : "Positive") + "confirmation for interrogation command");
                //else if (asdu.Cot == CauseOfTransmission.ACTIVATION_TERMINATION)
                //    Console.WriteLine("Interrogation command terminated");
            }
            else
            {
                LoggerService.Service.Info("IEC60870", "Unknown message type:" + asdu.TypeId);
            }
            return true;
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
            string saddr = deviceInfo.Contains("|") ? deviceInfo.Substring(0, deviceInfo.IndexOf("|")) : deviceInfo;
            bool isselect = false;
            try
            {
                if (deviceInfo.Contains("|"))
                {
                    isselect = Convert.ToBoolean(deviceInfo.Substring(deviceInfo.IndexOf("|")).Replace("|", ""));
                }
            }
            catch
            {

            }
            switch (tp)
            {
                case TagType.Bool:
                    mProxy.SendControlCommand(CauseOfTransmission.ACTIVATION, mData.StationId, new SingleCommand(int.Parse(saddr),Convert.ToBoolean(value), isselect, 0));
                    break;
                case TagType.Short:
                case TagType.UShort:
                    mProxy.SendControlCommand(CauseOfTransmission.ACTIVATION, mData.StationId, new SetpointCommandScaled(int.Parse(saddr), new ScaledValue(Convert.ToInt16(value)), new SetpointCommandQualifier(isselect, 0)));
                    break;
                case TagType.Int:
                case TagType.UInt:
                    mProxy.SendControlCommand(CauseOfTransmission.ACTIVATION, mData.StationId, new SetpointCommandScaled(int.Parse(saddr), new ScaledValue(Convert.ToInt32(value)), new SetpointCommandQualifier(isselect, 0)));
                    break;
                case TagType.Float:
                    mProxy.SendControlCommand(CauseOfTransmission.ACTIVATION, mData.StationId, new SetpointCommandShort(int.Parse(saddr), Convert.ToSingle(value), new SetpointCommandQualifier(isselect, 0)));
                    break;
                case TagType.Double:
                    mProxy.SendControlCommand(CauseOfTransmission.ACTIVATION, mData.StationId, new SetpointCommandShort(int.Parse(saddr), Convert.ToSingle(value), new SetpointCommandQualifier(isselect, 0)));
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        protected override void OnCommChanged(bool result)
        {
            if(result)
            {
                mIsReady = true;
                this.mComm.ClearBuffer();
            }
            else
            {
                mIsReady = false;
            }
            base.OnCommChanged(result);
        }


        private Thread mScanThread;

        private void StartScan()
        {
            mScanThread = new Thread(ScanProcess);
            mScanThread.IsBackground = true;
            mScanThread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ScanProcess()
        {

            while (mIsStarted)
            {
                if (mIsReady)
                {
                    while (mComm.AvaiableLenght() > 0)
                        mProxy.Run();
                }
                Thread.Sleep(1);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverRuntime NewApi()
        {
            return new IEC60870_101_Driver();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new IEC60870_101_DriverData();
            mData.LoadFromXML(xe);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...



    }
}
