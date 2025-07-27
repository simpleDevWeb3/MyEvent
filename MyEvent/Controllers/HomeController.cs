using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
