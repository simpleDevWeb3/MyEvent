using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("favourite")]
public class FavouriteController : Controller
{
    private readonly DB _db;

    public FavouriteController(DB db)
    {
        _db = db;
    }

    [HttpGet("")]
    public IActionResult Followed()
    {
        var events = _db.Events
                        .Include(e => e.Detail)
                        .Include(e => e.Address)
                        .Include(e => e.Category)
                        .ToList();

        return View("Followed", events); // Points to Views/Favourite/Followed.cshtml
    }
}
