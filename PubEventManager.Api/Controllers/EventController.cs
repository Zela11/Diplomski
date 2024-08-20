using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using PubEventManager.Application.Dtos;
using PubEventManager.Application.IServices;

namespace PubEventManager.Api.Controllers
{
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] EventDto eventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventService.AddEventAsync(eventDto);
            if (!result)
            {
                return BadRequest("Adding failed.");
            }

            return Ok(new { message = "Event created successfully." });
        }
    }
}
