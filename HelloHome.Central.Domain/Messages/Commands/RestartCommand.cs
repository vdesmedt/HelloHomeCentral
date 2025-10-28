namespace HelloHome.Central.Domain.Messages.Commands
{
    public class RestartCommand : OutgoingMessage
    {
        public override string ToString()
        {
            return $"[RestartCommand: NodeId={ToRfAddress}]";
        }
    }
}