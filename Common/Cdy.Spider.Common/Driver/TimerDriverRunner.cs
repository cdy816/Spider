//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/6 8:47:09.
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
    public abstract class TimerDriverRunner:DriverRunnerBase
    {

        #region ... Variables  ...
        private System.Timers.Timer mTimer;
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
        public override void Start()
        {
            base.Start();
            if (Data.Model == WorkMode.Active)
            {
                mTimer = new System.Timers.Timer(Data.ScanCircle);
                mTimer.Elapsed += MTimer_Elapsed;
                mTimer.Start();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            mTimer.Enabled = false;
            ProcessTimerElapsed();
            mTimer.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ProcessTimerElapsed()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Stop()
        {
            if (mTimer != null)
            {
                mTimer.Stop();
                mTimer.Dispose();
                mTimer = null;
            }
            base.Stop();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
