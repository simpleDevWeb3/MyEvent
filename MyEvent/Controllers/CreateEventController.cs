using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static MyEvent.Models.DB;

namespace MyEvent.Controllers;

public class CreateEventController : Controller
{
    private readonly DB db;
    private readonly IWebHostEnvironment en;
    private readonly Helper hp;
    private readonly GeoService _geo;
    public CreateEventController(DB db, IWebHostEnvironment en, Helper hp, GeoService geo)
    {
        this.db = db;
        this.en = en;
        this.hp = hp;
        _geo = geo;
    }

    private string NextId(string id, string prefix, string format)
    {
        int n = int.Parse(id[prefix.Length..]);
        return $"{prefix}{(n + 1).ToString(format)}";
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

    public bool CheckTime(TimeOnly StartTime, TimeOnly EndTime)
    {
        return (EndTime - StartTime).TotalMinutes >= 30 && EndTime > StartTime;
    }

    public bool CheckPrice(decimal price)
    {
        return price >= 0 && price <= 200.00m;
    }

    public async Task<bool> CheckAdress(AddressVM vm)
    {
        string fullAddress = $"{vm.Premise}, {vm.Street}, {vm.Postcode} {vm.City}, {vm.State}";
        var cordinates = await _geo.GetCoordinatesAsync(fullAddress);
        var (lat, lon) = (0.0, 0.0);        //static first
        if (cordinates != null)
        {
            (lat, lon) = cordinates.Value;
        }

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
    public async Task<IActionResult> CreateEvent(EventVM vm)
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

        string fullAddress = $"{vm.Address.Premise}, {vm.Address.Street}, {vm.Address.Postcode} {vm.Address.City}, {vm.Address.State}";
        var cordinates = await _geo.GetCoordinatesAsync(fullAddress);
        var (lat, lon) = (0.0, 0.0);        //static first
        if (cordinates != null)
        {
            (lat, lon) = cordinates.Value;
        }
        else
        {
            ModelState.AddModelError("Address", "Address not found.");
        }

        if (ModelState.IsValid)
        {
            string id = db.Addresses.Max(a => a.Id) ?? "ADDR0000";
            Address a = new()
            {
                Id = NextId(id, "ADDR", "D4"),
                Premise = vm.Address.Premise,
                Street = vm.Address.Street,
                City = vm.Address.City,
                State = vm.Address.State,
                Postcode = vm.Address.Postcode,
                Latitude = lat,//5.4194,
                Longitude = lon, //100.3422,
            };
            db.Addresses.Add(a);

            id = db.Events.Max(e => e.Id) ?? "EVT00000";
            Event e = new()
            {
                Id = NextId(id, "EVT", "D5"),
                Title = vm.Title.Trim().ToUpper(),
                ImageUrl = hp.SavePhoto(vm.ImageUrl, "images/Events"),
                CategoryId = vm.CategoryId,
                AddressId = a.Id,
                Price = vm.Price,
            };

            id = db.Details.Max(d => d.Id) ?? "DET00000";
            Detail d = new()
            {
                Id = NextId(id, "DET", "D5"),
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
