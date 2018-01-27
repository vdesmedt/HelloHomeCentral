using System;
using System.Collections.Generic;
using HelloHome.Central.Hub.MessageChannel.Messages.Commands;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Encoders
{
    public class RestartCommandEncoder : MessageEncoder<RestartCommand>
    {
        protected override byte[] EncodeInternal(RestartCommand message)
        {
            var bytes = new List<byte> {2 + (1 << 2)};
            return bytes.ToArray ();            
        }
    }
}