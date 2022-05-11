using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicFramework.Attributes;

namespace TopicFramework.Controllers
{
    internal class TopicControllerEntry
    {
        public TopicControllerEntry(TopicController controller, Type type, List<TopicHandlerEntry> handlers, List<Type> parameters)
        {
            ControllerAttribute = controller;
            Handlers = handlers;
            Type = type;
            Parameters = parameters;
        }

        public TopicController ControllerAttribute { get; }

        public List<Type> Parameters = new List<Type>();

        public List<TopicHandlerEntry> Handlers { get; }

        public Type Type { get; }
    }
}
