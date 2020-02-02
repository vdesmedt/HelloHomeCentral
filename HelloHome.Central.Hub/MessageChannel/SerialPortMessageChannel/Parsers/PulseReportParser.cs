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
				FromRfAddress = BitConverter.ToUInt16(record, 0),
				Rssi = BitConverter.ToInt16 (record, 2),
				NewPulse1 = BitConverter.ToUInt16(record, 5),
				NewPulse2 = BitConverter.ToUInt16(record, 7),
				NewPulse3 = BitConverter.ToUInt16(record, 9),
			};
		}

		#endregion
	}
}

