using HelloHome.Central.Domain.Entities;

namespace HelloHome.Central.Hub.MessageChannel.Messages.Commands
{
    public class NodeConfigCommand : OutgoingMessage
	{
	    public long Signature { get; set; }
	    public byte NewRfAddress { get; set; }
		public NodeFeature ExtraFeatures { get; set; }
	    public byte NodeInfoFreq { get; set; }
	    public byte EnvironmentFreq { get; set; }
	}
}

