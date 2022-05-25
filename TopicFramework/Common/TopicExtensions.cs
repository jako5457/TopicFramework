using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TopicFramework.Common
{
    public static class TopicExtensions
    {
        /// <summary>
        /// Adds a String payload to the message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="Payload">The string message to be added</param>
        /// <returns></returns>
        public static TopicMessage WithPayload(this TopicMessage message,string Payload)
        {
            message.Payload = Payload;
            return message;
        }

        /// <summary>
        /// Adds object to Message as a json string.
        /// Overrides the payload.
        /// </summary>
        /// <param name="type">The type to be serialized to</param>
        /// <param name="message"></param>
        /// <param name="obj">Object to serialized into the message</param>
        /// <returns></returns>
        public static TopicMessage WithJsonPayload(this TopicMessage message,Type type,object obj)
        {
            message.Payload = JsonSerializer.Serialize(obj,type);
            return message;
        }

        /// <summary>
        /// Adds object to Message as a json string. 
        /// Overrides the payload.
        /// </summary>
        /// <typeparam name="T">The type to be serialized to</typeparam>
        /// <param name="message"></param>
        /// <param name="obj">Object to serialized into the message</param>
        /// <returns></returns>
        public static TopicMessage WithJsonPayload<T>(this TopicMessage message, T obj)
        {
            message.Payload = JsonSerializer.Serialize<T>(obj);
            return message;
        }

        /// <summary>
        /// Deserializes the payload to a object.
        /// </summary>
        /// <typeparam name="T">The type to be deserialized to</typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public static T FromJsonPayload<T>(this TopicMessage message)
        {
            return JsonSerializer.Deserialize<T>(message.Payload);
        }

        /// <summary>
        /// Overrides the topic
        /// </summary>
        /// <param name="message"></param>
        /// <param name="topic">The topic to be set</param>
        /// <returns></returns>
        public static TopicMessage WithTopic(this TopicMessage message,string topic)
        {
            message.Topic = topic;
            return message;
        }

        /// <summary>
        /// Overrides the topic
        /// </summary>
        /// <param name="message"></param>
        /// <param name="topicMessage">The message that have a ReturntTopic attached</param>
        /// <returns></returns>
        public static TopicMessage WithTopic(this TopicMessage message, TopicMessage topicMessage)
        {
            message.Topic = topicMessage.ReturnTopic;
            return message;
        }

        /// <summary>
        /// Overrides ReturnTopic
        /// </summary>
        /// <param name="message"></param>
        /// <param name="Topic">The topic to be set</param>
        /// <returns></returns>
        public static TopicMessage WithReturnTopic(this TopicMessage message, string Topic)
        {
            message.ReturnTopic = Topic;
            return message;
        }

        public static bool HasReturnTopic(this TopicMessage message) {

            if (message.ReturnTopic != string.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
