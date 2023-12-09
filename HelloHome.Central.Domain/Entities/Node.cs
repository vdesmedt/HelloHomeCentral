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

        public string Identifier { get; set; }
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

    public enum NodeType
    {
        HelloNergie = 1, //Hal and Dry Sensor + SI7021
        HelloJulo = 2, //Weather station 
        HelloRelay = 3, //Control x relays and measure current through them + x push buttons + optional SI7021
        HelloSwitch = 4, //4 push button + optional LCD
        ElectronicLoad = 98,
        Simulator = 99,
    }

    public class NodeMetadata
    {
        public Node Node { get; set; }

        public NodeType NodeType { get; set; }

        public string Name { get; set; }

        public NodeFeature ExtraFeatures { get; set; }

        /// <summary>
        /// Github commmit hash of the code running on the node
        /// </summary>
        public string Version { get; set; }
    }

    public class NodeAggregatedData
    {
        public Node Node { get; set; }
        public float? VIn { get; set; }
        public int SendErrorCount { get; set; }
        public float? Temperature { get; set; }
        public float? Humidity { get; set; }
        public float? AtmosphericPressure { get; set; }
        public int Rssi { get; set; }
        public int NodeStartCount { get; set; }
        public DateTime StartupTime { get; set; }
        public float MaxUpTimeRaw { get; set; }

        public TimeSpan MaxUpTime
        {
            get => TimeSpan.FromDays(MaxUpTimeRaw);
            set => MaxUpTimeRaw = (float) value.TotalDays;
        }
    }
}