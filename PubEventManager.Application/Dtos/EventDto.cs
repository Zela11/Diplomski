using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PubEventManager.Application.Dtos
{
    public class EventDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int ManagerId { get; set; }
        public IFormFile Image { get; set; } // Koristi IFormFile za primanje slike
    }
}
