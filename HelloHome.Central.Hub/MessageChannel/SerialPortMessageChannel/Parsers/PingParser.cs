using System;
using System.Reflection.Metadata;
using System.Text;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Domain.Messages;
using HelloHome.Central.Domain.Messages.Reports;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers.Base;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
    [ParserFor(Constants.Message.Report.PingReport)]
    public class PingParser : IMessageParser
    {
        //{from:2}{rssi:2}{msgType:1}{msgId:1}{millis:4}{CRLF:2}
        public IncomingMessage Parse(byte[] record)
        {
            var expectedLenght = 12;
            if(record.Length != expectedLenght)
                throw new ArgumentException($"{nameof(PingReport)} should be {expectedLenght} bytes long (was {record.Length})");
            return new PingReport() {
                FromRfAddress = BitConverter.ToUInt16(record, 0),
                Rssi = BitConverter.ToInt16 (record, 2),
                //Byte 4 is msgType
                MsgId = record[5],
                Millis = BitConverter.ToUInt32(record, 6),
            };
        }
    }
}