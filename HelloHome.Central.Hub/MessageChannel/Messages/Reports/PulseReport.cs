namespace HelloHome.Central.Hub.MessageChannel.Messages.Reports
{
	public class PulseReport : IncomingMessage
	{
		public int SubNode { get; set; }
		public int NewPulses { get; set; }

		public override string ToString ()
		{
			return $"[PulseReport: Node={FromRfAddress}, SubNode={SubNode}, NewPulses={NewPulses}]";
		}
	}
}

