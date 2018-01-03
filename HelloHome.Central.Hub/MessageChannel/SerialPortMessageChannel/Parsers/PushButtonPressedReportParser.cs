using System;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
    [ParserFor(0 + (4 << 2))]
    public class PushButtonPressedReportParser : IMessageParser
    {
        public IncomingMessage Parse(byte[] record)
        {
            return new PushButtonPressedReport {
                FromRfAddress = record [0],
                Rssi = BitConverter.ToInt16(record,1),
                PushSensorNumber = record[4]
            };
        }
    }
}