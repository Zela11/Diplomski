using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Application.Dtos
{
    public class ReservationDto
    {
        public int GuestId { get; set; }
        public int EventId { get; set; }
        public int TableId { get; set; }
        public TimeOnly? ArrivalTime { get; set; }
    }
}
