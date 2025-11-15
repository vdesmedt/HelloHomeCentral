using System;
using System.Threading.Tasks;
using HelloHome.Central.Domain.CmdQrys.Base;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Logic;

namespace HelloHome.Central.Domain.CmdQrys
{
    public interface ICreateNodeCommand : ICommand
    {
        Task<Node> ExecuteAsync(long signature, int rfId, NodeType nodeType);
    }

    public class CreateNodeCommand(
        IUnitOfWork ctx,
        INodeLogger nodeLogger) : ICreateNodeCommand
    {
        public async Task<Node> ExecuteAsync(long signature, int rfId, NodeType nodeType)
        {
            var node = new Node
            {
                Signature = signature,
                RfAddress = rfId,
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
            await ctx.Nodes.AddAsync(node);
            nodeLogger.Log(node, "CRTD");
            return node;
        }
    }
}