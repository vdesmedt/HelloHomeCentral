using System;
using System.Collections.Generic;
using HelloHome.Central.Hub.MessageChannel.Messages.Commands;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders
{
	public class NodeConfigEncoder : MessageEncoder<NodeConfigCommand>
	{
		readonly PinConfigEncoder _pinConfigEncoder;

		public NodeConfigEncoder (PinConfigEncoder pinConfigEncoder)
		{
			this._pinConfigEncoder = pinConfigEncoder;			
		}

		protected override byte[] EncodeInternal (NodeConfigCommand message)
		{
			var bytes = new List<byte> {2 + (0 << 2)};
			bytes.AddRange(BitConverter.GetBytes(message.Signature));
			bytes.Add(message.NewRfAddress);
			bytes.Add((byte)message.ExtraFeatures);
		    bytes.Add(message.NodeInfoFreq);
		    bytes.Add(message.EnvironmentFreq);

			return bytes.ToArray ();
		}
	}
}

