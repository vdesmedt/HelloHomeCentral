using System;
using System.Text;
using HelloHome.Central.Domain.Entities;
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
		    var expectedLenght = 17 + 3 + 2;
			if(record.Length != expectedLenght)
				throw new ArgumentException($"{nameof(NodeStartedReport)} should be {expectedLenght} bytes long (was {record.Length})");
            return new NodeStartedReport {
                FromRfAddress = record [0],
                Rssi = (int)BitConverter.ToInt16 (record, 1),
	            NodeType = (NodeType)record[4],
                Signature = BitConverter.ToInt64 (record, 5),
	            Version = Encoding.ASCII.GetString(record, 13, 7),
            };
		}

		#endregion
	}
}

