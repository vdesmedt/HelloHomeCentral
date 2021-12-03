using System.Runtime.Serialization;
using HelloHome.Central.Domain.Entities;

namespace HelloHome.Central.Hub.MessageChannel.Messages.Reports
{
    public class NodeStartedReport : IncomingMessage, ISignedMessage
	{
		public string Version { get; set; }
		public long Signature { get; set; }
        public int StartCount { get; set; }
        public NodeType NodeType { get; set; }

		public override string ToString ()
		{
            return $"[NodeStartedReport: From={FromRfAddress}, MsgId={MsgId}, Version={Version}, Signature={Signature}, Type={NodeType}, StartCount={StartCount}]";
		}
	}
}