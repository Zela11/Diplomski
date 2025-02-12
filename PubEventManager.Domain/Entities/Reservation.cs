﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Domain.Entities
{
    public class Reservation
    {
        public int GuestId { get; set; }
        public int EventId { get; set; }
        public int TableId { get; set; }
        public TimeOnly? ArrivalTime { get; set; } 
    }
}
