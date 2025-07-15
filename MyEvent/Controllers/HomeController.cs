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
     

        return View(e); ; // Full view with layout for normal browser request
    }

    public IActionResult Map() {
 
        return View(); 
    }
}
