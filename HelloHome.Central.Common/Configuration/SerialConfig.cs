using System.Dynamic;

namespace HelloHome.Central.Common.Configuration
{
    public class SerialConfig
    {
        public virtual string Port { get; set; }
        public virtual int BaudRate { get; set; }
        public virtual int TimeOut { get; set; } = 500;
    }
}