using System.Linq.Expressions;
using System.Threading.Tasks;
using HelloHome.Central.Domain;
using HelloHome.Central.Domain.CmdQrys;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Entities.Includes;
using HelloHome.Central.Hub.NodeBridge;
using Microsoft.AspNetCore.Mvc;

namespace HelloHome.Central.Hub.WebApi
{
    [ApiController]
    [Route("[controller]")]
    public class PulseController(
        IUnitOfWork unitOfWork,
        IFindPortQuery findPortQuery,
        IAddPulseOffsetCommand addPulseOffsetCommand,
        IEnergyMeterSnapshotCommand energyMeterSnapshotCommand)
        : ControllerBase
    {
        [HttpGet("Offset")]
        public async Task<ActionResult<bool>> PulseOffset(int nodeId, int portNumber, int offset)
        {
            var port = await findPortQuery.ByNodeIdAndPortNumberAsync(nodeId, portNumber, PortInclude.None);
            if (port == null)
                return NotFound();
            await addPulseOffsetCommand.ExecuteAsync(port.Id, offset);
            await unitOfWork.CommitAsync();
            return true;
        }

        [HttpGet("SetValue")]
        public async Task<ActionResult<int>> SetValue(int nodeId, int portNumber, int value)
        {
            var port = await findPortQuery.ByNodeIdAndPortNumberAsync<PulseSensor>(nodeId, portNumber, PortInclude.None);
            if (port == null)
                return NotFound();
            var offset = value - port.PulseCount;
            await addPulseOffsetCommand.ExecuteAsync(port.Id, offset);
            await unitOfWork.CommitAsync();
            return offset;
        }

        [HttpPut("{nodeIdentifier}/{portName}/Snapshot")]
        public async Task<ActionResult<bool>> CreateSnapshot(string nodeIdentifier, string portName, [FromForm] decimal snapshot)
        {
            var port = await findPortQuery.ByNodeIdentifierAndPortNameAsync<PulseSensor>(nodeIdentifier, portName);
            if (port == null)
                return NotFound();
            var snap = await energyMeterSnapshotCommand.CreateSnapshot(port.Id, (double)snapshot);
            await unitOfWork.CommitAsync();
            return true;
        }
    }
}