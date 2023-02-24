//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/5/1 22:07:12.
//  Version 1.0
//  种道洋
//==============================================================

using InSpiderDevelopServerClientAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace InSpiderDevelopWindow.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerUserManagerViewModel : ViewModelBase,IModeSwitch
    {

        #region ... Variables  ...
        
        private ServerUserItemViewModel mModel;

        private ObservableCollection<ServerUserItemViewModel> mUsers = new ObservableCollection<ServerUserItemViewModel>();

        private ServerUserItemViewModel mCurrentSelectedUser;

        private ICommand mAddCommand;

        private ICommand mRemoveCommand;

        private List<string> mPermissionCach = null;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public ServerUserManagerViewModel()
        {

        }
        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public ICommand AddCommand
        {
            get
            {
                if(mAddCommand==null)
                {
                    mAddCommand = new RelayCommand(() => {
                        AddUser();
                    });
                }
                return mAddCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand RemoveCommand
        {
            get
            {
                if(mRemoveCommand==null)
                {
                    mRemoveCommand = new RelayCommand(() => {
                        RemoveUser();
                        //&& mCurrentSelectedUser.Name!=ServerHelper.Helper.UserName
                    }, ()=> { return mCurrentSelectedUser != null && mCurrentSelectedUser.Name!= "Admin" ; });
                }
                return mRemoveCommand;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ServerUserItemViewModel CurrentSelectedUser
        {
            get
            {
                return mCurrentSelectedUser;
            }
            set
            {
                if (mCurrentSelectedUser != value)
                {
                    if (mCurrentSelectedUser != null)
                    {
                        mCurrentSelectedUser.CheckDatabase();
                        mCurrentSelectedUser.Update();
                    }
                    mCurrentSelectedUser = value;
                    OnPropertyChanged("CurrentSelectedUser");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ServerUserItemViewModel> Users
        {
            get
            {
                return mUsers;
            }
            set
            {
                mUsers = value;
                OnPropertyChanged("Users");
            }
        }


        ///// <summary>
        ///// 
        ///// </summary>
        //public UserTreeItemViewModel Model
        //{
        //    get
        //    {
        //        return mModel;
        //    }
        //    set
        //    {
        //        if (mModel != value)
        //        {
        //            mModel = value;
        //            OnPropertyChanged("Model");
        //            mPermissionCach = null;
                    
        //        }
        //    }
        //}

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetAvailableName(string name)
        {
            //todo
            var users = DevelopServiceHelper.Helper.GetUsers().Keys.ToList();
            string sname = name;
            for (int i = 1; i < int.MaxValue; i++)
            {
                if (!users.Contains(sname)) return sname;
                else
                {
                    sname = name + i;
                }
            }
            return name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CheckNameAvaiable(string name)
        {
            //todo
            var users = DevelopServiceHelper.Helper.GetUsers().Keys.ToList();
            return !users.Contains(name);
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddUser()
        {
            string newUserName = GetAvailableName("user");
            var umode = new ServerUserItemViewModel(newUserName,false) { IsNew = true, IsEdit = true,  ParentModel = this };
            umode.IntDatabase(mPermissionCach);
            Users.Add(umode);
            CurrentSelectedUser = umode;
        }

        /// <summary>
        /// 
        /// </summary>
        private void RemoveUser()
        {
            mCurrentSelectedUser.ParentModel = null;

            int id = Users.IndexOf(mCurrentSelectedUser);
            if(!mCurrentSelectedUser.IsNew)
            {
                //todo
                if (DevelopServiceHelper.Helper.RemoveUser(mCurrentSelectedUser.Name))
                {
                    Users.Remove(mCurrentSelectedUser);
                }
            }
            else
            {
                Users.Remove(mCurrentSelectedUser);
            }

           
            mCurrentSelectedUser = null;

            if (Users.Count > id) CurrentSelectedUser = Users[id];

            else CurrentSelectedUser = Users[Users.Count - 1];


        }

        /// <summary>
        /// 
        /// </summary>
        public void QueryUsers()
        {
           // todo
            ObservableCollection<ServerUserItemViewModel> utmp = new ObservableCollection<ServerUserItemViewModel>();
            var users = DevelopServiceHelper.Helper.GetUsers();
            if (users != null)
            {
                foreach (var vv in users)
                {
                    var uu = new ServerUserItemViewModel(vv.Key, vv.Value.Item1) { ParentModel = this, Database = vv.Value.Item3, CanNewDatabase = vv.Value.Item2 };
                    utmp.Add(uu);
                }
            }
            Users = utmp;

            if (mPermissionCach == null)
                mPermissionCach = DevelopServiceHelper.Helper.LoadMachines().Keys.ToList();

            foreach (var vv in Users)
            {
                vv.IntDatabase(mPermissionCach);
                vv.IsChanged = false;
            }

            if (Users.Count > 0) CurrentSelectedUser = Users[0];
        }

        /// <summary>
        /// 
        /// </summary>
        public void Active()
        {
            System.Threading.Tasks.Task.Run(() => {
                QueryUsers();
            });
        }

        public void DeActive()
        {
            if (mCurrentSelectedUser != null)
            {
                mCurrentSelectedUser.CheckDatabase();
                mCurrentSelectedUser.Update();
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    public class ServerUserItemViewModel : ViewModelBase
    {

        #region ... Variables  ...

        private List<PermissionItemModel> mPermissionModel;

        private bool mIsNew = false;

        private bool mIsEdit = false;

        private bool mIsPasswordChanged = false;

        private string mName;
        private string mPassword="";

        private bool mIsAdmin;


        public bool mCanNewDatabase;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public ServerUserItemViewModel(string name,bool isadmin)
        {
            mName = name;
            mIsAdmin = isadmin;
        }
        #endregion ...Constructor...

        #region ... Properties ...

        public bool IsChanged { get; set; } = false;

        /// <summary>
            /// 
            /// </summary>
        public bool IsEdit
        {
            get
            {
                return mIsEdit;
            }
            set
            {
                if (mIsEdit != value)
                {
                    mIsEdit = value;
                    OnPropertyChanged("IsEdit");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool IsNew
        {
            get
            {
                return mIsNew;
            }
            set
            {
                if (mIsNew != value)
                {
                    mIsNew = value;
                    OnPropertyChanged("IsNew");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public List<PermissionItemModel> PermissionModel
        {
            get
            {
                return mPermissionModel;
            }
            set
            {
                if (mPermissionModel != value)
                {
                    mPermissionModel = value;
                    IsChanged = true;
                    OnPropertyChanged("PermissionModel");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                if (mName != value && ParentModel.CheckNameAvaiable(value))
                {
                    string oldname = mName;
                    mName = value;
                    if(!IsNew)
                    {
                        if(! UpdateUserName(oldname, mName))
                        {
                            mName = oldname;
                        }
                    }
                    if(IsEdit)
                    {
                        IsEdit = false;
                        Update();
                    }
                }
                else
                {
                    IsEdit = false;
                }
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            get
            {
                return mPassword;
            }
            set
            {
                mIsPasswordChanged = true;
                mPassword = value;
                IsChanged = true;
                OnPropertyChanged("Password");
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public bool IsAdmin
        {
            get
            {
                return mIsAdmin;
            }
            set
            {
                if (mIsAdmin != value && Name != "Admin")
                {
                    mIsAdmin = value;
                    SelectAllPermission();
                    IsChanged = true;
                    OnPropertyChanged("IsAdmin");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public bool CanNewDatabase
        {
            get
            {
                return mCanNewDatabase;
            }
            set
            {
                if (mCanNewDatabase != value)
                {
                    mCanNewDatabase = value;
                    IsChanged = true;
                    OnPropertyChanged("CanNewDatabase");
                }
            }
        }




        private List<string> mDatabase=new List<string>();

        /// <summary>
        /// 
        /// </summary>
        public List<string> Database
        {
            get
            {
                return mDatabase;
            }
            set
            {
                if (!IsStringListEquals(mDatabase, value))
                {
                    mDatabase = value;
                    IsChanged = true;
                }
                OnPropertyChanged("Permissions");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ServerUserManagerViewModel ParentModel { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        private void SelectAllPermission()
        {
            foreach (var vv in PermissionModel)
            {
                vv.AppendSelect = IsAdmin;
            }
        }

        bool IsStringListEquals(List<string> ll, List<string> tt)
        {
            if ((ll == null && tt == null) || (ll.Count == 0 && tt.Count == 0)) return true;
            if ((ll == null && tt != null) || (ll != null && tt == null)) return false;
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            foreach (var vv in ll)
            {
                sb1.Append(vv);
            }

            foreach (var vv in tt)
            {
                sb2.Append(vv);
            }
            return sb1.ToString().Equals(sb2.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        public void IntDatabase(List<string> allpermission)
        {
            List<PermissionItemModel> ptmp = new List<PermissionItemModel>();
            foreach (var vv in allpermission)
            {
                PermissionItemModel pm = new PermissionItemModel() { Name = vv };
                pm.IsSelected = this.Database.Contains(vv);
                ptmp.Add(pm);
            }
            PermissionModel = ptmp;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CheckDatabase()
        {
            if (this.PermissionModel != null)
                this.Database = this.PermissionModel.Where(e => e.IsSelected).Select(e => e.Name).ToList();
        }

        /// <summary>
        /// 更新用户名称
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        private bool UpdateUserName(string oldName,string newName)
        {
            var vv = DevelopServiceHelper.Helper.ReNameUser(oldName, newName);
            if (vv && CurrentUserManager.Manager.UserName == oldName)
            {
                CurrentUserManager.Manager.UserName = newName;
                CurrentUserManager.Manager.OnRefreshName();
            }
            return vv;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            if (this.ParentModel == null) return;

            if (IsNew)
            {
                if (DevelopServiceHelper.Helper.AddUser(this.Name, this.Password, IsAdmin, CanNewDatabase))
                {
                    IsNew = false;
                }
                DevelopServiceHelper.Helper.UpdateUser(this.Name, IsAdmin, CanNewDatabase, this.Database);
            }
            else
            {
                if (IsChanged)
                {
                    DevelopServiceHelper.Helper.UpdateUser(this.Name, IsAdmin, CanNewDatabase, this.Database);
                    IsChanged = false;
                }
                if (mIsPasswordChanged)
                {
                    mIsPasswordChanged = false;
                    DevelopServiceHelper.Helper.UpdateUserPassword(this.Name, this.Password);
                }
            }

        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    public class PermissionItemModel : ViewModelBase
    {

        #region ... Variables  ...
        private bool mIsSelected = false;
        private string mName = "";
        private bool mAppendSelect = false;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public bool AppendSelect
        {
            get
            {
                return mAppendSelect;
            }
            set
            {
                if (mAppendSelect != value)
                {
                    mAppendSelect = value;
                    OnPropertyChanged("AppendSelect");
                    OnPropertyChanged("IsSelected");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return mIsSelected | AppendSelect;
            }
            set
            {
                if (mIsSelected != value)
                {
                    mIsSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                if (mName != value)
                {
                    mName = value;
                    OnPropertyChanged("Name");
                }
            }
        }


        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

}
