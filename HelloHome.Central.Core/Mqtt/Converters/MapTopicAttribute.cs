namespace HelloHome.Central.Core.Mqtt.Converters;

[AttributeUsage(AttributeTargets.Class)]
public class MapTopicAttribute(string topic) : Attribute
{
    public string Topic { get; init; } = topic;
}