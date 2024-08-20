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
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public async Task<bool> AddEventAsync(EventDto newEvent)
        {
            var eventToAdd = new Event
            {
                Name = newEvent.Name,
                Description = newEvent.Description,
                ManagerId = newEvent.ManagerId,
                Date = newEvent.Date,
            };
            await _eventRepository.AddAsync(eventToAdd);
            return true;
        }
    }
}
