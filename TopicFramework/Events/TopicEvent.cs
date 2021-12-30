using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicFramework.Common;

namespace TopicFramework.Events
{
    /// <summary>
    /// A event that can be triggered by a specific topic
    /// </summary>
    public class TopicEvent
    {
        /// <summary>
        /// Creates a TopicEvent
        /// </summary>
        /// <param name="topicName"></param>
        /// <param name="action"></param>
        public TopicEvent(string topicName, Action<TopicMessage> action)
        {
            TopicName = topicName;
            Action = action;
        }

        /// <summary>
        /// Name of the topic to trigger on
        /// </summary>
        public string TopicName { get; private set; }

        /// <summary>
        /// Action to trigger
        /// </summary>
        public Action<TopicMessage> Action { get; private set; }
    }
}
