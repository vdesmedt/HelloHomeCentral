using System;
using System.Runtime;
using HelloHome.Central.Common;
using NLog.Time;

namespace HelloHome.Central.Domain.Entities
{
    public abstract class Condition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public abstract bool Check();
    }

    public abstract class PortBasedCondition<TPort> : Condition where TPort : Port
    {
        public TPort Port { get; set; }
        public int PortId { get; set; }
    }

    public abstract class RelayBasedCondition : PortBasedCondition<RelayActuatorPort>
    {
    }

    public class RelayOnCondition : RelayBasedCondition
    {
        public override bool Check()
        {
            return Port.RelayState == OnOffState.On;
        }
    }

    public class RelayOffCondition : RelayBasedCondition
    {
        public override bool Check()
        {
            return Port.RelayState == OnOffState.Off;
        }
    }

    public abstract class SwitchBasedCondition : PortBasedCondition<SwitchSensorPort>
    {
    }

    public class SwitchOnCondition : SwitchBasedCondition
    {
        public override bool Check()
        {
            return Port.SwitchState == OnOffState.On;
        }
    }

    public class SwitchOffCondition : SwitchBasedCondition
    {
        public override bool Check()
        {
            return Port.SwitchState == OnOffState.Off;
        }
    }

    public abstract class EnvironmentBasedCondition : PortBasedCondition<EnvironmentSensorPort>
    {
    }

    public class TemperatureWithinRangeCondition : EnvironmentBasedCondition
    {
        public float MinTemperature { get; set; }
        public float MaxTemperature { get; set; }

        public override bool Check()
        {
            return Port.Temperature >= MinTemperature && Port.Temperature <= MaxTemperature;
        }
    }

    public class TimeOfTheDayCondition : Condition
    {
        private readonly ITimeProvider _timeProvider;

        public TimeOfTheDayCondition()
        {
        }

        public TimeOfTheDayCondition(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }

        public override bool Check()
        {
            var currentTime = _timeProvider.UtcNow.TimeOfDay;
            return currentTime >= From && currentTime <= To;
        }
    }
}