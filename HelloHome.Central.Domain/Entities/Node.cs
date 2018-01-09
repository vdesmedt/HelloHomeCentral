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
            Metadata = new NodeMetadata();
            AggregatedData = new NodeAggregatedData();
        }
        
        public int Id { get; set; }

        public long Signature { get; set; }

        public byte RfNetwork { get; set; }

        public byte RfAddress { get; set; }

        public DateTime LastSeen { get; set; }

        public NodeMetadata Metadata { get; set; }
        
        public NodeAggregatedData AggregatedData { get; set; }
        
        public IList<Port> Ports { get; set; }
        
        public IList<NodeLog> Logs { get; set; }

        public IList<NodeHistory> NodeHistory { get; set; }       
    }
}
