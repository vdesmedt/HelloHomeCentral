using System;
using System.Collections.Generic;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.Messages.Commands;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders.Base;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders
{
    public class RestartCommandEncoder : MessageEncoder<RestartCommand>
    {
        protected override void EncodeBody(RestartCommand message, List<byte> bytes)
        {
        }

        protected override byte Discriminator => Constants.Message.Command.NodeRestartCommand;
    }
}