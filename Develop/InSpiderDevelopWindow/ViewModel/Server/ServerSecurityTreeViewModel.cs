using System;
using System.Collections.Generic;
using System.Text;

namespace InSpiderDevelopWindow
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerSecurityTreeViewModel: HasChildrenTreeItemViewModel
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        public ServerSecurityTreeViewModel()
        {
            mIsLoaded = true;
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CanAddChild()
        {
            return false;
        }
        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
