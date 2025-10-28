using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.Messages.Commands;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders.Base;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders
{
    public class SetRelayStateEncoder : MessageEncoder<SetRelayStateCommand>
    {
        protected override void EncodeBody(SetRelayStateCommand message, List<byte> encoded)
        {
            encoded.Add(message.PortNumber);
            encoded.Add(message.NewState);
        }

        protected override byte Discriminator => Constants.Message.Command.SetRelayStateCommand;
    }
}