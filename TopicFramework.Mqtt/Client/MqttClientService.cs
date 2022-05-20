using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicFramework.Mqtt.Client
{
    public class MqttClientService : IHostedService, IDisposable
    {

        private IMqttClient _Client = default!;

        private readonly ILogger<MqttClientService> _Logger;
        private readonly MqttFactory _MqttFactory;
        private readonly Action<MqttClientOptionsBuilder> _BuildAction;
        private readonly TopicInstance _TopicInstance;

        public MqttClientService(ILogger<MqttClientService> logger, TopicInstance instance, MqttFactory factory, Action<MqttClientOptionsBuilder> BuildAction)
        {
            _Client = factory.CreateMqttClient();
            _MqttFactory = factory;
            _BuildAction = BuildAction;
            _Logger = logger;
            _TopicInstance = instance;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var configBuilder = _MqttFactory.CreateClientOptionsBuilder();
            _BuildAction(configBuilder);

            _Client.UseApplicationMessageReceivedHandler(new MqttClientHandler(_TopicInstance,_Logger));

            _TopicInstance.MessageOutEvent += _TopicInstance_MessageOutEvent;

            _Logger.LogInformation("Connecting to broker...");
            await _Client.ConnectAsync(configBuilder.Build());
            await _Client.SubscribeAsync("#");
            _Logger.LogInformation("Connected to broker...");
        }

        private async void _TopicInstance_MessageOutEvent(object? sender, Common.TopicMessage e)
        {
            MqttApplicationMessage message = new MqttApplicationMessage()
            {
                ContentType = e.PayloadContentType,
                Payload = Encoding.UTF8.GetBytes(e.Payload),
                Topic = e.Topic,
                QualityOfServiceLevel = (MqttQualityOfServiceLevel)e.Qos
            };

            await _Client.PublishAsync(message);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _Logger.LogInformation("Disconnecting from broker...");
            await _Client.DisconnectAsync();
            _Logger.LogInformation("Disconnected from broker...");
        }

        public void Dispose()
        {
            _Client.Dispose();
        }
    }
}
