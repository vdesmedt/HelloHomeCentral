using System;
using System.Collections.Generic;
using HelloHome.Central.Domain.Messages.Commands;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders.Base;
using Constants = HelloHome.Central.Common.Constants;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders
{
	public class NodeConfigEncoder : MessageEncoder<NodeConfigCommand>
	{
		protected override void EncodeBody (NodeConfigCommand message, List<byte> bytes)
		{
			bytes.AddRange(BitConverter.GetBytes((long)message.Signature));
			bytes.AddRange(BitConverter.GetBytes((UInt16)message.NewRfAddress));
			bytes.AddRange(BitConverter.GetBytes((UInt16)message.ExtraFeatures));
		    bytes.Add((byte)message.NodeInfoFreq);
		    bytes.Add((byte)message.EnvironmentFreq);
		}
		protected override byte Discriminator => Constants.Message.Command.NodeConfigCommand;
	}
}

