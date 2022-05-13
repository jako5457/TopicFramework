using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client.Receiving;
using TopicFramework.Common;

namespace TopicFramework.Mqtt.Broker
{
    public class MqttBrokerHandler : IMqttApplicationMessageReceivedHandler
    {
        private readonly TopicInstance _TopicInstance;

        public MqttBrokerHandler(TopicInstance topicInstance)
        {
            _TopicInstance = topicInstance;
        }

        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            TopicMessage message = new TopicMessage()
            {
                Topic = eventArgs.ApplicationMessage.Topic,
                Payload = eventArgs.ApplicationMessage.ConvertPayloadToString(),
                Qos = (int)eventArgs.ApplicationMessage.QualityOfServiceLevel,
                PayloadContentType = eventArgs.ApplicationMessage.ContentType
            };

            await _TopicInstance.ParseTopicAsync(message);
        }
    }
}
