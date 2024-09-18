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
            var existingEvent = await _eventRepository.GetByDate(newEvent.Date);
            if(existingEvent != null)
            {
                return false;
            }
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

        public async Task<List<Event>> GetAllAsync()
        {
            return await _eventRepository.GetAllAsync();
        }

        public async Task<EventDto> GetByIdAsync(int id)
        {
            var foundEvent = await _eventRepository.GetByIdAsync(id);
            if(foundEvent == null)
            {
                return null;
            }
            return new EventDto
            {
                Date = foundEvent.Date,
                Name = foundEvent.Name,
                ManagerId = foundEvent.ManagerId,
                Description = foundEvent.Description,
            };
        }
    }
}
