using HelloHome.Central.Domain.Entities;

namespace HelloHome.Central.Hub.MessageChannel.Messages.Commands
{
    public class SetRelayStateCommand : OutgoingMessage
    {
        public byte PortNumber { get; set; }
        public byte NewState { get; set; }

        public override string ToString()
        {
            return $"[SetRelayStateCommand: NodeId={ToRfAddress}, PortNumber={PortNumber}, NewState={NewState}]";
        }
    }
}