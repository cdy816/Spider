using Cdy.Spider;
using System;

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

            ApiFactory.Factory.LoadForRun();
            DriverFactory.Factory.LoadForRun();
            ChannelFactory.Factory.LoadForRun();


            ServiceLocator.Locator.Registor<IApiFactory>(ApiFactory.Factory);
            ServiceLocator.Locator.Registor<ICommChannelFactory>(ChannelFactory.Factory);
            ServiceLocator.Locator.Registor<IDriverFactory>(DriverFactory.Factory);
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

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CheckNameExit(string name)
        {
            return DeviceManager.CheckExist(name);
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


                Load();
                InterfaceRegistor();

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
            mDriver.Load();
            mChannel.Load();
            mDevice.Load();
            mApi.Load();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InterfaceRegistor()
        {
            ServiceLocator.Locator.Registor<IDriverRuntimeManager>(mDriver);
            ServiceLocator.Locator.Registor<ICommChannelRuntimeManager>(mChannel);
            ServiceLocator.Locator.Registor<IDeviceRuntimeManager>(mDevice);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            try
            {
                LoggerService.Service.Info("Runner", "Ready to start " + Name);

                foreach (var vv in mDevice.Devices)
                {
                    vv.Start();
                }

                
                foreach (var vv in mApi.Apis)
                {
                    vv.Start();
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
        public void Stop()
        {
            foreach (var vv in mApi.Apis)
            {
                vv.Stop();
            }

            foreach (var vv in mDevice.Devices)
            {
                vv.Stop();
            }
            mIsStarted = false;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
