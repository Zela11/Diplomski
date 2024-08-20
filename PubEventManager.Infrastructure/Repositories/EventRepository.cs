using Microsoft.EntityFrameworkCore;
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
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;
        public EventRepository(AppDbContext context)
        {
            _context = context;

        }
        public async Task AddAsync(Event newEvent)
        {
            if (newEvent != null) { await _context.Events.AddAsync(newEvent); }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Event>> GetAllAsync()
        {
            return await _context.Events
                .ToListAsync();
        }
    }
}