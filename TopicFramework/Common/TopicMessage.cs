using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicFramework.Common
{
    public class TopicMessage
    {
        /// <summary>
        /// Quality Of Service of the topic
        /// </summary>
        public int Qos { get; set; }

        /// <summary>
        /// The topic the message goes to.
        /// </summary>
        public string Topic { get; set; }

        public string Payload { get; set; }
    }
}
