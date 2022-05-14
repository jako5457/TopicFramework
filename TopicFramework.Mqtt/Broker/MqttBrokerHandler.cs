using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client.Receiving;
using MQTTnet.Protocol;
using TopicFramework.Common;

namespace TopicFramework.Mqtt.Broker
{
    public class MqttBrokerHandler : IMqttApplicationMessageReceivedHandler
    {
        private readonly TopicInstance _TopicInstance;
        private readonly ILogger _Logger;

        public MqttBrokerHandler(TopicInstance topicInstance,ILogger<MqttBrokerService> logger)
        {
            _TopicInstance = topicInstance;
            _Logger = logger;
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

            if (MqttServiceSettings.DebugLog)
            {
                _Logger.LogDebug($"@@DebugLog@@ \n Topic: {eventArgs.ApplicationMessage.Topic} \n QOS: {(int)eventArgs.ApplicationMessage.QualityOfServiceLevel} ({Enum.GetName<MqttQualityOfServiceLevel>(eventArgs.ApplicationMessage.QualityOfServiceLevel)}) \n ##Payload \n {eventArgs.ApplicationMessage.ConvertPayloadToString()} ##Payload \n @@DebugLog@@");
            }

            await _TopicInstance.ParseTopicAsync(message);
        }
    }
}
