using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Receiving;
using MQTTnet.Protocol;
using MQTTnet.Server;
using TopicFramework.Common;
using TopicFramework.Middleware;

namespace TopicFramework.Mqtt.Broker
{
    public class MqttBrokerHandler : IMqttApplicationMessageReceivedHandler, IMqttServerConnectionValidator
    {
        private readonly TopicInstance _TopicInstance;
        private readonly ILogger _Logger;
        private readonly ConnectionMiddlewareProvider _ConnectionMiddleware;

        public MqttBrokerHandler(ConnectionMiddlewareProvider connectionMiddleware, TopicInstance topicInstance,ILogger<MqttBrokerService> logger)
        {
            _TopicInstance = topicInstance;
            _Logger = logger;
            _ConnectionMiddleware = connectionMiddleware;
        }

        public MqttBrokerHandler(TopicInstance topicInstance, ILogger<MqttBrokerService> logger)
        {
            _TopicInstance = topicInstance;
            _Logger = logger;
            _ConnectionMiddleware = null!;
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

        public async Task ValidateConnectionAsync(MqttConnectionValidatorContext context)
        {
            BrokerConnectionInfo info = new BrokerConnectionInfo()
            {
                ClientId = context.ClientId,
                UserName = context.Username,
                Password = context.Password,
            };

            if(_ConnectionMiddleware != null)
            {
                if (await _ConnectionMiddleware.ExecuteAsync(_ConnectionMiddleware.ServiceProvider, info))
                {
                    context.ReasonCode = MqttConnectReasonCode.Success;
                }
                else
                {
                    context.ReasonCode = MqttConnectReasonCode.NotAuthorized;
                }
            }
        }
    }
}
