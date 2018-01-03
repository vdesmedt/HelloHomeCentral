using HelloHome.Central.Hub.MessageChannel.Messages;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders
{
	public abstract class MessageEncoder<TMessage> : IMessageEncoder where TMessage : OutgoingMessage
	{
		#region IMessageEncoder implementation

	    public byte[] Encode (Message message)
		{
			return EncodeInternal (message as TMessage);
		}

		#endregion

		protected abstract byte[] EncodeInternal (TMessage message);
	}
}

