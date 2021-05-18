using System.Threading.Tasks;
using Kmd.Logic.Cpr.Events.Receiver.Models;
using Kmd.Logic.Cpr.Events.Receiver.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kmd.Logic.Cpr.Events.Receiver.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventService eventService;

        public EventsController(EventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await this.eventService.GetEventstAsync().ConfigureAwait(false);
            return this.Ok(events);
        }

        [HttpPost]
        public async Task<IActionResult> PostEvent([FromBody] CprEvent cprEvent)
        {
            if (this.eventService.RejectEventsMode)
            {
                return this.Problem("Event receiver is in reject mode. Flip the switch to accept events");
            }

            await this.eventService.AddEventAsync(cprEvent).ConfigureAwait(false);

            return this.Ok();
        }
    }
}
