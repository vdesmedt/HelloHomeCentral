using System;
using System.Collections.Generic;
using System.Text;
using HelloHome.Central.Common;

namespace HelloHome.Central.Domain.Entities
{
    public class Node
    {
        public virtual int Id { get; set; }

        public virtual long Signature { get; set; }

        public virtual byte RfNetwork { get; set; }

        public virtual byte RfAddress { get; set; }

        public virtual DateTimeOffset LastStartupTime { get; set; }

        public virtual DateTime LastSeen { get; set; }

        public virtual NodeMetadata Metadata { get; set; }
        
        public virtual NodeAggregatedData AggregatedData { get; set; }
        
        public virtual IList<Port> Ports { get; set; }
        
        public virtual IList<NodeLog> Logs { get; set; }

        public virtual IList<CommunicationHistory> CommunicationHistory { get; set; }

        public void AddLog(string type, string data = null)
        {
            if(Logs == null)
                Logs = new List<NodeLog>();
            Logs.Add(new NodeLog { Time = TimeProvider.Current.UtcNow, NodeId = this.Id, Type = type, Data = data });
        }
        
    }
}
