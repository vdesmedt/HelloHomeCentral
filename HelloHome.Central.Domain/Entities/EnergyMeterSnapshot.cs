using System;

namespace HelloHome.Central.Domain.Entities;

public abstract class EnergyMeterSnapshot
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public double Snapshot { get; set; }
}

public abstract class EnergyMeterSnapshot<TPort>:EnergyMeterSnapshot where TPort : Port
{
    public TPort Port { get; set; }
}

public class PulseEnergyMeterSnapshot : EnergyMeterSnapshot<PulseSensor>
{
    public int PulseCount { get; set; }
}