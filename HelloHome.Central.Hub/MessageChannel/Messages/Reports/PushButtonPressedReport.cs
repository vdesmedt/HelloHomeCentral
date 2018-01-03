namespace HelloHome.Central.Hub.MessageChannel.Messages.Reports
{
    public class PushButtonPressedReport : IncomingMessage
    {
        public byte PushSensorNumber { get; set; }
    }
}