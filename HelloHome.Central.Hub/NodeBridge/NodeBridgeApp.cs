using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;

namespace HelloHome.Central.Hub.NodeBridge
{
    public class NodeBridgeApp : IHostedService
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(NodeBridgeApp));

        private readonly INodeBridge _nodeBridge;
        private CancellationTokenSource _commCts;
        private Task _commTask;
        private CancellationTokenSource _processCts;
        private Task _processTask;

        public NodeBridgeApp(INodeBridge nodeBridge)
        {
            _nodeBridge = nodeBridge;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _commCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _commTask = _nodeBridge.Communication(_commCts.Token);
            _processCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _processTask = _nodeBridge.Processing(_processCts.Token);
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