using HelloHome.Central.Domain.Entities;

namespace HelloHome.Central.Hub.MessageChannel.Messages.Reports
{
    public class PushButtonPressedReport : IncomingMessage
    {
        public byte PortNumber { get; set; }
        public PressStyle PressStyle { get; set; }
    }
}