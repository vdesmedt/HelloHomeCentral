using System;

namespace HelloHome.Central.Hub.MessageChannel.Messages.Commands
{
    public class PingCommand : OutgoingMessage
    {
        public UInt32 Millis { get; set; }
    }
}