using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static MyEvent.Models.DB;

namespace MyEvent.Controllers;

public class CreateEventController : Controller
{
    private readonly DB db;
    private readonly IWebHostEnvironment en;
    private readonly Helper hp;


    public CreateEventController(DB db, IWebHostEnvironment en, Helper hp)
    {
        this.db = db;
        this.en = en;
        this.hp = hp;
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

    public bool CheckTime(TimeOnly start_time, TimeOnly end_time)
    {
        return end_time - start_time >= TimeSpan.FromMinutes(30);
    }

    public bool CheckPrice(decimal price)
    {
        return price >= 0 && price <= 200.00m;
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

        if (!CheckTime(vm.StartTime, vm.EndTime))
        {
            ModelState.AddModelError("EndTime", "The minimum duration time is 30 minutes.");
        }

        if (!CheckPrice(vm.Price))
        {
            ModelState.AddModelError("Price", "Invalid Price.");
        }

        if (ModelState.IsValid("ImageUrl"))
        {
            // TODO
            var e = hp.ValidatePhoto(vm.ImageUrl);
            if (e != "") ModelState.AddModelError("ImageUrl", e);
        }

        if (ModelState.IsValid)
        {
            //1 Lebuh Light	George Town	Penang	10200	5.4194	100.3422	Penang City Hall
            Address a = new()
            {
                Id = "ADDR9999",
                Premise = "Sample Premise",
                Street = "Lebuh Light",
                City = "George Town",
                State = "Penang",
                Postcode = "10200",
                Latitude = 5.4194,
                Longitude = 100.3422,
            };
            db.Addresses.Add(a);
            
            Event e = new()
            {
                Id = "EVT99999",
                Title = vm.Title.Trim().ToUpper(),
                ImageUrl = hp.SavePhoto(vm.ImageUrl, "images/Events"),
                CategoryId = vm.CategoryId,
                AddressId = a.Id,
                Price = vm.Price,
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
            db.SaveChanges();
            
            TempData["Info"] = "Recoed inserted.";
            return RedirectToAction("Index", "Home");
        }
        ViewBag.Categories = new SelectList(db.Categories, "Id", "Name");
        return View();
    }
}
