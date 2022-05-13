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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TopicFramework.RabbitMQ
{
    public class TfRabbitmqService : IHostedService, IDisposable
    {
        public static bool MqttMode { get; set; } = false;
        public static bool MessageDebug { get; set; } = false;

        private const string _QueueName = "TopicFrameworkQueue";
        
        private readonly ILogger _Logger;
        private readonly TopicInstance _TopicInstance = default!;
        IConnection _Connection = default!;
        IModel _Model = default!;
        EventingBasicConsumer _Consumer = default!;

        public TfRabbitmqService(ILogger<TfRabbitmqService> logger,TopicInstance topicInstance,ConnectionFactory factory)
        {
            _TopicInstance = topicInstance;
            _Connection = factory.CreateConnection();
            _Logger = logger;
            _TopicInstance.MessageOutEvent += _topicInstance_MessageOutEvent;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            
            bool NotConnexted = true;
            do
            {
                try
                {
                    _Logger.LogInformation("Connecting to Broker.");
                    _Model = _Connection.CreateModel();
                    NotConnexted = false;
                }
                catch (Exception e)
                {
                    _Logger.LogWarning("Cant connect to broker. Retrying....");
                    _Logger.LogWarning(e.Message);
                }
            } while (NotConnexted);

            _Logger.LogInformation("Connected to Boker.");

            _Logger.LogInformation($"Creating Queue: {_QueueName}");
            _Model.QueueDeclare(_QueueName);
            _Logger.LogInformation($"Queue created.");

            if (MqttMode)
            {
                _Logger.LogInformation("Binding to Mqtt exchange.");
                _Model.QueueBind(_QueueName, "amq.topic", "#");
            }

            _Consumer = new EventingBasicConsumer(_Model);

            _Consumer.Received += _consumer_Received;
            
            _Model.BasicConsume(_QueueName, true, _Consumer);
            _Logger.LogInformation("Amqp service is running.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _Connection.Close();
            return Task.CompletedTask;
        }

        private async void _consumer_Received(object? sender, BasicDeliverEventArgs e)
        {

            if (MessageDebug)
            {
                _Logger.LogInformation($"From: {e.RoutingKey} - {enc.UTF8.GetString(e.Body.ToArray())}");
            }

            TopicMessage message = new TopicMessage()
            {
                Payload = enc.UTF8.GetString(e.Body.ToArray()),
                Topic = e.RoutingKey.Replace(".", "/"),
                Qos = e.BasicProperties.DeliveryMode,
                PayloadContentType = e.BasicProperties.ContentType
            };

            await _TopicInstance.ParseTopicAsync(message);
        }

        private void _topicInstance_MessageOutEvent(object? sender, TopicMessage e)
        {
            var channel = _Connection.CreateModel();

            var data = enc.UTF8.GetBytes(e.Payload);

            if (MqttMode)
            {
                channel.BasicPublish("amq.topic", e.Topic.Replace("/", "."), null, data);
            }
        }

        public void Dispose()
        {
            _Connection.Dispose();
        }

        
    }
}
