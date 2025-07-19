using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyEvent.Controllers;

public class HomeController : Controller
{
    private readonly DB db;
    public HomeController(DB db)
    {
        this.db = db;
    }

    [Route("/")]
    [Route("/Home")]
    public IActionResult Index()
    {
        var e = db.Events;

        if (e == null)
        {
            return NotFound();
        }
        return View(e); ; // Full view with layout for normal browser request
    }
    [Route("/Home/Map")]
    public IActionResult Map() {
 
        return View(); 
    }

    [HttpGet("/Home/{eventName}")]
    public IActionResult EventDetail(string id,string eventName)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound(); // or RedirectToAction("Index");

        var e = db.Events
                  .Include(e => e.Category)
                  .Include(e => e.Detail)
                  .Include(e => e.Address)
                  .FirstOrDefault(e => e.Id == id);

        if (e == null)
            return NotFound(); // or show a custom error page

        return View(e);
    }
}
