using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TopicFramework.Attributes;
using TopicFramework.Controllers;

namespace TopicFramework.Parsing
{
    internal static class TopicControllerMapper
    {
        public static List<TopicControllerEntry> Map(Assembly? assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }

            List<Type> controllers = assembly.GetTypes()
                .Where(t => typeof(TopicControllerBase).IsAssignableFrom(t))
                .ToList();

            List<TopicControllerEntry> Entries = new List<TopicControllerEntry>();

            foreach (var type in controllers.Where(c => c.GetCustomAttribute<TopicController>() != null))
            {
                TopicController topicControllerAttib = type.GetCustomAttribute<TopicController>();

                var methods = type.GetMethods()
                                    .Where(m => m.GetCustomAttribute<TopicHandler>() != null)
                                    .ToList();

                List<TopicHandlerEntry> TopicHandlers = new List<TopicHandlerEntry>();

                foreach (var method in methods)
                {
                    TopicHandler topicHandlerAttrib = method.GetCustomAttribute<TopicHandler>();

                    TopicHandlers.Add(new TopicHandlerEntry(topicHandlerAttrib, method.Name));
                }

                Entries.Add(new TopicControllerEntry(topicControllerAttib, type, TopicHandlers));
            }

            return Entries;
        }
    }
}
