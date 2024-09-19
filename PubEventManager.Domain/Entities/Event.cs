using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Domain.Entities;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public int ManagerId { get; set; }
    public byte[] Image { get; set; }
    [NotMapped] // Ako koristiš Entity Framework, koristi [NotMapped] da označiš da ovo svojstvo nije deo baze podataka
    public string ImageSrc { get; set; } // Base64 encoded image string
}
