using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace HelloHome.Central.Hub.NodeBridge
{
    public class NodeBridgeApp : IHostedService
    {
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
                _commCts.Cancel();
                await Task.WhenAny(_commTask, Task.Delay(-1, cancellationToken));
            }

            if (_processTask != null)
            {
                _processCts.Cancel();
                await Task.WhenAny(_processTask, Task.Delay(-1, cancellationToken));
            }
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}