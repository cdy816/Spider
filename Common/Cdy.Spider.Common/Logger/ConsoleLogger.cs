//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/1/20 9:03:14.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider
{
    public class ConsoleLogger : ILog
    {

        #region ... Variables  ...

        string debugFormate = "Debug {0} {1} {2}";

        string infoFormate = "Info {0} {1} {2}";

        string erroFormate = "Erro {0} {1} {2}";

        string warnFormate = "Warn {0} {1} {2}";

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        public void Debug(string name, string msg)
        {
            Console.WriteLine(string.Format(debugFormate, DateTime.Now.ToString(), name, msg));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        public void Erro(string name, string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Format(erroFormate, DateTime.Now.ToString(), name, msg));
            Console.ResetColor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        public void Info(string name, string msg)
        {
            Console.WriteLine(string.Format(infoFormate, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), name, msg));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        /// <param name="parameter"></param>
        public void Info(string name, string msg, object parameter)
        {
            Console.ForegroundColor = (ConsoleColor)(parameter);
            Console.WriteLine(string.Format(infoFormate, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), name, msg));
            Console.ResetColor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        public void Warn(string name, string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(string.Format(warnFormate, DateTime.Now.ToString(), name, msg));
            Console.ResetColor();
        }

        

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
