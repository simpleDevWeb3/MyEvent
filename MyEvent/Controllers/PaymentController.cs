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

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("_PaymentInfoPartial", ev); // ✅ Model guaranteed not null

        return View("PaymentInfo", ev); // ✅ Model guaranteed not null
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
            ModelState.AddModelError("", "User email not found in claims.");
            return View("PaymentInfo"); // reload the form instead of going home
        }

        var buyer = _db.Users.FirstOrDefault(u => u.Email == userEmail);
        if (buyer == null)
        {
            ModelState.AddModelError("", "User not found!");
            return View("PaymentInfo");
        }

        var ev = _db.Events.FirstOrDefault(e => e.Id == eventId);
        if (ev == null)
        {
            ModelState.AddModelError("", "Event not found!");
            return View("PaymentInfo");
        }

        // (Optional) Simulate payment validation
        if (string.IsNullOrWhiteSpace(CardNumber) || CardNumber.Length < 4)
        {
            ModelState.AddModelError("", "Invalid card number.");
            return View("PaymentInfo");
        }

        // Create ticket
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

            TempData["Success"] = "Payment successful!";
            return RedirectToAction("MyTickets", "Ticket");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error saving ticket: {ex.Message}");
            return View("PaymentInfo");
        }
    }




}
