using System;

namespace HelloHome.Central.Domain.Entities
{
    public class NodeMetadata
    {
        public int Id { get; set; }

        public Node Node { get; set; }

        public NodeType NodeType { get; set; }
        
        public string Name { get; set; }
        
        public NodeFeature ExtraFeatures { get; set; }
        
        /// <summary>
        /// Github commmit hash of the code running on the node
        /// </summary>
        public string Version { get; set; }
    }
}