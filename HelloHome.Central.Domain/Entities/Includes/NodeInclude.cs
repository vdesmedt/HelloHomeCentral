using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HelloHome.Central.Domain.Entities.Includes
{
	[Flags]
	public enum NodeInclude
	{
		None = 0,
		All = ~None,
		Metadata = 1,
		AggregatedData = 2,
		Ports = 4,
	}

	public static class NodeExtentions
	{
		public static IQueryable<Node> Include (this IQueryable<Node> query, NodeInclude include)
		{
			if (include.HasFlag (NodeInclude.Metadata))
				query = query.Include (x => x.Metadata);
			if (include.HasFlag (NodeInclude.Ports))
				query = query.Include (x => x.Ports);
			if (include.HasFlag (NodeInclude.AggregatedData))
				query = query.Include (x => x.AggregatedData);
			return query;
		}
	}
}
