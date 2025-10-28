using System;
using System.Collections.Generic;
using System.Text;
using HelloHome.Central.Domain.Messages.Commands;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders.Base;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders
{
    public class Rfm2PiConfigEncoder : MessageEncoder<RFM2piConfigCommand>
    {
        protected override void EncodeBody(RFM2piConfigCommand message, List<byte> encoded)
        {
            encoded.Add(message.NetworkId);
            encoded.Add(message.HighPower?(byte)1:(byte)0);
        }

        protected override byte Discriminator => Common.Constants.Message.Command.Rfm2PiConfigCommand;
    }
}