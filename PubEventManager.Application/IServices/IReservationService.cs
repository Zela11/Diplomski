﻿using PubEventManager.Application.Dtos;
using PubEventManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Application.IServices
{
    public interface IReservationService
    {
        Task<Reservation> CreateReservation(ReservationDto reservationDto);
        Task<IEnumerable<ReservationDto>> GetReservationsByEventId(int eventId); // Dodata metoda

    }
}
