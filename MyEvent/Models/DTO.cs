using System.ComponentModel.DataAnnotations;
using static MyEvent.Models.DB;

namespace MyEvent.Models;
#nullable disable warnings
public class EventDTO
{
    public string EventId { get; set; }
  
    public string Title { get; set; }

    public string ImageUrl { get; set; }
    public string Street { get; set; }
    public string City{ get; set; }
    public string State { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }

    public string Description { get; set; }

    public string Organizer { get; set; }
    public string ContactEmail { get; set; }
 
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
  
}
