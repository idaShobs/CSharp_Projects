using NUnit.Framework;

namespace Stomp
{
    public class FrameserializerTests
    {
        Frame frame;
        string frameAsString;
        [SetUp]
        public void Setup()
        {
            frame = new Frame("MESSAGE", "Hello world");
            frame["message-id"] = "1997";
            frame["subscription"] = "0";
            frame["destination"] = "/queue/a";
            frameAsString = $"MESSAGE\n" +
                $"content-length:11\n" +
                $"message-id:1997\n" +
                $"subscription:0\n" +
                $"destination:/queue/a\n" +
                $"\nHello world\0";
        }
        [Test]
        public void Frame_is_properly_serialized()
        {
            FrameSerializer serializer = new FrameSerializer();
            Assert.That(serializer.Serialize(frame), Is.EqualTo(frameAsString));
        }
        
        [TestCase("message-id")]
        [TestCase("subscription")]
        [TestCase("destination")]
        public void Headers_in_Frame_Are_Assigned_As_Key(string headername)
        {
            FrameSerializer serializer = new FrameSerializer();
            Frame deserialized = serializer.Deserialize(frameAsString);
            Assert.IsTrue(deserialized.Headers.ContainsKey(headername));
        }
        [Test]
        public void Command_in_Frame_Properly_Deserialized()
        {
            FrameSerializer serializer = new FrameSerializer();
            Frame deserialized = serializer.Deserialize(frameAsString);
            Assert.That(deserialized.Command, Is.EqualTo("MESSAGE"));
        }
        [Test]
        public void Body_in_Frame_Properly_Deserialized()
        {
            FrameSerializer serializer = new FrameSerializer();
            Frame deserialized = serializer.Deserialize(frameAsString);
            Assert.That(deserialized.Body, Is.EqualTo("Hello world"));
        }
    }
}