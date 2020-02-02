using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace HelloHome.Central.Hub.MessageChannel.Messages.Reports
{
	public class PulseReport : IncomingMessage
	{
		public int NewPulse1 { get; set; }
		public int NewPulse2 { get; set; }
		public int NewPulse3 { get; set; }

		public override string ToString ()
		{
			return $"[PulseReport: Node={FromRfAddress}, NewPulse1={NewPulse1}, NewPulse2={NewPulse2}, NewPulse3={NewPulse3}]";
		}

		public int[] NewPulses
		{
			get
			{
				return new int[] {NewPulse1, NewPulse2, NewPulse3};
			}
		}
	}
}

