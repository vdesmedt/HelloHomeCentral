using HelloHome.Central.Common.Mqtt.Topic;
using Xunit;

namespace HelloHome.Central.Tests.UnitTests.Common.Mqtt.Topic;

public class TopicBuilderTests
{
    [Fact]   
    public void can_build_command_topic()
    {
        var expected = "node/12/command/restart";
        var actual = HhTopic.ForCommand(Command.Restart).ToNode(12);
        Assert.Equal(expected, actual.ToString());
    }    
}