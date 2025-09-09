using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyEvent.Migrations;
using MyEvent.Models;
using System.IO;
using System.Security.Claims;
using X.PagedList.Extensions;
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

    public bool CheckTime(TimeOnly? StartTime, TimeOnly? EndTime)
    {
        if (StartTime.HasValue && EndTime.HasValue)
        {
            var duration = EndTime.Value - StartTime.Value;
            return duration.TotalMinutes >= 30 && EndTime > StartTime;
        }
        return true;
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
            foreach (var features in addressResponse)
            {
                if (features.Properties.Formatted == location)
                {
                    return features;
                }
            }

            HttpContext.Session.Remove("create_event_location_search");
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
                Premise = address.Properties.Premise,
                Street = address.Properties.Street,
                City = address.Properties.City,
                State = address.Properties.State,
                Postcode = address.Properties.Postcode,
                Latitude = address.Geometry.Coordinates[1],
                Longitude = address.Geometry.Coordinates[0],
            };
            db.Addresses.Add(a);

            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var admin = db.Admins.FirstOrDefault(a => a.Email == email);
            id = db.Events.Max(e => e.Id) ?? "EVT00000";
            Event e = new()
            {
                Id = NextId(id, "EVT", "D5"),
                Title = vm.Title.Trim().ToUpper(),
                ImageUrl = hp.SavePhoto(vm.ImageUrl, "Events"),
                AdminId = admin.Id,
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
            return RedirectToAction("EventCreated");
        }

        ViewBag.Categories = new SelectList(db.Categories, "Id", "Name");
        return View();
    }

    public async Task<IActionResult> UpdateEvent(string? id, string create_event_location_search = "")
    {
        var e = db.Events
                .Include(e => e.Address)
                .Include(e => e.Category)
                .Include(e => e.Detail)
                .FirstOrDefault(e => e.Id == id);

        if (e == null)
        {
            return RedirectToAction("EventCreated");
        }
        else if (e.Detail.Date < DateOnly.FromDateTime(DateTime.Now))
        {
            TempData["info"] = "The event is PASSED and not allowed to be changed.";
            return RedirectToAction("EventHistory");
        }

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

        var vm = new UpdateEventVM
        {
            Id = e.Id,
            Title = e.Title,
            Date = e.Detail.Date,
            StartTime = e.Detail.StartTime,
            EndTime = e.Detail.EndTime,
            CategoryId = e.CategoryId,
            Price = e.Price,
            Description = e.Detail.Description,
            ContactEmail = e.Detail.ContactEmail,
            ImageUrl = e.ImageUrl,
            Address = $"{e.Address.Premise}, {e.Address.Street}, {e.Address.Postcode} {e.Address.City}, {e.Address.State}",
        };

        ViewBag.Categories = new SelectList(db.Categories, "Id", "Name");
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateEvent(UpdateEventVM vm)
    {
        var e = db.Events
                .Include(e => e.Address)
                .Include(e => e.Detail)
                .Include(e => e.Category)
                .FirstOrDefault(e => e.Id == vm.Id);
        
        if (e == null)
        {
            return RedirectToAction("Index");
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

        if (vm.Image != null)
        {
            var error = hp.ValidatePhoto(vm.Image);
            if (error != "") ModelState.AddModelError("ImageUrl", error);
        }

        Feature? address = null;
        if (HttpContext.Session.GetString("create_event_location_search") != null && vm.Formatted_address != null)
        {
            address = await CheckAddress(vm.Formatted_address);
            if (address == null)
            {
                ModelState.AddModelError("Formatted_address", "Address not found.");
            }
        }

        if (ModelState.IsValid)
        {
            e.Title = vm.Title;
            e.CategoryId = vm.CategoryId;
            e.Price = vm.Price;
            if (vm.Image != null)
            {
                hp.DeletePhoto(e.ImageUrl, "images/Events");
                e.ImageUrl = hp.SavePhoto(vm.Image, "Events");
            }

            if (vm.Formatted_address != null && address != null)
            {
                e.Address.Premise = address.Properties.Premise;
                e.Address.Street = address.Properties.Street;
                e.Address.City = address.Properties.City;
                e.Address.State = address.Properties.State;
                e.Address.Postcode = address.Properties.Postcode;
                e.Address.Latitude = address.Geometry.Coordinates[1];
                e.Address.Longitude = address.Geometry.Coordinates[0];
            }

            e.Detail.Description = vm.Description;
            e.Detail.ContactEmail = vm.ContactEmail;
            e.Detail.Date = vm.Date;
            e.Detail.StartTime = vm.StartTime;
            e.Detail.EndTime = vm.EndTime;
           
            db.SaveChanges();

            TempData["Info"] = "Event edited successful.";
            return RedirectToAction("EventCreated");
        }

        return RedirectToAction("EventCreated");
    }

    [Route("/event_created")]
    public IActionResult EventCreated(string? name, string? sort, string? dir, int page = 1)
    {
        // (1) Searching ------------------------
        ViewBag.Name = name = name?.Trim() ?? "";

        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var searched = db.Events
                        .Include(e => e.Address)
                        .Include(e => e.Category)
                        .Include(e => e.Detail)
                        .Where(e => e.Admin.Email == email &&
                                    e.Detail.Date >= DateOnly.FromDateTime(DateTime.Now) &&
                                    e.Title.Contains(name));

        // (2) Sorting --------------------------
        ViewBag.Sort = sort;
        ViewBag.Dir = dir;

        Func<Event, object> fn = sort switch
        {
            "Id" => e => e.Id,
            "Name" => e => e.Title,
            "Category" => e => e.Category.Name,
            "Date" => e => e.Detail.Date,
            "Price" => e => e.Price,
            _ => e => e.Detail.Date,
        };

        var sorted = dir == "des" ?
             searched.OrderByDescending(fn) :
             searched.OrderBy(fn);

        // (3) Paging ---------------------------
        if (page < 1)
        {
            return RedirectToAction(null, new { name, sort, dir, page = 1 });
        }

        var m = sorted.ToPagedList(page, 10);

        if (page > m.PageCount && m.PageCount > 0)
        {
            return RedirectToAction(null, new { name, sort, dir, page = m.PageCount });
        }

        if (Request.IsAjax())
        {
            return PartialView("_event_listing", m);
        }

        return View(m);
    }

    [HttpPost]
    public IActionResult Delete(string? id)
    {
        var e = db.Events.Find(id);

        if (e != null)
        {
            TempData["Info"] = $"Event {e.Title} deleted.";
            //db.Events.Remove(e);
            //db.SaveChanges();
        }

        return RedirectToAction("EventCreated");
    }

    [Route("/event_detail/{id}")]
    public IActionResult EventDetail(string? id)
    {
        var m = db.Events
                .Include(e => e.Address)
                .Include(e => e.Category)
                .Include(e => e.Detail)
                .Include(e => e.Tickets)
                .ThenInclude(e => e.Buyer)
                .FirstOrDefault(e => e.Id == id);

        if (m != null)
        {
            return View(m);

        }
        return RedirectToAction("EventCreated");
    }

    [Route("/event_history")]
    public IActionResult EventHistory(string? name, string? sort, string? dir, int page = 1)
    {
        // (1) Searching ------------------------
        ViewBag.Name = name = name?.Trim() ?? "";

        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var searched = db.Events
                        .Include(e => e.Address)
                        .Include(e => e.Category)
                        .Include(e => e.Detail)
                        .Where(e => e.Admin.Email == email &&
                                    e.Detail.Date < DateOnly.FromDateTime(DateTime.Now) &&
                                    e.Title.Contains(name));

        // (2) Sorting --------------------------
        ViewBag.Sort = sort;
        ViewBag.Dir = dir;

        Func<Event, object> fn = sort switch
        {
            "Id" => e => e.Id,
            "Name" => e => e.Title,
            "Category" => e => e.Category.Name,
            "Date" => e => e.Detail.Date,
            "Price" => e => e.Price,
            _ => e => e.Detail.Date,
        };

        var sorted = dir == "des" ?
             searched.OrderByDescending(fn) :
             searched.OrderBy(fn);

        // (3) Paging ---------------------------
        if (page < 1)
        {
            return RedirectToAction(null, new { name, sort, dir, page = 1 });
        }

        var m = sorted.ToPagedList(page, 10);

        if (page > m.PageCount && m.PageCount > 0)
        {
            return RedirectToAction(null, new { name, sort, dir, page = m.PageCount });
        }

        if (Request.IsAjax())
        {
            return PartialView("_event_listing", m);
        }

        return View(m);
    }
}
