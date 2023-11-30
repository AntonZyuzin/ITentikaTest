using ITentikaTest.BackgroundServices;
using ITentikaTest.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ITentikaTest.Controllers
{
    public class GeneratorController : Controller
    {

        private readonly GeneratorHostedService _generatorHostedService;
        public GeneratorController(IServiceProvider serviceProvider)
        {
            _generatorHostedService = serviceProvider.GetServices<IHostedService>().OfType<GeneratorHostedService>().Single();
        }

        [HttpPost]
        [Route("GenerateEvent")]
        public async Task<IActionResult> GenerateEventAsync(EventType type)
        {
            await _generatorHostedService.GenerateEventByUser(type);
            return Ok();
        }
    }
}
