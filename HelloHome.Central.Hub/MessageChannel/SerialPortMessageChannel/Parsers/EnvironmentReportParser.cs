using System;
using HelloHome.Central.Common;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers.Base;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
    [ParserFor(Constants.Message.Report.EnvironmentReport)]
    public class EnvironmentReportParser : IMessageParser
	{
		#region IMessageParser implementation

	    public IncomingMessage Parse (byte[] record)
		{
			return new EnvironmentalReport 
			{
				FromRfAddress = record[0],
				Rssi = (int)BitConverter.ToInt16(record,1),
				Temperature = ((float)BitConverter.ToInt16(record, 4))/100.0f,
				Humidity = (float)BitConverter.ToInt16(record, 6),
				Pressure = (int)BitConverter.ToInt16(record, 8),
			};
		}

		#endregion
	}
}

