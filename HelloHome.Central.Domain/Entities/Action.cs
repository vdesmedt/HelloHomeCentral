using System.Collections.Generic;

namespace HelloHome.Central.Domain.Entities
{
    public abstract class Action
    {
        public int Id { get; set; }
        public int TriggerId { get; set; }
        public Trigger Trigger { get; set; }
        public int Sequence { get; set; }
        public List<ActuatorPort> Actuators { get; set; }
    }

    public abstract class RelayAction : Action
    {
    }

    public class TurnOnAction : RelayAction
    {
    }

    public class TurnOffAction : RelayAction
    {
    }
}