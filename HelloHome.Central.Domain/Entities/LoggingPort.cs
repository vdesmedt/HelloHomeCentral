using System.Collections.Generic;

namespace HelloHome.Central.Domain.Entities
{
    public class FloatDataLogPort : LoggingPort
    {
        public float Data { get; set; }
        public IList<FloatDataLogPortHistory> History { get; set; }
    }

    public class IntDataLogPort : LoggingPort
    {
        public int Data { get; set; }
        public IList<IntDataLogPortHistory> History { get; set; }
    }

    public abstract class LoggingPort : Port
    {
    }
}