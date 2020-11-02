using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Common.Configuration;
using HelloHome.Central.Hub.MessageChannel.Messages.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;

namespace HelloHome.Central.Hub.NodeBridge
{
    public class NodeBridgeApp : IHostedService
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(NodeBridgeApp));

        private readonly INodeBridge _nodeBridge;
        private readonly IOptionsMonitor<RFM2PiConfig> _rmf2PiConfig;
        private CancellationTokenSource _commCts;
        private Task _commTask;
        private CancellationTokenSource _processCts;
        private Task _processTask;

        public NodeBridgeApp(INodeBridge nodeBridge, IOptionsMonitor<RFM2PiConfig> rmf2PiConfig)
        {
            _nodeBridge = nodeBridge;
            _rmf2PiConfig = rmf2PiConfig;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _commCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _commTask = _nodeBridge.Communication(_commCts.Token);
            _processCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _processTask = _nodeBridge.Processing(_processCts.Token);
            _nodeBridge.Send(new RFM2piConfigCommand {ToRfAddress = 1, HighPower = _rmf2PiConfig.CurrentValue.HighPower, NetworkId = _rmf2PiConfig.CurrentValue.NetworkId});
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_commTask != null)
            {
                Logger.Info("Cancelling communication task");
                _commCts.Cancel();
                await Task.WhenAny(_commTask, Task.Delay(-1, cancellationToken));
                Logger.Info("Communication task ended");
            }

            if (_processTask != null)
            {
                Logger.Info("Cancelling processing task");
                _processCts.Cancel();
                await Task.WhenAny(_processTask, Task.Delay(-1, cancellationToken));
                Logger.Info("Procesing task ended");
            }
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}