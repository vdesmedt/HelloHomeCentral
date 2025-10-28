namespace HelloHome.Central.Domain.Messages.Commands
{
    public class RFM2piConfigCommand : OutgoingMessage
    {
        public byte NetworkId { get; set; }
        public bool HighPower { get; set; }
    }
}