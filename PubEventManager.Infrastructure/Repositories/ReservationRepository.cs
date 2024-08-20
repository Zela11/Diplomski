using PubEventManager.Domain.Entities;
using PubEventManager.Domain.IRepositories;
using PubEventManager.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _context;
        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Reservation reservation)
        {
            if(reservation != null) { await _context.Reservations.AddAsync(reservation); }
            await _context.SaveChangesAsync();  
        }
    }
}
