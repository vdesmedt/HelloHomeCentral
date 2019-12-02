using System;
using System.Collections.Generic;
using HelloHome.Central.Hub.MessageChannel.Messages;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders.Base
{
	public abstract class MessageEncoder<TMessage> : IMessageEncoder where TMessage : OutgoingMessage
	{
		#region IMessageEncoder implementation

	    public byte[] Encode (OutgoingMessage message)
		{
			var coreMessageBytes = new List<byte>();
			coreMessageBytes.Add(Discriminator);
			EncodeBody (message as TMessage, coreMessageBytes);

			var bytes = new List<byte>();
			bytes.AddRange(BitConverter.GetBytes(message.MessageId));
			bytes.AddRange(BitConverter.GetBytes(message.ToRfAddress));
			bytes.Add((byte)coreMessageBytes.Count);
			bytes.AddRange(coreMessageBytes);
			return bytes.ToArray();
		}

	    #endregion

		protected abstract void EncodeBody (TMessage message, List<byte> encoded);
		protected abstract byte Discriminator { get; }
	}
}

