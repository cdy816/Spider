using Opc.Ua;
using Opc.Ua.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Api.OpcUAServer
{
    /// <summary>
    /// Opc 服务
    /// </summary>
    internal class OPCServer
    {
        public static OPCServer Server = new OPCServer();

        private ApplicationInstance application;

        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; } = 8020;

        /// <summary>
        /// 
        /// </summary>
        public bool NoneSecurityMode { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public void Start()
        {
            try
            {
                string certPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "CertificateStores");
                var config = new ApplicationConfiguration()
                {
                    ApplicationName = "spideropcua",
                    ApplicationUri = Utils.Format(@"urn:{0}:spideropcua", System.Net.Dns.GetHostName()),
                    ApplicationType = ApplicationType.Server,
                    ServerConfiguration = new ServerConfiguration()
                    {
                        BaseAddresses = { "opc.tcp://localhost:"+ Port, "https://localhost:"+(Port+1) },
                        MinRequestThreadCount = 5,
                        MaxRequestThreadCount = 100,
                        MaxQueuedRequestCount = 200,
                      
                    },
                    SecurityConfiguration = new SecurityConfiguration
                    {
                        ApplicationCertificate = new CertificateIdentifier { StoreType = "X509Store", StorePath = @"CurrentUser\Spider", SubjectName = Utils.Format(@"CN={0},S=beijing,E=cdy816@hotmail.com,C=CN,O=cdy, DC={1}", "SpiderOpcua", System.Net.Dns.GetHostName()) },
                        TrustedIssuerCertificates = new CertificateTrustList { StoreType = @"Directory", StorePath = certPath + @"\Issuer" },
                        TrustedPeerCertificates = new CertificateTrustList { StoreType = @"Directory", StorePath = certPath + @"\Peer" },
                        RejectedCertificateStore = new CertificateTrustList { StoreType = @"Directory", StorePath = certPath + @"\Rejected" },
                        AutoAcceptUntrustedCertificates = true,
                        AddAppCertToTrustedStore = true,
                    },
                    TransportConfigurations = new TransportConfigurationCollection(),
                    TransportQuotas = new TransportQuotas { OperationTimeout = 15000 },
                    ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 },
                    TraceConfiguration = new TraceConfiguration(),
                  
                };
                          
                config.ServerConfiguration.SecurityPolicies.Add(new ServerSecurityPolicy() { SecurityMode = MessageSecurityMode.Sign, SecurityPolicyUri = "http://opcfoundation.org/UA/SecurityPolicy#Basic256Sha256" });
                
                config.ServerConfiguration.SecurityPolicies.Add(new ServerSecurityPolicy() { SecurityMode = MessageSecurityMode.SignAndEncrypt, SecurityPolicyUri = "http://opcfoundation.org/UA/SecurityPolicy#Basic256Sha256" });
                
                if(NoneSecurityMode)
                config.ServerConfiguration.SecurityPolicies.Add(new ServerSecurityPolicy() { SecurityMode = MessageSecurityMode.None, SecurityPolicyUri = "http://opcfoundation.org/UA/SecurityPolicy#None" });

                config.Validate(ApplicationType.Server).GetAwaiter().GetResult();
                if (config.SecurityConfiguration.AutoAcceptUntrustedCertificates)
                {
                    config.CertificateValidator.CertificateValidation += (s, e) => 
                    {
                        e.Accept = (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted);
                    };
                }

                application = new ApplicationInstance
                {
                    ApplicationName = "spideropcua",
                    ApplicationType = ApplicationType.Server,
                    ApplicationConfiguration = config
                };
               
                bool certOk = application.CheckApplicationInstanceCertificate(false, 0).Result;
                if (!certOk)
                {
                    Console.WriteLine("证书验证失败!");
                }

                var dis = new DiscoveryServerBase();
                // start the server.
                application.Start(new SpiderOpcuaServer() { UserName = UserName, Password= Password }).Wait();

                Console.WriteLine("Opc Server 提供服务的地址:"+ "opc.tcp://localhost:" + Port +","+ "https://localhost:" + (Port + 1));
               
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("启动OPC-UA服务端触发异常:" + ex.Message);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            application?.Stop();
        }

    }
}
