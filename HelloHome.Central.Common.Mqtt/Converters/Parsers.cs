using System.Text.Json;
using HelloHome.Central.Common.Mqtt.Topic;
using HelloHome.Central.Domain.Messages;
using HelloHome.Central.Domain.Messages.Reports;
using MQTTnet;

namespace HelloHome.Central.Common.Mqtt.Converters;

public interface IMessageParserFactory
{
    IMessageParser GetParserFor(HhTopic topic);
}

public interface IMessageParser
{
    Message ParseMessage(MqttApplicationMessage mqtMsg);
}

public abstract class MessageParser<T> : IMessageParser  where T : Message
{
    public Message ParseMessage(MqttApplicationMessage mqtMsg)
    {
        var json = mqtMsg.ConvertPayloadToString();
        var rpt = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        if (rpt == null)
            throw new JsonException( $"Json could not be parsed into type {typeof(T).Name}");
        return rpt;
    }
}

[MapTopic(Report.Started)]
public class NodeStartedReportParser : MessageParser<NodeStartedReport>;

[MapTopic(Report.Environment)]
public class EnvironmentalReportParser : MessageParser<EnvironmentalReport>;

[MapTopic(Report.Pulses)]
public class PulseReportParser : MessageParser<PulseReport>;