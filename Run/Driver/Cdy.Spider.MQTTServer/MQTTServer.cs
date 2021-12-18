//==============================================================
//  Copyright (C) 2020 Chongdaoyang Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/9/4 22:49:33 .
//  Version 1.0
//  CDYWORK
//==============================================================

using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdy.Spider.MQTTServer
{
    /// <summary>
    /// 
    /// </summary>
    public class MQTTServer
    {

        #region ... Variables  ...

        /// <summary>
        /// The MQTT server.
        /// </summary>
        private IMqttServer mqttServer;

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
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; } = 1833;

        /// <summary>
        /// 
        /// </summary>
        public bool IsStarted
        {
            get
            {
                return mIsStarted;
            }
        }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            this.mqttServer = new MqttFactory().CreateMqttServer();
        }

        /// <summary>
        /// 
        /// </summary>
        public async void Start()
        {
            if (mIsStarted)
            {
                return;
            }
            mIsStarted = true;
            var storage = new JsonServerStorage();
            storage.Clear();

            var options = new MqttServerOptions();
            options.DefaultEndpointOptions.Port = Port;
            options.Storage = storage;
            options.EnablePersistentSessions = true;
            options.ConnectionValidator = new MqttServerConnectionValidatorDelegate(
                c =>
                {
                    if (c.ClientId.Length < 10)
                    {
                        c.ReasonCode = MqttConnectReasonCode.ClientIdentifierNotValid;
                        return;
                    }

                    if (c.Username != UserName)
                    {
                        c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                        return;
                    }

                    if (c.Password != Password)
                    {
                        c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                        return;
                    }

                    c.ReasonCode = MqttConnectReasonCode.Success;

                    LoggerService.Service.Info("MQTTServer", $"Client {c.Endpoint} connection successful.");

                });

            try
            {
                await this.mqttServer.StartAsync(options);
            }
            catch (Exception ex)
            {
                LoggerService.Service.Erro("MQTTServer", ex.Message);
                await this.mqttServer.StopAsync();
                this.mqttServer = null;
                mIsStarted = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public async void Stop()
        {
            await mqttServer.StopAsync();
            mqttServer = null;
            mIsStarted = false;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
