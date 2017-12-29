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
	    public bool SiEnable { get; set; }
	    public bool BmpEnable { get; set; }
	    public byte RestartCheckFreq { get; set; }
	    public byte NodeInfoFreq { get; set; }
	    public byte EnvironmentFreq { get; set; }
	}


}

