using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cdy.Link.Tcp
{
    public class SecurityService
    {

        #region ... Variables  ...
        
        /// <summary>
        /// 
        /// </summary>
        public static SecurityService Service = new SecurityService();

        private Dictionary<long, DateTime> mLoginer = new Dictionary<long, DateTime>();

        private long mCount = 0;

        private bool mIsClosed = false;

        private Thread mScanThread;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public long Login(string username,string password)
        {
            if (username == UserName && password == Password)
            {
                lock (mLoginer)
                {
                    mCount++;
                    long lid = (32 << DateTime.Now.Millisecond) + mCount;

                    mLoginer.Add(lid, DateTime.Now);
                    return lid;
                }
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public bool CheckLogin(string username,string pass)
        {
            return username == UserName && pass == Password;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RefreshId(long id)
        {
            lock (mLoginer)
            {
                if (mLoginer.ContainsKey(id))
                {
                    mLoginer[id] = DateTime.Now;
                }
                    return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckId(long id)
        {
            lock (mLoginer)
            {
                if (mLoginer.ContainsKey(id))
                    return true;
            }
            return false;
        }

        private void ThreadPro()
        {
            while(!mIsClosed)
            {
                DateTime dt = DateTime.Now;
                foreach(var vv in mLoginer.ToArray())
                {
                    if((dt - vv.Value).TotalSeconds>300)
                    {
                        lock(mLoginer)
                        mLoginer.Remove(vv.Key);
                    }
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            mScanThread = new Thread(ThreadPro);
            mScanThread.IsBackground = true;
            mScanThread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            mIsClosed = true;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
