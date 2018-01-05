using System.Collections.Generic;

namespace HelloHome.Central.Domain.Entities
{
    public abstract class Trigger
    {
        public int Id { get; set; }
        public List<Action> Actions { get; set; }
    }

    public abstract class SensorTrigger : Trigger
    {
        public SensorPort SensorPort { get; set; }
    }

    public class CronTrigger : Trigger
    {
        public string CronExpression { get; set; }
    }

    public class PushTrigger : SensorTrigger
    {
    }

    public class SwitchTrigger : SensorTrigger
    {
        public bool? TriggerOnState { get; set; }
    }

    public class VarioTrigger : SensorTrigger
    {
        public int MinDelta { get; set; }
    }
}