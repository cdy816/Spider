using Cdy.Spider;
using InSpiderDevelop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InSpiderDevelopServer
{
    public class Service
    {

        #region ... Variables  ...
        
        private GrpcDBService grpcDBService = new GrpcDBService();
        //private WebAPIDBService webDBService = new WebAPIDBService();

        /// <summary>
        /// 
        /// </summary>
        public static Service Instanse = new Service();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public string WorkPath { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void Start(int grpcPort = 5001)
        {
            try
            {

                LoggerService.Service.Info("Service", "Ready to start....");
                RegsiteService();

                StartService();
                grpcDBService.Start(grpcPort);

            }
            catch (Exception ex)
            {
                LoggerService.Service.Erro("Service", "start " + ex.Message);
            }
        }

        private void StartService()
        {
            Cdy.Spider.ApiFactory.Factory.LoadForDevelop();
            Cdy.Spider.ChannelFactory2.Factory.LoadForDevelop();
            Cdy.Spider.DriverFactory.Factory.LoadForDevelop();
            Cdy.Spider.LinkFactory.Factory.LoadForDevelop();

            DevelopManager.Manager.Load(WorkPath);
        }

        /// <summary>
        /// 
        /// </summary>
        private void RegsiteService()
        {
            SecurityManager.Manager.Init();

            ServiceLocator.Locator.Registor<IApiFactory>(ApiFactory.Factory);
            ServiceLocator.Locator.Registor<ICommChannelFactory2>(ChannelFactory2.Factory);
            ServiceLocator.Locator.Registor<IDriverFactory>(DriverFactory.Factory);
            ServiceLocator.Locator.Registor<ILinkFactory>(LinkFactory.Factory);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            grpcDBService.StopAsync();
            //webDBService.StopAsync();
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
