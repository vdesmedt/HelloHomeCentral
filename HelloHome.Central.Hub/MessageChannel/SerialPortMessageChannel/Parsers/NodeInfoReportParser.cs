using System;
using HelloHome.Central.Common;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers.Base;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
    [ParserFor(Constants.Message.Report.NodeInfoReport)]
    public class NodeInfoReportParser : IMessageParser
	{
		#region IMessageParser implementation

	    public IncomingMessage Parse (byte[] record)
		{
			var voltage = BitConverter.ToInt16 (record, 7) / 100.0f;
			return new NodeInfoReport {
				FromRfAddress = BitConverter.ToUInt16(record, 0),
				Rssi = BitConverter.ToInt16(record,2),
				SendErrorCount = BitConverter.ToInt16(record, 5),
				Voltage = voltage > 0 ? voltage : (float?)null,
			};
		}
		#endregion
	}
}

