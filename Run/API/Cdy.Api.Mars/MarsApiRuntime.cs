using Cdy.Spider;
using System;
using System.Xml.Linq;

namespace Cdy.Api.Mars
{
    /// <summary>
    /// 
    /// </summary>
    public class MarsApiRuntime : Cdy.Spider.ApiBase
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private ApiData mData;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override ApiData Data => mData;

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new ApiData();
            mData.LoadFromXML(xe);
        }

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
        public override void Start()
        {
            base.Start();
        }


        /// <summary>
        /// 
        /// </summary>
        public override void Stop()
        {
            base.Stop();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
