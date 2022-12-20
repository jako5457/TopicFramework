using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Server;
using MQTTnet.Formatter;
using Microsoft.Extensions.Logging;
using TopicFramework.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace TopicFramework.Mqtt.Broker
{
    public class MqttBrokerService : IHostedService, IDisposable
    {

        private IMqttServer _Server = default!;

        private readonly ILogger<MqttBrokerService> _Logger;
        private readonly MqttFactory _MqttFactory;
        private readonly Action<MqttServerOptionsBuilder> _BuildAction;
        private readonly TopicInstance _TopicInstance;
        IServiceProvider _ServiceProvider;
        public MqttBrokerService(IServiceProvider serviceProvider,ILogger<MqttBrokerService> logger,TopicInstance instance,MqttFactory factory,Action<MqttServerOptionsBuilder> BuildAction)
        {
            _Server = factory.CreateMqttServer();
            _MqttFactory = factory;
            _BuildAction = BuildAction;
            _Logger = logger;
            _TopicInstance = instance;
            _ServiceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //Configure Server
            var configbuilder = _MqttFactory.CreateServerOptionsBuilder();
            _BuildAction(configbuilder);

            //define events
            var service = _ServiceProvider.GetService<ConnectionMiddlewareProvider>();
            var handler = new MqttBrokerHandler(_TopicInstance, _Logger);
            if (service != null)
            {
                handler = new MqttBrokerHandler(service,_TopicInstance, _Logger);
            }

            configbuilder.WithConnectionValidator(handler);
            _Server.UseApplicationMessageReceivedHandler(handler);

            _TopicInstance.MessageOutEvent += _TopicInstance_MessageOutEvent;

            //Start
            await _Server.StartAsync(configbuilder.Build());
        }

        private async void _TopicInstance_MessageOutEvent(object? sender, Common.TopicMessage e)
        {
            MqttApplicationMessage message = new MqttApplicationMessage()
            {
                Payload = Encoding.UTF8.GetBytes(e.Payload),
                Topic = e.Topic,
                QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)e.Qos,
                ResponseTopic = e.ReturnTopic
            };
            await _Server.PublishAsync(message);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _Server.StopAsync();
        }

        public void Dispose()
        {
            _Server.Dispose();
            _Server = default!;
        }
    }
}
