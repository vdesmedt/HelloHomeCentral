using System;
using System.Collections.Generic;
using System.Text;

namespace HelloHome.Central.Domain.Entities
{
    public class Node
    {
        public int Id { get; set; }

        public long Signature { get; set; }

        public byte RfAddress { get; set; }

        public DateTimeOffset LastStartupTime { get; set; }

        public DateTime LastSeen { get; set; }

        public NodeMetadata Metadata { get; set; }
    }
}
