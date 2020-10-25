using System;

namespace HelloHome.Central.Hub.MessageChannel.Messages.Commands
{
    public class PongCommand : OutgoingMessage
    {
        public UInt32 Millis { get; set; }
        public Int16 ReceiveRssi { get; set; }

        public override string ToString()
        {
            return $"[PongCommand: NodeId={ToRfAddress}, Mills={Millis}, ReceiveRssi={ReceiveRssi}]";
        }

    }
}