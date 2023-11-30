using ITentikaTest.BackgroundServices;
using ITentikaTest.Domain.IRepositories;
using ITentikaTest.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ITentikaTest.Controllers
{
    public class ProcessorController : Controller
    {
        private readonly ProcessorHostedService _processorHostedService;
        private readonly IIncidentRepository _incidentRepository;
        public ProcessorController(IServiceProvider serviceProvider, IIncidentRepository incidentRepository)
        {
            _processorHostedService = serviceProvider.GetServices<IHostedService>().OfType<ProcessorHostedService>().Single();
            _incidentRepository = incidentRepository;
        }

        [HttpPost]
        [Route("SendEvent")]
        public IActionResult SendEvent([FromBody] Event userEvent)
        {
            _processorHostedService.AcceptEvent(userEvent);
            return Ok();
        }

        [HttpGet]
        [Route("GetIncidents")]
        public IActionResult GetIncidents(
            [FromQuery] string? sortProperty = null,
            [FromQuery] int? page = null,
            [FromQuery]  int? limit = null)
        {
            var incidents = _incidentRepository.GetIncidents(sortProperty, page, limit);
            return Ok(incidents);
        }
    }
}
