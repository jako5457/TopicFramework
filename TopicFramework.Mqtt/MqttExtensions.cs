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


    }
}
