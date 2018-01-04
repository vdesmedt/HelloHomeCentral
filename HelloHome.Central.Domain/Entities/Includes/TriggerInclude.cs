using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HelloHome.Central.Domain.Entities.Includes
{
    [Flags]
    public enum TriggerInclude
    {
        None = 0,
        All = ~None,
        Actions = 1,
        Actuators = 2,
        ActuatorsNode = 4,
        Sensors = 8,
        SensorsNode = 16,
    }

    public static class TriggerQueryableExtentions
    {
        public static IQueryable<Trigger> Include (this IQueryable<Trigger> query, NodeInclude include)
        {
            if (include.HasFlag (TriggerInclude.Actions))
                query = query.Include(t => t.Actions);
            if (include.HasFlag (TriggerInclude.Actuators))
                query = query.Include (t => t.Actions.Select(a => a.Actuators));
            if (include.HasFlag (TriggerInclude.ActuatorsNode))
                query = query.Include ( t => t.Actions.Select(a => a.Actuators.Select(p => p.Node)));

            if (include.HasFlag (TriggerInclude.Sensors))
                query = query.Include (t => t.Sensors);
            if (include.HasFlag (TriggerInclude.SensorsNode))
                query = query.Include ( t => t.Sensors.Select(a => a.Node));
            return query;
        }
    }

}