using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace TopicFramework.RabbitMQ
{
    public static class TfRabbitExtensions
    {
        
        public static IServiceCollection AddTfRabbit(this IServiceCollection services)
        {
            services.AddHostedService<TfRabbitmqService>();
            return services;
        }

        public static IServiceCollection AddTfRabbitConnectionFactory(this IServiceCollection services,Action<ConnectionFactory> action)
        {
            var service = new ConnectionFactory();
            action(service);
            services.AddSingleton<ConnectionFactory>(service);
            return services;
        }
    }
}
