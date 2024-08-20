using PubEventManager.Application.Dtos;
using PubEventManager.Application.IServices;
using PubEventManager.Domain.Entities;
using PubEventManager.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        public ReservationService(IReservationRepository reservationRepository) 
        {
            _reservationRepository = reservationRepository;
        }
        public async Task<bool> CreateReservation(ReservationDto reservationDto)
        {
            var res = new Reservation
            {
                EventId = reservationDto.EventId,
                GuestId = reservationDto.GuestId,
                TableId = reservationDto.TableId,
                ArrivalTime = reservationDto.ArrivalTime,
            };
            await _reservationRepository.AddAsync(res);
            return true;
        }
    }
}
