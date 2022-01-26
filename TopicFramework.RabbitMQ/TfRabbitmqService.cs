using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopicFramework.Common;
using TopicFramework;
using RabbitMQ.Client;
using enc = System.Text.Encoding;
using RabbitMQ.Client.Events;

namespace TopicFramework.RabbitMQ
{
    public class TfRabbitmqService : ITfRabbitmqService
    {
        public static bool HasMqtt { get; set; }

        private const string QueueName = "TopicFrameworkQueue";

        private readonly TopicInstance _topicInstance;
        IConnection _connection;
        EventingBasicConsumer _consumer;

        public TfRabbitmqService(TopicInstance topicInstance,ConnectionFactory factory)
        {
            _topicInstance = topicInstance;
            _connection = factory.CreateConnection();

            _topicInstance.MessageOutEvent += _topicInstance_MessageOutEvent;

            SetupConsumer();
        }

        private void SetupConsumer()
        {
            var model = _connection.CreateModel();

            model.QueueDeclare(QueueName);

            if (HasMqtt)
            {
                model.QueueBind(QueueName,"amq.topic","#");
            }

            _consumer = new EventingBasicConsumer(model);

            _consumer.Received += _consumer_Received;

            model.BasicConsume(QueueName, true, _consumer);

        }

        private async void _consumer_Received(object? sender, BasicDeliverEventArgs e)
        {
            TopicMessage message = new TopicMessage()
            {
                Payload = enc.UTF8.GetString(e.Body.ToArray()),
                Topic = e.RoutingKey.Replace(".", "/"),
                Qos = 2
            };

            await _topicInstance.ParseTopicAsync(message);
        }

        private void _topicInstance_MessageOutEvent(object? sender, TopicMessage e)
        {
            var channel = _connection.CreateModel();

            var data = enc.UTF8.GetBytes(e.Payload);

            if (HasMqtt)
            {
                channel.BasicPublish("amq.topic", e.Topic.Replace("/", "."), null, data);
            }
        }
    }
}
