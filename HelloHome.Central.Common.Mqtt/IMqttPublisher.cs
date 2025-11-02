namespace HelloHome.Central.Common.Mqtt;

public interface IMqttPublisher
{
    Task PublishAsync(string topic, string payload, CancellationToken cancellationToken);
}