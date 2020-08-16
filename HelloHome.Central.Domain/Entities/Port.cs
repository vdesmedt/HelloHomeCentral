using System.Dynamic;
using System.Reflection;

namespace HelloHome.Central.Domain.Entities
{
    public enum ReservedPortNumber : byte
    {
        NodeHealth = 1,
        Environment = 2,
        Last = 5,
    }

    public abstract class Port
    {
        public int Id { get; set; }
        public Node Node { get; set; }
        public int NodeId { get; set; }

        public byte PortNumber { get; set; }
        public string Name { get; set; }
    }


    public enum PressStyle
    {
        Click = 1,
        DoubleClick = 2,
        LongClick = 3,
    }

    public enum OnOffState
    {
        Off = 0,
        On = 1,
        Unknown = -1
    }
}