using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelloHome.Central.Domain.CmdQrys.Base;
using HelloHome.Central.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelloHome.Central.Domain.CmdQrys
{
    public interface IFindTriggersForPortQuery
    {
        Task<IList<TTrigger>>  ByPortIdAsync<TTrigger>(int portId, CancellationToken cToken) where TTrigger : SensorTrigger;
    }

    public class FindTriggersForPortQuery : IQuery, IFindTriggersForPortQuery
    {
        private readonly IUnitOfWork _ctx;

        public FindTriggersForPortQuery(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }
        
        public async Task<IList<TTrigger>>  ByPortIdAsync<TTrigger>(int portId, CancellationToken cToken) where TTrigger : SensorTrigger
        {
            return await _ctx.Triggers
                .OfType<TTrigger>()
                .Include(x=>x.Actions)
                .ThenInclude(a => ((ActuatorAction)a).Actuator)
                .ThenInclude(p=>p.Node)
                .Where(x => x.SensorPortId == portId).ToListAsync(cToken);
        }
    }
}