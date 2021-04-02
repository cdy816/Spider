//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/9/1 11:02:13.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider.TcpClient.Develop
{
    public class TcpClientChannelDevelop : ChannelDevelopBase
    {

        #region ... Variables  ...

        private TcpClientChannelData mData;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

        /// <summary>
        /// 
        /// </summary>
        public override ChannelData Data { get => mData; set => mData=value as TcpClientChannelData; }

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "TcpClient";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Config()
        {
            return new TcpClientChannelConfigViewModel() { Model = mData };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ICommChannelDevelop NewChannel()
        {
            return new TcpClientChannelDevelop() { Data = CreatNewData() };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ChannelData CreatNewData()
        {
            return new TcpClientChannelData() { Port = 6000 };
        }
    }
}
