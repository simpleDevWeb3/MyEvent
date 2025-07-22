using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
                            ImageUrl = e.ImageUrl,

                            Category = new CategoryDTO
                            {
                                Id = e.Category.Id,
                                Name = e.Category.Name
                            },

                            Address = new AddressDTO
                            {
                                Street = e.Address.Street,
                                City = e.Address.City,
                                State = e.Address.State,
                                Longitude = e.Address.Longitude,
                                Latitude = e.Address.Latitude
                            },

                            Detail = new EventDetailDTO
                            {
                                Date = e.Detail.Date,
                                Organizer = e.Detail.Organizer,
                                ContactEmail = e.Detail.ContactEmail,
                                StartTime = e.Detail.StartTime,
                                EndTime = e.Detail.EndTime,
                                Description = e.Detail.Description
                            }
                        }).ToList();


            return Ok(all);
        }

        //Api base on Id
        [HttpGet("Id/{Id}")]
        public IActionResult Id(string? Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                return BadRequest("Event Id is required.");
            }


            var data = db.Events
                        .Include(e => e.Address)
                        .Where(e => e.Id == Id)
                        .Include(e => e.Detail)
                        .Include(e => e.Category)
                        .Select(e => new EventDTO
                        {
                            EventId = e.Id,
                            Title = e.Title,
                            ImageUrl = e.ImageUrl,

                            Category = new CategoryDTO
                            {
                                Id = e.Category.Id,
                                Name = e.Category.Name
                            },

                            Address = new AddressDTO
                            {
                                Street = e.Address.Street,
                                City = e.Address.City,
                                State = e.Address.State,
                                Longitude = e.Address.Longitude,
                                Latitude = e.Address.Latitude
                            },

                            Detail = new EventDetailDTO
                            {
                                Date = e.Detail.Date,
                                Organizer = e.Detail.Organizer,
                                ContactEmail = e.Detail.ContactEmail,
                                StartTime = e.Detail.StartTime,
                                EndTime = e.Detail.EndTime,
                                Description = e.Detail.Description
                            }
                        });

            if (!data.Any())
            {
                return NotFound("No events found in the specified city.");
            }


            return Ok(data);
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
                        .Where(e => e.Address.City == location || e.Address.State == location)
                        .Include(e => e.Detail)
                        .Include(e => e.Category)
                        .Select(e => new EventDTO
                        {
                            EventId = e.Id,
                            Title = e.Title,
                            ImageUrl = e.ImageUrl,

                            Category = new CategoryDTO
                            {
                                Id = e.Category.Id,
                                Name = e.Category.Name
                            },

                            Address = new AddressDTO
                            {
                                Street = e.Address.Street,
                                City = e.Address.City,
                                State = e.Address.State,
                                Longitude = e.Address.Longitude,
                                Latitude = e.Address.Latitude
                            },

                            Detail = new EventDetailDTO
                            {
                                Date = e.Detail.Date,
                                Organizer = e.Detail.Organizer,
                                ContactEmail = e.Detail.ContactEmail,
                                StartTime = e.Detail.StartTime,
                                EndTime = e.Detail.EndTime,
                                Description = e.Detail.Description
                            }
                        }).ToList();

            if (!data.Any())
            {
                return NotFound("No events found in the specified city.");
            }


            return Ok(data);
        }


        [HttpGet("Search/{query}")]
        public IActionResult Search(string? query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Please enter something.");
            }

            query = query.ToLower().Trim(); // normalize input

            var data = db.Events
                        .Include(e => e.Address)
                        .Include(e => e.Category)
                        .Include(e => e.Detail)
                        .Where(e =>
                            e.Address.City.ToLower().Contains(query) ||
                            e.Address.State.ToLower().Contains(query) ||
                            e.Address.Street.ToLower().Contains(query) ||
                            e.Category.Name.ToLower().Contains(query) ||
                            e.Detail.Organizer.ToLower().Contains(query)||
                            e.Title.ToLower().Contains(query) 
                        )
                        .Select(e => new EventDTO
                        {
                            EventId = e.Id,
                            Title = e.Title,
                            ImageUrl = e.ImageUrl,

                            Category = new CategoryDTO
                            {
                                Id = e.Category.Id,
                                Name = e.Category.Name
                            },

                            Address = new AddressDTO
                            {
                                Street = e.Address.Street,
                                City = e.Address.City,
                                State = e.Address.State,
                                Longitude = e.Address.Longitude,
                                Latitude = e.Address.Latitude
                            },

                            Detail = new EventDetailDTO
                            {
                                Date = e.Detail.Date,
                                Organizer = e.Detail.Organizer,
                                ContactEmail = e.Detail.ContactEmail,
                                StartTime = e.Detail.StartTime,
                                EndTime = e.Detail.EndTime,
                                Description = e.Detail.Description
                            }
                        })
                        .ToList();

            if (!data.Any())
            {
                return NotFound("No events found matching your query.");
            }

            return Ok(data);
        }

        [HttpGet("{category}")]
        public IActionResult Category(string? category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return BadRequest("Not Exist.");
            }
            category = Uri.UnescapeDataString(category).Trim().ToLowerInvariant();
            var data = db.Events
                        .Include(e => e.Address)
                        .Include(e => e.Category)
                        .Include(e => e.Detail)
                        .Where(
                            e => e.Category.Name == category        
                        )
                        .Select(e => new EventDTO
                        {
                            EventId = e.Id,
                            Title = e.Title,
                            ImageUrl = e.ImageUrl,

                            Category = new CategoryDTO
                            {
                                Id = e.Category.Id,
                                Name = e.Category.Name
                            },

                            Address = new AddressDTO
                            {
                                Street = e.Address.Street,
                                City = e.Address.City,
                                State = e.Address.State,
                                Longitude = e.Address.Longitude,
                                Latitude = e.Address.Latitude
                            },

                            Detail = new EventDetailDTO
                            {
                                Date = e.Detail.Date,
                                Organizer = e.Detail.Organizer,
                                ContactEmail = e.Detail.ContactEmail,
                                StartTime = e.Detail.StartTime,
                                EndTime = e.Detail.EndTime,
                                Description = e.Detail.Description
                            }
                        })
                        .ToList();

            if (!data.Any())
            {
                return NotFound("No events found for " + category);
            }

            return Ok(data);
        }



    }
}
