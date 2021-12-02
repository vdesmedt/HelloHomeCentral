using System.Threading.Tasks;
using HelloHome.Central.Domain;
using HelloHome.Central.Domain.CmdQrys;
using HelloHome.Central.Domain.Entities.Includes;
using HelloHome.Central.Hub.NodeBridge;
using Microsoft.AspNetCore.Mvc;

namespace HelloHome.Central.Hub.WebApi
{
    [ApiController]
    [Route("[controller]")]
    public class PulseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INodeBridge _hub;
        private readonly IFindPortQuery _findPortQuery;
        private readonly IAddPulseOffsetCommand _addPulseOffsetCommand;

        public PulseController(IUnitOfWork unitOfWork, NodeBridge.INodeBridge hub, IFindPortQuery findPortQuery, IAddPulseOffsetCommand addPulseOffsetCommand)
        {
            _unitOfWork = unitOfWork;
            _hub = hub;
            _findPortQuery = findPortQuery;
            _addPulseOffsetCommand = addPulseOffsetCommand;
        }

        [HttpGet("Offset")]
        public async Task<ActionResult<bool>> PulseOffset(int nodeId, int portNumber, int offset)
        {
            var port = await _findPortQuery.ByNodeIdAndPortNumberAsync(nodeId, portNumber, PortInclude.None);
            if (port == null)
                return NotFound();
            await _addPulseOffsetCommand.ExecuteAsync(port.Id, offset);
            await _unitOfWork.CommitAsync();
            return true;
        }

    }
}