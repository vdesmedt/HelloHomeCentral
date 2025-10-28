using System;
using System.Collections.Generic;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.Messages.Commands;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders.Base;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders
{
    public class PongEncoder : MessageEncoder<PongCommand>
    {
        protected override void EncodeBody(PongCommand message, List<byte> encoded)
        {
            encoded.AddRange(BitConverter.GetBytes(message.Millis));
            encoded.AddRange(BitConverter.GetBytes(message.ReceiveRssi));
        }

        protected override byte Discriminator => Constants.Message.Command.PongCommand;
    }
}