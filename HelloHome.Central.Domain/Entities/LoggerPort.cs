using System.Collections.Generic;

namespace HelloHome.Central.Domain.Entities
{
    public class FloatLogger : LoggerPort
    {
        public float Data { get; set; }
        public IList<FloatLoggerHistory> History { get; set; }
    }

    public class IntLogger : LoggerPort
    {
        public int Data { get; set; }
        public IList<IntLoggerHistory> History { get; set; }
    }

    public abstract class LoggerPort : Port
    {
    }
}