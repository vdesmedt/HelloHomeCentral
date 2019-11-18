using System.Collections.Generic;
using HelloHome.Central.Common;
using HelloHome.Central.Common.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HelloHome.Central.Hub.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ITimeProvider _timeProvider;
        private readonly SerialConfig _serialConfig;

        public ValuesController(ITimeProvider timeProvider, IOptions<SerialConfig> serialConfig)
        {
            _timeProvider = timeProvider;
            _serialConfig = serialConfig.Value;
        }
        
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] {"value1", "value2", _timeProvider.UtcNow.ToString(), _serialConfig.Port };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }        
    }
}