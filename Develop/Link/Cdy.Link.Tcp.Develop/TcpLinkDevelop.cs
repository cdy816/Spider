using Cdy.Spider;
using System;

namespace Cdy.Link.Tcp.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class TcpLinkDevelop : Cdy.Spider.LinkDevelopBase
    {

        #region ... Variables  ...
        private Cdy.Link.Tcp.TcpLinkData mData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public override LinkData Data { get => mData; set => mData = value as Cdy.Link.Tcp.TcpLinkData; }

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "TcpLink";

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Config()
        {
            if (mData == null) mData = new TcpLinkData() { Port = 10000, UserName = "Admin", Password = "Admin" };
            return new ApiConfigViewModel() { Model = mData };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override LinkData CreatNewData()
        {
            return new TcpLinkData() { Port = 10000, UserName = "Admin", Password = "Admin" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ILinkDevelop NewApi()
        {
            return new TcpLinkDevelop() { Data = CreatNewData() };
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...





    }
}
