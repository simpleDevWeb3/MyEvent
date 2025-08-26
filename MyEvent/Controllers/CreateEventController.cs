using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyEvent.Models;
using static MyEvent.Models.DB;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        var min = DateOnly.FromDateTime(DateTime.Now);  //today
        var max = min.AddYears(1);  //1 year after

        if (min < date && date < max)
            return true;

        return false;
    }

    public bool CheckTime(TimeOnly StartTime, TimeOnly EndTime)
    {
        return (EndTime - StartTime).TotalMinutes >= 30 && EndTime > StartTime;
    }

    public bool CheckPrice(decimal price)
    {
        return price >= 0 && price <= 200.00m;
    }

    public async Task<Feature?> CheckAddress(string location)
    {
        var s = HttpContext.Session.GetString("create_event_location_search");
        var addressResponse = await _geo.GetCoordinatesAsync(s ?? "");

        if (addressResponse != null)
        {
            foreach (var features in addressResponse.Features)
            {
                if (features.Properties.Formatted == location)
                {
                    return features;
                }
            }
            return null;
        }
        return null;
    }

    [Route("/create_event")]
    public async Task<IActionResult> CreateEvent(string create_event_location_search)
    {
        if (Request.IsAjax())
        {
            var coordinates = await _geo.GetCoordinatesAsync(create_event_location_search);

            if (coordinates != null && create_event_location_search != null)
            {
                HttpContext.Session.SetString("create_event_location_search", create_event_location_search);
                return PartialView("_SearchResult", coordinates);
            }
            return PartialView("_SearchResult");
        }

        ViewBag.Categories = new SelectList(db.Categories, "Id", "Name");

        return View();
    }

    [HttpPost]
    [Route("/create_event")]
    public async Task<IActionResult> CreateEvent(EventVM vm)
    {
        if (ModelState.IsValid("CategoryId") && !db.Categories.Any(c => c.Id == vm.CategoryId))
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
            var e = hp.ValidatePhoto(vm.ImageUrl);
            if (e != "") ModelState.AddModelError("ImageUrl", e);
        }

        Feature? address = await CheckAddress(vm.Formatted_address);
        if (address == null)
        {
            ModelState.AddModelError("Address", "Address not found.");
        }
        
        if (ModelState.IsValid && address != null)
        {
            string id = db.Addresses.Max(a => a.Id) ?? "ADDR0000";
            Address a = new()
            {
                Id = NextId(id, "ADDR", "D4"),
                Premise = address.Properties.Premise ?? "",
                Street = address.Properties.Street,
                City = address.Properties.City,
                State = address.Properties.State,
                Postcode = address.Properties.Postcode,
                Latitude = address.Geometry.Coordinates[1],
                Longitude = address.Geometry.Coordinates[0],
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

            TempData["Info"] = "Event created successful.";
            return RedirectToAction("Index", "Home");
        }

        
        ViewBag.Categories = new SelectList(db.Categories, "Id", "Name");
        return View();
    }
}
