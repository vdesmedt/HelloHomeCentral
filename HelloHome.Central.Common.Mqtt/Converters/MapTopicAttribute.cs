using HelloHome.Central.Common.Mqtt.Topic;

namespace HelloHome.Central.Common.Mqtt.Converters;

[AttributeUsage(AttributeTargets.Class)]
public class MapTopicAttribute : Attribute
{
    public Report? Report { get; init; }
    public Command? Command { get; init; }
    public MapTopicAttribute(Report report)
    {
        Report = report;
        Topic = $"{report}".ToLower();
    }

    public MapTopicAttribute(Command command)
    {
        Command = command;
        Topic = $"{command}".ToLower();
    }
    
    public string Topic { get; init; }
}