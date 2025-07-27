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


    public bool CheckCategoryId(string CategoryId)
    {
        return db.Categories.Any(c => c.Id == CategoryId);
    }

    public bool CheckDate(DateOnly date)
    {
        if (DateOnly.FromDateTime(DateTime.Now) > date)
            return false;


        return true;
    }

    [Route("/create_event")]
    public IActionResult CreateEvent()
    {
        ViewBag.Categories = new SelectList(db.Categories, "Id", "Name"); 
        return View();
    }

    [HttpPost]
    [Route("/create_event")]
    public IActionResult CreateEvent(EventVM vm)
    {
        if (ModelState.IsValid("CategoryId") &&
            !db.Categories.Any(c => c.Id == vm.CategoryId))
        {
            ModelState.AddModelError("CategoryId", "Invalid Category.");
        }

        if (!CheckDate(vm.Date))
        {
            ModelState.AddModelError("Date", "Suspicious Date");
        }

        if (ModelState.IsValid)
        {
            Event e = new()
            {
                Id = "EVT99999",
                Title = vm.Title.Trim().ToUpper(),
                ImageUrl = "Sample Image",
                CategoryId = vm.CategoryId,
                AddressId = "ADDR0001",
            };

            Detail d = new()
            {
                Id = "DET99999",
                Description = vm.Description,
                Organizer = "Sample Organizer",
                ContactEmail = vm.ContactEmail,
                Date = vm.Date,
                StartTime = vm.StartTime,
                EndTime = vm.EndTime,
                EventId = e.Id,
            };
            e.Detail = d;
            db.Events.Add(e);
            //db.SaveChanges();

            TempData["Info"] = "Recoed inserted.";
            return RedirectToAction("Index");
        }
        ViewBag.ProgramList = new SelectList(db.Categories, "Id", "Name");
        return View();
    }

}
