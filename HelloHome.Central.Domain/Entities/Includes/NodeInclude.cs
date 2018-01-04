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
		Facts = 1,
		Metadata = 2,
		Ports = 4,
		AggregatedData = 8,
	}

	public static class NodeExtentions
	{
		public static IQueryable<Node> Include (this IQueryable<Node> query, NodeInclude include)
		{
			if (include.HasFlag (NodeInclude.Metadata))
				query = query.Include (_ => _.Metadata);
			if (include.HasFlag (NodeInclude.Ports))
				query = query.Include (_ => _.Ports);
			if (include.HasFlag (NodeInclude.AggregatedData))
				query = query.Include (_ => _.AggregatedData);
			return query;
		}
	}
}
