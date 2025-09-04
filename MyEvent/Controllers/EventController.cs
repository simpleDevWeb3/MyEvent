using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.Xml;
using static MyEvent.Models.DB;


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
                        })
                       .ToList();


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

        [HttpGet("getParticipant/{eventId}")]
        public IActionResult getParticipant(string? eventId)
        {

            //step 1 : finds participant data
            var participantIds = db.Tickets
                               .Where(t => t.EventId == eventId)
                               .Select(t => t.BuyerId)
                               .Distinct()
                               .ToList();

            //step2: get user data
            var user = db.Users
                       .Where(u=> participantIds.Contains(u. Id))
                        .Select(u => new UserDTO
                        {
                            Id = u.Id,
                            Email = u.Email,
                            Name = u.Name,
                            PhotoURL = u.PhotoURL,
                            Role = u.Role
                        })
                       .ToList();

            return Ok(user);
        }


        [HttpGet("Recommended/{query}/{eventId}")]
        public IActionResult Recommended(string? query ,string?eventId)
        {
            // handle find participant joined also event 
            if (query == "Participant Also Joined")
            {
                //step 1 : finds participant data
                var participantIds = db.Tickets
                                   .Where(t => t.EventId == eventId)
                                   .Select(t => t.BuyerId)
                                   .Distinct()
                                   .ToList();

                // Step 2: Find other events that these participants also joined
                var events = db.Tickets
                            .Where(t => participantIds.Contains(t.BuyerId) && t.EventId != eventId)
                             .Select(t => new EventDTO
                             {
                                 EventId = t.Event.Id,
                                 Title = t.Event.Title,
                                 ImageUrl = t.Event.ImageUrl,

                                 Category = new CategoryDTO
                                 {
                                     Id = t.Event.Category.Id,
                                     Name = t.Event.Category.Name
                                 },

                                 Address = new AddressDTO
                                 {
                                     Street = t.Event.Address.Street,
                                     City = t.Event.Address.City,
                                     State = t.Event.Address.State,
                                     Longitude = t.Event.Address.Longitude,
                                     Latitude = t.Event.Address.Latitude
                                 },

                                 Detail = new EventDetailDTO
                                 {
                                     Date = t.Event.Detail.Date,
                                     Organizer = t.Event.Detail.Organizer,
                                     ContactEmail = t.Event.Detail.ContactEmail,
                                     StartTime = t.Event.Detail.StartTime,
                                     EndTime = t.Event.Detail.EndTime,
                                     Description = t.Event.Detail.Description
                                 }
                             })
                            .ToList();
                            

                //2. return result
                return Ok(events); 
            }


            // handle  same cateogy , and same organizer 
            var searchResult = Search(query) as OkObjectResult;

            if (searchResult?.Value == null)
            {
                return NotFound("No events found.");
            }

            return Ok(searchResult.Value);
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

      [HttpGet("filter")]
      public IActionResult Filter(
          [FromQuery(Name = "Query")] string? query,
          [FromQuery(Name = "Category")] string? category,
          [FromQuery(Name = "StartDate")] DateOnly? startDate,
          [FromQuery(Name = "EndDate")] DateOnly? endDate,
          [FromQuery(Name = "City")] string? city,
          [FromQuery(Name = "Price")] string? price,
          [FromQuery(Name = "Organizer")] string? organizer)
        {
            // Start with all events
            var events = db.Events
                .Include(e => e.Address)
                .Include(e => e.Category)
                .Include(e => e.Detail)
                .AsQueryable();

         
            var loweredQuery = query?.ToLower();

            if (!string.IsNullOrEmpty(loweredQuery))
            {
                events = events.Where(e =>
                    e.Address.City.ToLower().Contains(loweredQuery) ||
                    e.Address.State.ToLower().Contains(loweredQuery) ||
                    e.Address.Street.ToLower().Contains(loweredQuery) ||
                    e.Category.Name.ToLower().Contains(loweredQuery) ||
                    e.Detail.Organizer.ToLower().Contains(loweredQuery) ||
                    e.Title.ToLower().Contains(loweredQuery)
                );
            }

            if (!string.IsNullOrEmpty(category))
                events = events.Where(e => e.Category.Name == category);

            if (startDate.HasValue)
                events = events.Where(e => e.Detail.Date >= startDate.Value);

            if (endDate.HasValue)
                events = events.Where(e => e.Detail.Date <= endDate.Value);

            if (!string.IsNullOrEmpty(city))
                events = events.Where(e => e.Address.City.ToLower().Contains(city));

            if (!string.IsNullOrEmpty(organizer))
                events = events.Where(e => e.Detail.Organizer.ToLower().Contains(organizer));

          
            var result = events.Select(e => new EventDTO
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

            return Ok(result);
        }




        [HttpGet("/Participant")]
        public IActionResult Participant(String Id)
        {
            return Ok();
        }



    }
}
