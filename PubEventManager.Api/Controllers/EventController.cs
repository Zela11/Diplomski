using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using PubEventManager.Application.Dtos;
using PubEventManager.Application.IServices;
using PubEventManager.Application.Services;
using PubEventManager.Domain.Entities;

namespace PubEventManager.Api.Controllers;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] EventDto eventDto)
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
    [HttpGet]
    public async Task<ActionResult<List<Event>>> GetAll()
    {
        var events = await _eventService.GetAllAsync();
        return Ok(events);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Event>> GetEventById(int id)
    {
        var foundEvent = await _eventService.GetByIdAsync(id);
        if (foundEvent == null) return NotFound();

        return Ok(foundEvent);
    }
    [HttpGet("{id}/report")]
    public async Task<IActionResult> GenerateEventReport(int id)
    {
        var pdfStream = await _eventService.GenerateEventReportAsync(id);

        if (pdfStream == null)
        {
            return NotFound("Event not found or no reservations.");
        }

        return File(pdfStream.ToArray(), "application/pdf", "EventReport.pdf");
    }

}
