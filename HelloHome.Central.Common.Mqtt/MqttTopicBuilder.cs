using MQTTnet;

namespace HelloHome.Central.Common.Mqtt;

public interface ITopicBuilder
{
    void Build();
}

public class TopicBuilder(MqttApplicationMessage msg) : ITopicBuilder
{
    private readonly MqttApplicationMessage _msg = msg;

    public void Build()
    {
        throw new NotImplementedException();
    }
}

public static class MqttTopicBuilderExtensions
{
    public static ITopicBuilder BuildTopic(this MqttApplicationMessage msg, int nodeId,
        MqttTopicBuilder.ReportType reportType)
    {
        return new TopicBuilder(msg);
    }
}

public class MqttTopicBuilder
{
    public MqttTopicBuilder()
    {
        
    }
    public enum MessageType : byte
    {
        Report = 0,
        Command = 2,
    }

    public enum ReportType : byte
    {
        NodeStarted = 1
    }

    public enum CommandType : byte
    {
        NodeConfig = 1
    }

    public string For(int nodeId, ReportType reportType)
    {
        return $"node/{nodeId}/report/{reportType}";
    }
    public string For(int nodeId, CommandType commandType)
    {
        return $"node/{nodeId}/command/{commandType}";
    }
}