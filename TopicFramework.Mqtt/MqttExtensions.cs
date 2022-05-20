using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicFramework.Mqtt.Broker;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Server;
using MQTTnet.Client.Options;
using TopicFramework.Mqtt.Client;

namespace TopicFramework.Mqtt
{
    public static class MqttExtensions
    {

        public static IServiceCollection AddTfMqttBrokerService(this IServiceCollection serviceCollection,Action<MqttServerOptionsBuilder> action)
        {
            serviceCollection.AddHostedService<MqttBrokerService>(p =>
            {
                return new MqttBrokerService(
                                p.GetRequiredService<ILogger<MqttBrokerService>>(),
                                p.GetRequiredService<TopicInstance>(),
                                p.GetRequiredService<MqttFactory>(), 
                                action);
            });
            return serviceCollection;
        }

        public static IServiceCollection AddTfMqttClientService(this IServiceCollection serviceCollection, Action<MqttClientOptionsBuilder> action)
        {
            serviceCollection.AddHostedService<MqttClientService>(p =>
            {
              return new MqttClientService(
                                p.GetRequiredService<ILogger<MqttClientService>>(),
                                p.GetRequiredService<TopicInstance>(),
                                p.GetRequiredService<MqttFactory>(),
                                action);
            });
            return serviceCollection;
        }

    }
}
