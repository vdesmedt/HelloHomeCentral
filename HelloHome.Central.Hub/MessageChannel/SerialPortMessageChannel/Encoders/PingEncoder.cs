using System;
using System.Collections.Generic;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.Messages.Commands;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders.Base;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders
{
    public class PingEncoder : MessageEncoder<PingCommand>
    {
        protected override void EncodeBody(PingCommand message, List<byte> encoded)
        {
            encoded.AddRange(BitConverter.GetBytes(message.Millis));
        }

        protected override byte Discriminator => Constants.Message.Command.PingCommand;
    }
}