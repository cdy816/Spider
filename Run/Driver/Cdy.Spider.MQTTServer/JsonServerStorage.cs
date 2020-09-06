// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonServerStorage.cs" company="Haemmer Electronics">
//   Copyright (c) 2020 All rights reserved.
// </copyright>
// <summary>
//   The JSON server storage.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cdy.Spider.MQTTServer
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using MQTTnet;
    using MQTTnet.Server;

    using Newtonsoft.Json;

    /// <inheritdoc cref="IMqttServerStorage"/>
    /// <summary>
    /// The JSON server storage.
    /// </summary>
    /// <seealso cref="IMqttServerStorage"/>
    public class JsonServerStorage : IMqttServerStorage
    {
        /// <summary>
        /// The file name.
        /// </summary>
        private readonly string filename = Path.Combine(Directory.GetCurrentDirectory(), "Retained.json");

        /// <summary>
        /// Clears the file.
        /// </summary>
        public void Clear()
        {
            if (File.Exists(this.filename))
            {
                File.Delete(this.filename);
            }
        }

        /// <inheritdoc cref="IMqttServerStorage"/>
        /// <summary>
        /// Loads the retained messages.
        /// </summary>
        /// <returns>A <see cref="IList{T}"/> of <see cref="MqttApplicationMessage"/>.</returns>
        /// <seealso cref="IMqttServerStorage"/>
        public async Task<IList<MqttApplicationMessage>> LoadRetainedMessagesAsync()
        {
            await Task.CompletedTask;

            if (!File.Exists(this.filename))
            {
                return new List<MqttApplicationMessage>();
            }

            try
            {
                var json = await File.ReadAllTextAsync(this.filename);
                return JsonConvert.DeserializeObject<List<MqttApplicationMessage>>(json);
            }
            catch
            {
                return new List<MqttApplicationMessage>();
            }
        }

        /// <inheritdoc cref="IMqttServerStorage"/>
        /// <summary>
        /// Saves the retained messages to a file.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        /// <seealso cref="IMqttServerStorage"/>
        public async Task SaveRetainedMessagesAsync(IList<MqttApplicationMessage> messages)
        {
            await Task.CompletedTask;
            var json = JsonConvert.SerializeObject(messages);
            await File.WriteAllTextAsync(this.filename, json);
        }
    }
}
