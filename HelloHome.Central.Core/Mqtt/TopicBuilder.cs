namespace HelloHome.Central.Core.Mqtt;

public class TopicBuilder
{
    public TopicBuilder()
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