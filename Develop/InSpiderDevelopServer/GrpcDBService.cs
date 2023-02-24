using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using System.Threading;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Cdy.Spider;

namespace InSpiderDevelopServer
{
    public class GrpcDBService
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        public static GrpcDBService Service = new GrpcDBService();
        private IHost mhost;
        private int mPort=5001;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        ///// <summary>
        ///// 
        ///// </summary>
        //public GrpcDBService()
        //{
        //    DBDevelopService.SecurityManager.Manager.Init();
        //    //注册日志
        //    ServiceLocator.Locator.Registor<ILog>(new ConsoleLogger());
        //}

        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void Start(int port)
        {
            try
            {
                LoggerService.Service.Info("GrpcDBService", "Ready to start to GrpcDBService......");
                StartAsync("0.0.0.0", port);
            }
            catch(Exception ex)
            {
                LoggerService.Service.Erro("GrpcDBService",ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public async void StartAsync(string ip, int port)
        {
            string sip = ip;
            mPort = port;
            if (!IsWin7)
            {
                if (!sip.StartsWith("https://"))
                {
                    sip = "https://" + ip;
                }
            }
            else
            {
                if (!sip.StartsWith("http://"))
                {
                    sip = "http://" + ip;
                }
            }
            sip += ":" + port;
            mhost = CreateHostBuilder(sip,port).Build();
           
            LoggerService.Service.Info("GrpcDBService", "启动服务:"+ sip);
            try
            {
                await mhost.StartAsync();
            }
            catch(Exception ex)
            {
                LoggerService.Service.Erro("GrpcDBService", ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async void StopAsync()
        {
            LoggerService.Service.Info("GrpcDBService", "关闭服务:");
            await mhost.StopAsync();
            mhost.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsWin7
        {
            get
            {
                return Environment.OSVersion.Version.Major < 8 && Environment.OSVersion.Platform == PlatformID.Win32NT;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverUrl"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string serverUrl,int mPort) =>
           Host.CreateDefaultBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.AddFilter("Grpc", LogLevel.Warning);
                    logging.SetMinimumLevel(LogLevel.Warning);
                })
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   if (IsWin7)
                   {
                       //Win 7 的情况下使用 不支持TLS 的 HTTP/2
                       webBuilder.ConfigureKestrel(options =>
                       {
                           options.Listen(System.Net.IPAddress.Parse("0.0.0.0"), mPort, a => a.Protocols =
                                Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2);
                       });
                   }
                   else
                   {
                       string spath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mars.pfx");
                       if (System.IO.File.Exists(spath))
                       {
                           webBuilder.UseKestrel(options =>
                           {
                               options.ListenAnyIP(mPort, listenOps =>
                               {
                                   listenOps.UseHttps(callback =>
                                   {
                                       callback.AllowAnyClientCertificate();
                                       callback.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(spath, "mars");
                                   });
                               });
                           });
                       }
                       else
                       {
                           webBuilder.UseUrls(serverUrl);
                       }
                   }
                   webBuilder.UseStartup<Startup>();
               });

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
