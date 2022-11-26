using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicFramework.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddTfConnectionMiddleware(this IServiceCollection services,Action<ConnectionMiddlewareProvider> action)
        {
            services.AddTransient<ConnectionMiddlewareProvider>(i =>
            {
                var p = new ConnectionMiddlewareProvider();
                action(p);
                return p;
            });
            return services;
        }

        public static IServiceCollection AddTfTopicMiddleware(this IServiceCollection services, Action<TopicMiddlewareProvider> action)
        {
            services.AddTransient<TopicMiddlewareProvider>(i =>
            {
                var p = new TopicMiddlewareProvider();
                action(p);
                return p;
            });
            return services;
        }
    }
}
