using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using MyEvent.Models;

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
    public IActionResult Payment(string eventId)
    {
        var ev = _db.Events
                    .Include(e => e.Category)
                    .Include(e => e.Detail)
                    .Include(e => e.Address)
                    .FirstOrDefault(e => e.Id == eventId);

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("_PaymentPartial", ev);

        return View("Payment", ev);
    }

    [Authorize]
    public IActionResult PaymentInfo(string eventId)
    {
        var ev = _db.Events
                    .Include(e => e.Category)
                    .Include(e => e.Detail)
                    .Include(e => e.Address)
                    .FirstOrDefault(e => e.Id == eventId);

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("_PaymentInfoPartial", ev); // This contains the input form

        return View("PaymentInfo", ev);
    }


    [Authorize]
    [HttpPost]
    public IActionResult ProcessPayment(int eventId, string HolderName, string HolderEmail, string CardNumber, string Expiry, string CVV)
    {
        var userId = int.Parse(s: User.FindFirst("UserId").Value); // Assuming claim stores user ID

        var ticket = new Ticket
        {
            EventId = eventId.ToString(), // Your EventId is string
            BuyerId = userId,
            HolderName = HolderName,
            HolderEmail = HolderEmail
        };

        _db.Tickets.Add(ticket);
        _db.SaveChanges();

        TempData["Success"] = "Payment successful!";
        return RedirectToAction("MyTickets", "Ticket");
    }

    [Authorize]
    [HttpPost]
    public IActionResult DeletePayment(string eventId)
    {
        var pm = _db.Events.FirstOrDefault(p => p.Id == eventId);
        if (pm != null)
        {
            _db.Events.Remove(pm);
            _db.SaveChanges();
        }

        return RedirectToAction("Payment");
    }



}
