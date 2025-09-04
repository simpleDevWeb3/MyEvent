using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static MyEvent.Models.DB;

[Authorize]
public class NotificationController : Controller
{
    private readonly DB _db;
    public NotificationController(DB db) => _db = db;

    public IActionResult Notification()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return Unauthorized();

        var user = _db.Users.FirstOrDefault(u => u.Email == email);
        if (user == null) return View(new List<Ticket>());

        var tickets = _db.Tickets
            .Include(t => t.Event)
            .ThenInclude(e => e.Detail)
            .Where(t => t.BuyerId == user.Id)
            .ToList();

        // Filter out dismissed IDs from Session
        var dismissed = HttpContext.Session.GetString("DismissedTickets");
        var dismissedSet = string.IsNullOrEmpty(dismissed)
            ? new HashSet<int>()
            : dismissed.Split(',').Select(int.Parse).ToHashSet();

        tickets = tickets.Where(t => !dismissedSet.Contains(t.TicketId)).ToList();

        return View(tickets);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Dismiss(int ticketId)
    {
        // store dismissed in Session (persist across navigation)
        var dismissed = HttpContext.Session.GetString("DismissedTickets");
        var set = string.IsNullOrEmpty(dismissed)
            ? new HashSet<int>()
            : dismissed.Split(',').Select(int.Parse).ToHashSet();

        set.Add(ticketId);
        HttpContext.Session.SetString("DismissedTickets", string.Join(",", set));

        return Json(new { success = true });
    }
}
