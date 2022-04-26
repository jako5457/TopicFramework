using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicFramework.Events;

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
            topicInstance.Initialize(assembly,colletion.BuildServiceProvider(),action);
            colletion.AddSingleton<TopicInstance>(_ => topicInstance);
            return colletion;
        }
    }
}
