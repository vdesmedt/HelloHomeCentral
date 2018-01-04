using System;

namespace HelloHome.Central.Hub.MessageChannel.Messages.Commands
{
    public class NodeConfigCommand : OutgoingMessage
	{
	    public long Signature { get; set; }
	    public byte NewRfAddress { get; set; }
	    public byte Hal1Pin { get; set; }
	    public byte Hal2Pin { get; set; }
	    public byte DryPin { get; set; }
	    public byte VInTriggerPin { get; set; }
	    public byte VInMeasurePin { get; set; }
		public NodeFeature Features { get; set; }
		public byte RestartFreq { get; set; }
	    public byte NodeInfoFreq { get; set; }
	    public byte EnvironmentFreq { get; set; }
	}

	[Flags]
	public enum NodeFeature : byte
	{
		Hal1 = 1 << 0,
		Hal2 = 1 << 1,
		Dry = 1 << 2,
		VIn = 1 << 3,
		Si7021 = 1 << 4,
		Bmp = 1 << 5,		
	}


}

