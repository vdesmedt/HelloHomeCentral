using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Common.Configuration;
using HelloHome.Central.Domain.Messages.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace HelloHome.Central.Hub.NodeBridge
{
    public class NodeBridgeApp(
        ILogger<NodeBridgeApp> logger,
        INodeBridge nodeBridge,
        IOptionsMonitor<RFM2PiConfig> rmf2PiConfig)
        : IHostedService
    {
        private readonly ILogger<NodeBridgeApp> _logger = logger;
        private CancellationTokenSource _commCts;
        private Task _commTask;
        private CancellationTokenSource _processCts;
        private Task _processTask;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _commCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _commTask = nodeBridge.Communication(_commCts.Token);
            _processCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _processTask = nodeBridge.Processing(_processCts.Token);
            nodeBridge.Send(new RFM2piConfigCommand {ToRfAddress = 1, HighPower = rmf2PiConfig.CurrentValue.HighPower, NetworkId = rmf2PiConfig.CurrentValue.NetworkId});
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_commTask != null)
            {
                logger.LogInformation("Cancelling communication task");
                _commCts.Cancel();
                await Task.WhenAny(_commTask, Task.Delay(-1, cancellationToken));
                logger.LogInformation("Communication task ended");
            }

            if (_processTask != null)
            {
                logger.LogInformation("Cancelling processing task");
                _processCts.Cancel();
                await Task.WhenAny(_processTask, Task.Delay(-1, cancellationToken));
                logger.LogInformation("Procesing task ended");
            }
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}