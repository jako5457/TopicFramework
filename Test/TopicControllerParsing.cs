using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Reflection;

namespace Test
{
    [TestClass]
    public class TopicControllerParsing
    {

        [TestMethod]
        public void LoadTopicController()
        {
            TopicInstance topicInstance = new TopicInstance();

            ServiceCollection serviceCollection = new ServiceCollection();

            topicInstance.Initialize(serviceCollection.BuildServiceProvider(),Assembly.GetExecutingAssembly());
        }

        [TestMethod]
        public void InvokeTopicController()
        {
            TopicInstance topicInstance = new TopicInstance();

            ServiceCollection serviceCollection = new ServiceCollection();

            topicInstance.Initialize(serviceCollection.BuildServiceProvider(), Assembly.GetExecutingAssembly());

            topicInstance.ParseTopicAsync(new TopicMessage() { Payload = "Hello", Topic = "Test/Hello" });

            Assert.IsTrue(TestController.Trigger);
            Assert.IsFalse(TestController.Trigger2);
        }

    }


    [TopicController("Test")]
    public class TestController : TopicControllerBase
    {

        public static bool Trigger = false;

        public static bool Trigger2 = false;

        public override void OnInitialize(IServiceProvider serviceProvider)
        {
        }

        [TopicHandler("Hello")]
        public void TestTopicHandler()
        {
            if (Message.Payload == "Hello")
            {
                Trigger = true;
            }
        }

        [TopicHandler("World")]
        public void TestTopicHandler2()
        {
            if (Message.Payload == "Hello")
            {
                Trigger2 = true;
            }
        }
    }
}
