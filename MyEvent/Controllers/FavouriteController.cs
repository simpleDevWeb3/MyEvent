
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyEvent.Models;
using System.Security.Claims;
using static MyEvent.Models.DB;

namespace MyEvent.Controllers
{
    public class FavouriteController : Controller
    {
        private readonly DB _db;

        public FavouriteController(DB db)
        {
            _db = db;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Follow(string eventId)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var user = _db.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return Unauthorized();

            if (string.IsNullOrWhiteSpace(eventId))
                return BadRequest("Invalid event ID.");

            var ev = _db.Events
                        .Include(e => e.Detail)
                        .Include(e => e.Address)
                        .Include(e => e.Category)
                        .FirstOrDefault(e => e.Id == eventId);

            if (ev == null)
                return View("Error", "Event not found.");

            var existing = _db.FollowedEvents
                              .FirstOrDefault(f => f.EventId == ev.Id && f.UserId == user.Id);

            if (existing == null)
            {
                _db.FollowedEvents.Add(new FollowedEvent
                {
                    EventId = ev.Id,
                    UserId = user.Id,
                    FollowedDate = DateTime.Now
                });
                _db.SaveChanges();

                // Set the message here
                TempData["Followed"] = "Event Followed!";
            }

            if (existing != null)
            {
                TempData["FollowedError"] = "You already follow this event.";

                return RedirectToAction("FollowedEvents", "Favourite");
            }

            return RedirectToAction("FollowedEvents", "Favourite");
        }



        [Authorize]
        public IActionResult FollowedEvents()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            // Lookup user in DB
            var user = _db.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return Unauthorized();

            var followed = _db.FollowedEvents
                              .Include(f => f.Event)
                              .ThenInclude(e => e.Detail)
                              .Include(f => f.Event.Category)
                              .Where(f => f.UserId == user.Id)
                              .ToList();

            return View("Followed", followed);
        }




        [Authorize]
        [HttpPost]
        public IActionResult DeleteFollowed(int id)
        {
            var followed = _db.FollowedEvents.FirstOrDefault(f => f.Id == id);

            TempData.Clear(); // clears any old messages
            if (followed != null)
            {
                _db.FollowedEvents.Remove(followed);
                _db.SaveChanges();

                TempData["Delete"] = "Event deleted."; // only set when deleting
            }

            return RedirectToAction("FollowedEvents");
        }


    }
}
