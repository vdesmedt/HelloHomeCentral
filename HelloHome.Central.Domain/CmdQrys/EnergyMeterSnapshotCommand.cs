using System;
using System.Threading.Tasks;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.CmdQrys.Base;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Entities.Includes;
using JasperFx.Core;
using NLog;

namespace HelloHome.Central.Domain.CmdQrys;

public interface IEnergyMeterSnapshotCommand : ICommand
{
    Task<EnergyMeterSnapshot> CreateSnapshot(int portId, double snapshot);
}

public class EnergyMeterSnapshotCommand(IUnitOfWork ctx, FindPortQuery findPortQuery, ITimeProvider timerProvider) : IEnergyMeterSnapshotCommand
{
    private static readonly Logger Logger = LogManager.GetLogger(nameof(CreateNodeCommand));
    
    public async Task<EnergyMeterSnapshot> CreateSnapshot(int portId, double snapshot)
    {
        var port = await findPortQuery.ByPortIdAsyn(portId, PortInclude.None);
        if (port is PulseSensor pulseSensor)
        {
            var emSnapshot = new PulseEnergyMeterSnapshot
            {
                Port = pulseSensor,
                Snapshot = snapshot,
                PulseCount = pulseSensor.PulseCount,
                Timestamp = timerProvider.UtcNow
            };
            await ctx.EnergyMeterSnapshots.AddAsync(emSnapshot);
            return emSnapshot;
        }
        throw new ApplicationException("This port does not support snaphots.");
    }
}