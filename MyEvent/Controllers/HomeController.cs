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

   
    public IActionResult Index()
    {
        var e = db.Events;

        if (e == null)
        {
            return NotFound();
        }
        return View(e); ; // Full view with layout for normal browser request
    }

    public IActionResult Map() {
 
        return View(); 
    }

    [HttpGet]
    public IActionResult EventDetail(string id)
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
