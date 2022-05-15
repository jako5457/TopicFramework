using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicFramework.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace TopicFramework
{
    public static class TopicFramworkExtensions
    {
        public static IServiceCollection AddTopicFrameWork(this IServiceCollection colletion,Assembly assembly = null)
        {
            TopicInstance topicInstance = new TopicInstance();
            topicInstance.Initialize(colletion.BuildServiceProvider(), assembly);
            colletion.AddSingleton<TopicInstance>(_ => topicInstance);
            return colletion;
        }

        public static IServiceCollection AddTopicFrameWork(this IServiceCollection colletion, Assembly assembly, Action<List<TopicEvent>> action)
        {
            TopicInstance topicInstance = new TopicInstance();
            topicInstance.Initialize(assembly,action);
            colletion.AddSingleton<TopicInstance>(_ => topicInstance);
            return colletion;
        }

        public static WebApplication? UseTopicFramework(this WebApplication? application)
        {
            var instance = application.Services.GetRequiredService<TopicInstance>();

            if (instance == null)
                throw new InvalidOperationException("Sevice not loaded. Use AddTopicFramework() to load");

            instance.LoadServiceProvider(application.Services);

            return application;
        }

        public static IHost? UseTopicFramework(this IHost? application)
        {

            if (application == null)
                throw new InvalidOperationException("Application is NULL");

            var instance = application.Services.GetRequiredService<TopicInstance>();

            if (instance == null)
                throw new InvalidOperationException("Sevice not loaded. Use AddTopicFramework() to load");

            instance.LoadServiceProvider(application.Services);

            return application;
        }
    }
}
