using Cdy.Spider.Common;
using Cdy.Spider.Common.Helper;
using Cdy.Spider.IEC60870Driver;
using lib60870.CS101;
using lib60870.CS104;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.IEC60870Driver
{
    /// <summary>
    /// IEC60870 104 驱动
    /// </summary>
    public class IEC60870_104_Driver : DriverRunnerBase
    {

        #region ... Variables  ...

        private NetConnection mProxy;

        private IEC60870_104_DriverData mData;

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
        public override string TypeName => "IEC60870_104";

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

            if (this.mComm.TypeName == "TcpClient")
            {
                var mnp = new NetConnection(this.mComm);
                mnp.ReceiveTimeout = 1000;
                mProxy = mnp;
                mnp.SetASDUReceivedHandler(OnC104DataRecevice, null);
            }
            this.mComm.EnableSyncRead(true);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="asdu"></param>
        /// <returns></returns>
        private bool OnC104DataRecevice(object parameter, ASDU asdu)
        {
            //LoggerService.Service.Info("CS104", "data received!");

            if (asdu.TypeId == TypeID.M_SP_NA_1)
            {

                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var val = (SinglePointInformation)asdu.GetElement(i);
                    //Console.WriteLine("  IOA: " + val.ObjectAddress + " value: " + val.Value);
                    UpdateValue(val.ObjectAddress.ToString(), val.Value);
                }
            }
            else if (asdu.TypeId == TypeID.M_ME_TE_1)
            {

                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var msv = (MeasuredValueScaledWithCP56Time2a)asdu.GetElement(i);
                    //Console.WriteLine("  IOA: " + msv.ObjectAddress + " value: " + msv.ScaledValue);
                    UpdateValue(msv.ObjectAddress.ToString(), msv.ScaledValue.Value,msv.Quality.EncodedValue);
                }

            }
            else if (asdu.TypeId == TypeID.M_ME_TF_1)
            {
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var mfv = (MeasuredValueShortWithCP56Time2a)asdu.GetElement(i);
                    //Console.WriteLine("  IOA: " + mfv.ObjectAddress + " value: " + mfv.Value);
                    UpdateValue(mfv.ObjectAddress.ToString(), mfv.Value, mfv.Quality.EncodedValue);
                }
            }
            else if (asdu.TypeId == TypeID.M_SP_TB_1)
            {
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var val = (SinglePointWithCP56Time2a)asdu.GetElement(i);
                    //Console.WriteLine("  IOA: " + val.ObjectAddress + " value: " + val.Value);
                    UpdateValue(val.ObjectAddress.ToString(), val.Value, val.Quality.EncodedValue);
                }
            }
            else if (asdu.TypeId == TypeID.M_ME_NC_1)
            {
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var mfv = (MeasuredValueShort)asdu.GetElement(i);
                    //Console.WriteLine("  IOA: " + mfv.ObjectAddress + " value: " + mfv.Value);
                    UpdateValue(mfv.ObjectAddress.ToString(), mfv.Value, mfv.Quality.EncodedValue);
                }
            }
            else if (asdu.TypeId == TypeID.M_ME_NB_1)
            {
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var msv = (MeasuredValueScaled)asdu.GetElement(i);
                    //Console.WriteLine("  IOA: " + msv.ObjectAddress + " scaled value: " + msv.ScaledValue);
                    UpdateValue(msv.ObjectAddress.ToString(), msv.ScaledValue.Value, msv.Quality.EncodedValue);
                }

            }
            else if (asdu.TypeId == TypeID.M_ME_ND_1)
            {
                for (int i = 0; i < asdu.NumberOfElements; i++)
                {
                    var msv = (MeasuredValueNormalizedWithoutQuality)asdu.GetElement(i);
                    //Console.WriteLine("  IOA: " + msv.ObjectAddress + " NormalizedValue: " + msv.NormalizedValue);
                    UpdateValue(msv.ObjectAddress.ToString(), msv.NormalizedValue);
                }

            }
            else if (asdu.TypeId == TypeID.C_IC_NA_1)
            {
                if (asdu.Cot == CauseOfTransmission.ACTIVATION_CON)
                    Console.WriteLine((asdu.IsNegative ? "Negative" : "Positive") + "confirmation for interrogation command");
                else if (asdu.Cot == CauseOfTransmission.ACTIVATION_TERMINATION)
                    Console.WriteLine("Interrogation command terminated");
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
                LoggerService.Service.Info("CS104", "Connected");
                if (!mProxy.IsRunning)
                {
                    mProxy.Start();

                    mProxy.SendInterrogationCommand(CauseOfTransmission.ACTIVATION, mData.StationId, 20);
                }
            }
            else
            {
                LoggerService.Service.Info("CS104", "Disconnected");
                mProxy.Close();
               
            }
            base.OnCommChanged(result);
        }

        //protected override void ProcessTimerElapsed()
        //{
        //    if(mProxy.IsRunning)
        //    {
        //        mProxy.SendInterrogationCommand(CauseOfTransmission.ACTIVATION, mData.StationId, 20);
        //    }
        //    base.ProcessTimerElapsed();
        //}



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverRuntime NewApi()
        {
            return new IEC60870_104_Driver();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new IEC60870_104_DriverData();
            mData.LoadFromXML(xe);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...



    }
}
