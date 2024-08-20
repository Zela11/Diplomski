using PubEventManager.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Application.IServices
{
    public interface IReservationService
    {
        Task<bool> CreateReservation(ReservationDto reservationDto);
    }
}
