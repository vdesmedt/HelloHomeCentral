using System;

namespace HelloHome.Central.Domain.Messages.Commands
{
    public class PingCommand : OutgoingMessage
    {
        public UInt32 Millis { get; set; }
    }
}