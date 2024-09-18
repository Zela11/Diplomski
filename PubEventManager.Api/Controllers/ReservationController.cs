using Microsoft.AspNetCore.Mvc;
using PubEventManager.Application.Dtos;
using PubEventManager.Application.IServices;
using PubEventManager.Application.Services;
using PubEventManager.Domain.IRepositories;

namespace PubEventManager.Api.Controllers;
[ApiController]
[Route("api/reservations")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;
    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReservationDto reservationDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _reservationService.CreateReservation(reservationDto);
        if (!result)
        {
            return BadRequest("Adding failed.");
        }

        //await _emailService.SendReservationConfirmationEmail(reservationDto.GuestId);


        return Ok(new { message = "Reservation created successfully." });
    }
    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetReservationsByEventId(int eventId)
    {
        var reservations = await _reservationService.GetReservationsByEventId(eventId);
        return Ok(reservations);
    }
}
