using Cdy.Spider;
using System;
using System.Threading.Tasks;

namespace SpiderRuntime
{
    /// <summary>
    /// 
    /// </summary>
    public class Runer
    {

        #region ... Variables  ...
        private bool mIsStarted = false;

        private DriverManager mDriver;
        private ChannelManager mChannel;
        private DeviceManager mDevice;
        private APIManager mApi;
        private LinkManager mLink;

        /// <summary>
        /// 运行指示文件，运行时产生，退出时删除
        /// </summary>
        private string mRunInfoFile = "";

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        static Runer()
        {
            ServiceLocator.Locator.Registor<ILog>(new ConsoleLogger());
            ValueConvertManager.manager.Init();

            ApiFactory.Factory.LoadForRun();
            LinkFactory.Factory.LoadForRun();
            DriverFactory.Factory.LoadForRun();
            ChannelFactory2.Factory.LoadForRun();


            ServiceLocator.Locator.Registor<IApiFactory>(ApiFactory.Factory);
            ServiceLocator.Locator.Registor<ILinkFactory>(LinkFactory.Factory);
            ServiceLocator.Locator.Registor<ICommChannelFactory2>(ChannelFactory2.Factory);
            ServiceLocator.Locator.Registor<IDriverFactory>(DriverFactory.Factory);
            ServiceLocator.Locator.Registor<IScriptService>(ScriptService.Service);
        }

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public bool IsStarted { get { return mIsStarted; } }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Solution { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CheckNameExit(string name,string solution)
        {
            return DeviceManager.CheckExist(name,solution);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            try
            {

                mDriver = new DriverManager() { Name = Name };
                mDevice = new DeviceManager() { Name = Name };
                mChannel = new ChannelManager() { Name = Name };
                mApi = new APIManager() { Name = Name };
                mLink = new LinkManager() { Name = Name };

                Load();
                InterfaceRegistor();

                foreach (var vv in mLink.Links)
                {
                    ServiceLocator.Locator.Registor<ILink>(vv);
                    vv.Init();
                }

                //
                foreach (var vv in mDevice.Devices)
                {
                    vv.Init();
                }

                //
                foreach (var vv in mApi.Apis)
                {
                    vv.Init();
                }
            }
            catch(Exception ex)
            {
                LoggerService.Service.Erro("Runer_Init", ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Load()
        {
            mDriver.LoadSolution(Solution);
            mChannel.LoadSolution(Solution);
            mDevice.LoadSolution(Solution);
            mApi.LoadSolution(Solution);
            mLink.LoadSolution(Solution);
        }

        /// <summary>
        /// 
        /// </summary>
        private void InterfaceRegistor()
        {
            ServiceLocator.Locator.Registor<IDriverRuntimeManager>(mDriver);
            ServiceLocator.Locator.Registor<ICommChannelRuntimeManager>(mChannel);
            ServiceLocator.Locator.Registor<IDeviceRuntimeManager>(mDevice);
            ServiceLocator.Locator.Registor<IRealDataService>(mDevice);
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            try
            {
                LoggerService.Service.Info("Runner", "Ready to start " + Name);

                foreach (var vv in mLink.Links)
                {
                    vv.Start();
                }

                foreach (var vv in mDevice.Devices)
                {
                    vv.Start();
                }

                foreach (var vv in mApi.Apis)
                {
                    vv.Start();
                }

                Task.Run(() => {
                    Cdy.Spider.RealDataService.WebApiServer.Server.Start();
                });

                try
                {
                    this.mRunInfoFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), Solution+"_"+ Name);
                    var writer = System.IO.File.CreateText(this.mRunInfoFile);
                    writer.WriteLine(DateTime.Now.ToLocalTime().ToString());
                    writer.Close();
                }
                catch
                {

                }

            }
            catch(Exception ex)
            {
                LoggerService.Service.Info("Runner", "Start failed." + ex.Message);
            }
            mIsStarted = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReStart()
        {
            Stop();

            Load();
            InterfaceRegistor();

            foreach (var vv in mLink.Links)
            {
                ServiceLocator.Locator.Registor<ILink>(vv);
                vv.Init();
            }

            //
            foreach (var vv in mDevice.Devices)
            {
                vv.Init();
            }

            //
            foreach (var vv in mApi.Apis)
            {
                vv.Init();
            }
            Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            foreach(var vv in mLink.Links)
            {
                vv.Stop();
            }

            foreach (var vv in mApi.Apis)
            {
                vv.Stop();
            }

            foreach (var vv in mDevice.Devices)
            {
                vv.Stop();
            }

            Cdy.Spider.RealDataService.WebApiServer.Server.Stop();
            mIsStarted = false;

            if(!string.IsNullOrEmpty(mRunInfoFile))
            {
                System.IO.File.Delete(mRunInfoFile);
                mRunInfoFile = "";
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
