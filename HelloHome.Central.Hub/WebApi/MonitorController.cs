using HelloHome.Central.Hub.NodeBridge.Performance;
using Microsoft.AspNetCore.Mvc;

namespace HelloHome.Central.Hub.WebApi
{
    [ApiController]
    [Route("[controller]")]
    public class MonitorController : ControllerBase
    {
        private readonly IPerformanceStats _performanceStats;

        public MonitorController(IPerformanceStats performanceStats)
        {
            _performanceStats = performanceStats;
        }

        [HttpGet]
        public IPerformanceStats Index()
        {
            return _performanceStats;
        }

        [HttpGet("CallCount")]
        public long CallCount()
        {
            return _performanceStats.CallCount;
        }
        
    }
}