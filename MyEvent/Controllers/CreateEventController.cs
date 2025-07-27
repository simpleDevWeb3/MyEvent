using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static MyEvent.Models.DB;

namespace MyEvent.Controllers;

public class CreateEventController : Controller
{
    private readonly DB db;
    private readonly IWebHostEnvironment en;

    public CreateEventController(DB db, IWebHostEnvironment en)
    {
        this.db = db;
        this.en = en;
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
            return RedirectToAction("Index", "Home");
        }
        ViewBag.Categories = new SelectList(db.Categories, "Id", "Name");
        return View();
    }
}
