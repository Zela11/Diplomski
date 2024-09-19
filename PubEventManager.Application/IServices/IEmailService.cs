using PubEventManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Application.IServices
{
    public interface IEmailService
    {
        public Task SendReservationConfirmationEmail(Reservation reservation);
    }
}
