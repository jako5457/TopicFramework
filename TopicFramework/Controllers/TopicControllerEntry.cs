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
        public TopicControllerEntry(TopicController controller, Type type, List<TopicHandlerEntry> handlers)
        {
            ControllerAttribute = controller;
            Handlers = handlers;
            Type = type;
        }

        public TopicController ControllerAttribute { get; }

        public List<TopicHandlerEntry> Handlers { get; }

        public Type Type { get; }
    }
}
