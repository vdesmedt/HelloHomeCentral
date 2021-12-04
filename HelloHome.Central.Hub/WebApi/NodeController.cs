using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloHome.Central.Domain;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Entities.Includes;
using HelloHome.Central.Hub.MessageChannel;
using HelloHome.Central.Hub.MessageChannel.Messages.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloHome.Central.Hub.WebApi
{
    [ApiController]
    [Route("[controller]")]
    public class NodeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly NodeBridge.INodeBridge _hub;

        public NodeController(IUnitOfWork unitOfWork, NodeBridge.INodeBridge hub)
        {
            _unitOfWork = unitOfWork;
            _hub = hub;
        }
        [HttpGet]
        public IEnumerable<Node> Get()
        {
            var nodes = _unitOfWork.Nodes.Include(NodeInclude.Metadata).ToList();
            foreach (var n in nodes)
            {
                n.Metadata.Node = null;
                n.AggregatedData.Node = null;
            }

            return nodes;
        }

        [HttpGet("{id}")]
        public Node Get(int id)
        {
            var node =  _unitOfWork.Nodes.Include(NodeInclude.Metadata).Include(NodeInclude.AggregatedData).Single(_ => _.Id == id);
            node.Metadata.Node = null;
            node.AggregatedData.Node = null;
            return node;
        }

        [HttpGet("{id}/history-env")]
        public IEnumerable<EnvironmentHistory> GetHistory(int id)
        {
            return _unitOfWork.PortHistory.OfType<EnvironmentHistory>().Where(_ => _.Port.NodeId == id).ToList();
        }

        [HttpGet("{id}/restart")]
        public ActionResult<bool> Restart(int id)
        {
            var node = _unitOfWork.Nodes.SingleOrDefault(_ => _.Id == id);
            if (node == default(Node))
                return NotFound();
            _hub.Send(new RestartCommand { ToRfAddress = node.RfAddress });
            return true;
        }

        [HttpGet("{id}/ping")]
        public ActionResult<int> Ping(int id)
        {
            var node = _unitOfWork.Nodes.SingleOrDefault(_ => _.Id == id);
            if (node == default(Node))
                return NotFound();
            _hub.Send(new PingCommand { ToRfAddress = node.RfAddress, Millis = (UInt32)(DateTimeOffset.Now-DateTimeOffset.Now.Date).TotalMilliseconds});
            return 0;
        }
    }
}