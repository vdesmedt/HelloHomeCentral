using System;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
    [ParserFor(0 + (0 << 2))]
    public class NodeInfoReportParser : IMessageParser
	{
		#region IMessageParser implementation

	    public IncomingMessage Parse (byte[] record)
		{
			var voltage = ((float)BitConverter.ToInt16 (record, 8)) / 100.0f;
			return new NodeInfoReport {
				FromRfAddress = record [0],
				Rssi = (int)BitConverter.ToInt16(record,1),
				SendErrorCount = BitConverter.ToInt16(record, 4),
				StartCount = BitConverter.ToInt16(record, 6),
				Voltage = voltage > 0 ? voltage : (float?)null,
			};
		}
		#endregion
	}
}

