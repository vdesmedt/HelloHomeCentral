using System;
using System.Threading.Tasks;
using HelloHome.Central.Common;
using HelloHome.Central.Domain;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Hub.Logic;
using NLog;

namespace HelloHome.Central.Hub.Commands
{
    public interface ICreateNodeCommand : ICommand
    {
        Task<Node> ExecuteAsync(long signature, byte rfId, NodeType nodeType);
    }

    public class CreateNodeCommand : ICreateNodeCommand
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ();

        private readonly IUnitOfWork _ctx;
		readonly ITimeProvider _timeProvider;

		public CreateNodeCommand(
		    IUnitOfWork ctx, 
			ITimeProvider timeProvider)
        {
			_timeProvider = timeProvider;
			_ctx = ctx;
        }

        public  Task<Node> ExecuteAsync(long signature, byte rfId, NodeType nodeType)
        {
            var node = new Node
            {
                Signature = signature,
                RfAddress = rfId,
                RfNetwork = Constants.NetworkId,
                Metadata = new NodeMetadata
                {
                    Name = "Newly created",
                    NodeType = nodeType,                    
                },
                AggregatedData = new NodeAggregatedData
                {
                    MaxUpTime = TimeSpan.Zero,
                }
            };
            _ctx.Nodes.Add(node);
            node.AddLog("CRTD");

            return Task.FromResult(node);
        }
    }
}