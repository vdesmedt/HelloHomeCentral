using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.CmdQrys.Base;
using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelloHome.Central.Domain.CmdQrys
{
    public interface IAddPulseOffsetCommand : ICommand
    {
        Task ExecuteAsync(int portId, int offset);
    }

    public class AddPulseOffsetCommand(
        IUnitOfWork unitOfWork,
        ITimeProvider timeProvider)
        : IAddPulseOffsetCommand
    {
        public async Task ExecuteAsync(int portId, int offset)
        {
            var port = await unitOfWork.Ports.OfType<PulseSensor>().SingleAsync(_ => _.Id == portId);
            port.PulseCount += offset;
            port.History = new List<PulseHistory>
            {
                new PulseHistory
                {
                    Timestamp = timeProvider.UtcNow,
                    NewPulses = offset,
                    IsOffset = true,
                    Total = port.PulseCount
                }
            };
        }
    }
}