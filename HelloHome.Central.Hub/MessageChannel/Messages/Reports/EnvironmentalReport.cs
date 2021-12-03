namespace HelloHome.Central.Hub.MessageChannel.Messages.Reports
{
	public class EnvironmentalReport : IncomingMessage
	{
		public float Temperature { get; set; }

		public float Humidity { get; set; }

		public float Pressure { get; set; }

		public override string ToString ()
		{
			return $"[EnvironmentalInfoReport: NodeId={FromRfAddress}, MsgId={MsgId}, Temperature={Temperature}, Humidity={Humidity}, Pressure={Pressure}]";
		}
	}
}

