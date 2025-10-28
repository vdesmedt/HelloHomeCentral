using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Formatter;

namespace HelloHome.Central.Core.Mqtt;

/// <summary>
/// MqttHosterService manage the connection to the MQTT broker
/// </summary>
/// <param name="logger"></param>
/// <param name="mqttConfig">Hostname, port, client name, etc...</param>
/// <param name="mqttClient">A singleton mqttClient. The application should use one and only one instance of mqttClient.</param>
public class MqttHostedService(
    ILogger<MqttMessageHandlerWorker> logger, 
    IOptions<MqttSettings> mqttConfig, 
    IMqttClient mqttClient)
    : IHostedService
{
    private readonly MqttSettings _cfg = mqttConfig.Value;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var clientOptions = new MqttClientOptionsBuilder()
            .WithClientId(_cfg.ClientId)
            .WithTcpServer(_cfg.Host, _cfg.Port)
            .WithCleanSession(_cfg.CleanSession)
            .WithProtocolVersion(MqttProtocolVersion.V500)
            .WithKeepAlivePeriod(TimeSpan.FromSeconds(_cfg.KeepAliveSeconds))
            .Build();
        mqttClient.ConnectedAsync += e =>
        {
            logger.LogInformation($"[MQTT] Connected: {e.ConnectResult?.ResultCode} at {DateTimeOffset.Now}");
            //TODO: Subscribe to topics (after disconnection) or use freshSession=false ???
            return Task.CompletedTask;
        };
        
        SemaphoreSlim connectLock = new(1, 1);
        mqttClient.DisconnectedAsync += async e =>
        {
            logger.LogInformation($"[MQTT] Disconnected: Reason={e.Reason} Exception={e.Exception?.Message}");
            while (!mqttClient.IsConnected)
            {
                await connectLock.WaitAsync(cancellationToken);
                try
                {
                    if (!mqttClient.IsConnected)
                        await mqttClient.ConnectAsync(clientOptions, cancellationToken);
                }
                catch
                {
                    await Task.Delay(2000, cancellationToken);
                }
                finally
                {
                    connectLock.Release();
                }
            }
        };
        mqttClient.ConnectingAsync += e =>
        {
            logger.LogInformation($"[MQTT] Connecting {e.ClientOptions.ClientId} at {DateTimeOffset.Now} ");
            return Task.CompletedTask;
        };
        mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            logger.LogInformation($"[MQTT] Message Received: Topic={e.ApplicationMessage.Topic} Payload={e.ApplicationMessage.ConvertPayloadToString()}");
            return Task.CompletedTask;
        };

        await mqttClient.ConnectAsync(clientOptions, cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (!mqttClient.IsConnected)
            await mqttClient.DisconnectAsync(new MqttClientDisconnectOptionsBuilder()
                    .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection)
                    .Build()
                , cancellationToken);
    }
}