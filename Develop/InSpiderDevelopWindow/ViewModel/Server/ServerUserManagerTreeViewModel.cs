using InSpiderDevelopWindow.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace InSpiderDevelopWindow
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerUserManagerTreeViewModel : TreeItemViewModel
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
        /// <returns></returns>
        public override bool CanAddChild()
        {
            return false;
        }
        #endregion ...Properties...

        #region ... Methods    ...

        public override ViewModelBase GetModel(ViewModelBase mode)
        {
            if(mode is ServerUserManagerViewModel)
            {
                return mode;
            }
            else
            {
                return new ServerUserManagerViewModel();
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
