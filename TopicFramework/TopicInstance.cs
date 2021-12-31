using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TopicFramework.Attributes;
using TopicFramework.Common;
using TopicFramework.Controllers;
using TopicFramework.Events;
using TopicFramework.Parsing;

namespace TopicFramework
{
    public class TopicInstance
    {
        private List<TopicEvent> Events = new();
        private List<TopicControllerEntry> Controllers = new();

        /// <summary>
        /// Initializes instance and mapping of TopicControllers.
        /// </summary>
        public void Initialize(Assembly? assembly = null)
        {
            Controllers = TopicControllerMapper.Map(assembly);
        }

        /// <summary>
        /// Parses TopicMessage to the correct Controllers and Events.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task ParseTopic(TopicMessage message)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Adds TopicEvent to the Topic call stack
        /// </summary>
        /// <param name="topicEvent">The TopicEvent to be added</param>
        public void AddTopicEvent(TopicEvent topicEvent)
        {
            Events.Add(topicEvent);
        }

        /// <summary>
        /// Removes TopicEvent to the Topic call stack
        /// </summary>
        /// <param name="topicEvent">The TopicEvent to be removed</param>
        public void RemoveTopicEvent(string TopicName)
        {
            Events.RemoveAll(t => t.TopicName == TopicName);
        }

        /// <summary>
        /// Gets a list of Topicevents for this topic
        /// </summary>
        /// <param name="TopicName">the specific name of the topic</param>
        /// <returns></returns>
        public List<TopicEvent> GetTopicEvents(string TopicName)
        {
            return Events.Where(te => te.TopicName == TopicName).ToList();
        }
    }
}
