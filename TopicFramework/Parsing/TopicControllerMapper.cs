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
        /// <summary>
        /// Maps all TopicControllers in the assembly
        /// </summary>
        /// <param name="assembly">Lists of all topicControlllers in the assembly</param>
        /// <returns></returns>
        public static List<TopicControllerEntry> Map(Assembly? assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
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

                var Constructor = type
                                    .GetConstructors()
                                    .Where(c => c.IsPublic)
                                    .FirstOrDefault();

                List<Type> Params = new List<Type>();

                if (Constructor != null)
                {
                    var parameters = Constructor.GetParameters();
                    foreach (var param in parameters)
                        Params.Add(param.ParameterType);
                }

                List<TopicHandlerEntry> TopicHandlers = new List<TopicHandlerEntry>();

                foreach (var method in methods)
                {
                    TopicHandler topicHandlerAttrib = method.GetCustomAttribute<TopicHandler>();
                    TopicHandlers.Add(new TopicHandlerEntry(topicHandlerAttrib, method.Name));
                }

                Entries.Add(new TopicControllerEntry(topicControllerAttib, type, TopicHandlers,Params));
            }

            return Entries;
        }
    }
}
