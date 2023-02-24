using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InSpiderDevelopServer
{
    /// <summary>
    /// 
    /// </summary>
    public class SecurityManager
    {

        #region ... Variables  ...
        
        /// <summary>
        /// 
        /// </summary>
        public static SecurityManager Manager = new SecurityManager();

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, List<string>> mLogins = new Dictionary<string, List<string>>();

        /// <summary>
        /// 
        /// </summary>
        private List<string> mAvaiableKey = new List<string>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public Security.Security Securitys { get; set; }

        public static string SecurityDataFile = "";

        public static bool IsSubSystemMode = false;

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            Security.SecuritySerise ss = new Security.SecuritySerise();
            ss.Load(SecurityDataFile);
            this.Securitys = ss.Document;
         }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            Security.SecuritySerise ss = new Security.SecuritySerise() { Document = this.Securitys };
            ss.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        private bool LoginInner(string userName,string pass)
        {
            if(Securitys!=null&&Securitys.User.Users.ContainsKey(userName))
            {
                return Securitys.User.Users[userName].Password == pass;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public bool CheckPasswordIsCorrect(string userName,string pass)
        {
            if (Securitys != null && Securitys.User.Users.ContainsKey(userName))
            {
                return Securitys.User.Users[userName].Password == pass;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool CheckKeyAvaiable(string key)
        {
            return mAvaiableKey.Contains(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public string GetUserName(string loginId)
        {
            var users = mLogins.Where(e => e.Value.Contains(loginId)).FirstOrDefault().Key;
            return users;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public User GetUser(string loginId)
        {
            var user = mLogins.Where(e => e.Value.Contains(loginId)).FirstOrDefault().Key;
            if (Securitys.User.Users.ContainsKey(user))
            {
                return Securitys.User.Users[user];
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public bool CheckDatabase(string key,string database)
        {
            if (string.IsNullOrEmpty(database)|| IsSubSystemMode) return true;
            var users = mLogins.Where(e => e.Value.Contains(key)).FirstOrDefault().Key;
            if (Securitys.User.Users.ContainsKey(users))
            {
                var us = Securitys.User.Users[users];
                return us.Machines.Contains(database);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<string> GetDatabase(string key)
        {
            var users = mLogins.Where(e => e.Value.Contains(key)).FirstOrDefault().Key;
            if (Securitys.User.Users.ContainsKey(users))
            {
                return Securitys.User.Users[users].Machines;
            }
            return new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsAdmin(string key)
        {
            var users = mLogins.Where(e => e.Value.Contains(key)).FirstOrDefault().Key;
            if (Securitys.User.Users.ContainsKey(users))
            {
                var us = Securitys.User.Users[users];
                return us.IsAdmin;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool HasNewDatabasePermission(string key)
        {
            var users = mLogins.Where(e => e.Value.Contains(key)).FirstOrDefault().Key;
            if (Securitys.User.Users.ContainsKey(users))
            {
                var us = Securitys.User.Users[users];
                return us.NewMachines;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool HasDeleteDatabasePermission(string key)
        {
            var users = mLogins.Where(e => e.Value.Contains(key)).FirstOrDefault().Key;
            if (Securitys.User.Users.ContainsKey(users))
            {
                var us = Securitys.User.Users[users];
                return us.DeleteMachines;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public void RenameLoginUser(string oldName,string newName)
        {
            if(mLogins.ContainsKey(oldName)&&!mLogins.ContainsKey(newName))
            {
                var vv = mLogins[oldName];
                mLogins.Remove(oldName);
                mLogins.Add(newName, vv);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Login(string userName,string password)
        {
            if(LoginInner(userName,password))
            {
                string sid = Guid.NewGuid().ToString();
                string skey = userName;
                lock (mLogins)
                {
                    if (!mLogins.ContainsKey(userName))
                    {
                        mLogins.Add(userName, new List<string>() { sid });
                        mAvaiableKey.Add(sid);
                    }
                    else
                    {
                        mLogins[userName].Add(sid);
                        mAvaiableKey.Add(sid);
                    }
                }
                return sid;
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        public void Logout(string id)
        {
            lock (mLogins)
            {
                var user = mLogins.Where(e => e.Value.Contains(id));
                if (user.Count() > 0)
                {
                    var userName = user.FirstOrDefault().Key;
                    if (mLogins.ContainsKey(userName))
                    {
                        //var ids = mLogins[userName];
                        if (mAvaiableKey.Contains(id))
                        {
                            mAvaiableKey.Remove(id);
                        }
                        mLogins[userName].Remove(id);
                        //mLogins.Remove(userName);
                    }
                }
            }
        }


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
