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
        public App()
        {
            this.Startup += App_Startup;
        }


        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...

        private void App_Startup(object sender, StartupEventArgs e)
        {
            Cdy.Spider.ApiFactory.Factory.LoadForDevelop();
            Cdy.Spider.ChannelFactory.Factory.LoadForDevelop();
            Cdy.Spider.DriverFactory.Factory.LoadForDevelop();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
