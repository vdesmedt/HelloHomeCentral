using System.Collections.Generic;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.Messages.Commands;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders.Base;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders
{
    public class HeartBeatEncoder : MessageEncoder<HeartBeatCommand>
    {
        protected override void EncodeBody(HeartBeatCommand message, List<byte> encoded)
        {
            
        }

        protected override byte Discriminator => Constants.Message.Command.HeartBeatCommand;
    }
}