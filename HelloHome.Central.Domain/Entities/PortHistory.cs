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
    public class PulseHistory : PortHistory<PulseSensor>
    {
        public int NewPulses { get; set; }
        public int Total { get; set; }
        public bool IsOffset { get; set; }
    }
	
    public class NodeHealthHistory : PortHistory<NodeHealthSensor>
    {
        public float? VIn { get; set; }
        public int SendErrorCount { get; set; }
    }
	
    public class EnvironmentHistory : PortHistory<EnvironmentSensor>
    {
        public float? Temperature { get; set; }
        public float? Humidity { get; set; }
        public float? Pressure { get; set; }
    }
    public class SwitchHistory : PortHistory<SwitchSensor>
    {
        public OnOffState NewSensorState { get; set; }
    }

    public class VarioHistory : PortHistory<VarioSensor>
    {
        public int NewLevel { get; set; }
    }

    public class PushButtonHistory : PortHistory<PushButtonSensor>
    {
        public PressStyle PressStyle { get; set; }
    }

    public class FloatLoggerHistory : PortHistory<FloatLogger>
    {
        public float Data { get; set; }
    }
    public class IntLoggerHistory : PortHistory<IntLogger>
    {
        public int Data { get; set; }
    }

    public class RelayHistory : PortHistory<RelayActuator>
    {
        public OnOffState NewRelayState { get; set; }
    }
}