using System;
using System.Text;
using HelloHome.Central.Common;
using HelloHome.Central.Domain.Entities;
using HelloHome.Central.Hub.MessageChannel.Messages;
using HelloHome.Central.Hub.MessageChannel.Messages.Reports;
using HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers.Base;

namespace HelloHome.Central.Hub.MessageChannel.SerialPortMessageChannel.Parsers
{
    [ParserFor(Constants.Message.Report.NodeStartedReport)]
	public class NodeStartedParser : IMessageParser
	{
		#region IMessageParser implementation

	    public IncomingMessage Parse (byte[] record)
	    {
		    var expectedLenght = 26;
			if(record.Length != expectedLenght)
				throw new ArgumentException($"{nameof(NodeStartedReport)} should be {expectedLenght} bytes long (was {record.Length})");
            return new NodeStartedReport {
                FromRfAddress = BitConverter.ToUInt16(record, 0),
                Rssi = BitConverter.ToInt16 (record, 2),
	            NodeType = (NodeType)record[5],
                Signature = BitConverter.ToInt64 (record, 6),
	            Version = Encoding.ASCII.GetString(record, 14, 8),
                StartCount = BitConverter.ToUInt16(record, 22)
            };
		}

		#endregion
	}
}

