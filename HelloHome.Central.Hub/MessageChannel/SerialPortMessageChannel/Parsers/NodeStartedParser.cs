using System;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
    [ParserFor(0 + (3 << 2))]
	public class NodeStartedParser : IMessageParser
	{
		#region IMessageParser implementation

	    public IncomingMessage Parse (byte[] record)
		{
            return new NodeStartedReport {
                FromRfAddress = record [0],
                Rssi = (int)BitConverter.ToInt16 (record, 1),
                Major = record [4],
                Minor = record [5],
                Signature = BitConverter.ToInt64 (record, 6),
                NeedNewRfAddress = BitConverter.ToBoolean (record, 14),
            };
		}

		#endregion
	}
}

