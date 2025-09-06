using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyEvent.Models;
using System.Security.Claims;
using static MyEvent.Models.DB;
namespace MyEvent.Controllers;

public class PaymentController : Controller
{
    private readonly DB _db;

    public PaymentController(DB db)
    {
        _db = db;
    }

    [Authorize]
    public IActionResult PaymentInfo(string eventId)
    {
        if (string.IsNullOrWhiteSpace(eventId))
        {
            TempData["Error"] = "Invalid event ID!";
            return RedirectToAction("Index", "Home");
        }

        var ev = _db.Events
                    .Include(e => e.Category)
                    .Include(e => e.Detail)
                    .Include(e => e.Address)
                    .FirstOrDefault(e => e.Id == eventId);

        if (ev == null)
        {
            TempData["Error"] = "Event not found!";
            return RedirectToAction("Index", "Home");
        }

        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value
                        ?? User.FindFirst("email")?.Value;

        if (string.IsNullOrWhiteSpace(userEmail))
        {
            TempData["Error"] = "User email not found!";
            return RedirectToAction("Index", "Home");
        }

        var user = _db.Users.FirstOrDefault(u => u.Email == userEmail);
        if (user == null)
        {
            TempData["Error"] = "User not found!";
            return RedirectToAction("Index", "Home");
        }

        // Safe check for ticket existence
        bool isJoined = false;
        try
        {
            isJoined = _db.Tickets
                          .Where(t => t.EventId != null && t.BuyerId != null) // filter out bad data
                          .Any(t => t.EventId == eventId && t.BuyerId == user.Id);
        }
        catch (Exception ex)
        {
            // Log the error if needed
            TempData["Error"] = "Error checking ticket status.";
        }

        ViewBag.IsJoined = isJoined;

        // Render PaymentInfo if everything ok
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("_PaymentInfoPartial", ev);

        return View("PaymentInfo", ev);
    }


    [Authorize]
    [HttpPost]
    public IActionResult ProcessPayment(
    string eventId,
    string HolderName,
    string HolderEmail,
    string CardNumber,
    string Expiry,
    string CVV)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value
                        ?? User.FindFirst("email")?.Value;

        if (string.IsNullOrEmpty(userEmail))
        {
            ViewBag.ErrorMessage = "User email not found in claims.";
            return View("PaymentInfo");
        }

        var buyer = _db.Users.FirstOrDefault(u => u.Email == userEmail);
        if (buyer == null)
        {
            ViewBag.ErrorMessage = "User not found!";
            return View("PaymentInfo");
        }

        var ev = _db.Events.FirstOrDefault(e => e.Id == eventId);
        if (ev == null)
        {
            ViewBag.ErrorMessage = "Event not found!";
            return View("PaymentInfo");
        }

        // ✅ Check duplicate ticket
        var existingTicket = _db.Tickets.FirstOrDefault(t => t.EventId == ev.Id && t.BuyerId == buyer.Id);
        if (existingTicket != null)
        {
            ViewBag.ErrorMessage = "You already purchased a ticket for this event!";
            return View("PaymentInfo", ev);
        }

        if (string.IsNullOrWhiteSpace(CardNumber) || CardNumber.Length < 4)
        {
            ViewBag.ErrorMessage = "Invalid card number.";
            return View("PaymentInfo", ev);
        }

        var ticket = new Ticket
        {
            EventId = ev.Id,
            BuyerId = buyer.Id,
            HolderName = HolderName,
            HolderEmail = HolderEmail,
        };

        try
        {
            _db.Tickets.Add(ticket);
            _db.SaveChanges();

            TempData.Clear(); // ✅ reset old
            TempData["Success"] = "Payment successful!";

            return RedirectToAction("MyTickets", "Ticket");
        }
        catch (Exception ex)
        {
            TempData.Clear();
            TempData["Error"] = $"Error saving ticket: {ex.Message}";

            return RedirectToAction("MyTickets", "Ticket");
        }

    }


}
