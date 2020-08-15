//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/1/20 8:53:46.
//  Version 1.0
//  种道洋
//==============================================================
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        void Debug(string name,string msg);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        void Info(string name, string msg);


        void Info(string name, string msg, object parameter);
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        void Warn(string name, string msg);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        void Erro(string name, string msg);
    }
}
