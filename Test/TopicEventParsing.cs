namespace Test
{
    [TestClass]
    public class TopicEventParsing
    {
        [TestMethod]
        public void AddTopicEvent()
        {
            TopicInstance topicInstance = new TopicInstance();

            Action<TopicMessage> e = (msg) => Assert.Fail();

            topicInstance.AddTopicEvent(new TopicEvent("Test",e));

            int TopicCount = topicInstance.GetTopicEvents("Test").Count;

            Assert.AreEqual(1, TopicCount);
        }

        [TestMethod]
        public void RemoveTopicEvent()
        {
            TopicInstance topicInstance = new TopicInstance();

            var te = new TopicEvent("Test",(msg) => Assert.Fail());

            topicInstance.AddTopicEvent(te);

            topicInstance.RemoveTopicEvent("Test");

            int TopicCount = topicInstance.GetTopicEvents("Test").Count;

            Assert.AreEqual(0, TopicCount);
        }

        [TestMethod]
        public void TriggerTopicEvent()
        {
            string ExpectedMessage = "Hello";
            TopicInstance topicInstance = new TopicInstance();
            
            Action<TopicMessage> e = (msg) => Assert.AreEqual(ExpectedMessage,msg.Payload);

            topicInstance.AddTopicEvent(new TopicEvent("Test", e));

            int TopicCount = topicInstance.GetTopicEvents("Test").Count;

            TopicMessage message = new TopicMessage() { Payload = ExpectedMessage, Topic = "Test", };

            topicInstance.ParseTopicAsync(message);
        }
    }
}