using TopicFramework.Attributes;
using TopicFramework.Controllers;

namespace ApiWithBroker.TopicControllers
{
    [TopicController("hello")]
    public class HelloWorldTopicController : TopicControllerBase
    {

        private ILogger<HelloWorldTopicController> _logger;

        public HelloWorldTopicController(ILogger<HelloWorldTopicController> logger)
        {
            _logger = logger;
        }

        [TopicHandler("world")]
        public void HelloWorld()
        {
            _logger.LogInformation($"Recieved: {Message.Payload} - Qos: {Message.Qos} ");
        }

    }
}
