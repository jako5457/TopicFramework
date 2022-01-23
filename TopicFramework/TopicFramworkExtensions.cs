using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicFramework
{
    public static class TopicFramworkExtensions
    {
        public static IServiceCollection AddTopicFrameWork(this IServiceCollection colletion)
        {
            TopicInstance topicInstance = new TopicInstance();
            topicInstance.Initialize(Assembly.GetExecutingAssembly());
            colletion.AddSingleton<TopicInstance>(_ => topicInstance);
            return colletion;
        }
    }
}
