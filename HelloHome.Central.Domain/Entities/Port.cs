using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace HelloHome.Central.Domain.Entities
{
    public abstract class Port
    {
        public int Id { get; set; }
        public Node Node { get; set; }
        public int NodeId { get; set; }
        public string Name { get; set; }        
    }

    public abstract class SensorPort : Port
    {
        public List<SensorTrigger> Triggers { get; set; }
    }

    public class PulseSensorPort : SensorPort
    {
        public int PulseCount { get; set; }
        public IList<PulseHistory> PulseHistory { get; set; }
    }

    public class PushSensorPort : SensorPort
    {
    }
    
    public class SwitchSensorPort : SensorPort
    {        
        public bool State { get; set; }
    }

    public class VarioSensorPort : SensorPort
    {
        public int Value { get; set; }
    }


    public abstract class ActuatorPort : Port
    {
    }

    public class RelayActuatorPort : ActuatorPort
    {
    }
}