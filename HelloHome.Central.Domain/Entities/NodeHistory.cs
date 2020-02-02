using System;

namespace HelloHome.Central.Domain.Entities
{

	public abstract class NodeHistory 
	{ 
		public int Id { get; set; }
		public Node Node { get; set; }
		public int NodeId { get; set; }
		
		public DateTime Timestamp { get; set; }
	    public int Rssi { get; set; }
	}
	
	public class PulseHistory : NodeHistory
	{
		public PulseSensorPort PulseSensorPort { get; set; }
		public int PulseSensorPortId { get; set; }
		public int NewPulses { get; set; }
		public int Total { get; set; }
		public bool IsOffset { get; set; }
	}
	
	public class NodeHealthHistory : NodeHistory
	{
		public float? VIn { get; set; }
		public int SendErrorCount { get; set; }
	}
	
	public class EnvironmentDataHistory : NodeHistory
	{
		public float? Temperature { get; set; }
		public float? Humidity { get; set; }
		public float? Pressure { get; set; }
	}
}
