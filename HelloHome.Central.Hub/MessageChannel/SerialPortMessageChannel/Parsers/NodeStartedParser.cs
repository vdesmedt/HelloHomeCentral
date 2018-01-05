using System;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
    [ParserFor(ParserForAttribute.MessageType.Report, 3)]
	public class NodeStartedParser : IMessageParser
	{
		#region IMessageParser implementation

	    public IncomingMessage Parse (byte[] record)
		{
			if(record.Length != 16 + 3)
				throw new ArgumentException("NodeStartedReport should be 19 bytes long");
            return new NodeStartedReport {
                FromRfAddress = record [0],
                Rssi = (int)BitConverter.ToInt16 (record, 1),
                Signature = BitConverter.ToInt64 (record, 4),
	            Version = BitConverter.ToString(record, 12, 7),
            };
		}

		#endregion
	}
}

