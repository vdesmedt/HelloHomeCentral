using System.Threading.Channels;
using HelloHome.Central.Common.IoC.Factories;
using HelloHome.Central.Core.IoC.Factories;
using MQTTnet;

namespace HelloHome.Central.Core.Mqtt;

public class MqttMessageHandlerWorker(
    ILogger<MqttMessageHandlerWorker> logger, 
    MqttClientFactory mqttClientFactory,
    IMqttClient mqttClient,
    IMqttPublisher mqttPublisher,
    IMqttSubscriber mqttSubscriber,
    MessageFactory messageFactory,
    IMessageHandlerFactory messageHandlerFactory)
    : BackgroundService
{
    private readonly Channel<MqttApplicationMessageReceivedEventArgs> _inbox
        = Channel.CreateBounded<MqttApplicationMessageReceivedEventArgs>(
            new BoundedChannelOptions(100) { SingleReader = false, SingleWriter = true});
    private readonly Channel<MqttApplicationMessage> _outbox
        = Channel.CreateBounded<MqttApplicationMessage>(
            new BoundedChannelOptions(100) { SingleReader = true, SingleWriter = false});

    private async void HandleMqttMessageAssync(MqttApplicationMessageReceivedEventArgs e, CancellationToken stoppingToken)
    {
        var inMsg = messageFactory.FromMqtt(e.ApplicationMessage);
        var handler = messageHandlerFactory.BuildInNestedScope(inMsg);
        var outMsgs = await handler.Handler.HandleAsync(inMsg, stoppingToken);
        foreach (var outMsg in outMsgs) {
            var mqttOutMsg = messageFactory.FromMessage(outMsg);
            await _outbox.Writer.WriteAsync(mqttOutMsg, stoppingToken);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //Enqueue in Inbox
        mqttClient.ApplicationMessageReceivedAsync += e => {
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
        };
        
        //Subscribe to Node/# Topic
        await mqttClient.SubscribeAsync(
            mqttClientFactory.CreateTopicFilterBuilder().WithTopic("Node/#").Build(), 
            stoppingToken);
        
        //Process inbox in parallel
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
                        HandleMqttMessageAssync(msg, stoppingToken);
                    }
                }
            }, stoppingToken);
        }
        
        //Process outbox in serial
        while (!stoppingToken.IsCancellationRequested)
        {
            await foreach (var msg in _outbox.Reader.ReadAllAsync(stoppingToken))
            {
                await mqttClient.PublishAsync(msg, stoppingToken);
            }
        }
    }
}
