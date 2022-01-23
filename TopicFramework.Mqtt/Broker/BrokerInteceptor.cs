using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicFramework.Common;

namespace TopicFramework.Mqtt.Broker
{
    internal class BrokerInteceptor : IMqttServerClientMessageQueueInterceptor
    {

        private TopicInstance _topicInstance;

        public BrokerInteceptor(TopicInstance instance)
        {
            _topicInstance = instance;
        }

        public async Task InterceptClientMessageQueueEnqueueAsync(MqttClientMessageQueueInterceptorContext context)
        {
            TopicMessage message = new TopicMessage()
            {
                Topic = context.ApplicationMessage.Topic,
                Payload = Encoding.UTF8.GetString(context.ApplicationMessage.Payload),
                Qos = ((int)context.ApplicationMessage.QualityOfServiceLevel)
            };

            await _topicInstance.ParseTopicAsync(message);
        }
    }
}
