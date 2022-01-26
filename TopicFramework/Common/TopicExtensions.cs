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
        public static TopicMessage WithPayload(this TopicMessage message,string Payload)
        {
            message.Payload = Payload;
            return message;
        }

        public static TopicMessage WithJsonPayload(this TopicMessage message, object obj)
        {
            message.Payload = JsonSerializer.Serialize(obj, obj.GetType());
            return message;
        }

        public static T FromJsonPayload<T>(this TopicMessage message)
        {
            return JsonSerializer.Deserialize<T>(message.Payload);
        }

        public static TopicMessage WithTopic(this TopicMessage message,string topic)
        {
            message.Topic = topic;
            return message;
        }

    }
}
