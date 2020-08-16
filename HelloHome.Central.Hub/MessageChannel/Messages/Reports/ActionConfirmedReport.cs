namespace HelloHome.Central.Hub.MessageChannel.Messages.Reports
{
    public class ActionConfirmedReport : IncomingMessage
    {
        public OutgoingMessage ConfigmedAction { get; set; }
    }
}