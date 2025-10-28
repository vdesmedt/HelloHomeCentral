using System;

namespace HelloHome.Central.Domain.Messages
{
    public abstract class OutgoingMessage : Message
    {
	    private static UInt16 _nextId = 1;
		protected OutgoingMessage()
		{
			MessageId = _nextId++;
		}
	    public UInt16 MessageId { get; }
	    public int ToRfAddress { get; set; }
	}
}

