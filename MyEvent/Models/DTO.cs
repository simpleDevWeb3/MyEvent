using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;
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
    [JsonPropertyName("features")]
    public List<Feature> Features { get; set; }
}

public class Feature
{
    [JsonPropertyName("geometry")]
    public Geometry Geometry { get; set; }

    [JsonPropertyName("properties")]
    public Properties Properties { get; set; }
}

public class Geometry
{
    /*
    [JsonPropertyName("type")]
    public string Type { get; set; }
    */
    [JsonPropertyName("coordinates")]
    public List<double> Coordinates { get; set; } //lon, lat
}

public class Properties
{
    [JsonPropertyName("name")] public string Premise { get; set; }
    [JsonPropertyName("housenumber")] public string HouseNumber { get; set; }
    [JsonPropertyName("street")] public string Street { get; set; }
    [JsonPropertyName("suburb")] public string Suburb { get; set; }
    [JsonPropertyName("state")] public string State { get; set; }
    [JsonPropertyName("postcode")] public string Postcode { get; set; }
    [JsonPropertyName("city")] public string City { get; set; }

    [JsonPropertyName("formatted")]
    public string Formatted { get; set; }
    //public string AddressLine1 => string.Join(" ", new[] { HouseNumber, Premise });
    //public string AddressLine2 => string.Join(" ", new[] { Street, Suburb });
    //public string AddressLine3 => string.Join(" ", new[] { Postcode, City });

    //public string FullAddress => $"{AddressLine1}, {AddressLine2}, {AddressLine3}, {State}";
}

public class UserDTO
{

    public int Id { get; set; }

    public string Email { get; set; }

    public string Hash { get; set; }

    public string Name { get; set; }


    public string PhotoURL { get; set; }

    public string Role { get; set; }
}
public class TicketDTO
{
    public int TicketId { get; set; }

    public string EventId { get; set; }
    public EventDTO Event { get; set; }

    public int BuyerId { get; set; } 
    public UserDTO Buyer { get; set; }

    public string HolderName { get; set; }
    public string? HolderEmail { get; set; }

    public DateOnly PurchaseDate { get; set; }
}