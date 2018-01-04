using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Common;
using HelloHome.Central.Domain;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Entities.Includes;
using HelloHome.Central.Hub.Commands;
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
		readonly ITouchNode _touchNode;
	    private readonly IRfAddressStrategy _rfIdGenerationStrategy;
	    readonly ITimeProvider _timeProvider;

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
			var node = await _findNodeQuery.BySignatureAsync (request.Signature, NodeInclude.Facts | NodeInclude.Metadata);
		    if (node == default (Node))
		    {
			    Logger.Debug("Node not found based on signature {0}. A node will be created.", request.Signature);
		        var rfId = _rfIdGenerationStrategy.FindAvailableRfAddress();
				node = await _createNodeCommand.ExecuteAsync (request.Signature, rfId);
			} else if (request.NeedNewRfAddress)
			{
			    Logger.Debug("Node was found based on signature {0} but a new rfAddress was requestd", request.Signature);
			    node.RfAddress = _rfIdGenerationStrategy.FindAvailableRfAddress();
			}

			if (node.RfAddress != request.FromRfAddress)
		    {
		        Logger.Debug("Node with signature {1} received a new rfAddress ({0}).",node.RfAddress, node.Signature);
		        outgoingMessages.Add(new NodeConfigCommand
		        {
		            ToRfAddress = request.FromRfAddress,
		            Signature = request.Signature,
		            NewRfAddress = node.RfAddress
		        });
		    }
		    else
		    {
		        outgoingMessages.Add(new NodeConfigCommand
		        {
		            ToRfAddress = request.FromRfAddress,
		            Signature = request.Signature,
		        });
		    }
		    node.Metadata.Version = request.Version;
			node.AggregatedData.StartupTime = _timeProvider.UtcNow;
		    node.AddLog("STRT");
		    await _touchNode.TouchAsync (node, request.Rssi);
		}
	}
}