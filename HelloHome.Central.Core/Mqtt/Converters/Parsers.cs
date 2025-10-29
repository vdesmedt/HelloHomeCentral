using System.Text.Json;
using HelloHome.Central.Domain.Messages;
using HelloHome.Central.Domain.Messages.Reports;
using JasperFx.Core.Reflection;
using MQTTnet;

namespace HelloHome.Central.Core.Mqtt.Converters;
public interface IParser
{
    IncomingMessage Parse(MqttApplicationMessage mqtMsg);
}

public abstract class Parser<T> : IParser  where T : IncomingMessage
{
    public IncomingMessage Parse(MqttApplicationMessage mqtMsg)
    {
        var json = mqtMsg.ConvertPayloadToString();
        var rpt = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        if (rpt == null)
            throw new InvalidValueTypeException(typeof(NodeStartedReport), "Json could not be parsed into type");
        return rpt;
    }
}

[MapTopic("started")]
public class Parsers : Parser<NodeStartedReport>;