using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HelloHome.Central.Domain.Entities.Includes
{
    [Flags]
    public enum PortInclude
    {
        None = 0,
        All = ~None,
        Node = 1,
        NodeAggregatedData = 2,
    }

    public static class PortExtentions
    {
        public static IQueryable<Port> Include(this IQueryable<Port> query, PortInclude include)
        {
            if (include.HasFlag(PortInclude.Node))
                query = query.Include(x => x.Node);
            if (include.HasFlag(PortInclude.NodeAggregatedData))
                query = query.Include(x => x.Node.AggregatedData);
            return query;
        }
    }
}