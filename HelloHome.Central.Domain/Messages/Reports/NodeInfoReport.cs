namespace HelloHome.Central.Domain.Messages.Reports
{
	public class NodeInfoReport : IncomingMessage
	{
		public int SendErrorCount { get; set; }
		public float? Voltage { get; set; }

		public override string ToString ()
		{
			return $"[NodeInfoReport: NodeId={FromRfAddress}, MsgId={MsgId}, SendErrorCount={SendErrorCount}, Voltage={Voltage}]";
		}
	}
}

