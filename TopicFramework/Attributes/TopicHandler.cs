using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TopicHandler : Attribute
    {
        /// <summary>
        /// Sets method as TopicHandler
        /// </summary>
        /// <param name="route">the topic this handler is listening to</param>
        public TopicHandler(string route = "")
        {
            Route = route;
        }

        public string Route { get; }
    }
}
