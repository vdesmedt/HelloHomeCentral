using System;
using System.Reflection.Metadata;
using System.Text;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers.Base;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
    [ParserFor(Constants.Message.Report.PingReport)]
    public class PingParser : IMessageParser
    {
        public IncomingMessage Parse(byte[] record)
        {
            var expectedLenght = 11;
            if(record.Length != expectedLenght)
                throw new ArgumentException($"{nameof(PingReport)} should be {expectedLenght} bytes long (was {record.Length})");
            return new PingReport() {
                FromRfAddress = BitConverter.ToUInt16(record, 0),
                Rssi = BitConverter.ToInt16 (record, 2),
                Millis = BitConverter.ToUInt32(record, 5),
            };
        }
    }
}