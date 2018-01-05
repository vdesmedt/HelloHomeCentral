using System;

namespace HelloHome.Central.Domain.Entities
{

	public abstract class CommunicationHistory 
	{ 
		public virtual int Id { get; set; }
		public virtual int NodeId { get; set; }
		public virtual DateTime Timestamp { get; set; }
	    public virtual int Rssi { get; set; }
	}
	
	public class PulseHistory : CommunicationHistory
	{
		public virtual PulseSensorPort SensorPort { get; set; }
		public virtual int PortId { get; set; }
		public virtual int NewPulses { get; set; }
		public virtual int Total { get; set; }
		public virtual bool IsOffset { get; set; }
	}
	
	public class NodeHealthHistory : CommunicationHistory
	{
		public virtual float? VIn { get; set; }
		public virtual int SendErrorCount { get; set; }
	}
	
	public class EnvironmentDataHistory : CommunicationHistory
	{
		public virtual float? Temperature { get; set; }
		public virtual float? Humidity { get; set; }
		public virtual int? Pressure { get; set; }
	}
}
