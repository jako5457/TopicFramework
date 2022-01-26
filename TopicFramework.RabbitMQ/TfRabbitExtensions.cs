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
            services.AddSingleton<ITfRabbitmqService, TfRabbitmqService>();
            return services;
        }

        public static IServiceCollection AddConnectionFactory(this IServiceCollection services,Action<ConnectionFactory> action)
        {
            var service = new ConnectionFactory();
            action(service);
            services.AddSingleton<ConnectionFactory>(service);
            return services;
        }

        public static void StartTfRabbit(this IApplicationBuilder builder)
        {
            builder.ApplicationServices.GetRequiredService<ITfRabbitmqService>();
        }
    }
}
