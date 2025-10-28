using System;
using HelloHome.Central.Domain.Entities;
using Action = HelloHome.Central.Domain.Entities.Action;


namespace HelloHome.Central.Domain.Messages.Commands
{
    public interface IActionToCommandMapper
    {
        SetRelayStateCommand Map(TurnOnAction a);
        SetRelayStateCommand Map(TurnOffAction a);
        OutgoingMessage Map(Action a);
    }

    public class ActionToCommandMapper : IActionToCommandMapper
    {
        public OutgoingMessage Map(Action a)
        {
            if (a is TurnOnAction tnAction)
                return Map(tnAction);
            if (a is TurnOffAction tfAction)
                return Map(tfAction);
            throw new ArgumentOutOfRangeException($"Cannot map action of type {a.GetType().Name}");
        }

        public SetRelayStateCommand Map(TurnOnAction a)
        {
            return new SetRelayStateCommand
            {
                ToRfAddress = a.Relay.Node.RfAddress,
                PortNumber = a.Relay.PortNumber,
                NewState = 1
            };
        }

        public SetRelayStateCommand Map(TurnOffAction a)
        {
            return new SetRelayStateCommand
            {
                ToRfAddress = a.Relay.Node.RfAddress,
                PortNumber = a.Relay.PortNumber,
                NewState = 0
            };
        }
    }
}