namespace HelloHome.Central.Hub.MessageChannel.Messages.Reports
{
    public class PushButtonPressedReport : Report
    {
        public byte PushSensorNumber { get; set; }
    }
}