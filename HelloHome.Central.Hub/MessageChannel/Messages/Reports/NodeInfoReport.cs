namespace HelloHome.Central.Hub.MessageChannel.Messages.Reports
{
	public class NodeInfoReport : IncomingMessage
	{
		public int SendErrorCount { get; set; }
		public float? Voltage { get; set; }

		public override string ToString ()
		{
			return $"[NodeInfoReport: NodeId={FromRfAddress}, SendErrorCount={SendErrorCount}, Voltage={Voltage}]";
		}
	}
}

