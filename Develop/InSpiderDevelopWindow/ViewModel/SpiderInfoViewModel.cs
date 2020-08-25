//==============================================================
//  Copyright (C) 2020 Chongdaoyang Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/22 20:35:06 .
//  Version 1.0
//  CDYWORK
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace InSpiderDevelopWindow.ViewModel
{
    public class SpiderInfoViewModel:ViewModelBase
    {

        #region ... Variables  ...
        private string mTimeString;
        private System.Timers.Timer tim;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public SpiderInfoViewModel()
        {
            tim = new System.Timers.Timer();
            tim.Interval = 1000;
            tim.Elapsed += Tim_Elapsed;
            tim.Start();
        }


        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public string MarsTitle
        {
            get
            {
                return Res.Get("SpiderTitle");
            }
        }

        public string TimeString
        {
            get
            {
                return mTimeString;
            }
            set
            {
                mTimeString = value;
                OnPropertyChanged("TimeString");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tim_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimeString = DateTime.Now.ToString();
        }


        #endregion ...Properties...

        #region ... Methods    ...

        public override void Dispose()
        {
            tim.Stop();
            tim.Dispose();
            base.Dispose();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
