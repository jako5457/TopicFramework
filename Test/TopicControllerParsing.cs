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

            topicInstance.Initialize(Assembly.GetExecutingAssembly());
        }

    }


    [TopicController("Test")]
    public class TestController : TopicControllerBase
    {
        [TopicHandler("Hello")]
        public void TestTopicHandler()
        {
            Assert.AreEqual("Hello", Message.Topic);
        }
    }
}
