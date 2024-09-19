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
            byte[] imageData = null;
            if (newEvent.Image != null && newEvent.Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await newEvent.Image.CopyToAsync(memoryStream);
                    imageData = memoryStream.ToArray(); // Pretvaranje slike u byte[]
                }
            }
            var eventToAdd = new Event
            {
                Name = newEvent.Name,
                Description = newEvent.Description,
                ManagerId = newEvent.ManagerId,
                Date = newEvent.Date,
                Image = imageData,
            };
            await _eventRepository.AddAsync(eventToAdd);
            return true;
        }

        public async Task<List<Event>> GetAllAsync()
        {
            var events = await _eventRepository.GetAllAsync(); // Pretpostavljam da uzimaš događaje iz repozitorijuma

            foreach (var evt in events)
            {
                if (evt.Image != null)
                {
                    evt.ImageSrc = $"data:image/jpeg;base64,{Convert.ToBase64String(evt.Image)}"; // Dodaj Base64 string
                }
            }

            return events;
        }


        public async Task<Event> GetByIdAsync(int id)
        {
            var foundEvent = await _eventRepository.GetByIdAsync(id);
            if(foundEvent == null)
            {
                return null;
            }
            if (foundEvent.Image != null)
            {
                foundEvent.ImageSrc = $"data:image/jpeg;base64,{Convert.ToBase64String(foundEvent.Image)}"; // Dodaj Base64 string
            }
            return foundEvent;
            
        }
    }
}
