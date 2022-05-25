using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client.Receiving;
using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicFramework.Common;

namespace TopicFramework.Mqtt.Client
{
    internal class MqttClientHandler : IMqttApplicationMessageReceivedHandler
    {

        private readonly TopicInstance _TopicInstance;
        private readonly ILogger _Logger;

        public MqttClientHandler(TopicInstance topicInstance,ILogger<MqttClientService> logger)
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
                PayloadContentType = eventArgs.ApplicationMessage.ContentType,
                ReturnTopic = eventArgs.ApplicationMessage.ResponseTopic
            };

            if (MqttServiceSettings.DebugLog)
            {
                _Logger.LogInformation($"\n@@DebugLog@@ \nTopic: {eventArgs.ApplicationMessage.Topic} \nQOS: {(int)eventArgs.ApplicationMessage.QualityOfServiceLevel} ({Enum.GetName<MqttQualityOfServiceLevel>(eventArgs.ApplicationMessage.QualityOfServiceLevel)}) \n##Payload \n {eventArgs.ApplicationMessage.ConvertPayloadToString()} \n##Payload \n@@DebugLog@@\n");
            }

            await _TopicInstance.ParseTopicAsync(message);
        }
    }
}
