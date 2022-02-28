using TopicFramework.Attributes;
using TopicFramework.Common;
using TopicFramework.Controllers;

namespace RabbitMqExample.TopicControllers
{
    [TopicController("hello")]
    public class HelloTopicController : TopicControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [TopicHandler("world")]
        public async void HelloWorld()
        {
            var message = new TopicMessage()
                                .WithTopic("Hello")
                                .WithPayload(Message.Payload);
            await Instance.SendAsync(message);
        }

        [TopicHandler("weather")]
        public async void weather()
        {
            var forecast = new WeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };

            var message = new TopicMessage()
                                .WithTopic("waetherresponse")
                                .WithJsonPayload<WeatherForecast>(forecast);

            await Instance.SendAsync(message);
        }

    }
}
