using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicFramework.Attributes;

namespace TopicFramework.Controllers
{
    internal class TopicHandlerEntry
    {
        public TopicHandlerEntry(TopicHandler handlerAttribute, string methodName)
        {
            HandlerAttribute = handlerAttribute;
            MethodName = methodName;
        }

        public TopicHandler HandlerAttribute { get; }

        public string MethodName { get; }
    }
}
