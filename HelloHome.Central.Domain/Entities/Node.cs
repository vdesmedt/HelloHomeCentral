using System;
using System.Collections.Generic;
using System.Text;

namespace HelloHome.Central.Domain.Entities
{
    public class Node
    {
        public int Id { get; set; }
        public byte RfAddress { get; set; }
        public NodeMetadata Metadata { get; set; }      
    }

    public class NodeMetadata
    {
        public string Name { get; set; }
        public string Version { get; set; }
    }
}
