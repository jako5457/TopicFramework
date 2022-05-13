using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TopicFramework.Attributes;
using TopicFramework.Common;
using TopicFramework.Controllers;
using TopicFramework.Events;
using TopicFramework.Parsing;

namespace TopicFramework
{
    public class TopicInstance
    {
        private List<TopicEvent> Events = new();
        private List<TopicControllerEntry> Controllers = new();

        private IServiceProvider _ServiceProvider = default!;

        /// <summary>
        /// Triggers every time SendAsync is called
        /// </summary>
        public event EventHandler<TopicMessage> MessageOutEvent;

        /// <summary>
        /// Initializes instance and mapping of TopicControllers.
        /// </summary>
        public void Initialize(IServiceProvider serviceProvider, Assembly? assembly = null)
        {
            Controllers = TopicControllerMapper.Map(assembly);
            _ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// Initializes instance and mapping of TopicControllers.
        /// Will also define TopicEvents
        /// </summary>
        internal void Initialize(Assembly? assembly,Action<List<TopicEvent>> action)
        {
            Controllers = TopicControllerMapper.Map(assembly);
            action(Events);
        }

        /// <summary>
        /// Loads ServiceProvider before startup of services
        /// </summary>
        /// <param name="serviceProvider"></param>
        internal void LoadServiceProvider(IServiceProvider serviceProvider)
        {
            _ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// Sends message out to all connected brokers
        /// </summary>
        /// <param name="message">the message to be sent</param>
        /// <returns>Completed task</returns>
        public Task SendAsync(TopicMessage message)
        {
            MessageOutEvent?.Invoke(this, message);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Parses TopicMessage to the correct Controllers and Events.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task ParseTopicAsync(TopicMessage message)
        {

            if (_ServiceProvider == default!)
            {
                throw new InvalidOperationException("Sevice not loaded. Use UseTopicFramework() to load");
            }

            //Getting Relevant topicEvents
            List<TopicEvent> events = Events
                                        .Where(e => e.TopicName == message.Topic)
                                        .ToList();

            List<TopicControllerEntry> topicControllers = new();

            string StartTopic = "";
            string EndTopic = "";

            //Populates Handlers
            if (message.Topic.StartsWith("#/"))
            {
                string NextTopic = message.Topic.Substring(2);
                topicControllers = Controllers
                                        .Where(c => c.Handlers
                                                        .Exists(h => h.HandlerAttribute.Route == NextTopic)
                                        ).ToList();
            }
            else
            {
                string[] Topics = message.Topic.Split("/");
                StartTopic = Topics.FirstOrDefault("");
                EndTopic = string.Join("/",Topics.Skip(1));

                topicControllers = Controllers
                                    .Where(c => c.ControllerAttribute.Endpoint == StartTopic)
                                    .Where(c => c.Handlers.Exists(h => h.HandlerAttribute.Route == EndTopic))
                                    .ToList();
            }

            //Finds and executes handlers one by one
            foreach (TopicControllerEntry controllerEntry in topicControllers)
            {

                var scope = _ServiceProvider.CreateScope();

                List<object> Params = new List<object>();

                foreach (var p in controllerEntry.Parameters)
                {
                    Params.Add(scope.ServiceProvider.GetRequiredService(p));
                }

                foreach (TopicHandlerEntry handlerEntry in controllerEntry.Handlers.Where(h => h.HandlerAttribute.Route == EndTopic))
                {   
                    try
                    {
                        MethodInfo methodInfo = controllerEntry.Type.GetMethod(handlerEntry.MethodName);
                        TopicControllerBase controller = Activator.CreateInstance(controllerEntry.Type,Params.ToArray()) as TopicControllerBase;
                        controller.SetInstance(this, message);

                        methodInfo.Invoke(controller, new object[0]);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                } 
            }

            //Invokes topicEvents
            foreach(TopicEvent topicEvent in Events)
            {
                topicEvent.Action.Invoke(message);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Adds TopicEvent to the Topic call stack
        /// </summary>
        /// <param name="topicEvent">The TopicEvent to be added</param>
        public void AddTopicEvent(TopicEvent topicEvent)
        {
            Events.Add(topicEvent);
        }

        /// <summary>
        /// Removes TopicEvent to the Topic call stack
        /// </summary>
        /// <param name="topicEvent">The TopicEvent to be removed</param>
        public void RemoveTopicEvent(string TopicName)
        {
            Events.RemoveAll(t => t.TopicName == TopicName);
        }

        /// <summary>
        /// Gets a list of Topicevents for this topic
        /// </summary>
        /// <param name="TopicName">the specific name of the topic</param>
        /// <returns></returns>
        public List<TopicEvent> GetTopicEvents(string TopicName)
        {
            return Events.Where(te => te.TopicName == TopicName).ToList();
        }
    }
}
