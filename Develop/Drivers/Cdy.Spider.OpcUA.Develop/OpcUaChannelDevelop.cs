using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.OpcUA.Develop
{
    public class OpcUaChannelDevelop : ChannelDevelopBase
    {

        #region ... Variables  ...
        private OpcClient.OpcUAChannelData mData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public override ChannelData Data { get => mData; set => mData = value as OpcClient.OpcUAChannelData; }

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "OpcUa";

        #endregion ...Properties...

        #region ... Methods    ...


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannelDevelop NewChannel()
        {
            return new OpcUaChannelDevelop() { mData = new OpcClient.OpcUAChannelData() };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ChannelData CreatNewData()
        {
            return new OpcClient.OpcUAChannelData();
        }

        public override object Config()
        {
            throw new NotImplementedException();
        }


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...





    }
}
