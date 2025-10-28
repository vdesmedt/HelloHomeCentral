namespace HelloHome.Central.Domain.Messages.Reports
{
    public class ActionConfirmedReport : IncomingMessage
    {
        public OutgoingMessage ConfigmedAction { get; set; }
    }
}