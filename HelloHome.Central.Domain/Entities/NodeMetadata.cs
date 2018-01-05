namespace HelloHome.Central.Domain.Entities
{
    public class NodeMetadata
    {
        public int Id { get; set; }

        public Node Node { get; set; }
        
        public string Name { get; set; }
        
        public virtual int? EmonCmsNodeId { get; set; }

        /// <summary>
        /// Should be GitHub hash
        /// </summary>
        public string Version { get; set; }
    }
}