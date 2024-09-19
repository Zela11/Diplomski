using PubEventManager.Application.Dtos;
using PubEventManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Application.IServices
{
    public interface IEventService
    {
        Task<bool> AddEventAsync(EventDto newEvent);
        Task<List<Event>> GetAllAsync();
        Task<Event> GetByIdAsync(int id);
        Task<byte[]> GenerateEventReportAsync(int eventId);
    }
}
