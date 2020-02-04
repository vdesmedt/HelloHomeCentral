using System.Collections.Generic;
using System.Linq;
using HelloHome.Central.Domain;
using HelloHome.Central.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HelloHome.Central.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NodeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public NodeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IEnumerable<Node> Get()
        {
            return _unitOfWork.Nodes.ToList();
        }
    }
}