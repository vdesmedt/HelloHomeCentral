﻿using System;
using System.Threading.Tasks;
using HelloHome.Central.Domain.CmdQrys.Base;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Logic;
using NLog;

namespace HelloHome.Central.Domain.CmdQrys
{
    public interface ICreateNodeCommand : ICommand
    {
        Task<Node> ExecuteAsync(long signature, int rfId, NodeType nodeType);
    }

    public class CreateNodeCommand : ICreateNodeCommand
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(CreateNodeCommand));

        private readonly IUnitOfWork _ctx;
        private readonly INodeLogger _nodeLogger;

        public CreateNodeCommand(
            IUnitOfWork ctx,
            INodeLogger nodeLogger)
        {
            _ctx = ctx;
            _nodeLogger = nodeLogger;
        }

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
            await _ctx.Nodes.AddAsync(node);
            _nodeLogger.Log(node, "CRTD");
            return node;
        }
    }
}