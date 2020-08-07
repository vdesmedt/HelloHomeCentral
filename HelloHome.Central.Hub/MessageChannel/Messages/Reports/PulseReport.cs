using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace HelloHome.Central.Hub.MessageChannel.Messages.Reports
{
	public class PulseReport : IncomingMessage
	{
		public byte PortNumber { get; set; }
		public int NewPulse { get; set; }

		public override string ToString ()
		{
			return $"[PulseReport: Node={FromRfAddress}, PortNumber={PortNumber}, NewPulse={NewPulse}]";
		}
	}
}

