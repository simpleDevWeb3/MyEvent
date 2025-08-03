using System.ComponentModel.DataAnnotations;
using static MyEvent.Models.DB;

namespace MyEvent.Models;
#nullable disable warnings
public class EventDTO
{
    public string EventId { get; set; }
  
    public string Title { get; set; }
    
    public string ImageUrl { get; set; }

    public AddressDTO Address { get; set; }
    public EventDetailDTO Detail { get; set; }
    public CategoryDTO Category { get; set; }


}
public class AddressDTO
{
    
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}
public class EventDetailDTO
{
    public string Description { get; set; }

    public string Organizer { get; set; }
    public string ContactEmail { get; set; }

    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
public class CategoryDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
}

//cheng added!!!*************************************************
//cheng's code
public class GeoapifyGeocodingResponseDto
{
    public List<Feature> features { get; set; }
}

public class Feature
{
    public Geometry geometry { get; set; }
}

public class Geometry
{
    public List<double> coordinates { get; set; } // [lon, lat]
}
