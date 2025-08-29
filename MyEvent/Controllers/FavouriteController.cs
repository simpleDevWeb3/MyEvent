
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
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(); // User not logged in

            if (string.IsNullOrWhiteSpace(eventId))
                return BadRequest("Invalid event ID.");

            eventId = eventId.Trim();

            // Fetch event with details
            var ev = _db.Events
                        .Include(e => e.Detail)
                        .Include(e => e.Address)
                        .Include(e => e.Category)
                        .FirstOrDefault(e => e.Id == eventId);

            if (ev == null)
                return View("Error", "Event not found."); // Optional error view

            // Check if already followed
            var existing = _db.FollowedEvents
                              .FirstOrDefault(f => f.EventId == ev.Id && f.UserId == userId);

            if (existing == null)
            {
                _db.FollowedEvents.Add(new FollowedEvent
                {
                    EventId = ev.Id,
                    UserId = userId,
                    FollowedDate = DateTime.Now
                });

                _db.SaveChanges();
            }

            // Redirect to Followed Events page
            return RedirectToAction("FollowedEvents", "Favourite");
        }



        [Authorize]
        public IActionResult FollowedEvents()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var followed = _db.FollowedEvents
                              .Include(f => f.Event)
                              .ThenInclude(e => e.Detail)
                              .Include(f => f.Event.Category)
                              .Where(f => f.UserId == userId)
                              .ToList();

            return View("Followed",followed);
        }



        [Authorize]
        [HttpPost]
        public IActionResult DeleteFollowed(int id)
        {
            var followed = _db.FollowedEvents.FirstOrDefault(f => f.Id == id);
            if (followed != null)
            {
                _db.FollowedEvents.Remove(followed);
                _db.SaveChanges();
            }

            return RedirectToAction("FollowedEvents");
        }

    }
}
