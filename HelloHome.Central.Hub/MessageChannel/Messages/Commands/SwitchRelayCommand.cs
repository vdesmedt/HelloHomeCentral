namespace HelloHome.Central.Hub.MessageChannel.Messages.Commands
{
    public class SwitchRelayCommand : OutgoingMessage
    {
        public byte SwitchActuatorNumber { get; set; }
        public bool NewState { get; set; }
    }
}