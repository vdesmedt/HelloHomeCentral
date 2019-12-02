using System;
using HelloHome.Central.Common;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers.Base;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
    [ParserFor(Constants.Message.Report.PulseReport)]
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

