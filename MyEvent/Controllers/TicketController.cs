using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static MyEvent.Models.DB;
namespace MyEvent.Controllers;

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
        var name = User.FindFirst(ClaimTypes.Name)?.Value;
        var user = _db.Users.FirstOrDefault(u => u.Name == name);

        var tickets = _db.Tickets
            .Include(t => t.Event)
            .ThenInclude(e => e.Detail)
            .Where(t => t.BuyerId == user.Id)
            .ToList();

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
            return Unauthorized(); // Invalid claim type

        if (string.IsNullOrEmpty(eventId))
            return BadRequest("Invalid event ID");

        eventId = eventId.Trim();

        var ev = _db.Events
                    .Include(e => e.Category)
                    .Include(e => e.Detail)
                    .Include(e => e.Address)
                    .FirstOrDefault(e => e.Id == eventId);

        if (ev == null)
            return View(null); // or handle "event not found"

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
        // Reset TempData for this action
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





}
