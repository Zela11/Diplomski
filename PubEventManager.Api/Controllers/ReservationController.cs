﻿using Microsoft.AspNetCore.Mvc;
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
    private readonly IEmailService _emailService;
    public ReservationController(IReservationService reservationService, IEmailService emailService)
    {
        _reservationService = reservationService;
        _emailService = emailService;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReservationDto reservationDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _reservationService.CreateReservation(reservationDto);
        if (result == null)
        {
            return BadRequest("Adding failed.");
        }

        await _emailService.SendReservationConfirmationEmail(result);


        return Ok(new { message = "Reservation created successfully." });
    }
    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetReservationsByEventId(int eventId)
    {
        var reservations = await _reservationService.GetReservationsByEventId(eventId);
        return Ok(reservations);
    }
}
