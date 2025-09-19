using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static MyEvent.Models.DB;

namespace MyEvent.Controllers
{
    public class TicketController : Controller
    {
        private readonly DB _db;

        public TicketController(DB db)
        {
            _db = db;
        }

        [Authorize(Roles = "Member")]
        public IActionResult MyTickets()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            var tickets = _db.Tickets
                .Include(t => t.Event)
                .ThenInclude(e => e.Detail)
                .Where(t => t.HolderEmail == email)
                .ToList();

            var user = _db.Users.FirstOrDefault(u => u.Email == email);
            if (user != null && !tickets.Any())
            {
                tickets = _db.Tickets
                    .Include(t => t.Event)
                    .ThenInclude(e => e.Detail)
                    .Where(t => t.BuyerId == user.Id)
                    .ToList();
            }

            return View("Ticket", tickets);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddTicket(string eventId)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            if (string.IsNullOrEmpty(eventId))
                return BadRequest("Invalid event ID");

            eventId = eventId.Trim();

            var ev = _db.Events
                .Include(e => e.Category)
                .Include(e => e.Detail)
                .Include(e => e.Address)
                .FirstOrDefault(e => e.Id == eventId);

            if (ev == null)
                return View(null);

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

            return RedirectToAction("Payment", "Payment", new { eventId = ev.Id });
        }

        [HttpPost]
        public IActionResult DeleteTicket(int ticketId)
        {
            TempData.Remove("Delete");
            TempData.Remove("Error");
            TempData.Remove("Success");

            var ticket = _db.Tickets.Find(ticketId);
            if (ticket != null)
            {
                _db.Tickets.Remove(ticket);
                _db.SaveChanges();
                TempData["Delete"] = $"Ticket {ticketId} deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Ticket not found.";
            }

            return RedirectToAction("MyTickets");
        }

        // ✅ NEW: Batch Delete
        [HttpPost]
        public IActionResult BatchDeleteTickets(List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                TempData["Error"] = "No tickets selected for deletion.";
                return RedirectToAction("MyTickets");
            }

            var tickets = _db.Tickets.Where(t => ids.Contains(t.TicketId)).ToList();

            if (tickets.Any())
            {
                _db.Tickets.RemoveRange(tickets);
                _db.SaveChanges();
                TempData["Delete"] = $"{tickets.Count} ticket(s) deleted successfully.";
            }
            else
            {
                TempData["Error"] = "No matching tickets found.";
            }

            return RedirectToAction("MyTickets");
        }

        [Authorize(Roles = "Member")]
        public IActionResult TicketDetail(int id)
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = _db.Users.FirstOrDefault(u => u.Name == name);

            var ticket = _db.Tickets
                .Include(t => t.Event)
                .ThenInclude(e => e.Detail)
                .FirstOrDefault(t => t.TicketId == id && t.BuyerId == user.Id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View("TicketDetail", ticket);
        }
    }
}
