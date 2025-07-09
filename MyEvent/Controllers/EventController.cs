using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MyEvent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        readonly DB db; 

        public EventController(DB db)
        {
            this.db = db;
        }

        [HttpGet("All")]
        public IActionResult All()
        {
            var all = db.Events
                        .Include(e => e.Address)
                        .Select(e => new EventDTO
                        {
                            EventId = e.Id,
                            Title = e.Title,
                            Street =e.Address.Street,
                            City = e.Address.City,
                            State = e.Address.State,
                            Longitude = e.Address.Longitude,
                            Latitude = e.Address.Latitude
                        })
                        .ToList();


            return Ok(all);
        }

        //Api base on location 
        [HttpGet("Location/{location}")]
        public IActionResult Location(string? location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return BadRequest("City name is required.");
            }


            var data = db.Events
                        .Include(e => e.Address)
                        .Where(e=> e.Address.City == location || e.Address.State == location)
                        .Include(e=>e.Detail)
                        .Select(e => new EventDTO
                        {
                            EventId = e.Id,
                            Title = e.Title,
                            ImageUrl = e.ImageUrl,
                            Street = e.Address.Street,
                            City = e.Address.City,
                            State = e.Address.State,
                            Longitude = e.Address.Longitude,
                            Latitude = e.Address.Latitude,
                            Date = e.Detail.Date,
                            Organizer = e.Detail.Organizer,
                            StartTime = e.Detail.StartTime,
                            EndTime = e.Detail.EndTime,
                            Description = e.Detail.Description,
                            

                        })
                        .ToList();

            if (!data.Any())
            {
                return NotFound("No events found in the specified city.");
            }


            return Ok(data);
        }
    }


}
