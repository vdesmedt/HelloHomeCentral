using System.Collections.Generic;

namespace HelloHome.Central.Domain.Entities
{
	public abstract class Port
	{
		public virtual int NodePortId { get; set; }
		public virtual Node Node { get; set; }
		public virtual int NodeId { get; set; }
		public virtual int Number { get; set; }  
		public virtual string Name { get; set; }
	}

	public abstract class SensorPort : Port
	{
	    public List<Trigger> Triggers { get; set; }
	}

	public abstract class ActuatorPort : Port
	{
	    public List<Action> Actions { get; set; }
	}

	public class PulseSensor : SensorPort
	{ 
		public virtual int PulseCount { get; set; }
		public virtual IList<PulseHistory> PulseData { get; set; }
	}

    public class PushSensor : SensorPort
    {
    }

    public class SwitchSensor : SensorPort
	{ 
		public virtual bool State { get; set; }
	}

	public class VarioSensor : SensorPort
	{
		public virtual int Value { get; set; }
	}

	public class RelayActuator : ActuatorPort
	{ 
	}
}

