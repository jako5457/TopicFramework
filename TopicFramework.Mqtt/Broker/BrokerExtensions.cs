using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet.AspNetCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicFramework.Mqtt.Broker
{
    public static class BrokerExtensions
    {

        public static IServiceCollection AddMqttBroker(this IServiceCollection serviceProvider,int port = 1883)
        {

            serviceProvider.AddHostedMqttServerWithServices(options =>
            {
                options.WithDefaultEndpointPort(port);
                options.WithClientMessageQueueInterceptor(new BrokerInteceptor(options.ServiceProvider.GetRequiredService<TopicInstance>()));
            });

            serviceProvider.AddMqttConnectionHandler();
            return serviceProvider;
        }

        public static IApplicationBuilder UseMqttBroker(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMqttServer();
            return applicationBuilder;
        }
    }
}
