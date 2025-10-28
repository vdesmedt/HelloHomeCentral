using System;

namespace HelloHome.Central.Domain.Messages.Reports
{
    public class SendingStatusReport : IncomingMessage
    {
        public UInt16 MessageId { get; set; }
        public bool Success { get; set; }

        public override string ToString ()
        {
            return $"[SendingStatusReport: From={FromRfAddress}, Rssi={Rssi},  MessageId={MessageId}, Success={Success}]";
        }
    }
}