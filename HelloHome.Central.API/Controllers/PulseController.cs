using HelloHome.Central.Domain;
using HelloHome.Central.Domain.CmdQrys;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Entities.Includes;
using Microsoft.AspNetCore.Mvc;

namespace HelloHome.Central.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PulseController(
        ILogger<PulseController> logger,
        IUnitOfWork unitOfWork,
        IFindPortQuery findPortQuery,
        IAddPulseOffsetCommand addPulseOffsetCommand,
        IEnergyMeterSnapshotCommand energyMeterSnapshotCommand)
        : ControllerBase
    {
        [HttpPost("{portId}")]
        public async Task<ActionResult<bool>> AddPulse(int portId, [FromForm]int pulses)
        {
            logger.LogTrace("AddPulse called for portId {port-id} : {pulses}", portId, pulses);
            if(pulses == 0)
                return Problem("No pulses to add");
            var port = await findPortQuery.ByPortIdAsync<PulseSensor>(portId, PortInclude.None);
            if (port == null)
                return NotFound();
            await addPulseOffsetCommand.ExecuteAsync(port.Id, pulses);
            await unitOfWork.CommitAsync();
            return true;
        }

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