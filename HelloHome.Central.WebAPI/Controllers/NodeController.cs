using System.Collections.Generic;
using HelloHome.Central.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HelloHome.Central.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NodeController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Node> Get()
        {
            return new List<Node>
            {
                new Node { Id = 1 }
            };
        }
    }
}