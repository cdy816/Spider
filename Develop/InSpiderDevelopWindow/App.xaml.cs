using Cdy.Spider;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace InSpiderDevelopWindow
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        public App()
        {
            this.Startup += App_Startup;
        }

        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args != null)
            {
                foreach (var vv in e.Args)
                {
                    if(vv=="/a")
                    {
                        ServerHelper.Helper.AutoLogin = true;
                        //自动登录
                    }
                    else if(vv.StartsWith("server="))
                    {
                        ServerHelper.Helper.Server = vv.Substring("server=".Length);
                    }
                    else if(vv.StartsWith("username="))
                    {
                        ServerHelper.Helper.UserName = vv.Substring("username=".Length);
                    }
                    else if(vv.StartsWith("password="))
                    {
                        ServerHelper.Helper.Password = vv.Substring("password=".Length);
                    }
                }
            }

            Cdy.Spider.ApiFactory.Factory.LoadForDevelop();
            Cdy.Spider.ChannelFactory2.Factory.LoadForDevelop();
            Cdy.Spider.DriverFactory.Factory.LoadForDevelop();
            Cdy.Spider.LinkFactory.Factory.LoadForDevelop();

            RegsiteService();
        }

        /// <summary>
        /// 
        /// </summary>
        private void RegsiteService()
        {
            ServiceLocator.Locator.Registor<IApiFactory>(ApiFactory.Factory);
            ServiceLocator.Locator.Registor<ICommChannelFactory2>(ChannelFactory2.Factory);
            ServiceLocator.Locator.Registor<IDriverFactory>(DriverFactory.Factory);
            ServiceLocator.Locator.Registor<ILinkFactory>(LinkFactory.Factory);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
