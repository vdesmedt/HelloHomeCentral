using System;

namespace HelloHome.Central.Domain.Entities
{
    public abstract class PortHistory
    {
        public int Id { get; set; }
        public int PortId { get; set; }
        public DateTime Timestamp { get; set; }
        public int Rssi { get; set; }
    }

    public abstract class PortHistory<TPort>:PortHistory where TPort : Port
    {
        public TPort Port { get; set; }
    }
    public class PulseHistory : PortHistory<PulseSensorPort>
    {
        public int NewPulses { get; set; }
        public int Total { get; set; }
        public bool IsOffset { get; set; }
    }
	
    public class NodeHealthHistory : PortHistory<NodeHealthSensorPort>
    {
        public float? VIn { get; set; }
        public int SendErrorCount { get; set; }
    }
	
    public class EnvironmentHistory : PortHistory<EnvironmentSensorPort>
    {
        public float? Temperature { get; set; }
        public float? Humidity { get; set; }
        public float? Pressure { get; set; }
    }
    public class SwitchPortHistory : PortHistory<SwitchSensorPort>
    {
        public OnOffState NewSensorState { get; set; }
    }

    public class VarioButtonPortHistory : PortHistory<VarioSensorPort>
    {
        public int NewLevel { get; set; }
    }

    public class PushButtonHistory : PortHistory<PushSensorPort>
    {
        public PressStyle PressStyle { get; set; }
    }

    public class FloatDataLogPortHistory : PortHistory<FloatDataLogPort>
    {
        public float Data { get; set; }
    }
    public class IntDataLogPortHistory : PortHistory<IntDataLogPort>
    {
        public int Data { get; set; }
    }
}