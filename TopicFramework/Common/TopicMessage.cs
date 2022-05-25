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
        public int Qos { get; set; } = 0;

        /// <summary>
        /// The topic the message goes to or from.
        /// </summary>
        public string Topic { get; set; } = String.Empty;

        public string Payload { get; set; } = String.Empty;

        public string PayloadContentType { get; set; } = String.Empty;

        public string ReturnTopic { get; set; } = String.Empty;

    }
}
