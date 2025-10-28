using System;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.Messages;
using HelloHome.Central.Domain.Messages.Reports;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers.Base;


namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
    [ParserFor(Constants.Message.Report.SendingStatusReport)]
    public class SendingStatusParser : IMessageParser
    {
        public IncomingMessage Parse(byte[] record)
        {
            return new SendingStatusReport
            {
                FromRfAddress = BitConverter.ToUInt16(record,0),
                Rssi = BitConverter.ToInt16(record, 2),
                MessageId = BitConverter.ToUInt16(record, 5),
                Success = BitConverter.ToBoolean(record, 7)
            };
        }
    }
}