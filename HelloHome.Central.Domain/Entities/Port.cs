using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace HelloHome.Central.Domain.Entities
{
    public abstract class Port
    {
        public virtual int Id { get; set; }
        public virtual Node Node { get; set; }
        public virtual int NodeId { get; set; }
        public virtual int Number { get; set; }
        public virtual string Name { get; set; }
    }

    public abstract class SensorPort : Port
    {
        public List<SensorTrigger> Triggers { get; set; }
    }

    public class PulseSensorPort : SensorPort
    {
        public virtual int PulseCount { get; set; }
        public virtual IList<PulseHistory> PulseData { get; set; }
    }

    public class PushSensorPort : SensorPort
    {
    }
    
    public class SwitchSensorPort : SensorPort
    {        
        public virtual bool State { get; set; }
    }

    public class VarioSensorPort : SensorPort
    {
        public virtual int Value { get; set; }
    }


    public abstract class ActuatorPort : Port
    {
    }

    public class RelayActuatorPort : ActuatorPort
    {
    }
}