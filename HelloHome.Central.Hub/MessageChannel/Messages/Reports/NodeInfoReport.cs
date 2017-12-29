namespace HelloHome.Central.Hub.MessageChannel.Messages.Reports
{
	public class NodeInfoReport : Report
	{
		public int SendErrorCount { get; set; }
		public int StartCount { get; set; }
		public float? Voltage { get; set; }

		public override string ToString ()
		{
			return $"[NodeInfoReport: NodeId={FromRfAddress}, SendErrorCount={SendErrorCount}, Voltage={Voltage}]";
		}
	}
}

