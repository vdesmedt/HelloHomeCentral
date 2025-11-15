using HelloHome.Central.Common.Mqtt;
using HelloHome.Central.Domain;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Entities.Includes;
using HelloHome.Central.Domain.Messages.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MQTTnet.Protocol;

namespace HelloHome.Central.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NodeController(IUnitOfWork unitOfWork, IMqttPublisher mqttPublisher) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<Node>> Get()
        {
            var nodes = await unitOfWork.Nodes.Include(NodeInclude.Metadata).ToListAsync();
            
            foreach (var n in nodes)
            {
                n.Metadata.Node = null;
                n.AggregatedData.Node = null;
            }

            return nodes;
        }

        [HttpGet("{id}")]
        public async Task<Node> Get(int id)
        {
            var node =  await unitOfWork.Nodes.Include(NodeInclude.Metadata).Include(NodeInclude.AggregatedData).SingleAsync(_ => _.Id == id);
            node.Metadata.Node = null;
            node.AggregatedData.Node = null;
            return node;
        }

        [HttpGet("{id}/history-env")]
        public async Task<IEnumerable<EnvironmentHistory>> GetHistory(int id)
        {
            return await unitOfWork.PortHistory.OfType<EnvironmentHistory>().Where(_ => _.Port.NodeId == id).ToListAsync();
        }

        [HttpGet("{id}/restart")]
        public async Task<ActionResult<bool>> Restart(int id)
        {
            var node = await unitOfWork.Nodes.SingleOrDefaultAsync(n => n.Id == id);
            if (node == null)
                return NotFound();
            var restartCommand = new RestartCommand { ToRfAddress = node.RfAddress };
            await mqttPublisher.PublishAsync("core",restartCommand, CancellationToken.None);
            return true;
        }

        [HttpGet("{id}/ping")]
        public async Task<ActionResult<int>> Ping(int id)
        {
            var node = await unitOfWork.Nodes.SingleOrDefaultAsync(n => n.Id == id);
            if (node == null)
                return NotFound();
            //TODO: Implement using Mqtt
            //_hub.Send(new PingCommand { ToRfAddress = node.RfAddress, Millis = (UInt32)(DateTimeOffset.Now-DateTimeOffset.Now.Date).TotalMilliseconds});
            return 0;
        }
    }
}