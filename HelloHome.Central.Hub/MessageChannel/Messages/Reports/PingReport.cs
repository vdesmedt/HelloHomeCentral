using System;

namespace HelloHome.Central.Hub.MessageChannel.Messages.Reports
{
    public class PingReport : IncomingMessage
    {
        public UInt32 Millis { get; set; }

        public override string ToString()
        {
            return $"[PingReport: Node={FromRfAddress}, Millis={Millis}]";
        }
    }
}