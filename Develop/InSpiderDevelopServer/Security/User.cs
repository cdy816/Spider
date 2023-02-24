using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InSpiderDevelopServer
{
    public class User
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public List<string> Permissions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool NewMachines { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public bool DeleteMachines { get; set; }

        /// <summary>
        /// 允许访问的数据库
        /// </summary>
        public List<string> Machines { get; set; } = new List<string>();

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
