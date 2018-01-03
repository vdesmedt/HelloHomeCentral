using System;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
    [ParserFor(0 + (2 << 2))]
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

