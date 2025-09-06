using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyEvent.Models;
using System.Security.Claims;
using static MyEvent.Models.DB;

namespace MyEvent.Controllers;

public class HomeController : Controller
{
    private readonly DB db;
    private readonly IWebHostEnvironment en;

    public HomeController(DB db, IWebHostEnvironment en)
    {
        this.db = db;
        this.en = en;
    }

    [Route("/")]
    [Route("/Home")]
    public IActionResult Index()
    {
        var e = db.Categories;

        if (e == null)
        {
            return NotFound();
        }
        return View(e); ; // Full view with layout for normal browser request
    }
    [HttpGet("/Home/Search")]
    public IActionResult Search(string q) {

        var c = db.Categories;
                       
                        

        return View(c); 
    }

    [HttpGet("/Home/{eventName}")]
    public IActionResult EventDetail(string id)
    {
        var ev = db.Events
                    .Include(e => e.Category)
                    .Include(e => e.Detail)
                    .Include(e => e.Address)
                    .FirstOrDefault(e => e.Id == id);

        if (ev == null)
            return RedirectToAction("Index");

        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value
                        ?? User.FindFirst("email")?.Value;

        bool isJoined = false;

        if (!string.IsNullOrEmpty(userEmail))
        {
            var user = db.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user != null)
            {
                isJoined = db.Tickets.Any(t => t.EventId == id && t.BuyerId == user.Id);
            }
        }

        ViewBag.IsJoined = isJoined;

        return View(ev);
    }





}
