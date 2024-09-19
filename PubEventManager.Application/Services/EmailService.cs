using PubEventManager.Application.IServices;
using PubEventManager.Domain.Entities;
using PubEventManager.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using QRCoder;
using System.Drawing.Imaging;
using PubEventManager.Domain;
using Microsoft.Extensions.Options;

namespace PubEventManager.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IUserRepository _guestRepository;
        private readonly IEventRepository _eventRepository;
        private readonly SmtpSettings _emailSettings;

        public EmailService(IOptions<SmtpSettings> emailSettings, IUserRepository guestRepository, IEventRepository eventRepository)
        {
            _guestRepository = guestRepository;
            _eventRepository = eventRepository;
            _emailSettings = emailSettings.Value;
        }

        public async Task SendReservationConfirmationEmail(Reservation reservation)
        {
            var guest = await _guestRepository.GetByIdAsync(reservation.GuestId);
            var eventDetails = await _eventRepository.GetByIdAsync(reservation.EventId);
            if (guest == null || eventDetails == null)
            {
                throw new Exception("Guest or Event not found.");
            }
            var qrCode = GenerateQRCode(reservation.GuestId, reservation.EventId, reservation.TableId);

            var subject = "Reservation Confirmation";
            var body = $@"
                 <p>Dear {guest.FirstName} {guest.LastName},</p>
                 <p>Thank you for reserving a table at Maclaren's Pub. We are pleased to confirm your reservation for the event <strong>{eventDetails.Name}</strong> on <strong>{eventDetails.Date.ToString("MM/dd/yyyy HH:mm")}</strong>.</p>
                 <p>We look forward to your visit and wish you a pleasant stay at our pub!</p>
                 <p>Attached, you will find a QR code for your reservation. Please present and scan this QR code upon your arrival to ensure a smooth check-in process.</p>
                 <p>Best regards,<br/>
                 Maclaren's Pub<br/>
                 pubeventmanager@gmail.com<br/>
                 +381691600103</p>
            ";

            await SendEmail(guest.Email, subject, body, qrCode);
        }
        private byte[] GenerateQRCode(int guestId, int eventId, int tableId)
        {
            var qrData = $"{guestId}-{eventId}-{tableId}";

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new QRCode(qrCodeData);

                using (var qrCodeImage = qrCode.GetGraphic(20))
                using (var stream = new MemoryStream())
                {
                    qrCodeImage.Save(stream, ImageFormat.Png);
                    return stream.ToArray();
                }
            }
        }
        private async Task SendEmail(string to, string subject, string body, byte[] attachment = null)
        {
            using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
            {
                client.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(to);

                if (attachment != null)
                {
                    var attachmentStream = new MemoryStream(attachment);
                    var qrAttachment = new Attachment(attachmentStream, "QRCode.png", "image/png");

                    mailMessage.Attachments.Add(qrAttachment);
                }

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
