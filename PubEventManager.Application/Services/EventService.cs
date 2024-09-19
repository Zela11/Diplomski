using iTextSharp.text.pdf;
using PubEventManager.Application.Dtos;
using PubEventManager.Application.IServices;
using PubEventManager.Domain.Entities;
using PubEventManager.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text;


namespace PubEventManager.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IUserRepository _userRepository;
        public EventService(IEventRepository eventRepository, IReservationRepository reservationRepository, IUserRepository userRepository)
        {
            _eventRepository = eventRepository;
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
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

        public async Task<byte[]> GenerateEventReportAsync(int eventId)
        {
            int allTables = 11;

            var foundEvent = await _eventRepository.GetByIdAsync(eventId);
            if (foundEvent == null)
            {
                return null;
            }

            var manager = await _userRepository.GetByIdAsync(foundEvent.ManagerId);
            var eventReservations = await _reservationRepository.GetReservationsByEventId(foundEvent.Id);

            var guests = new List<User>();
            var guestIds = eventReservations.Select(r => r.GuestId).Distinct().ToList();

            foreach (int id in guestIds)
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (!guests.Any(g => g.Email == user.Email))
                {
                    guests.Add(user);
                }
            }

            using (var pdfStream = new MemoryStream())
            {
                using (var document = new iTextSharp.text.Document())
                {
                    var writer = PdfWriter.GetInstance(document, pdfStream);
                    document.Open();



                    var headerFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12);
                    var creationTimeFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10);

                    // Create a table for the header
                    var headerTable = new PdfPTable(2) // 2 columns
                    {
                        WidthPercentage = 100,
                    };

                    // Add company name cell without border
                    var companyNameCell = new PdfPCell(new Phrase("MacLaren's Pub", headerFont)) { Border = PdfPCell.NO_BORDER };
                    headerTable.AddCell(companyNameCell);

                    // Add creation time cell with right alignment
                    var creationTimeCell = new PdfPCell(new Phrase($"{DateTime.Now:MMMM dd, yyyy HH:mm}", creationTimeFont))
                    {
                        Border = PdfPCell.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_RIGHT // Align to right
                    };
                    headerTable.AddCell(creationTimeCell);

                    // Add the header table to the document
                    document.Add(headerTable);


                    // Create a new table for the line
                    var lineTable = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin // Set total width
                    };

                    // Create a cell with a border at the bottom
                    var lineCell = new PdfPCell(new Phrase(""))
                    {
                        Border = PdfPCell.NO_BORDER,
                        BorderWidthBottom = 1, // Set bottom border width
                        BorderColorBottom = BaseColor.BLACK // Set bottom border color
                    };

                    // Add the cell to the table
                    lineTable.AddCell(lineCell);
                    document.Add(new Paragraph("\n")); // Add some space

                    // Add the line table to the document
                    document.Add(lineTable);



                    // Title
                    var titleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 22, iTextSharp.text.Font.BOLD);
                    var titleParagraph = new Paragraph("Event Reservations Report", titleFont)
                    {
                        Alignment = iTextSharp.text.Element.ALIGN_CENTER
                    };
                    document.Add(titleParagraph);
                    document.Add(new Paragraph("\n")); // Add some space

                    // Event Information
                    var infoFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD);
                    var eventInfo = new PdfPTable(3)
                    {
                        WidthPercentage = 100
                    };

                    // Add cells without borders
                    var eventNameCell = new PdfPCell(new Phrase("Event Name:", infoFont)) { Border = PdfPCell.NO_BORDER };
                    var eventValueCell = new PdfPCell(new Phrase(foundEvent.Name, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12))) { Border = PdfPCell.NO_BORDER };
                    var organizedByCell = new PdfPCell(new Phrase("Organized by:", infoFont)) { Border = PdfPCell.NO_BORDER };
                    var managerCell = new PdfPCell(new Phrase($"{manager.FirstName} {manager.LastName}", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12))) { Border = PdfPCell.NO_BORDER };
                    var dateCell = new PdfPCell(new Phrase("Date:", infoFont)) { Border = PdfPCell.NO_BORDER };
                    var dateValueCell = new PdfPCell(new Phrase(foundEvent.Date.ToString("MMMM dd, yyyy"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12))) { Border = PdfPCell.NO_BORDER };


                    eventInfo.AddCell(eventNameCell);
                    eventInfo.AddCell(organizedByCell);
                    eventInfo.AddCell(dateCell);
                    eventInfo.AddCell(eventValueCell);
                    eventInfo.AddCell(managerCell);
                    eventInfo.AddCell(dateValueCell);

                    document.Add(eventInfo);
                    document.Add(new Paragraph("\n")); // Add some space
                    document.Add(lineTable);

                    var reservationsFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD);

                    var detailFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12);
                    document.Add(new Paragraph("Reservations:", reservationsFont));
                    foreach (var reservation in eventReservations)
                    {
                        var guest = guests.FirstOrDefault(g => g.Id == reservation.GuestId);
                        if (guest != null)
                        {
                            var reservationParagraph = new Paragraph($"• Guest: {guest.FirstName} {guest.LastName}, Table: {reservation.TableId}", detailFont)
                            {
                                IndentationLeft = 20 // Indent the paragraph
                            };
                            document.Add(reservationParagraph);
                        }
                    }

                    document.Add(new Paragraph("\n")); // Add some space
                    document.Add(lineTable);
                    var statisticsFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD);
                    document.Add(new Paragraph("Statistics:", statisticsFont));

                    var occupiedTables = eventReservations.Count;
                    var availableTables = allTables - occupiedTables;
                    var percentageOccupied = (double)occupiedTables / allTables * 100;

                    document.Add(new Paragraph($"• Number of Occupied Tables: {occupiedTables}", detailFont)
                    {
                        IndentationLeft = 20 // Indent the paragraph
                    });
                    document.Add(new Paragraph($"• Number of Available Tables: {availableTables}", detailFont)
                    {
                        IndentationLeft = 20 // Indent the paragraph
                    });
                    document.Add(new Paragraph($"• Percentage Occupied: {percentageOccupied:F2}%", detailFont)
                    {
                        IndentationLeft = 20 // Indent the paragraph
                    });
                    // Add some space at the end
                    document.Add(new Paragraph("\n"));

                    document.Close();
                }

                // Convert the stream to byte array
                return pdfStream.ToArray();
            }
        }







    }
}
