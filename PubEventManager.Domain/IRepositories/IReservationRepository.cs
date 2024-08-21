using PubEventManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Domain.IRepositories
{
    public interface IReservationRepository
    {
        Task AddAsync(Reservation reservation);
        Task<List<Reservation>> GetReservationsByEventId(int eventId); 
    }
}
