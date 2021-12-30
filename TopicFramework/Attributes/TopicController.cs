using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TopicController : Attribute
    {
        /// <summary>
        /// Sets Class as TopicController
        /// </summary>
        /// <param name="endpoint">a base topic enpoint this controller is listening to.</param>
        public TopicController(string endpoint = "")
        {
            Endpoint = endpoint;
        }

        public string Endpoint { get; }
    }
}
