using System;
using System.Collections.Generic;

namespace HelloHome.Central.Domain.Entities
{
    public abstract class Action
    {
        public int Id { get; set; }
        public int? TriggerId { get; set; }
        public Trigger Trigger { get; set; }
    }

    public class ScheduleAction : Action
    {
        public TimeSpan Delay { get; set; }
        public Action ScheduledAction { get; set; }
    }

    public abstract class ActuatorAction : Action 
    {
        public ActuatorPort Actuator { get; set; }
    }

    public abstract class RelayAction : ActuatorAction
    {
        public RelayActuatorPort Relay => (RelayActuatorPort)Actuator;
    }

    public class TurnOnAction : RelayAction
    {
    }

    public class TurnOffAction : RelayAction
    {
    }
}