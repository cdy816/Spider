using CoAP.Server.Resources;
using System;
using System.Xml.Linq;

namespace Cdy.Spider.CoapServer
{
    public class CoapServerChannel:ChannelBase2
    {

        #region ... Variables  ...

        /// <summary>
        /// 
        /// </summary>
        private CoapServerChannelData mData;

        private CoAP.Server.CoapServer mServer;

        private string mServerName;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "CoapServer";

        /// <summary>
        /// 
        /// </summary>
        public override ChannelData Data => mData;

        public override CommMode CommMode => CommMode.Simplex;



        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            mServer = new CoAP.Server.CoapServer(mData.Port);
            base.Init();
        }

        public override void Prepare(ChannelPrepareContext context)
        {
            mServerName = context.ContainsKey("DeviceName") ? context["DeviceName"].ToString() : this.mData.Name;
            base.Prepare(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public object OnDataCallBack(object value)
        {
           return this.OnReceiveCallBack("", value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannel2 NewApi()
        {
            return new CoapServerChannel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new CoapServerChannelData();
            mData.LoadFromXML(xe);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool InnerOpen()
        {
            base.InnerOpen();
            try
            {
                
                if (!string.IsNullOrEmpty(mServerName))
                {
                    mServer.Add(new ServerResource(mServerName) { Owner = this });
                }
                mServer.Start();
            }
            catch (Exception ex)
            {
                LoggerService.Service.Erro("CoapServer", ex.Message);
            }
            return true;
        }

        protected override bool InnerClose()
        {
            mServer.Stop();
            return base.InnerClose();
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 
    /// </summary>
    public class ServerResource : CoAP.Server.Resources.Resource
    {
        public ServerResource(string name):base(name)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public CoapServerChannel Owner { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchange"></param>
        protected override void DoPost(CoapExchange exchange)
        {
            if (exchange.Request.PayloadString != null)
            {
                var obj = Owner.OnDataCallBack(exchange.Request.PayloadString);
                if (obj != null)
                {
                    exchange.Respond(CoAP.StatusCode.Content);
                    exchange.Respond(obj.ToString());
                }
            }
            else
            {
                base.DoPost(exchange);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchange"></param>
        protected override void DoGet(CoapExchange exchange)
        {
            if (exchange.Request.PayloadString != null)
            {
                Owner.OnDataCallBack(exchange.Request.PayloadString);
            }
            base.DoGet(exchange);
        }

    }


}
