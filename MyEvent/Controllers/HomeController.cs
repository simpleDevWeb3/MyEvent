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
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            // Return only the view body (without _Layout)
            return PartialView("_IndexPartial",e); // You must create this partial
        }

        return View(e); ; // Full view with layout for normal browser request
    }

    public IActionResult Map() { 
        return View();
    }
}
