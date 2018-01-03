using System;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
    [ParserFor(0 + (1 << 2))]
    public class PulseReportParser : IMessageParser
	{
		public PulseReportParser ()
		{
		}

		#region IReportParser implementation

	    public IncomingMessage Parse (byte[] record)
		{
			return new PulseReport {
				FromRfAddress = record [0],
				Rssi = (int)BitConverter.ToInt16(record,1),
				SubNode = record [4],
				NewPulses = record[5],
			};
		}

		#endregion
	}
}

