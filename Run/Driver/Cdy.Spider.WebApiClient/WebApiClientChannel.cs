using System;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider.WebApiClient
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiClientChannel : ChannelBase2
    {
        #region ... Variables  ...

        private WebClient mClient;
        private WebApiClientChannelData mData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "WebAPIClient";

        /// <summary>
        /// 
        /// </summary>
        public override CommMode CommMode => CommMode.Simplex;

        /// <summary>
        /// 
        /// </summary>
        public override ChannelData Data => mData;

       

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fun"></param>
        /// <param name="sval"></param>
        /// <returns></returns>
        private string Post(string fun, string sval)
        {
            if (mClient == null)
                mClient = new WebClient();
            mClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            mClient.Encoding = Encoding.UTF8;
            return mClient.UploadString(mData.ServerUrl + "/" + fun, sval);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fun"></param>
        /// <param name="sval"></param>
        /// <returns></returns>
        private string Get(string fun, string sval)
        {
            if (mClient == null)
                mClient = new WebClient();
            mClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            mClient.Encoding = Encoding.UTF8;
            return mClient.UploadString(mData.ServerUrl + "/" + fun, "GET", sval);
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
            if(mData.Method == WebApiMethod.Get)
            {
                result = true;
                return Get(string.Empty, value.ToString());
            }
            else
            {
                result = true;
                return Post(string.Empty, value.ToString());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        protected override object WriteValueInner(string address, object value, int timeout, out bool result)
        {
            result = true;
            if (mData.Method == WebApiMethod.Get)
            {
                return Get(string.Empty, value.ToString());
            }
            else
            {
                return Post(string.Empty, value.ToString());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannel2 NewApi()
        {
            return new WebApiClientChannel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool InnerOpen()
        {
            mIsConnected = true;
            return base.InnerOpen();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new WebApiClientChannelData();
            mData.LoadFromXML(xe);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...




    }
}
