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
				FromRfAddress = BitConverter.ToUInt16(record, 0),
				Rssi = BitConverter.ToInt16 (record, 2),
				Temperature = ((float)BitConverter.ToInt16(record, 5))/100.0f,
				Humidity = (float)BitConverter.ToInt16(record, 7),
				Pressure = (int)BitConverter.ToInt16(record, 9),
			};
		}

		#endregion
	}
}

