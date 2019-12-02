using System;
using System.Collections.Generic;
using HelloHome.Central.Hub.MessageChannel.Messages.Commands;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders.Base;
using Constants = HelloHome.Central.Common.Constants;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders
{
	public class NodeConfigEncoder : MessageEncoder<NodeConfigCommand>
	{
		readonly PinConfigEncoder _pinConfigEncoder;

		public NodeConfigEncoder (PinConfigEncoder pinConfigEncoder)
		{
			this._pinConfigEncoder = pinConfigEncoder;			
		}

		protected override void EncodeBody (NodeConfigCommand message, List<byte> bytes)
		{
			bytes.AddRange(BitConverter.GetBytes((long)message.Signature));
			bytes.AddRange(BitConverter.GetBytes(message.NewRfAddress));
			bytes.AddRange(BitConverter.GetBytes((short)message.ExtraFeatures));
		    bytes.Add(message.NodeInfoFreq);
		    bytes.Add(message.EnvironmentFreq);
		}
		protected override byte Discriminator => Constants.Message.Command.NodeConfigCommand;
	}
}

