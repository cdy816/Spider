using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.CoapServer.Develop
{
    public class CoapServerChannelDevelop : ChannelDevelopBase
    {

        #region ... Variables  ...
        private CoapServer.CoapServerChannelData mData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public override ChannelData Data { get => mData; set => mData = value as CoapServer.CoapServerChannelData; }

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "CoapServer";

        #endregion ...Properties...

        #region ... Methods    ...


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannelDevelop NewChannel()
        {
            return new CoapServerChannelDevelop() { mData = new CoapServer.CoapServerChannelData() };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ChannelData CreatNewData()
        {
            return new CoapServer.CoapServerChannelData();
        }

        public override object Config()
        {
            return new CoapServerChannelConfigViewModel(){ Model = mData};
        }


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...





    }
}
