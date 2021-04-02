using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider.SerisePortClient.Develop
{
    public class SerisePortClientChannelDevelop : ChannelDevelopBase
    {

        #region ... Variables  ...
        private SerisePortClientChannelData mData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "SerisePort";

        /// <summary>
        /// 
        /// </summary>
        public override ChannelData Data { get => mData; set => mData = value as SerisePortClientChannelData; }
        
        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...


      
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Config()
        {
            return new SerisePortClientChannelViewModel() { Data = mData };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannelDevelop NewChannel()
        {
            return new SerisePortClientChannelDevelop() { Data = CreatNewData() };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ChannelData CreatNewData()
        {
            return new SerisePortClientChannelData() { PortName = "COM1", BandRate = 9600, Check = PortCheckType.None, DataSize = 8, StopBits = StopBits.One, ReTryCount = 3, DataSendTimeout = 1000, ReTryDuration = 1000, Timeout = 5000 };
        }
    }
}
