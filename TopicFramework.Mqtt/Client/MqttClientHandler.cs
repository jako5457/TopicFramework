using MQTTnet;
using MQTTnet.Client.Receiving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicFramework.Mqtt.Client
{
    internal class MqttClientHandler : IMqttApplicationMessageReceivedHandler
    {

        private readonly TopicInstance _TopicInstance;

        public MqttClientHandler(TopicInstance topicInstance)
        {
            _TopicInstance = topicInstance;
        }

        public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}
