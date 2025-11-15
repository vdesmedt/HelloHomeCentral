using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.CmdQrys;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Entities.Includes;
using HelloHome.Central.Domain.Handlers.Base;
using HelloHome.Central.Domain.Logic;
using HelloHome.Central.Domain.Logic.RfAddressStrategy;
using HelloHome.Central.Domain.Messages;
using HelloHome.Central.Domain.Messages.Commands;
using HelloHome.Central.Domain.Messages.Reports;
using Microsoft.Extensions.Logging;

namespace HelloHome.Central.Domain.Handlers
{
	public class NodeStartedHandler(
		ILogger<NodeStartedHandler> logger,
		IUnitOfWork dbCtx,
		IFindNodeQuery findNodeQuery,
		ICreateNodeCommand createNodeCommand,
		ITouchNode touchNode,
		IRfAddressStrategy rfIdGenerationStrategy,
		INodeLogger nodeLogger,
		ITimeProvider timeProvider)
		: MessageHandler<NodeStartedReport>(dbCtx)
	{
		protected override async Task HandleAsync (NodeStartedReport request, IList<OutgoingMessage> outgoingMessages, CancellationToken cToken)
		{
			var node = await findNodeQuery.BySignatureAsync (request.Signature, NodeInclude.Metadata | NodeInclude.AggregatedData );
		    if (node == default (Node)) {
			    logger.LogInformation("Node not found based on signature {signature}. A node will be created.", request.Signature);
		        var rfId = rfIdGenerationStrategy.FindAvailableRfAddress();
				node = await createNodeCommand.ExecuteAsync (request.Signature, rfId, request.NodeType);
			}

		    nodeLogger.Log(node, "STRT");
			node.Metadata.Version = request.Version;
            node.AggregatedData.NodeStartCount = request.StartCount;
            node.AggregatedData.StartupTime = timeProvider.UtcNow;
            node.Metadata.NodeType = request.NodeType;
		    touchNode.Touch (node, request.Rssi);
			
			outgoingMessages.Add(new NodeConfigCommand
			{
				Signature = request.Signature,
				ToRfAddress = request.FromRfAddress,
				NewRfAddress = node.RfAddress,
				ExtraFeatures = node.Metadata.ExtraFeatures				
			});
		}
	}
}