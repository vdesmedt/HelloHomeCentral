using System.Collections.Generic;

namespace HelloHome.Central.Domain.Entities
{
    public class RelayActuatorPort : ActuatorPort
    {
        public OnOffState RelayState { get; set; }
        public IList<RelayHistory> History { get; set; }
    }

    public abstract class ActuatorPort : Port
    {
    }
}