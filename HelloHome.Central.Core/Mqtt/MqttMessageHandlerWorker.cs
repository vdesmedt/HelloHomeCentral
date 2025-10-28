using System.Threading.Channels;
using MQTTnet;

namespace HelloHome.Central.Core.Mqtt;

public class MqttMessageHandlerWorker(
    ILogger<MqttMessageHandlerWorker> logger, 
    MqttClientFactory mqttClientFactory,
    IMqttClient mqttClient,
    IMqttPublisher mqttPublisher,
    IMqttSubscriber mqttSubscriber)
    : BackgroundService
{
    private readonly Channel<MqttApplicationMessageReceivedEventArgs> _inbox
        = Channel.CreateBounded<MqttApplicationMessageReceivedEventArgs>(
            new BoundedChannelOptions(100) { SingleReader = false, SingleWriter = false});

    protected void Handle(MqttApplicationMessageReceivedEventArgs e)
    {
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Task ChannelMqttApplicationMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            if (!_inbox.Writer.TryWrite(e))
            {
                _ = Task.Run(async () =>
                {
                    do
                    {
                        logger.LogWarning("[MQTT] failed to enqueue message in BoundedChannel, retrying in 1 seconds");
                        await Task.Delay(1000, stoppingToken);
                    } while (!_inbox.Writer.TryWrite(e));
                }, stoppingToken);
            }
            return Task.CompletedTask;
        }

        mqttClient.ApplicationMessageReceivedAsync += ChannelMqttApplicationMessage;
        
        await mqttClient.SubscribeAsync(
            mqttClientFactory.CreateTopicFilterBuilder().WithTopic("Node/#").Build(), 
            stoppingToken);
        
        int degreeOfParallelism = Environment.ProcessorCount;
        for (var i = 0; i < degreeOfParallelism; i++)
        {
            var taskIndex = i;
            _ = Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await foreach (var msg in _inbox.Reader.ReadAllAsync(stoppingToken))
                    {
                        logger.LogInformation($"[MQTT] processor {taskIndex} process msg with topic {msg.ApplicationMessage.Topic}");
                        Handle(msg);
                    }
                }
            }, stoppingToken);
        }
    }
}
