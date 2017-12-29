namespace HelloHome.Central.Hub.MessageChannel.Messages
{
    public abstract class OutgoingMessage : Message
	{
	    public byte ToRfAddress { get; set; }
	}
}

