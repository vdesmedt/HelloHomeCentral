using System.Collections.Generic;
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

    public abstract class SensorPort : Port
    {
        public List<SensorTrigger> Triggers { get; set; }
    }

    public class NodeHealthSensorPort : SensorPort
    {
        public int UpdateFrequency { get; set; }
        public int SendError { get; set; }
        public float VIn { get; set; }
        public IList<NodeHealthHistory> History { get; set; }
    }


    public class EnvironmentSensorPort : SensorPort
    {
        public int UpdateFrequency { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float AtmPressure { get; set; }
        public IList<EnvironmentHistory> History { get; set; }
    }

    public class PulseSensorPort : SensorPort
    {
        public int PulseCount { get; set; }
        public IList<PulseHistory> History { get; set; }
    }

    public enum PressStyle
    {
        Click = 1,
        DoubleClick = 2,
        LongClick = 3,
    }

    public class PushSensorPort : SensorPort
    {
        public IList<PushButtonHistory> History { get; set; }
        public PressStyle LastPressStyle { get; set; }
    }

    public enum OnOffState
    {
        Off = 0,
        On = 1,
        Unknown = -1
    }

    public class SwitchSensorPort : SensorPort
    {
        public OnOffState SwitchState { get; set; }
        public IList<SwitchPortHistory> History { get; set; }
    }

    public class VarioSensorPort : SensorPort
    {
        public int Level { get; set; }
        public IList<VarioButtonPortHistory> History { get; set; }
    }


    public abstract class ActuatorPort : Port
    {
    }

    public class RelayActuatorPort : ActuatorPort
    {
        public OnOffState RelayState { get; set; }
    }

    public abstract class LoggingPort : Port
    {
    }

    public class FloatDataLogPort : LoggingPort
    {
        public float Data { get; set; }
        public IList<FloatDataLogPortHistory> History { get; set; }
    }

    public class IntDataLogPort : LoggingPort
    {
        public int Data { get; set; }
        public IList<IntDataLogPortHistory> History { get; set; }
    }
}