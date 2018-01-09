using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Common;
using HelloHome.Central.Domain;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Entities.Includes;
using HelloHome.Central.Hub.Commands;
using HelloHome.Central.Hub.Logic;
using HelloHome.Central.Hub.Logic.RfAddressStrategy;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Commands;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using HelloHome.Central.Hub.Queries;
using NLog;

namespace HelloHome.Central.Hub.Handlers
{
	public class NodeStartedHandler : MessageHandler<NodeStartedReport>
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger ();

		private readonly IFindNodeQuery _findNodeQuery;
		private readonly ICreateNodeCommand _createNodeCommand;
		private readonly ITouchNode _touchNode;
	    private readonly IRfAddressStrategy _rfIdGenerationStrategy;
	    private readonly ITimeProvider _timeProvider;

		public NodeStartedHandler (
			IUnitOfWork dbCtx,
			IFindNodeQuery findNodeQuery,
			ICreateNodeCommand createNodeCommand,
			ITouchNode touchNode,
			IRfAddressStrategy rfIdGenerationStrategy,
			ITimeProvider timeProvider)
			: base (dbCtx)
		{
			_timeProvider = timeProvider;
			_findNodeQuery = findNodeQuery;
			_createNodeCommand = createNodeCommand;
			_touchNode = touchNode;
		    _rfIdGenerationStrategy = rfIdGenerationStrategy;
		}

		protected override async Task HandleAsync (NodeStartedReport request, IList<OutgoingMessage> outgoingMessages, CancellationToken cToken)
		{
			var node = await _findNodeQuery.BySignatureAsync (request.Signature, NodeInclude.Metadata | NodeInclude.Ports );
		    if (node == default (Node)) {
			    Logger.Info(() => $"Node not found based on signature {request.Signature}. A node will be created.");
		        var rfId = _rfIdGenerationStrategy.FindAvailableRfAddress();
				node = await _createNodeCommand.ExecuteAsync (request.Signature, rfId);
			}

			node.AddLog("STRT");
			node.Metadata.Version = request.Version;
			node.AggregatedData.StartupTime = _timeProvider.UtcNow;
		    _touchNode.Touch (node, request.Rssi);
			
			outgoingMessages.Add(new NodeConfigCommand
			{
				Signature = request.Signature,
				ToRfAddress = request.FromRfAddress,
				NewRfAddress = node.RfAddress,
				EnvironmentFreq = node.Metadata.EnvironmentFrequency,
				NodeInfoFreq = node.Metadata.NodeInfoFrequency,
				ExtraFeatures = node.Metadata.ExtraFeatures				
			});
		}
	}
}