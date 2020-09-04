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
            ApiFactory.Factory.LoadForRun();
            DriverFactory.Factory.LoadForRun();
            ChannelFactory.Factory.LoadForRun();
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
                foreach (var vv in DeviceManager.Manager.Devices)
                {
                    vv.Init();
                }

                //
                foreach (var vv in APIManager.Manager.Apis)
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
            //
            foreach(var vv in DeviceManager.Manager.Devices)
            {
                vv.Start();
            }

            //
            foreach(var vv in APIManager.Manager.Apis)
            {
                vv.Start();
            }

            mIsStarted = true;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            foreach (var vv in APIManager.Manager.Apis)
            {
                vv.Stop();
            }

            foreach (var vv in DeviceManager.Manager.Devices)
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
