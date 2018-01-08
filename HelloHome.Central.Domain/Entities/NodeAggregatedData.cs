using System;

namespace HelloHome.Central.Domain.Entities
{
    public class NodeAggregatedData
    {
        public int Id { get; set; }
        public Node Node { get; set; }
		public float? VIn { get; set; }
        public int SendErrorCount { get; set; }
        public float? Temperature { get; set; }
        public float? Humidity { get; set; }
        public float? AtmosphericPressure { get; set; }
		public int Rssi { get; set; }
		public DateTime StartupTime { get; set; }
        public float MaxUpTimeRaw { get; set; }

        public TimeSpan MaxUpTime
        {
            get => TimeSpan.FromDays(MaxUpTimeRaw);
            set => MaxUpTimeRaw = (float)value.TotalDays;
        }
    }
}