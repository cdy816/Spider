using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InSpiderDevelopServer
{
    public class UserDocument
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, User> mUsers = new Dictionary<string, User>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        public string Version { get; set; } = "0.1";

        /// <summary>
        /// 
        /// </summary>

        public Dictionary<string,User> Users
        {
            get
            {
                return mUsers;
            }
        }



        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void AddDefaultUser()
        {
            this.AddUser(new User() { Name = "Admin", Password = "Admin", IsAdmin=true,NewMachines=true,DeleteMachines=true });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldname"></param>
        /// <param name="newname"></param>
        /// <returns></returns>
        public bool RenameUser(string oldname,string newname)
        {
            if(mUsers.ContainsKey(oldname)&&!mUsers.ContainsKey(newname))
            {
                var uu = mUsers[oldname];
                mUsers.Remove(oldname);
                uu.Name = newname;
                mUsers.Add(uu.Name, uu);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public User GetUser(string name)
        {
            if (mUsers.ContainsKey(name))
            {
                return mUsers[name];
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public bool AddUser(User user)
        {
            if(!mUsers.ContainsKey(user.Name))
            {
                mUsers.Add(user.Name, user);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void RemoveUser(string name)
        {
            if (mUsers.ContainsKey(name))
            {
                mUsers.Remove(name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void ModifyUser(User user)
        {
            if (mUsers.ContainsKey(user.Name))
            {
                mUsers[user.Name] = user;
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
