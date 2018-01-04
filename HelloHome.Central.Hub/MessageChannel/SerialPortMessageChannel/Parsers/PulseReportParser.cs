using System;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
    [ParserFor(ParserForAttribute.MessageType.Report, 1)]
    public class PulseReportParser : IMessageParser
	{
		#region IReportParser implementation

	    public IncomingMessage Parse (byte[] record)
		{
			return new PulseReport {
				FromRfAddress = record [0],
				Rssi = (int)BitConverter.ToInt16(record,1),
				SubNode = record [4],
				NewPulses = BitConverter.ToUInt16(record, 5),
			};
		}

		#endregion
	}
}

