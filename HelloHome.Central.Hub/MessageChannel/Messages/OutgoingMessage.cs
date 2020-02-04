using System;
using System.ComponentModel;
using System.Text;
using System.Threading;
using HelloHome.Central.Common;

namespace HelloHome.Central.Hub.MessageChannel.Messages
{
    public abstract class OutgoingMessage : Message
    {
	    private static UInt16 _nextId = 1;
		protected OutgoingMessage()
		{
			MessageId = _nextId++;
		}
	    public UInt16 MessageId { get; set; }
	    public int ToRfAddress { get; set; }
	}
}

