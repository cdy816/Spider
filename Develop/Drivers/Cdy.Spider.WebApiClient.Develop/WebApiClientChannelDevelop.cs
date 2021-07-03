using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.WebApiClient.Develop
{
    public class WebApiClientChannelDevelop : ChannelDevelopBase
    {

        #region ... Variables  ...
        private WebApiClient.WebApiClientChannelData mData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public override ChannelData Data { get => mData; set => mData = value as WebApiClient.WebApiClientChannelData; }

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "WebAPIClient";

        #endregion ...Properties...

        #region ... Methods    ...


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannelDevelop NewChannel()
        {
            return new WebApiClientChannelDevelop() { mData = new WebApiClient.WebApiClientChannelData() };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ChannelData CreatNewData()
        {
            return new WebApiClient.WebApiClientChannelData();
        }

        public override object Config()
        {
            return new WebApiClientChannelConfigViewModel(){ Model = mData};
        }


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...





    }
}
