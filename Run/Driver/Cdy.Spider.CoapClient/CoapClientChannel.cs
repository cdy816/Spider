using System;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider.CoapClient
{
    public class CoapClientChannel : ChannelBase2
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private CoapClientChannelData mData;

        private CoAP.CoapClient mClient;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "CoapClient";

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
            base.Init();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override object ReadValueInner(object value, int timeout, out bool result)
        {
            string re;
            re = Post(value.ToString());
            result = true;
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override object ReadValueInner(Span<byte> value, int timeout, out bool result)
        {
            var re = Post(value.ToArray());
            result = true;
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        protected override object WriteValueInner(string address, object value, int timeout,out bool result)
        {
            try
            {
                result = true;
                return Post(value.ToString());
            }
            catch
            {
                result = false;
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        protected override bool WriteValueNoWaitInner(string address, object value, int timeout)
        {
            PostAsync(value.ToString());
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sval"></param>
        /// <returns></returns>
        private string Post(string sval)
        {
            var re = mClient.Post(sval);
            if(re!=null)
            {
                return re.ResponseText;
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sval"></param>
        private void PostAsync(string sval)
        {
             mClient.PostAsync(sval);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sval"></param>
        /// <returns></returns>
        private byte[] Post(byte[] sval)
        {
            var re = mClient.Post(sval,0);
            if (re != null)
            {
                return re.Bytes;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected override byte[] SendAndWaitInner(Span<byte> data, int timeout, out bool result)
        {
            var re = Post(data.ToArray());
            result = re != null;
            return re;
        }

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override bool SendInner(Span<byte> data)
        {
            return Post(data.ToArray())!=null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool InnerOpen()
        {
            mClient = new CoAP.CoapClient(new Uri(mData.ServerUrl));
            return base.InnerOpen();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool InnerClose()
        {
            return base.InnerClose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new CoapClientChannelData();
            mData.LoadFromXML(xe);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannel2 NewApi()
        {
            return new CoapClientChannel();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...



    }
}
