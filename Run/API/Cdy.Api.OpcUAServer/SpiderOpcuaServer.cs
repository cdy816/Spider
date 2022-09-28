using Opc.Ua;
using Opc.Ua.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Api.OpcUAServer
{
    /// <summary>
    /// Spider 数据库OPC服务
    /// </summary>
    internal class SpiderOpcuaServer : StandardServer
    {

        /// <summary>
            /// 
            /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
            /// 
            /// </summary>
        public string Password
        {
            get;
            set;
        }


        private ICertificateValidator m_userCertificateValidator;

        /// <summary>
        /// 创建一个Mars数据库的节点服务
        /// </summary>
        /// <param name="server"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        protected override MasterNodeManager CreateMasterNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        {
            Utils.Trace("Creating the Node Managers.");

            List<INodeManager> nodeManagers = new List<INodeManager>();

            // create the custom node managers.
            nodeManagers.Add(new SpiderNodeManager(server, configuration));

            // create master node manager.
            return new MasterNodeManager(server, configuration, null, nodeManagers.ToArray());
        }

        /// <summary>
        /// 加载内置的、不可更改的一些配置信息
        /// </summary>
        /// <returns></returns>
        protected override ServerProperties LoadServerProperties()
        {
            ServerProperties properties = new ServerProperties();

            properties.ManufacturerName = "Cdy";
            properties.ProductName = "Spider Opc Server";
            properties.ProductUri = "http://cdyfoundationorg/Spider";
            properties.SoftwareVersion = Utils.GetAssemblySoftwareVersion();
            properties.BuildNumber = Utils.GetAssemblyBuildNumber();
            properties.BuildDate = Utils.GetAssemblyTimestamp();
            return properties;
        }

        /// <summary>
        /// 创建多语言资源配置
        /// </summary>
        /// <param name="server"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        protected override ResourceManager CreateResourceManager(IServerInternal server, ApplicationConfiguration configuration)
        {
            ResourceManager resourceManager = new ResourceManager(server, configuration);

            System.Reflection.FieldInfo[] fields = typeof(StatusCodes).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            foreach (System.Reflection.FieldInfo field in fields)
            {
                uint? id = field.GetValue(typeof(StatusCodes)) as uint?;

                if (id != null)
                {
                    resourceManager.Add(id.Value, "en-US", field.Name);
                }
            }

            return resourceManager;
        }

        /// <summary>
        /// 服务开始启动
        /// </summary>
        /// <param name="configuration"></param>
        protected override void OnServerStarting(ApplicationConfiguration configuration)
        {
            Utils.Trace("服务启动...");
            base.OnServerStarting(configuration);
            CreateUserIdentityValidators(configuration);
        }

        /// <summary>
        /// 创建用于验证用户Token的对象
        /// </summary>
        private void CreateUserIdentityValidators(ApplicationConfiguration configuration)
        {
            for (int ii = 0; ii < configuration.ServerConfiguration.UserTokenPolicies.Count; ii++)
            {
                UserTokenPolicy policy = configuration.ServerConfiguration.UserTokenPolicies[ii];

                // create a validator for a certificate token policy.
                if (policy.TokenType == UserTokenType.Certificate)
                {
                    // check if user certificate trust lists are specified in configuration.
                    if (configuration.SecurityConfiguration.TrustedUserCertificates != null &&
                        configuration.SecurityConfiguration.UserIssuerCertificates != null)
                    {
                        CertificateValidator certificateValidator = new CertificateValidator();
                        certificateValidator.Update(configuration.SecurityConfiguration).Wait();
                        certificateValidator.Update(configuration.SecurityConfiguration.UserIssuerCertificates,
                            configuration.SecurityConfiguration.TrustedUserCertificates,
                            configuration.SecurityConfiguration.RejectedCertificateStore);

                        // set custom validator for user certificates.
                        m_userCertificateValidator = certificateValidator.GetChannelValidator();
                    }
                }
            }
        }

        /// <summary>
        /// 服务启动完成
        /// </summary>
        /// <param name="server"></param>
        protected override void OnServerStarted(IServerInternal server)
        {
            server.SessionManager.ImpersonateUser += SessionManager_ImpersonateUser;
            base.OnServerStarted(server);
        }

        /// <summary>
        /// 处理用户的Id改变时的情况
        /// </summary>
        /// <param name="session"></param>
        /// <param name="args"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void SessionManager_ImpersonateUser(Session session, ImpersonateEventArgs args)
        {
            // check for a user name token.
            UserNameIdentityToken userNameToken = args.NewIdentity as UserNameIdentityToken;

            if (userNameToken != null)
            {
                args.Identity = VerifyPassword(userNameToken);

                // set AuthenticatedUser role for accepted user/password authentication
                args.Identity.GrantedRoleIds.Add(ObjectIds.WellKnownRole_AuthenticatedUser);

                if (args.Identity is SystemConfigurationIdentity)
                {
                    // set ConfigureAdmin role for user with permission to configure server
                    args.Identity.GrantedRoleIds.Add(ObjectIds.WellKnownRole_ConfigureAdmin);
                    args.Identity.GrantedRoleIds.Add(ObjectIds.WellKnownRole_SecurityAdmin);
                }

                return;
            }

            // check for x509 user token.
            X509IdentityToken x509Token = args.NewIdentity as X509IdentityToken;

            if (x509Token != null)
            {
                VerifyUserTokenCertificate(x509Token.Certificate);
                args.Identity = new UserIdentity(x509Token);
                Utils.Trace("X509 Token Accepted: {0}", args.Identity.DisplayName);

                // set AuthenticatedUser role for accepted certificate authentication
                args.Identity.GrantedRoleIds.Add(ObjectIds.WellKnownRole_AuthenticatedUser);

                return;
            }

            // allow anonymous authentication and set Anonymous role for this authentication
            args.Identity = new UserIdentity();
            args.Identity.GrantedRoleIds.Add(ObjectIds.WellKnownRole_Anonymous);
        }

        /// <summary>
        /// 校验用户名和密码
        /// </summary>
        private IUserIdentity VerifyPassword(UserNameIdentityToken userNameToken)
        {
            string userName = userNameToken.UserName;
            string password = userNameToken.DecryptedPassword;
            if (String.IsNullOrEmpty(userName))
            {
                // an empty username is not accepted.
                throw ServiceResultException.Create(StatusCodes.BadIdentityTokenInvalid,
                    "Security token is not a valid username token. An empty username is not accepted.");
            }

            if (String.IsNullOrEmpty(password))
            {
                // an empty password is not accepted.
                throw ServiceResultException.Create(StatusCodes.BadIdentityTokenRejected,
                    "Security token is not a valid username token. An empty password is not accepted.");
            }

            if(this.UserName==userName && this.Password==password)
            {
                return new UserIdentity(userNameToken);
            }
            else
            {
                // construct translation object with default text.
                TranslationInfo info = new TranslationInfo(
                    "InvalidPassword",
                    "en-US",
                    "Invalid username or password.",
                    userName);

                // create an exception with a vendor defined sub-code.
                throw new ServiceResultException(new ServiceResult(
                    StatusCodes.BadUserAccessDenied,
                    "InvalidPassword",
                    LoadServerProperties().ProductUri,
                    new LocalizedText(info)));
            }

           
        }

        /// <summary>
        /// 校验证书
        /// </summary>
        private void VerifyUserTokenCertificate(X509Certificate2 certificate)
        {
            try
            {
                if (m_userCertificateValidator != null)
                {
                    m_userCertificateValidator.Validate(certificate);
                }
                else
                {
                    CertificateValidator.Validate(certificate);
                }
            }
            catch (Exception e)
            {
                TranslationInfo info;
                StatusCode result = StatusCodes.BadIdentityTokenRejected;
                ServiceResultException se = e as ServiceResultException;
                if (se != null && se.StatusCode == StatusCodes.BadCertificateUseNotAllowed)
                {
                    info = new TranslationInfo(
                        "InvalidCertificate",
                        "en-US",
                        "'{0}' is an invalid user certificate.",
                        certificate.Subject);

                    result = StatusCodes.BadIdentityTokenInvalid;
                }
                else
                {
                    // construct translation object with default text.
                    info = new TranslationInfo(
                        "UntrustedCertificate",
                        "en-US",
                        "'{0}' is not a trusted user certificate.",
                        certificate.Subject);
                }

                // create an exception with a vendor defined sub-code.
                throw new ServiceResultException(new ServiceResult(
                    result,
                    info.Key,
                    LoadServerProperties().ProductUri,
                    new LocalizedText(info)));
            }
        }

        /// <summary>
        /// 服务停止
        /// </summary>
        protected override void OnServerStopping()
        {
            Utils.Trace("服务停止..");
            base.OnServerStopping();
        }
    }
}
