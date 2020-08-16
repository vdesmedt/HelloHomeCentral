using System.Collections.Generic;

namespace HelloHome.Central.Domain.Entities
{
    public class VarioSensor : SensorPort
    {
        public int Level { get; set; }
        public IList<VarioHistory> History { get; set; }
    }

    public class SwitchSensor : SensorPort
    {
        public OnOffState SwitchState { get; set; }
        public IList<SwitchHistory> History { get; set; }
    }

    public class PushButtonSensor : SensorPort
    {
        public IList<PushButtonHistory> History { get; set; }
        public PressStyle LastPressStyle { get; set; }
    }

    public class PulseSensor : SensorPort
    {
        public int PulseCount { get; set; }
        public IList<PulseHistory> History { get; set; }
    }

    public class EnvironmentSensor : SensorPort
    {
        public int UpdateFrequency { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float AtmPressure { get; set; }
        public IList<EnvironmentHistory> History { get; set; }
    }

    public class NodeHealthSensor : SensorPort
    {
        public int UpdateFrequency { get; set; }
        public int SendError { get; set; }
        public float VIn { get; set; }
        public IList<NodeHealthHistory> History { get; set; }
    }

    public abstract class SensorPort : Port
    {
        public List<SensorTrigger> Triggers { get; set; }
    }
}