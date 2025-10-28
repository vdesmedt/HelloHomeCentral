using System;

namespace HelloHome.Central.Domain.Messages.Reports
{
    public class PongReport : IncomingMessage
    {
        public UInt32 MillisIn { get; set; }
        public UInt32 MillisOut { get; set; }
        public Int16 PingRssi { get; set; }

        public override string ToString ()
        {
            return $"[PongReport: Node={FromRfAddress}, MsgId={MsgId}, MillisIn={MillisIn}, MillisOut={MillisOut}, PingRssi={PingRssi}]";
        }
    }
}