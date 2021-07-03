using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.WebApiServer.Develop
{
    public class WebApiServerChannelDevelop : ChannelDevelopBase
    {

        #region ... Variables  ...
        private WebApiServerChannelData mData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public override ChannelData Data { get => mData; set => mData = value as WebApiServerChannelData; }

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "WebAPIServer";

        #endregion ...Properties...

        #region ... Methods    ...


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannelDevelop NewChannel()
        {
            return new WebApiServerChannelDevelop() { mData = new WebApiServerChannelData() };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ChannelData CreatNewData()
        {
            return new WebApiServerChannelData();
        }

        public override object Config()
        {
            return new WebApiServerChannelConfigViewModel(){ Model = mData};
        }


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...





    }
}
