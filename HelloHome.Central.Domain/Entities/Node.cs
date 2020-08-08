using System;
using System.Collections.Generic;
using System.Text;
using HelloHome.Central.Common;

namespace HelloHome.Central.Domain.Entities
{
    public class Node
    {
        public Node()
        {
        }
        
        public int Id { get; set; }

        /// <summary>
        /// UniqueId of the Flash chip
        /// </summary>
        public long Signature { get; set; }
        
        public int RfAddress { get; set; }

        public DateTime LastSeen { get; set; }

        public NodeMetadata Metadata { get; set; }
        
        public NodeAggregatedData AggregatedData { get; set; }
        
        public IList<Port> Ports { get; set; }
        
        public IList<NodeLog> Logs { get; set; }
    }
}
