namespace HelloHome.Central.Domain.Messages.Reports
{
	public class PulseReport : IncomingMessage
	{
		public byte PortNumber { get; set; }
		public int NewPulse { get; set; }

		public override string ToString ()
		{
			return $"[PulseReport: Node={FromRfAddress}, MsgId={MsgId}, PortNumber={PortNumber}, NewPulse={NewPulse}]";
		}
	}
}

