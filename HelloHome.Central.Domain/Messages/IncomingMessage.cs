using System;

namespace HelloHome.Central.Domain.Messages
{
	public class IncomingMessage : Message
	{
	    public int FromRfAddress { get; set; }
	    public Int16 Rssi { get; set; }
	    
	    public UInt16 MsgId { get; set; }
	}
}

