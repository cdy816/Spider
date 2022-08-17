using Cdy.Spider.OpcDAClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.OpcDA.Develop
{
    public class OpcDAChannelDevelop : ChannelDevelopBase
    {

        #region ... Variables  ...
        private OpcDAChannelData mData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public override ChannelData Data { get => mData; set => mData = value as OpcDAChannelData; }

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "OpcDA";

        #endregion ...Properties...

        #region ... Methods    ...


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannelDevelop NewChannel()
        {
            return new OpcDAChannelDevelop() { mData = new OpcDAChannelData() };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ChannelData CreatNewData()
        {
            return new OpcDAChannelData();
        }

        public override object Config()
        {
            return new OpcDAChannelConfigViewModel(){ Model = mData};
        }


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...





    }
}
