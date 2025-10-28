using System;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.Messages;
using HelloHome.Central.Domain.Messages.Reports;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers.Base;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
    [ParserFor(Constants.Message.Report.PongReport)]
    public class PongParser : IMessageParser
    {
        public IncomingMessage Parse(byte[] record)
        {
            var expectedLenght = 18;
            if(record.Length != expectedLenght)
                throw new ArgumentException($"{nameof(PongReport)} should be {expectedLenght} bytes long (was {record.Length})");
            return new PongReport() {
                FromRfAddress = BitConverter.ToUInt16(record, 0),
                Rssi = BitConverter.ToInt16 (record, 2),
                //Byte 4 is msgType
                MsgId = record[5],
                MillisIn = BitConverter.ToUInt32(record, 6),
                MillisOut = BitConverter.ToUInt32(record, 10),
                PingRssi = BitConverter.ToInt16(record, 14)
            };
        }
    }
}