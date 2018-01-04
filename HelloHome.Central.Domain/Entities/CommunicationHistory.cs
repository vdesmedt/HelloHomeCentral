using System;

namespace HelloHome.Central.Domain.Entities
{

	public class CommunicationHistory 
	{ 
		public virtual int Id { get; set; }
		public virtual int NodeId { get; set; }
	    public virtual string Type { get; set; }
		public virtual DateTime Timestamp { get; set; }
	    public virtual int Rssi { get; set; }
	}
}
