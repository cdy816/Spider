using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cdy.Spider.RealDataService
{

    /// <summary>
    /// 
    /// </summary>
    public  class WebApiServer
    {

        #region ... Variables  ...

        /// <summary>
        /// 
        /// </summary>
        public static WebApiServer Server = new WebApiServer();

        private IHost mHost;
        private bool mIsStarted = false;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool UseHttps { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public bool IsStarted { get { return mIsStarted; } }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            if (!mIsStarted)
            {
                mIsStarted = true;
                mHost = CreateHostBuilder(null).Build();
                mHost.Run();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            if (mIsStarted)
            {
                mIsStarted = false;
                mHost.StopAsync();
            }
        }

        public IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   if (UseHttps)
                   {
                       webBuilder.UseUrls("https://0.0.0.0:" + UserConfigDocument.ConfigInstance.Port);
                   }
                   else
                   {
                       webBuilder.UseUrls("http://0.0.0.0:" + UserConfigDocument.ConfigInstance.Port);
                   }
                   webBuilder.UseStartup<Startup>();
               });
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
