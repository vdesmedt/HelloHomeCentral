using System;
using System.Runtime.Serialization;

namespace HelloHome.Central.Hub.MessageChannel.Messages
{
	public class IncomingMessage : Message
	{
	    public int FromRfAddress { get; set; }
	    public int Rssi { get; set; }
	}
}

