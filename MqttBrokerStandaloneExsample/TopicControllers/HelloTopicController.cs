using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicFramework.Attributes;
using TopicFramework.Common;
using TopicFramework.Controllers;

namespace MqttBrokerStandaloneExsample.TopicControllers
{
    [TopicController("hello")]
    internal class HelloTopicController : TopicControllerBase
    {

        private readonly ILogger<HelloTopicController> _Logger;

        public HelloTopicController(ILogger<HelloTopicController> Logger)
        {
            _Logger = Logger;
        }

        [TopicHandler("world")]
        public async void helloWorld()
        {
            TopicMessage message = new TopicMessage()
                                            .WithTopic("hello/response")
                                            .WithPayload(Message.Payload);

            await Instance.SendAsync(message);
            _Logger.LogInformation("Responded to message: " + Message.Payload);
        }

        [TopicHandler("request")]
        public async void HelloReturn()
        {
            if (Message.HasReturnTopic())
            {
                TopicMessage message = new TopicMessage()
                                            .WithTopic(Message)
                                            .WithPayload($"Recieved: {Message.Payload}");

                await Instance.SendAsync(message);
            }
        }
    }
}
