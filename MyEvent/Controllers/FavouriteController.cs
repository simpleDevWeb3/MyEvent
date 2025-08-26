using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MyEvent.Models;
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
            if (string.IsNullOrWhiteSpace(eventId))
            {
                return BadRequest("Invalid event ID.");
            }

            var alreadyFollowed = _db.FollowedEvents.Any(f => f.EventId == eventId);
            if (!alreadyFollowed)
            {
                var ev = _db.Events
                            .Include(e => e.Detail)
                            .Include(e => e.Address)
                            .Include(e => e.Category)
                            .FirstOrDefault(e => e.Id == eventId);

                if (ev != null)
                {
                    _db.FollowedEvents.Add(new FollowedEvent
                    {
                        EventId = ev.Id,
                        Event = ev
                    });

                    _db.SaveChanges();
                }
            }

            return RedirectToAction("FollowedEvents");
        }

        public IActionResult FollowedEvents()
        {
            var followed = _db.FollowedEvents
                              .Include(f => f.Event)
                                .ThenInclude(e => e.Detail)
                              .Include(f => f.Event.Address)
                              .Include(f => f.Event.Category)
                              .ToList();

            return View("Followed", followed); // Ensure your view is Views/Favourite/Followed.cshtml
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
