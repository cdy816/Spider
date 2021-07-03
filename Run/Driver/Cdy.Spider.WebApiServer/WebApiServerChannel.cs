using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.WebApiServer
{
    public class WebApiServerChannel : ChannelBase2
    {

        #region ... Variables  ...
        private WebApiServerChannelData mData;
        private WebApiServer mServer;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "WebAPIServer";

        /// <summary>
        /// 
        /// </summary>
        public override ChannelData Data => mData;

        /// <summary>
        /// 
        /// </summary>
        public override CommMode CommMode => CommMode.Simplex;

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            base.Init();
            mServer = WebApiServiceManager.Manager.AddService(mData.Port, mData.UseHttps);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void Prepare(ChannelPrepareContext context)
        {
            string sname = context.ContainsKey("DeviceName") ? context["DeviceName"].ToString() : this.mData.Name;
            ServiceMapManager.Manager.RegistorServerMap(sname, new Func<string, string>(obj => {
                var res = OnReceiveCallBack("", obj);
                if (res != null)
                    return res.ToString();
                else
                    return string.Empty;
            }));
            base.Prepare(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool InnerOpen()
        {
            Task.Run(() => {
                mServer.Start();
            });
            return base.InnerOpen();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool InnerClose()
        {
            mServer.Stop();
            return base.InnerClose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannel2 NewApi()
        {
            return new WebApiServerChannel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new WebApiServerChannelData();
            mData.LoadFromXML(xe);
            base.Load(xe);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
