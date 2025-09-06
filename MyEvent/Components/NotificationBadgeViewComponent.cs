using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class NotificationBadgeViewComponent : ViewComponent
{
    private readonly DB _db;

    public NotificationBadgeViewComponent(DB db)
    {
        _db = db;
    }

    public IViewComponentResult Invoke()
    {
        var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        int upcomingCount = 0;
        var now = DateTime.Now;

        if (!string.IsNullOrEmpty(email))
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                var tickets = _db.Tickets
                    .Include(t => t.Event)
                    .ThenInclude(e => e.Detail)
                    .Where(t => t.BuyerId == user.Id)
                    .ToList();

                // Get dismissed IDs from Session
                var dismissed = HttpContext.Session.GetString("DismissedTickets");
                var dismissedSet = string.IsNullOrEmpty(dismissed)
                    ? new HashSet<int>()
                    : dismissed.Split(',').Select(int.Parse).ToHashSet();

                foreach (var t in tickets)
                {
                    if (dismissedSet.Contains(t.TicketId)) continue; // skip dismissed

                    if (t.Event?.Detail == null) continue;

                    var eventDate = t.Event.Detail.Date;
                    var startTime = t.Event.Detail.StartTime;
                    var eventStart = eventDate.ToDateTime(startTime);

                    if (eventStart > now && eventStart <= now.AddDays(3))
                    {
                        upcomingCount++;
                    }
                }
            }
        }

        return View(upcomingCount);
    }

}
