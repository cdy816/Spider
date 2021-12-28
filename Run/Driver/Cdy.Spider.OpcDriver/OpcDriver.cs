using System;
using System.Xml.Linq;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace Cdy.Spider
{
    public class OpcDriver : TimerDriverRunner
    {

        #region ... Variables  ...
        private OpcDriverData mData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        
        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "OpcDriver";

        /// <summary>
        /// 
        /// </summary>
        public override DriverData Data => mData;

        /// <summary>
        /// 
        /// </summary>
        public override ValueWriteType ValueType => ValueWriteType.Object;

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public override void Prepare()
        {
            using (ChannelPrepareContext ctx = new ChannelPrepareContext())
            {
                ctx.Add("IsSubscriptionMode", mData.Model == WorkMode.Passivity);
                if (mData.Model == WorkMode.Passivity)
                {
                    ctx.Add("Tags", mCachTags.Keys.ToList());
                    ctx.Add("DeviceName", this.Device.Name);
                }
                if (mComm != null)
                    mComm.Prepare(ctx);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mComm"></param>
        public override void RegistorReceiveCallBack(ICommChannel2 mComm)
        {
            mComm.RegistorReceiveCallBack(this.OnReceiveData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        protected override object OnReceiveData(string key, object data, out bool handled)
        {
            this.UpdateValue(key, data);
            handled = true;
            return null;
        }

      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        public override void WriteValue(string deviceInfo, object value, byte valueType)
        {
            mComm.Take();
            mComm.WriteValue(deviceInfo, value);
            mComm.Release();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessTimerElapsed()
        {
            int count = this.mCachTags.Count / mData.PackageCount;
            count = this.mCachTags.Count % mData.PackageCount > 0 ? count + 1 : count;

            for (int i = 0; i < count; i++)
            {
                int icount = (i + 1) * mData.PackageCount;
                if (icount > this.mCachTags.Count)
                {
                    icount = this.mCachTags.Count - i * mData.PackageCount;
                }
                var vkeys = this.mCachTags.Keys.Skip(i * mData.PackageCount).Take(icount);
                SendGroupTags(vkeys.ToList());
                Thread.Sleep(10);
            }

            base.ProcessTimerElapsed();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tags"></param>
        private void SendGroupTags(List<string> tags)
        {
            mComm.Take();
            //var result = mComm.SendAndWait("", tags);
            var result = mComm.ReadValue(tags);
            if(result!=null)
            {
                var irest = result as IEnumerable<object>;
                if(tags.Count== irest.Count())
                {
                    int i = 0;
                    foreach(var vv in irest)
                    {
                        UpdateValue(tags[i], vv);
                        i++;
                    }
                }
            }
            mComm.Release();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverRuntime NewApi()
        {
            return new OpcDriver();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new OpcDriverData();
            mData.LoadFromXML(xe);
            base.Load(xe);
        }


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...



    }
}
