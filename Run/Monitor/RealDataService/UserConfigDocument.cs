using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdy.Spider.RealDataService
{
    /// <summary>
    /// 
    /// </summary>
    public class UserConfigDocument
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        public static UserConfigDocument ConfigInstance = new UserConfigDocument();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public UserConfigDocument()
        {
            Init();
        }
        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; } = "Guest";

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; } = "Guest";

        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; } = 23232;

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool CheckUser(string username,string password)
        {
            return UserName == username && Password == password;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            try
            {
                string sfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "WebServerUser.cfg");
                if (System.IO.File.Exists(sfile))
                {
                    XElement xe = XElement.Load(sfile);
                    if(xe.Attribute("UserName")!=null)
                    {
                        this.UserName = xe.Attribute("UserName").Value;
                    }

                    if (xe.Attribute("Password") != null)
                    {
                        this.Password = xe.Attribute("Password").Value;
                    }

                    if (xe.Attribute("Port") != null)
                    {
                        this.Port = int.Parse(xe.Attribute("Port").Value);
                    }
                }
            }
            catch
            {

            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
