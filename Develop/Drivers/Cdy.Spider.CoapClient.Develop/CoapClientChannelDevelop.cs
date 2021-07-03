using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.CoapClient.Develop
{
    public class CoapClientChannelDevelop : ChannelDevelopBase
    {

        #region ... Variables  ...
        private CoapClient.CoapClientChannelData mData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public override ChannelData Data { get => mData; set => mData = value as CoapClient.CoapClientChannelData; }

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "CoapClient";

        #endregion ...Properties...

        #region ... Methods    ...


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannelDevelop NewChannel()
        {
            return new CoapClientChannelDevelop() { mData = new CoapClient.CoapClientChannelData() };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ChannelData CreatNewData()
        {
            return new CoapClient.CoapClientChannelData();
        }

        public override object Config()
        {
            return new CoapClientChannelConfigViewModel(){ Model = mData};
        }


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...





    }
}
