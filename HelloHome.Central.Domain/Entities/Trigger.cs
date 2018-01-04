using System.Collections.Generic;

namespace HelloHome.Central.Domain.Entities
{
    public abstract class Trigger
    {
        public int Id { get; set; }
        public List<SensorPort> Sensors { get; set; }
        public List<Action> Actions { get; set; }
    }

    public class CronTrigger : Trigger
    {
        public string CronExpression { get; set; }
    }

    public class PushTrigger : Trigger
    {
    }

    public class SwitchTrigger : Trigger
    {
        public bool? TriggerOnState { get; set; }
    }

    public class VarioTrigger : Trigger
    {
    }
}