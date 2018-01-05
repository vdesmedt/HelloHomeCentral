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
    }

    public static class TriggerQueryableExtentions
    {
        public static IQueryable<Trigger> Include (this IQueryable<Trigger> query, NodeInclude include)
        {
            if (include.HasFlag (TriggerInclude.Actions))
                query = query.Include(t => t.Actions);
            return query;
        }
    }

}