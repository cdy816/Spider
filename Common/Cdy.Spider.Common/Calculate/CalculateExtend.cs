using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public class CalculateExtend
    {

        #region ... Variables  ...
        
        public static CalculateExtend extend = new CalculateExtend();

        private List<string> mExtendDlls = new List<string>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public CalculateExtend()
        {
            Init();
        }
        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public List<string> ExtendDlls
        {
            get
            {
                return mExtendDlls;
            }
        }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        private void Init()
        {
            string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "Calculate.cfg");
            if(System.IO.File.Exists(sfile))
            {
                var dlls = System.IO.File.ReadAllLines(sfile);
                foreach(var vv in dlls)
                {
                    if(System.IO.Path.IsPathRooted(vv))
                    {
                        mExtendDlls.Add(vv);
                    }
                    else
                    {
                        mExtendDlls.Add(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), vv));
                    }
                }
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
