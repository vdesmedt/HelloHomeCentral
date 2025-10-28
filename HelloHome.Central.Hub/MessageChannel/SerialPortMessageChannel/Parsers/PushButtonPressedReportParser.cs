using System;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Messages;
using HelloHome.Central.Domain.Messages.Reports;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers.Base;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
    [ParserFor(Constants.Message.Report.PushButtonPressedReport)]
    public class PushButtonPressedReportParser : IMessageParser
    {
        public IncomingMessage Parse(byte[] record)
        {
            return new PushButtonPressedReport {
                FromRfAddress = BitConverter.ToUInt16(record, 0),
                Rssi = BitConverter.ToInt16(record,2),
                //Byte 4 is msgType
                MsgId = record[5],
                PortNumber = record[6],
                PressStyle = (PressStyle)record[7]
            };
        }
    }
}