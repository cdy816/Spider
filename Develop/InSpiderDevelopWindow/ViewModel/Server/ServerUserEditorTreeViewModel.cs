using InSpiderDevelopWindow.ViewModel;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace InSpiderDevelopWindow
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerUserEditorTreeViewModel : TreeItemViewModel
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        public ServerUserEditorTreeViewModel()
        {
            CurrentUserManager.Manager.RefreshNameEvent += Manager_RefreshNameEvent;
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

        /// <summary>
        /// 
        /// </summary>
        public override string Name { get => CurrentUserManager.Manager.UserName; set => base.Name = value; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public override ViewModelBase GetModel(ViewModelBase mode)
        {
            if(mode is ServerUserEditorViewModel)
            {
                return mode;
            }
            else
            {
                return new ServerUserEditorViewModel();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Manager_RefreshNameEvent(object sender, EventArgs e)
        {
            OnPropertyChanged("Name");
        }

        public override void Dispose()
        {
            CurrentUserManager.Manager.RefreshNameEvent -= Manager_RefreshNameEvent;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
