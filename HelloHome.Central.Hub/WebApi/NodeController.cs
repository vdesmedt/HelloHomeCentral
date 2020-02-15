using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloHome.Central.Domain;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Hub.MessageChannel;
using HelloHome.Central.Hub.MessageChannel.Messages.Commands;
using Microsoft.AspNetCore.Mvc;

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
            return _unitOfWork.Nodes.ToList();
        }

        [HttpGet("{id}")]
        public Node Get(int id)
        {
            return _unitOfWork.Nodes.Single(_ => _.Id == id);
        }

        [HttpGet("{id}/restart")]
        public ActionResult<bool> Restart(int id)
        {
            var node = _unitOfWork.Nodes.SingleOrDefault(_ => _.Id == 12);
            if (node == default(Node))
                return NotFound();
            _hub.Send(new RestartCommand { ToRfAddress = node.RfAddress });
            return true;
        }
    }
}