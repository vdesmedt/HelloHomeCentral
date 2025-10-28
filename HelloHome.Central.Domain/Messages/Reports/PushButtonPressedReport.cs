using HelloHome.Central.Domain.Entities;

namespace HelloHome.Central.Domain.Messages.Reports
{
    public class PushButtonPressedReport : IncomingMessage
    {
        public byte PortNumber { get; set; }
        public PressStyle PressStyle { get; set; }
    }
}