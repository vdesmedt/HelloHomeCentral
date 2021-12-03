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
		    var expectedLenght = 27;
			if(record.Length != expectedLenght)
				throw new ArgumentException($"{nameof(NodeStartedReport)} should be {expectedLenght} bytes long (was {record.Length})");
            return new NodeStartedReport {
                FromRfAddress = BitConverter.ToUInt16(record, 0),
                Rssi = BitConverter.ToInt16 (record, 2),
                //Byte 4 is msgType
                MsgId = record[5],
	            NodeType = (NodeType)record[6],
                Signature = BitConverter.ToInt64 (record, 7),
	            Version = Encoding.ASCII.GetString(record, 15, 8),
                StartCount = BitConverter.ToUInt16(record, 23)
            };
		}

		#endregion
	}
}

