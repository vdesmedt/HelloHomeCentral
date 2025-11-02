using HelloHome.Central.Domain;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Entities.Includes;
using Microsoft.AspNetCore.Mvc;

namespace HelloHome.Central.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NodeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public NodeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            //TODO: Implement using Mqtt
            //_hub.Send(new RestartCommand { ToRfAddress = node.RfAddress });
            return true;
        }

        [HttpGet("{id}/ping")]
        public ActionResult<int> Ping(int id)
        {
            var node = _unitOfWork.Nodes.SingleOrDefault(_ => _.Id == id);
            if (node == default(Node))
                return NotFound();
            //TODO: Implement using Mqtt
            //_hub.Send(new PingCommand { ToRfAddress = node.RfAddress, Millis = (UInt32)(DateTimeOffset.Now-DateTimeOffset.Now.Date).TotalMilliseconds});
            return 0;
        }
    }
}