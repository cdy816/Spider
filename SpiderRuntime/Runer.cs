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
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public bool IsStarted { get { return mIsStarted; } }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            try
            {
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
            DriverManager.Manager.Load();
            ChannelManager.Manager.Load();
            DeviceManager.Manager.Load();
            APIManager.Manager.Load();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InterfaceRegistor()
        {
            ServiceLocator.Locator.Registor<IDriverRuntimeManager>(DriverManager.Manager);
            ServiceLocator.Locator.Registor<ICommChannelRuntimeManager>(ChannelManager.Manager);
            ServiceLocator.Locator.Registor<IDeviceRuntimeManager>(DeviceManager.Manager);
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
