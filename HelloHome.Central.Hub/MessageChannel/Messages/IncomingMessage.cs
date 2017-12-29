using System.Runtime.Serialization;

namespace HelloHome.Central.Hub.MessageChannel.Messages
{
	public class IncomingMessage : Message
	{
	    public byte FromRfAddress { get; set; }
	    public int Rssi { get; set; }
	}
}

