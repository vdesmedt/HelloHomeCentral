using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.CmdQrys.Base;
using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace HelloHome.Central.Domain.CmdQrys
{
    public interface IAddPulseOffsetCommand : ICommand
    {
        Task ExecuteAsync(int portId, int offset);
    }

    public class AddPulseOffsetCommand : IAddPulseOffsetCommand
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITimeProvider _timeProvider;
        private static readonly Logger Logger = LogManager.GetLogger(nameof(CreateNodeCommand));

        public AddPulseOffsetCommand(IUnitOfWork unitOfWork, ITimeProvider timeProvider)
        {
            _unitOfWork = unitOfWork;
            _timeProvider = timeProvider;
        }

        public async Task ExecuteAsync(int portId, int offset)
        {
            var port = await _unitOfWork.Ports.OfType<PulseSensor>().SingleAsync(_ => _.Id == portId);
            port.PulseCount += offset;
            port.History = new List<PulseHistory>
            {
                new PulseHistory
                {
                    Timestamp = _timeProvider.UtcNow,
                    NewPulses = offset,
                    IsOffset = true,
                    Total = port.PulseCount
                }
            };
        }
    }
}